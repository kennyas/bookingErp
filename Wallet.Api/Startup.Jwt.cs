using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.Settings;
using Tornado.Shared.ViewModels;
using static Tornado.Shared.Helpers.AuthConstants;

namespace Wallet.Api 
{
    public partial class Startup
    {
        public void ConfigureIdentity(IServiceCollection services)
        {
            var authSettings = new AuthSettings();
            Configuration.Bind(nameof(AuthSettings), authSettings);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.SecretKey));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(options => {
                options.DefaultScheme = OpenIdConnectConstants.Schemes.Bearer;
                options.DefaultChallengeScheme = OpenIdConnectConstants.Schemes.Bearer;

            }).AddJwtBearer(options => {

                options.Authority = authSettings.Authority;
                options.RequireHttpsMetadata = authSettings.RequireHttps;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OpenIdConnectConstants.Claims.Name,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role,
                    IssuerSigningKey = signingKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                };
            });
        }

    }
}
