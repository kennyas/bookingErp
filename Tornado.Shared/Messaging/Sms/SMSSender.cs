using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Shared.Messaging.Sms
{
    public abstract class SMSSender
    {
        public abstract Task SendSmsAsync();
        public abstract void SendSms();
    }
}
