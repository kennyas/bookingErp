
namespace Tornado.Shared.Messaging.Email
{
    public class SmtpConfig 
    {
        public bool EnableSSl { get; set; }
        public int Port { get; set; }
        public string Server { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Sender { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}