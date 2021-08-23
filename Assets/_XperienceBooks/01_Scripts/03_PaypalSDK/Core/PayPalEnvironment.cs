

using System;
using System.Text;

namespace PaymentSDK.Core
{
    public class PayPalEnvironment
    {
        private string baseUrl;
        private string clientId;
        private string clientSecret;
       
        public PayPalEnvironment(string clientId, string clientSecret, string baseUrl)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.baseUrl = baseUrl;
        }

        public string BaseUrl()
        {
            return this.baseUrl;
        }

        public string AuthorizationString()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        }

        public string ClientId()
        {
            return clientId;
        }

        public string ClientSecret()
        {
            return clientSecret;
        }
    }

   
}