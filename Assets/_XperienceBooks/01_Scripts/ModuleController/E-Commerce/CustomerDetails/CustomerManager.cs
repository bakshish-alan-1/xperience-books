using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Networking;
using Intellify.core;

namespace Ecommerce.address
{
    public enum AddressType { 
        Shipping,
        Billing
    }

    public class CustomerManager : MonoBehaviour
    {
        public CustomerDetails m_customerDetails;

        public List<Address> m_ShippingAddressBook = new List<Address>();
        public List<Address> m_BillingAddressBook = new List<Address>();

        /// <summary>
        /// Address Prefab that will instantiate and display ON UI.
        /// </summary>
        public GameObject addressPrefab;
        public Transform m_ShippingAddressParent;
        public Transform m_BillingAddressParent;

        /// <summary>
        /// OVER
        /// </summary>
        ///

        public GameObject m_AddressListView;
        public GameObject m_AddressCreateView;

        [SerializeField] Button checkoutBtn;// countinue button in address view screen

        public string InAnimation = "";
        public string OutAnimation = "";

        public bool isSelectAddress = true;

        public GameObject m_ShiipingAddressScrollView;
        public GameObject m_BillingAddressScrollView;

        public Toggle IsBillingAddressSame;
        public TabView tabView;

        public int m_SelectedShippingAddress = -1;
        public int m_SelectedBillingAddress = -1;

        // Start is called before the first frame update
        void Start()
        {
            OpenSelectAddress();

            if (IsBillingAddressSame.isOn)
            {
                m_ShiipingAddressScrollView.SetActive(true);
                m_BillingAddressScrollView.SetActive(false);
            }
            else {

                m_ShiipingAddressScrollView.SetActive(false);
                m_BillingAddressScrollView.SetActive(true);
            }
        }

        #region CheckConnection
        IEnumerator CheckInternetConnection(Action<bool> action)
        {
            ApiManager.Instance.RayCastBlock();

            UnityWebRequest request = new UnityWebRequest("http://google.com");
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

        public void InitList(AddressBook addressBook)
        {
            m_ShippingAddressBook.Clear();
            m_BillingAddressBook.Clear();

            Debug.Log("InitList Address: " + m_SelectedShippingAddress + ", " + m_SelectedBillingAddress);

            Debug.Log("shipping Address Added " + addressBook.data.orderShippingAddress.Count);
            Debug.Log("billing Address Added " + addressBook.data.orderBillingAddress.Count);
            foreach (Address address in addressBook.data.orderShippingAddress) {

                UpdateAddressList(address, AddressType.Shipping);
            }
            int i = 0;
            foreach (Address address in addressBook.data.orderBillingAddress)
            {
                Address newAddress = address;
                if (m_SelectedBillingAddress != -1 && m_SelectedBillingAddress == i)
                { newAddress.is_default = true; Debug.Log("Bill==i: " + i); }

                UpdateAddressList(newAddress, AddressType.Billing);
                i += 1;
            }
        }

        private void OnEnable()
        {
            AddressCell.OnSelected += AddressSelected;
        }

        private void OnDisable()
        {
            AddressCell.OnSelected -= AddressSelected;
        }

        public void OpenSelectAddress() {
            isSelectAddress = true;
            m_AddressCreateView.GetComponent<Animator>().Play(OutAnimation);
            m_AddressListView.GetComponent<Animator>().Play(InAnimation);
        }

        public void OpenCreateAddress()
        {
            isSelectAddress = false;
            m_AddressListView.GetComponent<Animator>().Play(OutAnimation);
            m_AddressCreateView.GetComponent<Animator>().Play(InAnimation);
        }

        public void CreateNewAddress(Address address) {

            StoreAddress store = new StoreAddress();
            if (tabView.currentIndex == 0)
            {
                store.address_type = "shipping";
                UpdateAddressList(address, AddressType.Shipping);
            }
            else
            {
                store.address_type = "billing";
                UpdateAddressList(address, AddressType.Billing);
            }

            store.first_name = address.m_FirstName;
            store.last_name = address.m_LastName;
            store.country_code = address.m_CountryCode;
            store.phone_number = address.m_PhoneNumber;
            store.is_default = true;
            store.address_1 = address.m_address_1;
            store.address_2 = address.m_address_2;
            store.city = address.m_City;
            store.state = address.m_State;
            store.country = address.m_Country;
            store.postal_code = address.m_PostalCode;

            OpenSelectAddress();
            StoreAddresses(store);
        }

        private void StoreAddresses(StoreAddress address)
        {
            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.POST.ToString(), string.Format(ApiManager.Instance.properties.StoreAddress), JsonUtility.ToJson(address), GameManager.Instance.m_UserData.token, StoreAddressDone);
                }
                else
                    ApiManager.Instance.RaycastUnblock();
            }));
        }

        private void StoreAddressDone(bool success, object data, long statusCode)
        {
            if (success)
            {
                Debug.Log("store address success: " + data.ToString());
                RefreshList();
                ECommerceManager.Instance.isAddressApiCall = false;
                ECommerceManager.Instance.AddressView();
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }

        private void RefreshList()
        {
            m_ShippingAddressBook.Clear();
            m_BillingAddressBook.Clear();

            for (int i = 0; i < m_ShippingAddressParent.childCount; i++)
            {
                Destroy(m_ShippingAddressParent.GetChild(i).gameObject);
            }

            for (int j = 0; j < m_BillingAddressParent.childCount; j++)
            {
                Destroy(m_BillingAddressParent.GetChild(j).gameObject);
            }
        }

        public void UpdateAddressList(Address address, AddressType type) {

            if (type == AddressType.Shipping)
            {
                GameObject obj = Instantiate(addressPrefab, m_ShippingAddressParent, false);
                obj.GetComponent<Toggle>().group = m_ShippingAddressParent.GetComponent<ToggleGroup>();
                obj.GetComponent<AddressCell>().SetData(address, type, m_ShippingAddressBook.Count, address.is_default);
                m_ShippingAddressBook.Add(address);
            }
            else {
                GameObject obj = Instantiate(addressPrefab, m_BillingAddressParent, false);
                obj.GetComponent<Toggle>().group = m_BillingAddressParent.GetComponent<ToggleGroup>();
                obj.GetComponent<AddressCell>().SetData(address, type, m_BillingAddressBook.Count, address.is_default);
                m_BillingAddressBook.Add(address);
            }
            Debug.Log("Address Added " + type.ToString());
            if ((m_ShippingAddressParent.transform.childCount > 0 && IsBillingAddressSame.isOn) || (!IsBillingAddressSame.isOn && m_ShippingAddressParent.transform.childCount > 0 && m_BillingAddressParent.transform.childCount > 0))
                checkoutBtn.interactable = true;
            else
                checkoutBtn.interactable = false;
        }

        private void AddressSelected(int index, AddressType type)
        {
            if(type == AddressType.Shipping)//if (tabView.currentIndex == 0)
            {
                m_SelectedShippingAddress = index;
            }
            else {
                m_SelectedBillingAddress = index;
            }
            Debug.Log("Select Address: " + m_SelectedShippingAddress + ", " + m_SelectedBillingAddress);
        }

        public void IsBillingSameToggleChange() {


            if (!IsBillingAddressSame.isOn)
            {
                tabView.ActivateTab(1);
                tabView.ActivateDeactivateBillingAddress();
            }
            else {
                tabView.ActivateDeactivateBillingAddress();
                tabView.ActivateTab(0);
            }
        }

        public void Back() {

            if (isSelectAddress)
            {
                tabView.ActivateTab(0);
                ECommerceManager.Instance.OpenPanel("CartView");

            }
            else {
                OpenSelectAddress();
            }
        }

        public void ProceedForCheckOut() {

            if (m_SelectedShippingAddress < 0) {
                Debug.Log("Please Add Address and select that");
                return;
            }
            Debug.Log("Select Address: " + m_SelectedShippingAddress + ", " + m_SelectedBillingAddress);
            // Add Data in shipping Address Here
            m_customerDetails.m_ShippingAddress = m_ShippingAddressBook[m_SelectedShippingAddress];


            //Update Billing Address Details on base of IsSameAddress is checked or not
            if (!IsBillingAddressSame.isOn)
            {
                m_customerDetails.isBillingSameAsShipping = false;
                Toggle SelectedBillingAddress = m_BillingAddressParent.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault();

                m_customerDetails.m_BillingAddress = m_BillingAddressBook[m_SelectedBillingAddress];
            }
            else {

                m_customerDetails.isBillingSameAsShipping = true;
                m_customerDetails.m_BillingAddress = m_customerDetails.m_ShippingAddress;
            }
            ECommerceManager.Instance.OpenCheckOutPage();
        }
    }
}