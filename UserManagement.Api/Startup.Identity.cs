using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tornado.Shared.Helpers;
using Tornado.Shared.Models;
using Tornado.Shared.Settings;
using UserManagement.Core.Context;

namespace UserManagement.Api
{
    public partial class Startup
    {
        private async Task InitializeIdentityDbAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

            if (await manager.FindByClientIdAsync(ClientAppHelper.CLIENT_WEB_ID,
          cancellationToken) == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = ClientAppHelper.CLIENT_WEB_ID,
                    ClientSecret = ClientAppHelper.CLIENT_WEB_SECRET,
                    DisplayName = "Danfo Api",
                    Type = OpenIddictConstants.ClientTypes.Hybrid,
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles
                    }
                };

                await manager.CreateAsync(descriptor, cancellationToken);
            }
        }


        public void AddIdentityProvider(IServiceCollection services)
        {
            var authSettings = new AuthSettings();
            Configuration.Bind(nameof(AuthSettings), authSettings);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.SecretKey));

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.Password.RequiredLength = 6;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });

            var x509Certificate = new X509Certificate2(Path.Combine(
                          HostingEnvironment.ContentRootPath, "auth.pfx")
                      , "idsrv3test",X509KeyStorageFlags.MachineKeySet);

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<GigAuthDbContext>();
                })
                .AddServer(options =>
                {
                    options.RegisterScopes(OpenIdConnectConstants.Scopes.Email,
                        OpenIdConnectConstants.Scopes.Profile,
                        OpenIdConnectConstants.Scopes.Address,
                        OpenIdConnectConstants.Scopes.Phone,
                        OpenIddictConstants.Scopes.Roles,
                        OpenIdConnectConstants.Scopes.OfflineAccess,
                        OpenIdConnectConstants.Scopes.OpenId
                    );

                    if (!authSettings.RequireHttps)
                        options.DisableHttpsRequirement();

                    options.EnableTokenEndpoint("/api/authenticationapi/token")
                        .AllowRefreshTokenFlow()
                        .AcceptAnonymousClients()
                        .AllowPasswordFlow()
                        .SetAccessTokenLifetime(TimeSpan.FromMinutes(60))
                        .SetIdentityTokenLifetime(TimeSpan.FromMinutes(60))
                        .SetRefreshTokenLifetime(TimeSpan.FromMinutes(120))
                        .AddSigningCertificate(x509Certificate)
                        .UseJsonWebTokens();
                });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = options.Authority = authSettings.Authority;
                options.RequireHttpsMetadata = authSettings.RequireHttps;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OpenIdConnectConstants.Claims.Name,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role,
                    IssuerSigningKey = signingKey,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
        }

        public void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<GigmUser, GigmRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
           .AddEntityFrameworkStores<GigAuthDbContext>()
           .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(24);
            });
        }
    }
}