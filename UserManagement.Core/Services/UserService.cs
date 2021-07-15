using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.Enums;
using Tornado.Shared.FileStorage;
using Tornado.Shared.Helpers;
using Tornado.Shared.Models;
using Tornado.Shared.Statics;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Enums;
using UserManagement.Core.Helpers;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;
using static Tornado.Shared.Helpers.AuthConstants;

namespace UserManagement.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<GigmUser> _userManager;
        private readonly RoleManager<GigmRole> _roleManager;

        private readonly IStaffService _staffService;
        private readonly IBusBoyService _busBoyService;
        private readonly ICaptainService _captainService;
        private readonly IDepartmentService _departmentService;
        private readonly ICustomerService _customerService;
        private readonly ITotpService _totpService;
        private readonly IHttpUserService _currentUserService;
        private readonly IPartnerService _partnerService;

        private readonly IHttpContextAccessor _httpContext;

        private readonly IUnitOfWork _uow;

        protected readonly string StorageLocation;

        private readonly IFileStorageService _fileStore;
        private readonly IFileUploadService _fileUploadService;

        private readonly List<ValidationResult> results = new List<ValidationResult>();

        public UserService(
            UserManager<GigmUser> userManager,
            RoleManager<GigmRole> roleManager,
            IStaffService staffService,
            IDepartmentService departmentService,
            ITotpService totpService,
            ICustomerService customerService,
            IHttpUserService currentUserService,
            IPartnerService partnerService,
            ICaptainService captainService,
            IBusBoyService busBoyService,
            IUnitOfWork uow,
            IFileStorageService fileStore,
            IFileUploadService fileUploadService,
            IConfiguration config,
            IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _staffService = staffService;
            _departmentService = departmentService;
            _roleManager = roleManager;
            _totpService = totpService;
            _customerService = customerService;
            _currentUserService = currentUserService;
            _partnerService = partnerService;
            _uow = uow;
            _captainService = captainService;
            _busBoyService = busBoyService;
            _fileStore = fileStore;
            _fileUploadService = fileUploadService;
            StorageLocation = config.GetValue<string>("Filestore") ?? "Files";
            _httpContext = httpContext;
        }

        public async Task<List<ValidationResult>> DeleteStaff(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.IsDeleted || user.UserType != UserType.STAFF)
            {
                results.Add(new ValidationResult("Staff not found"));
                goto exit;
            }

            if (user.Email == Defaults.AdminEmail)
            {
                results.Add(new ValidationResult("Admin user cannot be deleted"));
                goto exit;
            }

            user.IsDeleted = true;
            user.LockoutEnabled = true;

            await _userManager.UpdateAsync(user);

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> DeleteCustomer(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.IsDeleted || user.UserType != UserType.CUSTOMER)
            {
                results.Add(new ValidationResult("Customer not found"));
                goto exit;
            }

            if (user.Email == Defaults.AdminEmail)
            {
                results.Add(new ValidationResult("Admin user cannot be deleted"));
                goto exit;
            }

            user.IsDeleted = true;
            user.LockoutEnabled = true;
            await _userManager.UpdateAsync(user);

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> DeleteCaptain(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.IsDeleted || user.UserType != UserType.CAPTAIN)
            {
                results.Add(new ValidationResult("Captain not found"));
                goto exit;
            }

            if (user.Email == Defaults.AdminEmail)
            {
                results.Add(new ValidationResult("Admin user cannot be deleted"));
                goto exit;
            }

            user.IsDeleted = true;
            user.LockoutEnabled = true;
            await _userManager.UpdateAsync(user);

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> DeleteBusBoy(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.IsDeleted || user.UserType != UserType.BUSBOY)
            {
                results.Add(new ValidationResult("BusBoy not found"));
                goto exit;
            }

            if (user.Email == Defaults.AdminEmail)
            {
                results.Add(new ValidationResult("Admin user cannot be deleted"));
                goto exit;
            }

            user.IsDeleted = true;
            user.LockoutEnabled = true;
            await _userManager.UpdateAsync(user);

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> DeletePartner(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.IsDeleted || user.UserType != UserType.PARTNER)
            {
                results.Add(new ValidationResult("Partner not found"));
                goto exit;
            }

            if (user.Email == Defaults.AdminEmail)
            {
                results.Add(new ValidationResult("Admin user cannot be deleted"));
                goto exit;
            }

            user.IsDeleted = true;
            user.LockoutEnabled = true;
            await _userManager.UpdateAsync(user);

        exit:
            return results;
        }

        private async Task ReverseUserCreated(GigmUser user)
        {
            await _userManager.DeleteAsync(user);
        }

        private async Task<List<ValidationResult>> CreateIdentityUser(GigmUser user, string password)
        {
            var validationResult = new List<ValidationResult>();

            var identityResult = await _userManager.CreateAsync(user, password);

            if (!identityResult.Succeeded)
                validationResult.Add(new ValidationResult(identityResult.Errors.FirstOrDefault().Description));

            return results;
        }

        public async Task AssignDefaultPermission(GigmUser user, string role)
        {
            var permissionFromRole = PermisionProvider
                                   .GetSystemDefaultRoles()
                                   .Where(x => x.Key == role).SelectMany(x => x.Value);

            await AssignPermissionsToUser(permissionFromRole.ToList(), user);
        }

        public async Task AssignPermissionsToUser(List<Permission> permissions, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return;

            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Any())
                await _userManager.RemoveClaimsAsync(user, userClaims);

            var userNewClaims = permissions.Select(p => new Claim(p.ToString(), ((int)p).ToString())).ToList();
            _userManager.AddClaimsAsync(user, userNewClaims).Wait();
        }

        public async Task AssignPermissionsToUser(List<Permission> permissions, GigmUser user)
        {
            if (user == null)
                return;

            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Any())
                await _userManager.RemoveClaimsAsync(user, userClaims);

            var userNewClaims = permissions.Select(p => new Claim(nameof(Permission), p.ToString())).ToList();
            _userManager.AddClaimsAsync(user, userNewClaims).Wait();
        }

        public async Task<List<ValidationResult>> CreateBusBoy(SetupBusBoyViewModel userModel)
        {
            if (!IsValid(userModel))
                goto exit;

            var user = new GigmUser
            {
                UserName = userModel.Email,
                LastName = userModel.LastName,
                FirstName = userModel.FirstName,
                MiddleName = userModel.MiddleName,
                Gender = userModel.Gender,
                Email = userModel.Email,
                UserType = UserType.BUSBOY,
                ChangePasswordOnLogin = true,
                Activated = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = userModel.PhoneNumber.TrimStart('0'),
            };

            userModel.Password = Utils.GenerateRandom(6);

            var userCreationResult = await CreateIdentityUser(user, userModel.Password);

            if (userCreationResult.Any())
                goto exit;

            userModel.Id = user.Id;

            var roleName = RoleHelpers.BUS_BOY;

            var roleResult = await AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
            {
                await ReverseUserCreated(user);
                results.AddRange(roleResult.Errors.Select(x => new ValidationResult(x.Description)));

                goto exit;
            }

            await AssignDefaultPermission(user, roleName);

            try
            {
                var busboy = new BusBoy
                {
                    UserId = user.Id,
                    CreatedBy = user.Id,
                    Status = userModel.Status ?? BusBoyStatus.Idle,
                };

                _busBoyService.Add(busboy);
            }
            catch
            {
                await ReverseUserCreated(user);
                throw;
            }

        exit:
            return results;
        }

        protected bool IsValid<T>(T entity)
        {
            return Validator.TryValidateObject(entity, new ValidationContext(entity, null, null), results, false);
        }

        public async Task<List<ValidationResult>> CreateStaff(SetupStaffViewModel userModel)
        {
            if (!IsValid(userModel))
                goto exit;

            var user = new GigmUser
            {
                UserName = userModel.Email,
                LastName = userModel.LastName,
                FirstName = userModel.FirstName,
                MiddleName = userModel.MiddleName,
                Gender = userModel.Gender,
                Email = userModel.Email,
                UserType = UserType.STAFF,
                ChangePasswordOnLogin = true,
                Activated = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Unit = userModel.Unit,
                PhoneNumber = userModel.PhoneNumber.TrimStart('0'),
                DialingCode = userModel.DialingCode,
            };

            userModel.Password = Utils.GenerateRandom(6);

            var userCreationResult = await CreateIdentityUser(user, userModel.Password);

            if (userCreationResult.Any())
                goto exit;

            userModel.Id = user.Id;

            var roleName = RoleHelpers.STAFF;

            var roleResult = await AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                results.AddRange(roleResult.Errors.Select(x => new ValidationResult(x.Description)));
                goto exit;
            }

            await AssignDefaultPermission(user, roleName);

            try
            {
                if (Guid.TryParse(userModel.DepartmentId, out Guid departmentId) && departmentId != Guid.Empty)
                {
                    var departmentUnit = _departmentService.GetById(departmentId);

                    if (departmentUnit is null)
                    {
                        results.Add(new ValidationResult("Selected department unit couldn't be found."));
                        goto exit;
                    }
                }
                if (results.Any())
                {
                    await ReverseUserCreated(user);
                    goto exit;
                }

                var staff = new Staff
                {
                    EmployeeCode = userModel.EmployeeCode,
                    UserId = user.Id,
                    CreatedBy = user.Id,
                    DepartmentId = departmentId,
                };

                _staffService.Add(staff);
            }
            catch
            {
                await ReverseUserCreated(user);
                throw;
            }

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> CreateCaptain(SetupCaptainViewModel userModel)
        {
            if (!IsValid(userModel))
                goto exit;

            var user = new GigmUser
            {
                UserName = userModel.Email,
                LastName = userModel.LastName,
                FirstName = userModel.FirstName,
                MiddleName = userModel.MiddleName,
                Gender = userModel.Gender,
                Email = userModel.Email,
                UserType = UserType.CAPTAIN,
                ChangePasswordOnLogin = true,
                Activated = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = userModel.PhoneNumber.TrimStart('0'),
                DialingCode = userModel.DialingCode
            };

            userModel.Password = Utils.GenerateRandom(6);

            var userCreationResult = await CreateIdentityUser(user, userModel.Password);

            if (userCreationResult.Any())
                goto exit;

            userModel.Id = user.Id;

            var roleName = RoleHelpers.CAPTAIN;

            var roleResult = await AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
            {
                await ReverseUserCreated(user);
                results.AddRange(roleResult.Errors.Select(x => new ValidationResult(x.Description)));
                goto exit;
            }

            await AssignDefaultPermission(user, roleName);

            try
            {
                var captain = new Captain
                {
                    UserId = user.Id,
                    CreatedBy = user.Id,
                    EmployeeCode = userModel.EmployeeCode,
                    Status = userModel.Status ?? CaptainStatus.Idle,
                };

                _captainService.Add(captain);
            }
            catch
            {
                await ReverseUserCreated(user);
                throw;
            }

        exit:
            return results;
        }

        private async Task<IdentityResult> AddToRoleAsync(GigmUser user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(
                                    new GigmRole()
                                    {
                                        Name = roleName
                                    });

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            return roleResult;
        }

        public async Task<List<ValidationResult>> CreatePartner(SetupPartnerViewModel userModel)
        {
            if (!IsValid(userModel))
                goto exit;

            var user = new GigmUser
            {
                UserName = userModel.Email,
                LastName = userModel.LastName,
                FirstName = userModel.FirstName,
                MiddleName = userModel.MiddleName,
                Gender = userModel.Gender,
                Email = userModel.Email,
                UserType = UserType.PARTNER,
                ChangePasswordOnLogin = true,
                Activated = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = userModel.PhoneNumber.TrimStart('0'),
                DialingCode = userModel.DialingCode
            };

            userModel.Password = Utils.GenerateRandom(6);

            var userCreationResult = await CreateIdentityUser(user, userModel.Password);

            if (userCreationResult.Any())
                goto exit;

            userModel.Id = user.Id;

            var roleName = UserRole.PARTNER.ToString();

            var roleResult = await AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
            {
                await ReverseUserCreated(user);
                results.AddRange(roleResult.Errors.Select(x => new ValidationResult(x.Description)));
                goto exit;
            }

            await AssignDefaultPermission(user, roleName);

            if (results.Any())
            {
                await ReverseUserCreated(user);
                goto exit;

            }

            try
            {
                var partner = new Partner
                {
                    UserId = user.Id,
                    CreatedBy = user.Id,
                    PartnerAddress = userModel.PartnerAddress,
                    PartnerEmail = userModel.PartnerEmail,
                    PartnerPhoneNumber = userModel.PartnerPhoneNumber,
                };

                _partnerService.Add(partner);
            }
            catch
            {
                await ReverseUserCreated(user);
                throw;
            }

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> CreateCustomerUser(CustomerRegisterViewModel model)
        {
            if (!IsValid(model))
                goto exit;

            var user = new GigmUser
            {
                UserName = model.PhoneNumber,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Gender = model.Gender,
                Email = model.Email,
                UserType = UserType.CUSTOMER,
                PhoneNumber = model.PhoneNumber.TrimStart('0'),
                DialingCode = model.DialingCode,
                CreatedOnUtc = Clock.Now
            };

            var accountResult = await _userManager.CreateAsync(user, model.Password);
            if (!accountResult.Succeeded)
            {
                results.AddRange(accountResult.Errors.Select(x => new ValidationResult(x.Description)));
                goto exit;
            }

            var roleName = RoleHelpers.CUSTOMER;

            var roleResult = await AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
            {
                await ReverseUserCreated(user);
                results.AddRange(roleResult.Errors.Select(x => new ValidationResult(x.Description)));
                goto exit;
            }

            await AssignDefaultPermission(user, roleName);
            try
            {
                _customerService.Add(new Customer
                {
                    UserId = user.Id,
                });

                model.Id = user.Id;
            }
            catch
            {
                await ReverseUserCreated(user);
                throw;
            }

        exit:
            return results;
        }

        public async Task<string> GenerateEmailConfirmationToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.EmailConfirmed)
                return string.Empty;

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<List<ValidationResult>> ConfirmAccountEmail(string email, string confirmationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                results.Add(new ValidationResult("Account not found."));
                goto exit;
            }

            if (user.EmailConfirmed)
            {
                results.Add(new ValidationResult("Email already confirmed!"));
                goto exit;
            }

            var confirmationResult = await _userManager.ConfirmEmailAsync(user, confirmationToken);

            if (!confirmationResult.Succeeded)
                results.AddRange(confirmationResult.Errors.Select(x => new ValidationResult(x.Description)));

            exit:
            return results;
        }

        public async Task<List<ValidationResult>> ConfirmAccountPhoneNumber(string userName, int Otp)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null || user.IsDeleted)
            {
                results.Add(new ValidationResult("Account not found."));
                goto exit;
            }

            if (user.PhoneNumberConfirmed)
            {
                results.Add(new ValidationResult("Phone Number already confirmed!"));
                goto exit;
            }

            if (!VerifyUserOTP(user, Otp))
            {
                results.Add(new ValidationResult("Invalid OTP supplied. Please try again."));
                goto exit;
            }

            user.PhoneNumberConfirmed = true;
            user.Activated = true;

            await _userManager.UpdateAsync(user);

        exit:
            return results;
        }

        public async Task<int?> GeneratePhoneConfirmationOTP(string userNameOrEmail)
        {
            var user = await _userManager.FindByEmailAsync(userNameOrEmail)
                    ?? await _userManager.FindByNameAsync(userNameOrEmail);

            if (user is null || (string.IsNullOrEmpty(user.PhoneNumber) && string.IsNullOrEmpty(user.Email)) || user.Activated)
                return null;

            return GenerateUserOTP(user);
        }

        public int GenerateUserOTP(GigmUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException("User not found");
            }

            return _totpService.Generate(user.UserName);
        }

        public bool VerifyUserOTP(GigmUser user, int otp)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return _totpService.Verify(otp, user.UserName);
        }

        public async Task<GigmUser> FindUser(string usernameOrEmail)
        {
            var user = await _userManager.FindByEmailAsync(usernameOrEmail)
                    ?? await _userManager.FindByNameAsync(usernameOrEmail);

            return user;
        }

        public Task<List<StaffListViewModel>> GetAllStaff(BaseSearchViewModel model, out int totalCount)
        {
            if (!model.PageIndex.HasValue || model.PageIndex.Value < CoreConstants.PageIndex)
                model.PageIndex = CoreConstants.PageIndex;

            if (!model.PageTotal.HasValue || model.PageTotal.Value < CoreConstants.PageIndex)
                model.PageTotal = CoreConstants.PageTotal;

            var query = _customerService.SqlQuery<StaffListViewDto>(@"Select usr.Id,
                        usr.FirstName, usr.LastName, usr.Email,
                        usr.PhoneNumber,-- s.EmployeeCode Code, usr.DialingCode,
                        usr.LastLoginDate, usr.UserName, --usr.Unit,
                        ISNULL(COUNT(usr.Id) over(), 0) TotalCount

                        from GigUser usr
                        left join Staff s on s.UserId=usr.id
                        left join Department d on d.Id=s.DepartmentId
                        left join GigUserRole usRrol on usRrol.UserId=usr.id
                        left join GigRole gigrol on usRrol.RoleId=gigrol.Id
                        where usr.IsDeleted=0 and usr.UserType=0
                        AND ( (@p0 IS NULL) 
    						  OR ( usr.[FirstName] LIKE '%' + @p0 + '%'
    								  OR usr.[LastName] LIKE '%' + @p0 + '%'
									  OR usr.[Email] LIKE '%' + @p0 + '%'
									  OR usr.[UserName] LIKE '%' + @p0 + '%'
									  OR usr.[PhoneNumber] LIKE '%' + @p0 + '%'
									  OR d.[Name] LIKE '%' + @p0 + '%'
									  OR s.[EmployeeCode] LIKE '%' + @p0 + '%'))

                        ORDER  BY usr.Id desc
    					OFFSET ((@p1-1) * @p2) ROWS FETCH NEXT @p2 ROWS ONLY", model.Keyword, model.PageIndex, model.PageTotal);

            totalCount = query.FirstOrDefault()?.TotalCount ?? 0;

            return Task.FromResult(query.Select(x => new StaffListViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                LastLoginDate = x.LastLoginDate?.ToShortDateString(),
                UserName = x.UserName,
            }).ToList());
        }

        public Task<List<CaptainListViewModel>> GetAllCaptain(BaseSearchViewModel model, out int totalCount)
        {
            if (!model.PageIndex.HasValue || model.PageIndex.Value < CoreConstants.PageIndex)
                model.PageIndex = CoreConstants.PageIndex;

            if (!model.PageTotal.HasValue || model.PageTotal.Value < CoreConstants.PageIndex)
                model.PageTotal = CoreConstants.PageTotal;

            var query = from user in _userManager.Users
                        where !user.IsDeleted
                        && (string.IsNullOrWhiteSpace(model.Keyword) ||
                   (user.FirstName.Contains(model.Keyword) ||
                       user.LastName.Contains(model.Keyword) ||
                       user.Email.Contains(model.Keyword) ||
                       user.PhoneNumber.Contains(model.Keyword)))
                       && (user.UserType == UserType.CAPTAIN)
                        select new CaptainListViewModel
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            UserName = user.UserName,
                            LastLoginDate = user.LastLoginDate.ToString()
                        };

            totalCount = query.Count();

            return query.AsNoTracking()
                               .Paginate(model.PageIndex.Value, model.PageTotal.Value)
                               .ToListAsync();
        }

        public Task<List<BusBoyListViewModel>> GetAllBusBoy(BaseSearchViewModel model, out int totalCount)
        {
            if (!model.PageIndex.HasValue || model.PageIndex.Value < CoreConstants.PageIndex)
                model.PageIndex = CoreConstants.PageIndex;

            if (!model.PageTotal.HasValue || model.PageTotal.Value < CoreConstants.PageIndex)
                model.PageTotal = CoreConstants.PageTotal;

            var query = from user in _userManager.Users
                        where !user.IsDeleted
                        && (string.IsNullOrWhiteSpace(model.Keyword) ||
                   (user.FirstName.Contains(model.Keyword) ||
                       user.LastName.Contains(model.Keyword) ||
                       user.Email.Contains(model.Keyword) ||
                       user.PhoneNumber.Contains(model.Keyword)))
                       && (user.UserType == UserType.BUSBOY)
                        select new BusBoyListViewModel
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            UserName = user.UserName,
                            LastLoginDate = user.LastLoginDate.Value.ToString(CoreConstants.DateFormat),
                        };

            totalCount = query.Count();

            return query.AsNoTracking()
                               .Paginate(model.PageIndex.Value, model.PageTotal.Value)
                               .ToListAsync();
        }

        public Task<List<CustomerListViewModel>> GetAllCustomer(BaseSearchViewModel model, out int totalCount)
        {
            if (!model.PageIndex.HasValue || model.PageIndex.Value < CoreConstants.PageIndex)
                model.PageIndex = CoreConstants.PageIndex;

            if (!model.PageTotal.HasValue || model.PageTotal.Value < CoreConstants.PageIndex)
                model.PageTotal = CoreConstants.PageTotal;

            var users = _userManager.Users;

            var query = from user in users

                        where !user.IsDeleted
                        && (string.IsNullOrWhiteSpace(model.Keyword) ||
                   (user.FirstName.Contains(model.Keyword) ||
                       user.LastName.Contains(model.Keyword) ||
                       user.Email.Contains(model.Keyword) ||
                       user.PhoneNumber.Contains(model.Keyword)))
                       && (user.UserType == UserType.CUSTOMER)
                        select new CustomerListViewModel
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            UserName = user.UserName,
                            LastLoginDate = user.LastLoginDate.ToString(),
                        };

            totalCount = query.Count();

            return query.AsNoTracking()
                               .Paginate(model.PageIndex.Value, model.PageTotal.Value)
                               .ToListAsync();
        }

        public Task<List<PartnerListViewModel>> GetAllPartner(BaseSearchViewModel model, out int totalCount)
        {
            if (!model.PageIndex.HasValue || model.PageIndex.Value < CoreConstants.PageIndex)
                model.PageIndex = CoreConstants.PageIndex;

            if (!model.PageTotal.HasValue || model.PageTotal.Value < CoreConstants.PageIndex)
                model.PageTotal = CoreConstants.PageTotal;

            var query = _partnerService.SqlQuery<PartnerListViewDto>(@"
                        Select usr.Id, isnull(Count(usr.Id) over(),0) TotalCount,
                        usr.FirstName, usr.LastName, usr.Email,
                        usr.PhoneNumber, usr.UserName, usr.LastLoginDate

                        from GigUser usr
                        left join [Partner] p on p.UserId=usr.Id
                        left join GigUserRole usRrol on usRrol.UserId=usr.id
                        left join GigRole gigrol on usRrol.RoleId=gigrol.Id
                        where usr.IsDeleted=0 and usr.UserType=1
                        AND ( (@p0 IS NULL) 
    						  OR ( usr.[FirstName] LIKE '%' + @p0 + '%'
    								  OR usr.[LastName] LIKE '%' + @p0 + '%'
									  OR usr.[Email] LIKE '%' + @p0 + '%'
									  OR usr.[UserName] LIKE '%' + @p0 + '%'
									  OR usr.[PhoneNumber] LIKE '%' + @p0 + '%'))

                        ORDER  BY usr.Id desc
    					OFFSET ((@p1-1) * @p2) ROWS FETCH NEXT @p2 ROWS ONLY", model.Keyword, model.PageIndex, model.PageTotal);

            totalCount = query.FirstOrDefault()?.TotalCount ?? 0;

            return Task.FromResult(query.Select(user => new PartnerListViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                LastLoginDate = user.LastLoginDate?.ToString(CoreConstants.DateFormat),
            }).ToList());
        }

        public Task<PartnerViewModel> GetPartnerById(string userId)
        {
            return Task.FromResult(_uow.Repository<Partner>().SqlQuery<PartnerViewDto>(@"Select usr.Id,
                        usr.FirstName, usr.LastName, usr.Email, usr.PhoneNumber,
                        usr.Gender,usr.LastLoginDate, usr.MiddleName, usr.UserName,
                        usr.PhoneNumber, pat.Id as PartnerId,
                        pat.PartnerAddress, pat.[PartnerPhoneNumber], pat.[PartnerEmail],
                        gigrol.[Name] [Role], usr.DialingCode

                        from GigUser usr
                        left join [Partner] pat on pat.userId=usr.Id
                        left join GigUserRole usRrol on usRrol.UserId=usr.id
                        left join GigRole gigrol on usRrol.RoleId=gigrol.Id
                        where usr.IsDeleted=0 and usr.Id=@p0 and usr.UserType=1", userId)
                .Select(x => new PartnerViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    GenderTitle = x.Gender.ToString(),
                    Gender = (int)x.Gender,
                    LastLoginDate = x.LastLoginDate?.ToShortDateString(),
                    MiddleName = x.MiddleName,
                    PartnerAddress = x.PartnerAddress,
                    PartnerEmail = x.PartnerEmail,
                    PartnerPhoneNumber = x.PartnerPhoneNumber,
                    UserName = x.UserName,
                    Role = x.Role,
                    PartnerId = x.PartnerId,
                    DialingCode = x.DialingCode
                }).FirstOrDefault());
        }

        public Task<StaffViewModel> GetStaffById(string userId)
        {
            return Task.FromResult(_uow.Repository<Customer>().SqlQuery<StaffListViewDto>(@"Select usr.Id,
                        usr.FirstName, usr.LastName, usr.Email, usr.PhoneNumber,
                        usr.Gender,usr.LastLoginDate, usr.MiddleName, usr.UserName,
                        usr.PhoneNumber, usr.Unit, d.[Name] Department, s.EmployeeCode,
                        gigrol.[Name] [Role], usr.DialingCode

                        from GigUser usr
                        left join Staff s on s.UserId=usr.id
                        left join Department d on d.Id=s.DepartmentId
                        left join GigUserRole usRrol on usRrol.UserId=usr.id
                        left join GigRole gigrol on usRrol.RoleId=gigrol.Id
                        where usr.IsDeleted=0 and usr.Id=@p0 and usr.UserType=0", userId)
                .Select(x => new StaffViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    GenderTitle = x.Gender.ToString(),
                    Gender = (int)x.Gender,
                    LastLoginDate = x.LastLoginDate?.ToString(CoreConstants.DateFormat),
                    MiddleName = x.MiddleName,
                    UserName = x.UserName,
                    Role = x.Role,
                    Department = x.Department,
                    Unit = x.Unit,
                    DialingCode = x.DialingCode,
                    EmployeeCode = x.EmployeeCode
                }).FirstOrDefault());
        }

        public Task<CaptainViewModel> GetCaptainById(string userId)
        {
            return Task.FromResult(_uow.Repository<Captain>().SqlQuery<CaptainViewDto>(@"Select usr.Id,
                        usr.FirstName, usr.LastName, usr.Email, usr.PhoneNumber,
                        usr.Gender, usr.LastLoginDate, usr.MiddleName, usr.UserName,
                        usr.PhoneNumber, gigrol.[Name] [Role], usr.DialingCode,c.EmployeeCode,c.Status

                        from GigUser usr
						join Captain c on c.UserId=usr.Id
                        left join GigUserRole usRrol on usRrol.UserId=usr.id
                        left join GigRole gigrol on usRrol.RoleId=gigrol.Id
                        where usr.IsDeleted=0 and usr.Id=@p0 and usr.UserType=3 and c.isdeleted=0", userId)
                .Select(x => new CaptainViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    GenderTitle = x.Gender.ToString(),
                    Gender = (int)x.Gender,
                    LastLoginDate = x.LastLoginDate?.ToShortDateString(),
                    MiddleName = x.MiddleName,
                    UserName = x.UserName,
                    Role = x.Role,
                    DialingCode = x.DialingCode,
                    Status = x.Status,
                    EmployeeCode = x.EmployeeCode
                }).FirstOrDefault());
        }

        public Task<BusBoyViewModel> GetBusBoyById(string userId)
        {
            return Task.FromResult(_uow.Repository<Customer>().SqlQuery<BusBoyViewDto>(@"Select usr.Id,
                        usr.FirstName, usr.LastName, usr.Email, usr.PhoneNumber,
                        usr.Gender, usr.LastLoginDate, usr.MiddleName, usr.UserName,
                        usr.PhoneNumber, gigrol.[Name] [Role], usr.DialingCode, b.[Status]

                        from GigUser usr
                        join Busboy b on b.UserId=usr.Id
                        left join GigUserRole usRrol on usRrol.UserId=usr.id
                        left join GigRole gigrol on usRrol.RoleId=gigrol.Id
                        where usr.IsDeleted=0 and usr.Id=@p0 and usr.UserType=4", userId)
                .Select(x => new BusBoyViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    GenderTitle = x.Gender.ToString(),
                    Gender = (int)x.Gender,
                    LastLoginDate = x.LastLoginDate?.ToShortDateString(),
                    MiddleName = x.MiddleName,
                    UserName = x.UserName,
                    Role = x.Role,
                    DialingCode = x.DialingCode,
                    Status = x.Status
                }).FirstOrDefault());
        }

        public Task<UserViewModel> GetCustomerById(string userId)
        {
            return Task.FromResult(_uow.Repository<Customer>().SqlQuery<UserViewDto>(@"Select usr.Id,
                        usr.FirstName, usr.LastName, usr.Email, usr.PhoneNumber,
                        usr.Gender, usr.LastLoginDate, usr.MiddleName, usr.UserName,
                        usr.PhoneNumber, gigrol.[Name] [Role], usr.DialingCode

                        from GigUser usr
                        left join GigUserRole usRrol on usRrol.UserId=usr.id
                        left join GigRole gigrol on usRrol.RoleId=gigrol.Id
                        where usr.IsDeleted=0 and usr.Id=@p0 and usr.UserType=2", userId)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    GenderTitle = x.Gender.ToString(),
                    Gender = (int)x.Gender,
                    LastLoginDate = x.LastLoginDate?.ToShortDateString(),
                    MiddleName = x.MiddleName,
                    UserName = x.UserName,
                    Role = x.Role,
                    DialingCode = x.DialingCode
                }).FirstOrDefault());
        }

        public async Task<List<ValidationResult>> UpdateStaffUser(EditStaffViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            if (user is null || user.IsDeleted && user.UserType != UserType.STAFF)
            {
                results.Add(new ValidationResult("User NotFound or Deleted."));
                goto exit;
            }

            await UpdateUserInfo(userViewModel, user);

            await UpdateStaff(userViewModel, user);

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> UpdateCaptainUser(EditCaptainViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            if (user is null || user.IsDeleted || user.UserType != UserType.CAPTAIN)
            {
                results.Add(new ValidationResult("User NotFound or Deleted."));
                goto exit;
            }

            await UpdateUserInfo(userViewModel, user);

            await UpdateCaptain(userViewModel, user);

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> UpdateBusBoyUser(EditBusBoyViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            if (user is null || user.IsDeleted || user.UserType != UserType.BUSBOY)
            {
                results.Add(new ValidationResult("User NotFound or Deleted."));
                goto exit;
            }

            await UpdateUserInfo(userViewModel, user);

            await UpdateBusBoy(userViewModel, user);

        exit:
            return results;
        }

        private async Task UpdateBusBoy(EditBusBoyViewModel userViewModel, GigmUser user)
        {
            var busBoy = _busBoyService.FirstOrDefault(x => x.UserId == user.Id);

            if (busBoy != null)
            {
                if (userViewModel.Status != null && busBoy.Status != userViewModel.Status)
                {
                    busBoy.Status = userViewModel.Status.Value;
                    await _busBoyService.UpdateAsync(busBoy);
                }
            }
        }

        private async Task UpdateCaptain(EditCaptainViewModel userViewModel, GigmUser user)
        {
            var captain = _captainService.FirstOrDefault(x => x.UserId == user.Id);

            if (captain != null)
            {
                if (userViewModel.Status != null && captain.Status != userViewModel.Status)
                {
                    captain.Status = userViewModel.Status.Value;
                    await _captainService.UpdateAsync(captain);
                }
            }
        }

        public async Task<List<ValidationResult>> UpdatePartnerUser(EditPartnerViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            if (user is null || user.IsDeleted || user.UserType != UserType.PARTNER)
            {
                results.Add(new ValidationResult("User NotFound or Deleted."));
                goto exit;
            }

            await UpdateUserInfo(userViewModel, user);

            await UpdatePartner(userViewModel, user);

        exit:
            return results;
        }

        public async Task<List<ValidationResult>> UpdateCustomerUser(EditUserViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            if (user is null || user.IsDeleted || user.UserType != UserType.CUSTOMER)
            {
                results.Add(new ValidationResult("User NotFound or Deleted."));
                goto exit;
            }

            await UpdateUserInfo(userViewModel, user);

        exit:
            return results;
        }

        private async Task UpdatePartner(EditPartnerViewModel userViewModel, GigmUser user)
        {
            var partner = _partnerService.FirstOrDefault(x => x.UserId == user.Id);

            if (partner != null)
            {
                if (!string.IsNullOrWhiteSpace(userViewModel.PartnerAddress) && !string.Equals(partner.PartnerAddress, userViewModel.PartnerAddress))
                    partner.PartnerAddress = userViewModel.PartnerAddress;

                if (!string.IsNullOrWhiteSpace(userViewModel.PartnerEmail) && !string.Equals(partner.PartnerEmail, userViewModel.PartnerEmail))
                    partner.PartnerEmail = userViewModel.PartnerEmail;

                if (!string.IsNullOrWhiteSpace(userViewModel.PartnerPhoneNumber) && !string.Equals(partner.PartnerPhoneNumber, userViewModel.PartnerPhoneNumber))
                    partner.PartnerPhoneNumber = userViewModel.PartnerPhoneNumber;

                await _partnerService.UpdateAsync(partner);
            }
        }

        private async Task UpdateStaff(EditStaffViewModel userViewModel, GigmUser user)
        {
            if (!string.IsNullOrWhiteSpace(userViewModel.Unit) && !string.Equals(user.Unit, userViewModel.Unit))
            {
                user.Unit = userViewModel.Unit;
                await _userManager.UpdateAsync(user);
            }

            var staff = _staffService.SingleOrDefault(x => x.UserId == user.Id);

            if (staff != null)
            {
                if (Guid.TryParse(userViewModel.DepartmentId, out Guid departmentId) && departmentId != Guid.Empty)
                {
                    if (!staff.DepartmentId.Equals(departmentId))
                    {
                        var departmentUnit = _departmentService.GetById(departmentId);

                        if (departmentUnit != null)
                            staff.DepartmentId = departmentUnit.Id;
                    }
                }

                await _staffService.UpdateAsync(staff);
            }
        }

        private async Task UpdateUserInfo(EditUserViewModel userViewModel, GigmUser user)
        {
            if (!string.Equals(user.LastName, userViewModel.LastName))
                user.LastName = userViewModel.LastName;

            if (!string.Equals(user.FirstName, userViewModel.FirstName))
                user.FirstName = userViewModel.FirstName;

            if (!string.Equals(user.PhoneNumber, userViewModel.PhoneNumber))
            {
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.PhoneNumberConfirmed = false;
            }

            if (!string.Equals(user.Email, userViewModel.Email))
            {
                user.Email = userViewModel.Email;
                user.EmailConfirmed = false;
            }


            if (!string.IsNullOrWhiteSpace(userViewModel.MiddleName) && !string.Equals(user.MiddleName, userViewModel.MiddleName))
                user.MiddleName = userViewModel.MiddleName;

            user.Gender = userViewModel.Gender;

            await _userManager.UpdateAsync(user);
        }

        public async Task<List<ValidationResult>> ChangePassword(ChangePasswordViewModel model)
        {
            if (!IsValid(model))
                goto exit;

            var principal = _currentUserService.GetCurrentUser();

            if (principal is null)
            {
                results.Add(new ValidationResult("User could not be determinned"));
                goto exit;
            }
            var user = await _userManager.FindByIdAsync(principal.UserId.ToString());

            if (user is null)
            {
                results.Add(new ValidationResult("User notfound"));
                goto exit;
            }

            if (!await _userManager.CheckPasswordAsync(user, model.NewPassword))
                results.Add(new ValidationResult("Your password couldn't be changed"));

            var changeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!changeResult.Succeeded)
                results.AddRange(changeResult.Errors.Select(x => new ValidationResult(x.Description)));

            exit:
            return results;
        }

        public async Task<(List<ValidationResult> validationResults, string path)>
            CreateProfilePicture(PictureCreateViewModel model)
        {
            if (!IsValid(model))
                goto exit;

            var user = await _userManager.FindByIdAsync(_currentUserService.GetCurrentUser().UserId.ToString());

            if (user is null || user.IsDeleted || !user.Activated)
                results.Add(new ValidationResult("Invalid user"));

            var filePath = UploadStream(model.File.OpenReadStream(), model.File.FileName);

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var newpath = RenametoLocalFileTime(filePath);

                var fileInfo = _fileStore.GetFile(newpath);

                await _fileUploadService.AddAsync(new FileUpload
                {
                    CreatedBy = user.Id,
                    Size = fileInfo.FileSizeWithSuffix(),
                    Path = fileInfo.Name,
                    Name = model.File.FileName
                });

                user.PictureUrl = AbsolutePath($"{StorageLocation }/{fileInfo.Name}");
                await _userManager.UpdateAsync(user);

                return (results, user.PictureUrl);
            }
            else
            {
                results.Add(new ValidationResult("Your file couldn't be uploaded. Try again or contact Support."));
            }

        exit:
            return (results, string.Empty);
        }

        private string UploadStream(Stream stream, string filePath)
        {
            if (_fileStore.FileExists(filePath))
            {
                filePath = _fileStore.RenameDuplicateFile(filePath);
            }

            if (!_fileStore.TrySaveStream(filePath, stream))
                filePath = null;

            return filePath;
        }

        string RenametoLocalFileTime(string filePath)
        {
            var newPath = filePath.Replace(Path.GetFileNameWithoutExtension(filePath),
                         Clock.Now.ToFileTime().ToString());

            _fileStore.RenameFile(filePath, newPath);

            return newPath;
        }

        public string AbsolutePath(string contentPath)
        {
            var request = _httpContext.HttpContext.Request;
            return Path.Combine($"{request.Scheme}://{request.Host.Value}/{request.PathBase}", contentPath);
        }

        public async Task<(List<ValidationResult> errors, UserResetPasswordModel userOtpModel)>
            GeneratePasswordResetOTP(string usernameOrEmail)
        {
            var user = await FindUser(usernameOrEmail);

            if (user is null || user.IsDeleted || !user.Activated)
                results.Add(new ValidationResult("Invalid user"));

            var otp = GenerateUserOTP(user);

            return (results, new UserResetPasswordModel
            {
                Otp = otp,
                DialingCode = user.DialingCode,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            });
        }

        public async Task<List<ValidationResult>> ResetPassword(PasswordResetModel model)
        {
            var user = await FindUser(model.UsernameOrEmail);

            if (user is null || user.IsDeleted)
            {
                results.Add(new ValidationResult("User not found"));
                goto exit;
            }
            if (VerifyUserOTP(user, model.Otp))
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);

                if (!resetResult.Succeeded)
                {
                    results.AddRange(resetResult.Errors.Select(x => new ValidationResult(x.Description)));
                    goto exit;
                }
            }
            else
            {
                results.Add(new ValidationResult("OTP is invalid"));
                goto exit;
            }

        exit:
            return results;
        }

        public async Task<bool> VerifyPin(string pin)
        {
            var principal = _currentUserService.GetCurrentUser();

            if (principal is null)
            {
                throw new Exception("User could not be determined");
            }

            var user = await _userManager.FindByIdAsync(principal.UserId.ToString());

            if (user is null)
            {
                throw new Exception("User not found");
            }

            if (!string.IsNullOrWhiteSpace(user.Pin))
            {
                var compareResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.Pin, pin);

                if (compareResult == PasswordVerificationResult.Success
                    || compareResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<ValidationResult>> CreateOrUpdatePin(PinCreateModel model)
        {
            if (!IsValid(model))
                goto exit;

            var principal = _currentUserService.GetCurrentUser();

            if (principal is null)
            {
                results.Add(new ValidationResult("User could not be determined"));
                goto exit;
            }

            var user = await _userManager.FindByIdAsync(principal.UserId.ToString());

            if (user is null)
            {
                results.Add(new ValidationResult("User notfound"));
                goto exit;
            }

            if (!string.IsNullOrWhiteSpace(user.Pin))
            {
                if (string.IsNullOrWhiteSpace(model.OldPin))
                {
                    results.Add(new ValidationResult("Old pin is required"));
                    goto exit;
                }

                var compareResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.Pin, model.OldPin);

                if (compareResult == PasswordVerificationResult.Success
                    || compareResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.Pin = _userManager.PasswordHasher.HashPassword(user, model.NewPin);
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    results.Add(new ValidationResult("Old pin is invalid"));
                }
            }
            else
            {
                user.Pin = _userManager.PasswordHasher.HashPassword(user, model.NewPin);
                await _userManager.UpdateAsync(user);
            }

        exit:
            return results;
        }
    }
}