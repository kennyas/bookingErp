using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Tornado.Shared.Models
{
    public class AuditLogModel : BaseEntity 
    {              
        public Guid UserId { get; set; }        
        public string Ipaddress { get; set; }
        public string Channel { get; set; } //mobile, ussd, web, kiosk, etc
        public string ActionType { get; set; }
        public string Comments { get; set; }
        public string ModuleName { get; set; } 
        public string parameters { get; set; }
    }
}
