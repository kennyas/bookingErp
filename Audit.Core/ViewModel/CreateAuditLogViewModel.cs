using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Core.ViewModels
{
    public class CreateAuditLogViewModel
    {
        public string UserId { get; set; }
        //public string Ipaddress { get; set; }
        public string Channel { get; set; } //mobile, ussd, web, kiosk, etc
        public string ActionType { get; set; } //what was done.
        public string Comments { get; set; } //further comment on the action
        public string ModuleName { get; set; } //apicontroller/action method called to perform the action.
    }
}
