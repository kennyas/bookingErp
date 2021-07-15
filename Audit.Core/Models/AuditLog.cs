using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Audit.Core.Models
{
    public class AuditLog : BaseEntity
    {              
        public Guid UserId { get; set; }        
        public string Ipaddress { get; set; }
        public string Channel { get; set; } 
        public string ActionType { get; set; }
        public string Comments { get; set; }
        public string ModuleName { get; set; } 
        public string Parameters { get; set; }
    }
}
