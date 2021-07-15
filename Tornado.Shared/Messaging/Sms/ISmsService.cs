using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Messaging.Sms
{
    public interface ISmsService
    {
        void SendSMSNow(string message, string sender = "", params string[] recipient);
    }
}
