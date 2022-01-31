using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleSideMenu;
using Ecommerce.address;
using Ecommerce.Category;
using Ecommerce.checkout;
using Intellify.core;
using KetosGames.SceneTransition;
using UnityEngine;
using UnityEngine.Networking;

namespace Ecommerce
{
    [Serializable]
    public class CategoryData
    {
        public int id;
        public string category_name;
        public string category_image;
    }

    [Serializable]
    public struct CategoryList
    {
        public bool success;
        public int code;
        public List<CategoryData> data;
        public string message;
    }

    [Serializable]
    public class CartProduct {
        public bool isActive = true;
        public Product m_product;
        public List<int> m_SelectedAttributes = new List<int>();
        public int m_TotalQty;
        public float m_FinalPrice;
    }

    /*[Serializable]
    public class Order {
       public List<CartProduct> m_PurchasedProduct;
       public float m_TotalPriceIncludingTexAmount;
    }*/

    [Serializable]
    public class OrderDetails
    {
        public string m_OrderID;
        public string m_TotalPrice;
        public CustomerDetails m_CustomerDetails;
        public List<CartProduct> m_FinalCart = new List<CartProduct>();

    }


    [Serializable]
    public class CustomerDetails {
        public bool isBillingSameAsShipping = true;
        public Address m_BillingAddress;
        public Address m_ShippingAddress;
    }

    [Serializable]
    public class AddressManager
    {
        public List<Address> orderBillingAddress = new List<Address>();
        public List<Address> orderShippingAddress= new List<Address>();
    }

    [Serializable]
    public class AddressBook {

       public AddressManager data;
    }

    [Serializable]
    public class Address {

        public int m_ID;
        public string m_FirstName;
        public string m_LastName;
        public string m_CountryCode;
        public string m_PhoneNumber;
        public bool is_default;
        public string m_address_1;
        public string m_address_2;
        public string m_City;
        public string m_State;
        public string m_Country;
        public string m_PostalCode;
    }

    public class ECommerceManager : MonoBehaviour
    {
        [SerializeField]
        WindowManager M_WindowManager;

        public static ECommerceManager Instance;

        [SerializeField]
        SimpleSideMenu submenu;

        [SerializeField]
        ProductList m_ProductListView;

        [SerializeField]
        ProductDetails ProductView;

        [SerializeField]
        CategoryViewController m_CategoryView;

        [SerializeField]
        CartController m_CartView;

        [SerializeField]
        CustomerManager m_CustomerDetailsView;

        [SerializeField]
        Checkout m_CheckOutView;

        public List<CategoryData> m_CategoryList = new List<CategoryData>();

        //  public List<Product> Products = new List<Product>();

        public bool isAddressApiCall = false;

        bool isInventoryApiCall = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            GetFeaturedProduct();
            GetCategoryList();
        }

        //Checking Intenet Connection before Every API Call & Handle Internet Connection Error 
        #region CheckConnection
        IEnumerator CheckInternetConnection(Action<bool> action)
        {
           ApiManager.Instance.RayCastBlock();

            UnityWebRequest request = new UnityWebRequest("https://google.com");
            yield return request.SendWebRequest();
            if (request.isNetworkError)
            {
                ApiManager.Instance.errorWindow.SetErrorMessage("Internet Connection Failed !", "Please, Check you internet connection and try again.", "TRY AGAIN", ErrorWindow.ResponseData.InternetIssue, false);
                ApiManager.Instance.RaycastUnblock();
                action(false);
            }
            else
            {
                action(true);
            }
        }
        #endregion

        #region Feature ProductList
        public void GetFeaturedProduct()
        {
            int bookID = GameManager.Instance.currentBook.book_id;
            int chapterID = GameManager.Instance.currentBook.chapter_id;
            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.GET.ToString(), string.Format(ApiManager.Instance.properties.GetFeatureProductList, bookID, chapterID), string.Empty, GameManager.Instance.m_UserData.token, GetFeaturedProductList);
                }
            }));
        }

        public void GetFeaturedProductList(bool success, object data, long statusCode)
        {
            if (success)
            {
                 Products response = JsonUtility.FromJson<Products>(data.ToString());
                 m_ProductListView.InitList(response.data, "Featured");
                 OpenPanel("ProductList");
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
                OpenPanel("ProductList");
            }
            ApiManager.Instance.RaycastUnblock();
        }
        #endregion

        public void BackToMainScreen() {

            SceneLoader.LoadScene("01_Home");
        }

        public void ViewProductDetails(Product m_Product)
        {
            m_ProductListView.RefreshListData();
            ProductView.UpdateProductDetails(m_Product);
            OpenPanel("ProductView");
        }

        public void OpenPanel(string panelName) {
            M_WindowManager.OpenWindows(panelName);
        }

        public void ProductList() {
            OpenPanel("ProductList");
        }

        #region Get Product By Category ID
        public void GetProductByCategoryID(int index) {
            Debug.Log("Get category of index: " + index);
            int catID = m_CategoryList[index].id;
            string catName = m_CategoryList[index].category_name;

            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.GET.ToString(), string.Format(ApiManager.Instance.properties.GetProductByCategory, catID), string.Empty, GameManager.Instance.m_UserData.token, CategoryListByID);
                }
            }));
        }

        private void CategoryListByID(bool success, object data, long statusCode)
        {
            Products response = JsonUtility.FromJson<Products>(data.ToString());
            if (success)
            {
                Debug.Log("Data Added" + response.data.Count);

                m_ProductListView.InitList(response.data, "");
                ProductList();
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }

        #endregion

        #region Category List
        public void GetCategoryList() {
            //For closing open Hamburger Menu

            if (m_CategoryList.Count <= 0)
            {
                StartCoroutine(CheckInternetConnection(isConnected =>
                {
                    if (isConnected)
                    {
                        APIClient.CallWebAPI(Method.GET.ToString(), ApiManager.Instance.properties.GetAllCategory, string.Empty, GameManager.Instance.m_UserData.token, SetCategoryList);
                    }
                }));
            }
            else {
                OpenPanel("CategoryList");
            }
            submenu.Close();
        }
        public void SetCategoryList(bool success, object data, long statusCode) {

            if (!isInventoryApiCall)
            { isInventoryApiCall = true; GameManager.Instance.OnCheckToUnlockModule(5); }
            if (success)
            {
                Debug.Log("inside SetCategoryList");
                CategoryList response = JsonUtility.FromJson<CategoryList>(data.ToString());
                //Clear previouly loaded category data
                m_CategoryView.ClearCategoryList(response.data);

                m_CategoryList.AddRange(response.data);
                m_CategoryView.GenerateCategoryCells();
            }
            else {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }

        #endregion

        public void ProductListByCategory() {

        }

        public void BuyNowButtonHit()
        {
            Debug.Log("BuyNowButtonHit");
            CartController.Instance.ClearCartList();
            ProductView.AddToCart(true);
            if (isAddressApiCall == false)
                GetAddresses();

            OpenPanel("CustomerDetails");
        }

        public void OpenCategoryList()
        {
            OpenPanel("CategoryList");
            submenu.Close(); 
        }

        public void OpenCartView()
        {
            OpenPanel("CartView");
            submenu.Close();
        }

        public void OpenOrderListView()
        {
            GetOrderList();
            OpenPanel("PreviousOrder");
            submenu.Close();
        }

        #region PreviousOrderList
        public void GetOrderList()
        {
            Debug.Log("Calling GetOrderList Api: ");
            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.GET.ToString(), string.Format(ApiManager.Instance.properties.Order), string.Empty, GameManager.Instance.m_UserData.token, GetOrderList);
                }
            }));
        }

        public void GetOrderList(bool success, object data, long statusCode)
        {
            Debug.Log("GetAddressList Api: " + success.ToString()+" , data: "+data.ToString());
            if (success)
            {
                PreviousOrder.Instace.OrderListFound(data);
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }
        #endregion

        public void OpenHamburgerMenu() {

            submenu.Open();
        }

        public void AddToCart(CartProduct product, bool callFromBuyBtn)
        {
            m_CartView.UpdateCart(product);
            if (!callFromBuyBtn)
                OpenPanel("CartView");
        }

        #region Addresses
        public void GetAddresses()
        {
            Debug.Log("Calling GetAddress Api: ");
            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.GET.ToString(), string.Format(ApiManager.Instance.properties.GetAddressList), string.Empty, GameManager.Instance.m_UserData.token, GetAddressList);
                }
            }));
        }

        public void GetAddressList(bool success, object data, long statusCode)
        {
            if (success)
            {
                AddressBook response = JsonUtility.FromJson<AddressBook>(data.ToString());
                isAddressApiCall = true;
                m_CustomerDetailsView.InitList(response);
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }
        #endregion

        public void AddressView() {

            if (CartController.Instance.t_TotalCheckoutPrice > 0)
            {
                //Get List of address here and set in Address script
                if (isAddressApiCall == false)
                    GetAddresses();

                OpenPanel("CustomerDetails");
            }
        }

        public void OpenCheckOutPage() {
            //Set Checkout data from here before Proced to checkout page
            List<CartProduct> cart = new List<CartProduct>();
            for (int i = 0; i < m_CartView.m_FinalCart.Count; i++) {

                if (m_CartView.m_FinalCart[i].isActive) {

                    cart.Add(m_CartView.m_FinalCart[i]);
                }
            }
            m_CheckOutView.UpdateView(cart,m_CustomerDetailsView.m_customerDetails);
            OpenPanel("Checkout");
        }
    }
}
