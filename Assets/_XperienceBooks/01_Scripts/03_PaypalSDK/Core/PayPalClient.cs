using Ecommerce.checkout;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace PaymentSDK.Core
{

    #region Enum Store

    public enum PaymentEnvironment
    {
        Sandbox,
        Production
    }

    public enum OrderStatus
    {
        CREATED,
        SAVED,
        APPROVED,
        VOIDED,
        COMPLETED,
        PAYER_ACTION_REQUIRED
    }

    #endregion

    public class PayPalClient : BaseClient
    {
        public delegate void action(bool success, object data);

        #region PrivateMethods
        private bool HasAccessToken => !string.IsNullOrEmpty(accessToken);
        private static string accessToken;
        #endregion

      


        private static bool IsValid(long code) => (int)code / 100 == 2;


        string authenticate(string username, string password)
        {

            Debug.Log(username + " " + password);
            string auth = username + ":" + password;
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }

        #region oauth2/token
        private void GetAccessToken(UnityAction callback)
        {
            if (HasAccessToken)
            {
                callback();
                return;
            }

            UnityWebRequest request = new UnityWebRequest(PayPalEnvironment.BaseUrl() + "v1/oauth2/token", "POST");

            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Accept-Language", "en_US");
            request.SetRequestHeader("Authorization", "Basic "+ PayPalEnvironment.AuthorizationString());


            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes("grant_type=client_credentials"));

            request.SendWebRequest().completed += _ =>
            {
                var response = JsonUtility.FromJson<AccessToken>(request.downloadHandler.text);
                accessToken = response?.access_token;
                callback();
            };
        }
        #endregion

        #region API_CALL
        /// Make a raw API request to PayPal.
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="endPoint">i.e. v2/checkout/orders</param>
        /// <param name="jsonData">Data to post/patch.</param>
        /// <param name="callback">Returns server response in JSON format (status code, response string).</param>
        public void Request(string method, string endPoint, string jsonData, UnityAction<long, string> callback)
        {

            GetAccessToken(MakeApiCall);

            void MakeApiCall()
            {

                Debug.Log("Api Called : " + PayPalEnvironment.BaseUrl() + endPoint);

                UnityWebRequest request = new UnityWebRequest(PayPalEnvironment.BaseUrl() + endPoint, method);

                request.SetRequestHeader("Authorization", "Bearer " + accessToken);
                request.SetRequestHeader("Content-Type", "application/json");

                if (!string.IsNullOrEmpty(jsonData))
                    request.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(jsonData)) { contentType = "application/json" };

                request.downloadHandler = new DownloadHandlerBuffer();

                request.SendWebRequest().completed += _ =>
                {
                    callback(request.responseCode, request.downloadHandler.text);
                };
            };
        }
        #endregion





        #region checkout/orders

        public static void Buy(CheckOut checkOut, action onComplete = null)
        {
            new PayPalClient().CreateOrder(checkOut, onComplete);
        }

        public void CreateOrder(CheckOut checkOut, action onComplete)
        {
            CallApiToCreateOrder(checkOut, onComplete);
        }

        private void CallApiToCreateOrder(CheckOut checkOut, action onComplete)
        {

            //Some Changes as per Config File. 

            checkOut.purchase_units[0].amount.currency_code = properties.currencyCode;
            checkOut.application_context.return_url = properties.payPalSuccessUrl;
            checkOut.application_context.cancel_url = properties.payPalCancelUrl;

            Debug.Log(JsonUtility.ToJson(checkOut));

            Request("POST", "v2/checkout/orders", JsonUtility.ToJson(checkOut), (status, res) =>
            {
              
                var response = JsonUtility.FromJson<Order>(res);

                if (!IsValid(status) || response?.status != "CREATED")
                {
                    onComplete?.Invoke(false, res);
                    return;
                }

                string approvalLink = string.Empty;

                foreach (Link link in response.links)
                    if (link.rel == "approve")
                        approvalLink = link.href;


                onComplete?.Invoke(true, res);
            });
        }

        #endregion


        #region Confirm Payment


        public static void ConfirmPayment(string orderId, action onComplete = null)
        {
            new PayPalClient().ConfirmPurchase(orderId, onComplete);
        }

        public void ConfirmPurchase(string orderId, action onComplete)
        {
            Request("GET", string.Format("v2/checkout/orders/{0}", orderId), string.Empty, (status, res) =>
            {
                Order order = JsonUtility.FromJson<Order>(res);
                Debug.Log(res);
                bool success = IsValid(status) && order.status == "APPROVED";
                onComplete?.Invoke(success, res);
            });
        }

        #endregion

        #region Capture Payment

        public static void CapturePayment(string orderId, action onComplete = null)
        {
            Debug.Log("inside CapturePayment");
            new PayPalClient().CaptureAmount(orderId, onComplete);
        }

        public void CaptureAmount(string orderId, action onComplete)
        {
            Request("POST", string.Format("v2/checkout/orders/{0}/capture", orderId), string.Empty, (status, res) =>
            {
                Order order = JsonUtility.FromJson<Order>(res);
                Debug.Log(res);
                bool success = IsValid(status) && order.status == "COMPLETED";  
                onComplete?.Invoke(success, res);
            });
        }
        #endregion
    }
}
