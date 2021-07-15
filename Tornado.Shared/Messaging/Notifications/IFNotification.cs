using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.Enums;

namespace Tornado.Shared.Messaging.Notifications
{
    public interface IFNotification
    {
        Task PushToMobileAsync(string deviceToken, string message, string title = "GIGMS");
        Task PushToMobileAsync(string deviceToken, PushNotificationType messageType, object payload, string message = "You have received a push notification", string title = "GIGMS");

    }
}
