using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace Ecommerce
{    
    [Serializable]
    public class OrderItemImage
    {
        public int order_item_id;
        public string image_name;
        public string created_at;
        public string updated_at;
        public int created_by;
        public int updated_by;
    }

    [Serializable]
    public class OrderItems
    {
        public int product_id;
        public string product_name;
        public string product_currency;
        public string product_price;
        public string order_price;
        public string product_desc;
        public int order_qty;
        public List<Attributes> product_attributes;
        public List<int> order_attributes;
        public string created_at;
        public string updated_at;
        public List<OrderItemImage> order_item_images;
    }

    [Serializable]
    public class UserAddress
    {
        public int id;
        public string first_name;
        public string last_name;
        public string country_code;
        public string phone_number;
        public bool is_default;
        public string address_1;
        public string address_2;
        public string city;
        public string state;
        public string country;
        public string postal_code;
    }

    [Serializable]
    public class OrderDetail
    {
        public int id;
        public int user_id;
        public int total_qty;
        public string final_price;
        public string system_order_id;
        public string paypal_order_id;
        public string status;
        public string payment_status;
        public string created_at;
        public List<OrderItems> order_items;
        public UserAddress order_shipping_address;
        public UserAddress order_billing_address;
    }

    [Serializable]
    public class PreviousOrderList
    {
        public bool success;
        public int code;
        public List<OrderDetail> data;
        public string message;
    }

    public class PreviousOrder : MonoBehaviour
    {
        public static PreviousOrder Instace = null;

        [SerializeField]
        private GameObject OrderCell;

        [SerializeField] private Transform OrderParent;

        [SerializeField]
        private PreviousOrderList response;

        [SerializeField]
        TMP_Text OrderDate, OrderId, PaymentStatus, DeliveryStatus;

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
        public GameObject m_OrderList;
        public GameObject m_PreviosOrderDetail;

        public string OutAnimation = "";
        public string InAnimation = "";

        public List<GameObject> cartList = new List<GameObject>();
        public float m_TotalPayout = 0;
        public float m_TotalCharges = 0;
        public float m_FinalPayOut = 0;

        bool isOrderSummery = false;
        private void Awake()
        {
            if (Instace == null)
                Instace = this;
        }

        private void OnEnable()
        {
            OrderData.OnMoreDetail += MoreButtonHit;
        }

        private void OnDisable()
        {
            OrderData.OnMoreDetail -= MoreButtonHit;
        }

        private void MoreButtonHit(int mainOrder, int orderIndex)
        {
            Debug.Log("MoreDetails list: " + mainOrder);
            // Reset all cart Instance
            cartList.Clear();
            for (int i = 0; i < cartViewParent.childCount; i++)
            {
                Destroy(cartViewParent.GetChild(i).transform.gameObject);
            }

            OpenOrderSummery();
            UpdateView(response.data, mainOrder, orderIndex);
        }

        private void ResetOrderList()
        {
            for (int i = 0; i < OrderParent.childCount; i++)
            {
                Destroy(OrderParent.GetChild(i).gameObject);
            }
            Debug.Log("ResetOrderList: " + OrderParent.childCount);
        }

        public void OrderListFound(object data)
        {
            Debug.Log("OrderListFound: " + data.ToString());
            ResetOrderList();
            response = JsonUtility.FromJson<PreviousOrderList>(data.ToString());

            int cnt = 0;
            for (int i = response.data.Count - 1; i >= 0; i--)
            {
                cnt += response.data[i].order_items.Count;
                for (int j = 0; j < response.data[i].order_items.Count; j++)
                {
                    GameObject obj = Instantiate(OrderCell, OrderParent, false);
                    string imgURL = "";
                    if (response.data[i].order_items[j].order_item_images.Count > 0)
                        imgURL = response.data[i].order_items[j].order_item_images[0].image_name;

                    obj.GetComponent<OrderData>().setData(i, j, response.data[i].order_items[j].product_name, response.data[i].order_items[j].product_desc, response.data[i].created_at,response.data[i].status, imgURL);
                }
            }
            Debug.Log("total qty: " + cnt);
        }

        public void UpdateView(List<OrderDetail> cart, int mainOdr, int orderIndex)
        {
            Debug.Log("order of: " + mainOdr + ", " + orderIndex);
            UserAddress billing = new UserAddress();
            billing = cart[mainOdr].order_billing_address;

            UserAddress shipping = new UserAddress();
            shipping = cart[mainOdr].order_shipping_address;

            float TotalProductAmt = 0;

            OrderDate.text = "Order Date: " + cart[mainOdr].created_at;
            OrderId.text = "Order id: " + cart[mainOdr].id;
            DeliveryStatus.text = "Delivery status: " + cart[mainOdr].status;
            PaymentStatus.text = "Payment status: " + cart[mainOdr].payment_status;

            StringBuilder BillingAddress = new StringBuilder();
            BillingAddress.AppendLine(billing.first_name + " " + billing.last_name + " - (" + billing.country_code + billing.phone_number + ")");
            BillingAddress.AppendLine(billing.address_1);
            BillingAddress.AppendLine(billing.address_2 + " , " + billing.city);
            BillingAddress.AppendLine(billing.state + " , " + billing.country + " - " + billing.postal_code);

            TextBox_BillingAddress.text = BillingAddress.ToString();


            StringBuilder shippingAddress = new StringBuilder();
            shippingAddress.AppendLine(shipping.first_name + " " + shipping.last_name + " - (" + shipping.country_code + shipping.phone_number + ")");
            shippingAddress.AppendLine(shipping.address_1);
            shippingAddress.AppendLine(shipping.address_2 + ", " + shipping.city);
            shippingAddress.AppendLine(shipping.state + " , " + shipping.country + " - " + shipping.postal_code);

            TextBox_ShippingAddress.text = shippingAddress.ToString();

            int i = 0;
            foreach (OrderDetail product in cart)
            {
                if (i == mainOdr)
                {
                    for (int j = 0; j < product.order_items.Count; j++)
                    {
                        GameObject obj = Instantiate(OrderPrefab, cartViewParent, false);
                        TotalProductAmt += float.Parse(product.order_items[j].order_price);
                        obj.GetComponent<CheckOutOrderCell>().SetPreviousOrderData(product.order_items[j], 0); 
                        cartList.Add(obj);
                    }
                    break;
                }
                i++;
            }

            m_TotalPayout = TotalProductAmt;
            TextBox_TotalAmount.text = "$ " + m_TotalPayout.ToString();


            m_FinalPayOut = float.Parse(cart[mainOdr].final_price);

            TextBox_FinalAmount.text = "$ " + m_FinalPayOut;


            TransformRefresh.gameObject.SetActive(true);
        }

        public void OpenOrderSummery()
        {
            isOrderSummery = true;
            m_PreviosOrderDetail.GetComponent<Animator>().Play(InAnimation);
            m_OrderList.GetComponent<Animator>().Play(OutAnimation);
        }

        public void OpenPreviousOrder()
        {
            isOrderSummery = false;
            m_OrderList.GetComponent<Animator>().Play(InAnimation);
            m_PreviosOrderDetail.GetComponent<Animator>().Play(OutAnimation);
        }

        public void BackBtn()
        {
            if (isOrderSummery)
            {
                OpenPreviousOrder();
                for (int i = 0; i < cartViewParent.childCount; i++)
                {
                    Destroy(cartViewParent.GetChild(i).gameObject);
                }
            }
            else
            {
                ECommerceManager.Instance.GetFeaturedProduct();
                ProductDetails.Instance.Reset();
                ProductList.Instance.RefreshListData();
            }
        }
    }
}
