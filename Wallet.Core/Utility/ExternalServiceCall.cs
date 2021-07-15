using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Core.Utility
{
    public class ExternalServiceCall
    {
        private static void IgnoreSSL()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
        }

        public static async Task<string> Post(string urlendpoint, string key)
        {
            // var returnValue = new HRStaffObject();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(urlendpoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

                    IgnoreSSL();

                    HttpResponseMessage response = await client.GetAsync(urlendpoint); //.Result;
                    if (response != null)
                    {
                        // response?.EnsureSuccessStatusCode();
                        string response_string = await response.Content.ReadAsStringAsync();
                        return response_string;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (WebException xe)
            {
                using var resp = new StreamReader(xe.Response.GetResponseStream());
                return resp.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
