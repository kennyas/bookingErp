using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Tornado.Shared.Settings;

namespace Booking.Api
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