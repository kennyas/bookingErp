using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Infrastructure;

namespace Tornado.Shared.Extensions
{
    public  static class CurrentUserExtension
    {
        public static void AddCurrentUserPermissionHandler(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddHttpContextAccessor();
            services.AddScoped<IHttpUserService, HttpUserService>();

            //SL
            ServiceLocator.SetLocatorProvider(services.BuildServiceProvider());

        }
    }
}