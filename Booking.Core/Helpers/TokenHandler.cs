using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Core.Helpers
{
    public class TokenHttpHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContext;

        public TokenHttpHandler(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accesstoken = _httpContext.HttpContext.Request.Headers["Authorization"].ToString();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
