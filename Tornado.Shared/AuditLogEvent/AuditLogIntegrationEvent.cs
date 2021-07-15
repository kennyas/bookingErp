using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Tornado.Shared.AuditLogEvent
{
    public class AuditLogIntegrationEvent : IntegrationEvent
    {

        public Guid UserId { get; set; }
        public string Ipaddress { get; set; }
        public string Channel { get; set; } //mobile, ussd, web, kiosk, etc
        public string ActionType { get; set; }
        public string Comments { get; set; }
        public string ModuleName { get; set; }
        public string TransParams { get; set; }
        public string LogDate { get; set; }


        public AuditLogIntegrationEvent(
            Guid userId, string ipaddress,
            string channel, string actionType, string comments, string moduleName, string transType, string logDate)
        {
            UserId = userId;
            Ipaddress = ipaddress;
            Channel = channel;
            ActionType = actionType;
            Comments = comments;
            ModuleName = moduleName;
            TransParams = transType;
            LogDate = logDate; ;
        }
    }
}
