using System;
using Microsoft.AspNetCore.Http;
using Tornado.Shared.Identity;

namespace Tornado.Shared.AspNetCore
{
    public class HttpUserService : IHttpUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public HttpUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public UserPrincipal GetCurrentUser()
        {
            if (_httpContext.HttpContext != null && _httpContext.HttpContext.User != null)
            {
                return new UserPrincipal(_httpContext.HttpContext.User);
            }

            throw new Exception("Current user cannot be determined");
        }
    }

    public interface IHttpUserService
    {
        UserPrincipal GetCurrentUser();
    }
}