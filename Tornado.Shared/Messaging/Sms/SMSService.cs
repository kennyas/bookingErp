using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.Extensions;
using Tornado.Shared.Timing;

namespace Tornado.Shared.Messaging.Sms
{
    public class SMSService : ISmsService
    {
        public static IWebClient WebClientSource;

        public SMSService(IWebClient webClient)
        {
            WebClientSource = webClient;
        }

        public void SendSMSNow(string message, string sender = "", params string[] recipient)
        {
            var model = new SmsLiveModel
            {
                Message = message,
                Sender = sender,
                Recipient = recipient
            };

            Task.WaitAll(OgoSMS(model).SendSmsAsync());
        }

        public SMSSenderModel OgoSMS(SmsLiveModel model)
        {
            var smsBody = model.Message;
            var recipient = model.Recipient.ArrayToCommaSeparatedString().UrlEncode();

            var url = "http://www.ogosms.com/dynamicapi/?username=gigm&password=gigasmin&sender=GIGM.COM&route=1&numbers=" + recipient + "&message=" + smsBody;
            var senderModel = new SMSSenderModel
            {
                ApiUrl = url,
                Method = "GET"
            };
            return senderModel;
        }

    }
}
