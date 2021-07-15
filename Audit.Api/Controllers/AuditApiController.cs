using System.Threading.Tasks;
using Audit.Core.Events;
using Audit.Core.Models;
using Audit.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.ViewModels;

namespace Audit.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    // [ApiController]
    [AllowAnonymous]
    public class AuditApiController : BaseController
    {
        //private readonly IAuditService _auditService;
        //public AuditApiController(IAuditService auditService)
        //{
        //    _auditService = auditService;
        //}

        //[HttpPost]
        //public async Task<ApiResponse<AuditLog>> CreateAuditLog([FromBody]AuditLogIntegrationEvent model) 
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var Ipaddress = HttpContext.Connection.RemoteIpAddress.ToString();
        //        model.Ipaddress = Ipaddress;
        //        return await _auditService.CreateAuditLogAsync(model).ConfigureAwait(false);
        //    }).ConfigureAwait(false);
        //}
    }
}