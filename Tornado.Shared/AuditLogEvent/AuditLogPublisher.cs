using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AuditLogEvent.Interface;
using Tornado.Shared.Models;

namespace Tornado.Shared.AuditLogEvent
{
    public class AuditLogPublisher
    {
        public static async Task PublishEventAsync(HttpRequest request, IHttpUserService _currentUserService, IAuditLogEventService _auditLogEventService)
        {
            // var re = Request;
            var headers = request.Headers;
            if (headers.ContainsKey("auditLog"))
            {
                var auditLog = headers["auditLog"];
                var auditLogModel = JsonConvert.DeserializeObject<AuditLogModel>(auditLog);
                await _auditLogEventService.PublishEvent(new AuditLogIntegrationEvent(_currentUserService.GetCurrentUser().UserId, auditLogModel.Ipaddress, auditLogModel.Channel, auditLogModel.ActionType, auditLogModel.Comments, auditLogModel.ModuleName,
                                auditLogModel.parameters, auditLogModel.CreatedOn.ToString()));
            }
        }
    }
}
