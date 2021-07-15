using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Tornado.Shared.Messaging.Sms
{
    public interface IWebClient
    {
        string DoRequest(string endpoint, string method = "GET", string body = null, Dictionary<string, string> headers = null, string contentType = null, X509Certificate clientCertificate = null);
    }
}
