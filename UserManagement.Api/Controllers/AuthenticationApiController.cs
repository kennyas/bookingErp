using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Mvc.Internal;
using OpenIddict.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Enums;
using Tornado.Shared.Helpers;
using Tornado.Shared.Models;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Events;
using UserManagement.Core.Helpers;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthenticationApiController : BaseController
    {
        private readonly SignInManager<GigmUser> _signInManager;
        private readonly UserManager<GigmUser> _userManager;

        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IUserService _userService;
        private readonly IUserMnagementEventService _userEventService;

        public AuthenticationApiController(
            IUserService userService,
            IOptions<IdentityOptions> identityOptions,
            SignInManager<GigmUser> signInManager,
            UserManager<GigmUser> userManager,
            IUserMnagementEventService userEventService)
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
            _userEventService = userEventService;
        }

        [AllowAnonymous, HttpPost]
        //[ServiceFilter(typeof(LoginFilter))]
        public async Task<IActionResult> Token([ModelBinder(BinderType = typeof(OpenIddictMvcBinder))] OpenIdConnectRequest request)
        {
            try
            {
                if (request.IsPasswordGrantType())
                {
                    var user = await _userManager.FindByNameAsync(request.Username).ConfigureAwait(false)
                        ?? await _userManager.FindByEmailAsync(request.Username).ConfigureAwait(false);

                    if (user is null)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "Login or password is incorrect."
                        });
                    }

                    if (!user.Activated)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "Your profile has not been activated."
                        });
                    }

                    if (!await _signInManager.CanSignInAsync(user).ConfigureAwait(false) || user.IsDeleted)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "You are not allowed to sign in."
                        });
                    }

                    if (_userManager.SupportsUserTwoFactor && await _userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false))
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "You are not allowed to sign in."
                        });
                    }

                    if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user).ConfigureAwait(false))
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "Your profile is temporary locked."
                        });
                    }

                    //mumu - why you no change ur password on ur first login ?
                    if (user.ChangePasswordOnLogin && user.LastLoginDate.HasValue)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "Please reset your password to continue."
                        });
                    }

                    if (!await _userManager.CheckPasswordAsync(user, request.Password).ConfigureAwait(false))
                    {
                        if (_userManager.SupportsUserLockout)
                        {
                            await _userManager.AccessFailedAsync(user).ConfigureAwait(false);
                        }

                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "Login or password is incorrect."
                        });
                    }

                    if (_userManager.SupportsUserLockout)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user).ConfigureAwait(false);
                    }

                    user.LastLoginDate = Clock.Now;
                    await _userManager.UpdateAsync(user).ConfigureAwait(false);

                    // Create a new authentication ticket.
                    var ticket = await CreateTicketAsync(request, user).ConfigureAwait(false);
                    return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
                }
                else if (request.IsRefreshTokenGrantType())
                {
                    // Retrieve the claims principal stored in the refresh token.
                    var info = await HttpContext.AuthenticateAsync(
                        OpenIddictServerDefaults.AuthenticationScheme).ConfigureAwait(false);

                    // Retrieve the user profile corresponding to the refresh token.
                    // Note: if you want to automatically invalidate the refresh token
                    // when the user password/roles change, use the following line instead:
                    // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                    var user = await _userManager.GetUserAsync(info.Principal).ConfigureAwait(false);
                    if (user == null)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "The refresh token is no longer valid."
                        });
                    }

                    // Ensure the user is still allowed to sign in.
                    if (!await _signInManager.CanSignInAsync(user).ConfigureAwait(false))
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "The user is no longer allowed to sign in."
                        });
                    }
                    // Create a new authentication ticket, but reuse the properties stored
                    // in the refresh token, including the scopes originally granted.
                    var ticket = await CreateTicketAsync(request, user, info.Properties).ConfigureAwait(false);
                    return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
                }

                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "The specified grant type is not supported."
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest oidcRequest, GigmUser user,
       AuthenticationProperties properties = null)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user).ConfigureAwait(false);
            var identity = (ClaimsIdentity)principal.Identity;

            AddUserClaims(user, identity);

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal, properties, OpenIddictServerDefaults.AuthenticationScheme);

            if (!oidcRequest.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                ticket.SetScopes(new[]
                {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles,

                }.Intersect(oidcRequest.GetScopes()));
            }

            ticket.SetResources("resource_server");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            var destinations = new List<string>
            {
                OpenIdConnectConstants.Destinations.AccessToken
            };

            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Audience && ticket.HasScope(OpenIddictConstants.Claims.Audience))

                    )
                {
                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            var name = new Claim(OpenIdConnectConstants.Claims.GivenName, user.FullName ?? "[NA]");
            name.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim(name);

            var usertype = new Claim(OpenIdConnectConstants.Claims.Audience, user.UserType.ToString());
            usertype.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim(usertype);
            return ticket;
        }

        private void AddUserClaims(GigmUser user, ClaimsIdentity identity)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            identity.AddClaim(JwtClaimTypes.PhoneNumber,  user.PhoneNumber);
            identity.AddClaim(JwtClaimTypes.Email, user.Email);
            identity.AddClaim(ClaimTypesHelper.LastName, string.IsNullOrEmpty(user.LastName) ? string.Empty : user.LastName);
            identity.AddClaim(ClaimTypesHelper.FirstName, string.IsNullOrEmpty(user.FirstName) ? string.Empty : user.FirstName);
            identity.AddClaim(ClaimTypesHelper.HasPin, $"{!string.IsNullOrWhiteSpace(user.Pin)}");

            if (!string.IsNullOrWhiteSpace(user.DialingCode))
                identity.AddClaim(ClaimTypesHelper.DialingCode, user.DialingCode);


            if (user.LastLoginDate.HasValue)
                identity.AddClaim(ClaimTypesHelper.LastLogin, user.LastLoginDate?.ToString("dd/MM/yyyy"));
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse<string>> ChangePassword(ChangePasswordViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<string>(errors: "Request is invalid, Please confirm and try again.");

                var results = await _userService.ChangePassword(model).ConfigureAwait(false);

                if (!results.Any())
                    return new ApiResponse<string>("Password changed successfully.");

                return new ApiResponse<string>(errors: results.Select(x => x.ErrorMessage).ToArray());
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Get logged-in user profile basic information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse<UserProfileViewModel>> GetBasicProfile()
        {
            return await HandleApiOperationAsync(async () =>
               {
                   var user = await _userManager.FindByIdAsync(CurrentUser.UserId.ToString()).ConfigureAwait(false);

                   return new ApiResponse<UserProfileViewModel>(

                       data: new UserProfileViewModel
                       {
                           FirstName = user.FirstName,
                           LastName = user.LastName,
                           Email = user.Email,
                           PhoneNumber = user.PhoneNumber,
                           Activated = user.Activated,
                           JoinDate = user.CreatedOnUtc.ToString(CoreConstants.DateFormat),
                           FullName = user.FullName,
                           LastLoginDate = user?.LastLoginDate?.ToString(CoreConstants.DateTimeFormat),
                           RoleName = user.UserType.ToString(),
                           Gender = user.Gender,
                           UserId = user.Id,
                           Picture = user.PictureUrl,
                           HasPin = !string.IsNullOrWhiteSpace(user.Pin)
                       });
               }).ConfigureAwait(false);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<dynamic>> Register(CustomerRegisterViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<dynamic>(errors: ListModelErrors.ToArray());

                var results = await _userService.CreateCustomerUser(model).ConfigureAwait(false);

                if (!results.Any())
                {
                    var confirmationToken = await _userService.GeneratePhoneConfirmationOTP(model.Email).ConfigureAwait(false);

                    if (confirmationToken.HasValue)
                        await _userEventService.PublishAndLogEvent(new CustomerAccountActivationIntegrationEvent(
                        model.FirstName, model.Email, model.DialingCode,
                        model.PhoneNumber, confirmationToken.Value.ToString())).ConfigureAwait(false);
                }

                return new ApiResponse<dynamic>(errors: results.Select(x => x.ErrorMessage).ToArray());
            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<dynamic>> UploadPicture([FromForm] PictureCreateViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<dynamic>(errors: ListModelErrors.ToArray());

                var (results, filepath) = await _userService.CreateProfilePicture(model).ConfigureAwait(false);

                if (!results.Any())
                    return new ApiResponse<dynamic>(filepath);

                return new ApiResponse<dynamic>(errors: results.Select(x => x.ErrorMessage).ToArray());
            }).ConfigureAwait(false);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> ConfirmAccountPhoneNumber(string username, int token)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var results = await _userService.ConfirmAccountPhoneNumber(username, token).ConfigureAwait(false);

                return new ApiResponse<string>(errors: results.Select(x => x.ErrorMessage).ToArray());
            }).ConfigureAwait(false);
        }

        [HttpGet("{usernameOrEmail}")]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> RequestPasswordReset([Required] string usernameOrEmail)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<string>(errors: ListModelErrors.ToArray());

                var (result, userOtpModel) = await _userService.GeneratePasswordResetOTP(usernameOrEmail).ConfigureAwait(false);

                if (result.Any())
                {
                    return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray(),
                        codes: ApiResponseCodes.ERROR);
                }

                await _userEventService.PublishAndLogEvent(new PasswordResetIntegrationEvent(
                        userOtpModel.UserName, userOtpModel.Email, userOtpModel.DialingCode,
                        userOtpModel.PhoneNumber, userOtpModel.Otp.ToString())).ConfigureAwait(false);

                return new ApiResponse<string>("Successfully sent OTP for password reset");
            }).ConfigureAwait(false);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> ResetPassword(PasswordResetModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.ResetPassword(model).ConfigureAwait(false);
                if (result.Any())
                {
                    return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray(),
                        codes: ApiResponseCodes.ERROR);
                }
                return new ApiResponse<string>("Successfully reset password for " + model.UsernameOrEmail);
            }).ConfigureAwait(false);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> ResendActivationOTP(string usernameOrEmail)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var user = await _userService.FindUser(usernameOrEmail).ConfigureAwait(false);

                var confirmationToken = _userService.GenerateUserOTP(user);

                await _userEventService.PublishAndLogEvent(new CustomerAccountActivationIntegrationEvent(
                    user.FirstName, user.Email, user.DialingCode,
                    user.PhoneNumber, confirmationToken.ToString())).ConfigureAwait(false);

                return new ApiResponse<string>("Successfully resent the activation OTP");
            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<string>> UpdatePin(PinCreateModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var results = await _userService.CreateOrUpdatePin(model).ConfigureAwait(false);

                if (results.Any())
                {
                    return new ApiResponse<string>(errors: results.Select(x => x.ErrorMessage).ToArray(),
                        codes: ApiResponseCodes.ERROR);
                }

                return new ApiResponse<string>("Pin updated successfully");

            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> VerifyPin(PinVerifyModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.VerifyPin(model.Pin).ConfigureAwait(false);
                return new ApiResponse<bool>(result);

            }).ConfigureAwait(false);
        }
    }
}