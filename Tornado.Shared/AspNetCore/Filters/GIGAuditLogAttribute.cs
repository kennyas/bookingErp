using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Tornado.Shared.Models;
using Tornado.Shared.Timing;


namespace Tornado.Shared.AspNetCore.Filters
{
    public class GIGAuditLogAttribute : ActionFilterAttribute
    {
        // private readonly AuditLogEventService _auditLogEvent;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // our code before action executes              
            try
            {
                var obj = context.ActionArguments;
                var values = obj.ContainsKey("request") ? obj["Value"] : obj.FirstOrDefault();
                var jsonstring = values != null ? JsonConvert.SerializeObject(values) : context.HttpContext.Request.QueryString.ToString();
                var resultJson = JObject.Parse(jsonstring);

                var a = new AuditLogModel
                {
                    ActionType = context.RouteData.Values["action"].ToString(),
                    Ipaddress = context.HttpContext.Connection.RemoteIpAddress.ToString() == "::1" ? "127.0.0.1" : context.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Channel = context.HttpContext.Request.Scheme, //context.HttpContext.User.Identity.Name,
                    CreatedOn = Clock.Now,
                    IsDeleted = false,
                    ModuleName = context.RouteData.Values["controller"].ToString() + "/" + context.RouteData.Values["action"].ToString(),
                    Comments = "OnActionExecuting " + " " + context.RouteData.Values["action"].ToString(),
                    parameters = resultJson["Value"].ToString()
                };
                var auditLog = JsonConvert.SerializeObject(a);
                context.HttpContext.Request.Headers.Add("auditLog", auditLog);
                base.OnActionExecuting(context);

                //publish event
                //  _auditLogEvent.PublishEvent(new AuditLogIntegrationEvent(new Guid(), a.Ipaddress, a.Channel, a.ActionType, a.Comments,
                //                                a.ModuleName, "OnActionExecuting", Clock.Now.ToString()));
            }
            catch //(Exception ex)
            {
                // throw ex;
            }
        }

        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    // our code after action executes
        //    string response = string.Empty;
        //    try
        //    {
        //        var content = context.HttpContext.Response;
        //        using (var reader = new StreamReader(content.Body))
        //        {
        //            response = reader.ReadToEnd();
        //        }

        //        var a = new AuditLogModel
        //        {
        //            ActionType = context.RouteData.Values["action"].ToString(),
        //            Ipaddress = context.HttpContext.Connection.RemoteIpAddress.ToString() == "::1" ? "127.0.0.1" : context.HttpContext.Connection.RemoteIpAddress.ToString(),
        //            Channel = context.HttpContext.Request.Scheme,//context.HttpContext.User.Identity.Name,
        //            CreatedOn = Clock.Now,
        //            IsDeleted = false,
        //            ModuleName = context.RouteData.Values["controller"].ToString() + "/" + context.RouteData.Values["action"].ToString(),
        //            Comments = "OnActionExecuted " + " " + context.RouteData.Values["action"].ToString(),
        //            parameters = response
        //        };

        //        var auditLog = JsonConvert.SerializeObject(a);
        //        context.HttpContext.Request.Headers.Add("auditLog", auditLog);
        //        base.OnActionExecuted(context);

        //        //publish event
        //        //  _auditLogEvent.PublishEvent(new AuditLogIntegrationEvent(new Guid(), a.Ipaddress, a.Channel, a.ActionType, a.Comments,
        //        //                                     a.ModuleName, "OnActionExecuting", Clock.Now.ToString()));
        //    }
        //    catch //(Exception ex)
        //    {
        //        //throw ex;
        //    }
        //}
    }
}
