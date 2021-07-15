using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.Enums;

namespace Tornado.Shared.Messaging.Notifications
{
    public class FNotification : IFNotification
    {
        private readonly string _serverKey;
        private readonly string _endPoint;
        public FNotification()
        {
            _serverKey = ConfigurationManager.AppSettings.Get("FB_Server_API_Key");
            _endPoint = ConfigurationManager.AppSettings.Get("FB_message_send");
        }

        private async Task _SendAsync(Message message)
        {

            if (message == null)
            {
                return;
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", new System.Net.Http.Headers.AuthenticationHeaderValue("key", $"={_serverKey}").ToString());
                // Set up the content
                var jsonMessage = JsonConvert.SerializeObject(message);
                var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_endPoint, content);

                var cnt = await response.Content.ReadAsStringAsync();

            }
        }

        public async Task PushToMobileAsync(string deviceToken, string message, string title = "GIGMS")
        {
            if (string.IsNullOrEmpty(deviceToken) || string.IsNullOrEmpty(message))
                return;

            var pushMessage = new Message
            {
                to = deviceToken,
                notification = new Notification
                {
                    body = message,
                    title = title
                },
                data = new Data
                {
                    messageType = PushNotificationType.Normal,
                    title = title
                }
            };

            await _SendAsync(pushMessage);
        }

        public async Task PushToMobileAsync(string deviceToken, PushNotificationType messageType, object payload, string message = "You have received a push notification", string title = "GIGMS")
        {
            if (string.IsNullOrEmpty(deviceToken))
                return;

            var pushMessage = new Message
            {
                content_available = true,
                priority = FcmMessagePriorityConstants.HIGH,
                to = deviceToken,
                notification = new Notification
                {
                    body = message,
                    title = title,
                },
                data = new Data
                {
                    body = payload,
                    title = title,
                    messageType = messageType
                }
            };

            await _SendAsync(pushMessage);
        }
    }
}
