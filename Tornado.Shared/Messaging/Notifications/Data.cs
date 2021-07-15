using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Enums;

namespace Tornado.Shared.Messaging.Notifications
{
    public class Data
    {
        public PushNotificationType messageType;
        public string title { get; set; }
        public object body { get; set; } // can be string or object based on PushNotificationType
    }
}
