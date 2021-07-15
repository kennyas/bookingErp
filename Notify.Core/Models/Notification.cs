using Notify.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Notify.Core.Models
{
    public class GIGMNotification : BaseEntity
    {
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string ServiceName { get; set; }
    }
}
