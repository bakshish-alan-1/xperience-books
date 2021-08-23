using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System.Linq;
using PaymentSDK.Core;
using System;

namespace Ecommerce.checkout
{

    public class Checkout : MonoBehaviour
    {
        public static Checkout Instance = null;

        public OrderDetails orderDetails;


        [SerializeField]
        TextMeshProUGUI TextBox_BillingAddress;

        [SerializeField]
        TextMeshProUGUI TextBox_ShippingAddress;

        [SerializeField]
        TextMeshProUGUI TextBox_TotalAmount;

        [SerializeField]
        TextMeshProUGUI TextBox_ChargeAmount;

        [SerializeField]
        TextMeshProUGUI TextBox_FinalAmount;

        public GameObject OrderPrefab;
        public Transform cartViewParent;

        public Transform TransformRefresh;


        public float m_TotalPayout = 0;
        public float m_TotalCharges = 0;
        public float m_FinalPayOut = 0;


        public List<GameObject> cartList = new List<GameObject>();


        public void UpdateView(List<CartProduct> cart , CustomerDetails details) {
            orderDetails = new OrderDetails();

            List<CartProduct> newCart = new List<CartProduct>();
            for (int i = 0; i < cart.Count; i++)
            {
                CartProduct nCart = new CartProduct();
                nCart = cart[i];
                if (cart[i].m_SelectedAttributes[0] >= 0)
                {
                    nCart.m_product.image[0] = cart[i].m_product.attributes[cart[i].m_SelectedAttributes[0]].color_image;
                    newCart.Add(nCart);
                }
            }

            orderDetails.m_FinalCart.AddRange(cart);
            orderDetails.m_CustomerDetails = details;

            ResetCart();
            foreach (CartProduct product in cart) {
                GameObject obj = Instantiate(OrderPrefab, cartViewParent, false);
                obj.GetComponent<CheckOutOrderCell>().SetData(product, 0);
                cartList.Add(obj);
            }

            StringBuilder BillingAddress = new StringBuilder();
            BillingAddress.AppendLine(details.m_BillingAddress.m_FirstName + " " + details.m_BillingAddress.m_LastName + " - (" + details.m_BillingAddress.m_CountryCode + details.m_BillingAddress.m_PhoneNumber + ")");
            BillingAddress.AppendLine(details.m_BillingAddress.m_address_1);
            BillingAddress.AppendLine(details.m_BillingAddress.m_address_2 +" , "+ details.m_BillingAddress.m_City);
            BillingAddress.AppendLine(details.m_BillingAddress.m_State + " , " + details.m_BillingAddress.m_Country + " - " + details.m_BillingAddress.m_PostalCode);

            TextBox_BillingAddress.text = BillingAddress.ToString();


            StringBuilder shippingAddress = new StringBuilder();
            shippingAddress.AppendLine(details.m_ShippingAddress.m_FirstName + " " + details.m_ShippingAddress.m_LastName + " - (" + details.m_ShippingAddress.m_CountryCode + details.m_ShippingAddress.m_PhoneNumber + ")");
            shippingAddress.AppendLine(details.m_ShippingAddress.m_address_1);
            shippingAddress.AppendLine(details.m_ShippingAddress.m_address_2 +", "+ details.m_ShippingAddress.m_City);
            shippingAddress.AppendLine(details.m_ShippingAddress.m_State+" , "+ details.m_ShippingAddress.m_Country +" - "+details.m_ShippingAddress.m_PostalCode);

            TextBox_ShippingAddress.text = shippingAddress.ToString();


            m_TotalPayout = cart.Select(x => x.m_FinalPrice).Sum();
            TextBox_TotalAmount.text = "$ "+m_TotalPayout.ToString();


            m_FinalPayOut = m_TotalPayout + m_TotalCharges;

            TextBox_FinalAmount.text = "$ " + m_FinalPayOut;


            TransformRefresh.gameObject.SetActive(true);

            orderDetails.m_TotalPrice = m_FinalPayOut.ToString();
        }

        public void ResetCart() {

            foreach (GameObject obj in cartList) {
                Destroy(obj);
            }
            cartList.Clear();
        }

        public void Back()
        {
            ECommerceManager.Instance.OpenPanel("CustomerDetails");
        }


        /// <summary>
        /// /
        ///
        /// Payment Addition need to arrange
        /// 
        /// </summary>

        public UniWebView webView;
        public GameObject m_Browser;

        public GameObject loadingPanel;
        public GameObject dialogBox;
        public Order lastOrder;

        public string System_order_id = "";
        void Start()
        {
            if (Instance == null)
                Instance = this;

        }

        public void BuyProduct() {

            dialogBox.SetActive(false);
            loadingPanel.SetActive(true);

            PayPalClient.Buy(CreateOrder(), PlaceOrder);
        }

        private void PlaceOrder(bool success, object data)
        {
            if (success)
            {
                lastOrder = new Order();
                lastOrder = JsonUtility.FromJson<Order>(data.ToString());
                string approvalLink = string.Empty;
                orderDetails.m_OrderID = lastOrder.id;
                Debug.Log("orderDetails.m_OrderID: " + orderDetails.m_OrderID);
                foreach (Link link in lastOrder.links)
                    if (link.rel == "approve")
                        approvalLink = link.href;
                // Debug.Log("Order Approve Link : "+approvalLink);
                //  webView.Show();
                //  webView.Load(approvalLink);
                CreateWebView(approvalLink);

                // call order api here
                System_order_id = "";
                ApiManager.Instance.NewOrder(orderDetails);
            }
            else
                Debug.LogError(data.ToString());
        }


        public void CreateWebView(string url)
        {

            if (webView != null)
                return;

            webView = m_Browser.AddComponent<UniWebView>();

            webView.Frame = new Rect(0, 0, Screen.width, Screen.height); // 1
            webView.SetShowToolbar(false);
            webView.Load(url);       // 2
            webView.Show();

            webView.OnShouldClose += (view) => {
                ConfirmPayment();
                webView = null;
                return true;
            };
            webView.OnMessageReceived += (view, message) => {
                if (message.Path.Equals("close"))
                {
                    ConfirmPayment();
                    Destroy(webView);
                    webView = null;

                }
            };
        }

        public void NewOrderRegisteSuccess(string registerId)
        {
            System_order_id = registerId;
            Debug.Log("NewOrderRegisteSuccess: " + System_order_id + " , " + registerId);
        }

        public void ConfirmPayment()
        {
            loadingPanel.SetActive(false);
            dialogBox.SetActive(true);

            dialogBox.GetComponent<DialogBox>().SetDialogBox("Payment Completed", "Verification in progress..","",false);

            PayPalClient.ConfirmPayment(lastOrder.id, ConfirmPayment);
        }

        public void ConfirmPayment(bool success, object data)
        {
          var Order = JsonUtility.FromJson<Order>(data.ToString());
            if (success)
            {
                dialogBox.GetComponent<DialogBox>().SetDialogBox("Order Placed", "Order has been placed sucess fully, Order ID: " + Order.id, "Done");

                // messageBox.text = "Order has been placed sucess fully, Order ID : " + Order.id;

                Debug.Log("Order Purchase Sucessfully");

                if (Order.status.ToLower() == "approve" || Order.status.ToLower() == "approved")
                {
                    PayPalClient.CapturePayment(lastOrder.id, ConfirmCapturePayment);
                }
                else
                {
                    // update order status
                    if (System_order_id != "")
                        ApiManager.Instance.UpdateOrder(System_order_id, Order.status);
                }
            }
            else
            {
                dialogBox.GetComponent<DialogBox>().SetDialogBox("Payment Failed", "Order Approval failed , If Amount dedducted from your account will refund in 24 hours. Please keep Order ID : " + Order.id, "Failed");
                // messageBox.text = "Order Approval failed , If Amount dedducted from your account will refund in 24 hours. Please keep Order ID : " + Order.id;
                Debug.Log(Order.status);
            }
           // closeButton.interactable = true;
        }

        public CheckOut CreateOrder() {
            CheckOut checkOUT = new CheckOut();
            try
            {
                Debug.Log("1");
                // https://developer.paypal.com/docs/api/orders/v2/#definition-order_application_context
                #region application_context
                #endregion

                //https://developer.paypal.com/docs/api/orders/v2/#definition-purchase_unit_request
                #region purchase_units
                // Paypal Purchase Unity Object Set here , We can set info like Internal ORDER_ID , Order Discription , Amount , Total

                PurchaseUnit purchaseUnit = new PurchaseUnit();
                Debug.Log("2");

                Amount amount = new Amount();
                amount.currency_code = ""; // For now it will be empty , this value fille by PayPalClient.cs on base of paypal Configration file.
                amount.value = orderDetails.m_TotalPrice;


                Shipping shipping = new Shipping();
                PaymentSDK.Core.Address address = new PaymentSDK.Core.Address();
                address.address_line_1 = orderDetails.m_CustomerDetails.m_ShippingAddress.m_address_1;
                address.address_line_2 = orderDetails.m_CustomerDetails.m_ShippingAddress.m_address_2;
                address.admin_area_1 = orderDetails.m_CustomerDetails.m_ShippingAddress.m_City;
                address.admin_area_2 = orderDetails.m_CustomerDetails.m_ShippingAddress.m_State;
                address.postal_code = orderDetails.m_CustomerDetails.m_ShippingAddress.m_PostalCode;
                address.country_code = orderDetails.m_CustomerDetails.m_ShippingAddress.m_Country;
                shipping.address = address;

                Debug.Log("3");

                purchaseUnit.reference_id = "PUHF"; //Dummy Server Order ID.
                purchaseUnit.amount = amount; // Set in Purchase Unite
                purchaseUnit.shipping = shipping;

                Debug.Log("4");


                // Basic and static value set as per PayPal Requirement. 
                checkOUT.intent = "CAPTURE";
                checkOUT.purchase_units.Add(purchaseUnit);

                #endregion


                Debug.Log(JsonUtility.ToJson(checkOUT));
            }
            catch (Exception ex) {
                Debug.Log("Issue In Create ORDER : "+ex.Message);
            }
            return checkOUT;
        }

        public void ConfirmCapturePayment(bool success, object data)
        {
            Debug.Log("ConfirmCapturePayment: " + data.ToString() + " , success: " + success);
            var Order = JsonUtility.FromJson<Order>(data.ToString());
            Debug.Log("ConfirmCapturePayment: " + Order.status.ToString()+ " , success: " + success);
            if (success)
            {
                Debug.Log("ConfirmCapturePayment: " + Order.status);
                ApiManager.Instance.UpdateOrder(System_order_id, Order.status);
            }
            else
            {
                dialogBox.GetComponent<DialogBox>().SetDialogBox("Payment Failed", "Order Approval failed , If Amount dedducted from your account will refund in 24 hours. Please keep Order ID : " + Order.id, "Done");
                // messageBox.text = "Order Approval failed , If Amount dedducted from your account will refund in 24 hours. Please keep Order ID : " + Order.id;
                Debug.Log(Order.status);
            }
            // closeButton.interactable = true;
        }
    }
}
