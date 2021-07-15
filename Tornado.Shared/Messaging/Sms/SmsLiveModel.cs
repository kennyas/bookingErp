using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Messaging.Sms
{
    public class SmsLiveModel
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public string[] Recipient { get; set; }
        public string Token { get; set; }
    }
}