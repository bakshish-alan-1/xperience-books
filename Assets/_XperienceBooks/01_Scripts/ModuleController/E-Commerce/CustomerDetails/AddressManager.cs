using Ecommerce;
using Ecommerce.address;
using Intellify.core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Ecommerce.address
{
    public class RemoveData
    {
        public int address_id;
        public string address_type;
    }

    public class StoreAddress
    {
        public string address_type;
        public int address_id;
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

    public class AddressManager : MonoBehaviour
    {
        public static AddressManager Instance = null;

        public GameObject addressPrefab;
        public Transform m_ShippingAddressParent;
        public Transform m_BillingAddressParent;

        public GameObject m_AddressListView;
        public GameObject m_AddressCreateView;

        public string InAnimation = "";
        public string OutAnimation = "";

        public Button addAddressButton;
        public UpdateAddressField updateField;

        public List<Address> m_ShippingAddressBook = new List<Address>();
        public List<Address> m_BillingAddressBook = new List<Address>();

        int editIndexID = 0;

        void Start()
        {
            if (Instance == null)
                Instance = this;
                        
        }

        public void GetAddressesData()
        { cleanAddressList(); GetAddresses(); }

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

        #region Addresses
        private void GetAddresses()
        {
            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.GET.ToString(), string.Format(ApiManager.Instance.properties.GetAddressList), string.Empty, GameManager.Instance.m_UserData.token, GetAddressList);
                }
            }));
        }

        private void GetAddressList(bool success, object data, long statusCode)
        {
            if (success)
            {
                AddressBook response = JsonUtility.FromJson<AddressBook>(data.ToString());
                InitList(response);
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }

        private void StoreAddresses(StoreAddress address)
        {
            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.POST.ToString(), string.Format(ApiManager.Instance.properties.StoreAddress), JsonUtility.ToJson(address), GameManager.Instance.m_UserData.token, StoreAddressDone);
                }
            }));
        }

        private void StoreAddressDone(bool success, object data, long statusCode)
        {
            if (success)
            {
                Debug.Log("store address success: " + data.ToString());
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }

        private void UpdateAddresses(StoreAddress address)
        {
            Debug.Log("Update Address");
            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.POST.ToString(), string.Format(ApiManager.Instance.properties.UpdateAddress), JsonUtility.ToJson(address), GameManager.Instance.m_UserData.token, UpdateAddressDone);
                }
            }));
        }

        private void UpdateAddressDone(bool success, object data, long statusCode)
        {
            if (success)
            {
                Debug.Log("update address success: " + data.ToString());
                RefreshList();
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }

        private void DeleteAddress(int index, AddressType _type)
        {
            Debug.Log("Delete Index: " + index);
            RemoveData user = new RemoveData();
            if (TabView.Instace.currentIndex == 0)
                user.address_id = m_ShippingAddressBook[index].m_ID;
            else
                user.address_id = m_BillingAddressBook[index].m_ID;

            if (TabView.Instace.currentIndex == 0)
                user.address_type = "shipping";
            else
                user.address_type = "billing";

            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    APIClient.CallWebAPI(Method.DELETE.ToString(), string.Format(ApiManager.Instance.properties.DeleteAddress), JsonUtility.ToJson(user), GameManager.Instance.m_UserData.token, DeleteAddress);
                }
            }));
        }

        private void DeleteAddress(bool success, object data, long statusCode)
        {
            Debug.Log("DeleteAddress: " + success.ToString());
            if (success)
            {
                RefreshList();
            }
            else
            {
                ApiManager.Instance.APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
            }
            ApiManager.Instance.RaycastUnblock();
        }

        #endregion

        private void InitList(AddressBook addressBook)
        {
            m_ShippingAddressBook.Clear();
            m_BillingAddressBook.Clear();

            Debug.Log("shipping Address Added " + addressBook.data.orderShippingAddress.Count);
            Debug.Log("billing Address Added " + addressBook.data.orderBillingAddress.Count);
            foreach (Address address in addressBook.data.orderShippingAddress)
            {

                UpdateAddressList(address, AddressType.Shipping);
            }
            foreach (Address address in addressBook.data.orderBillingAddress)
            {
                UpdateAddressList(address, AddressType.Billing);
            }
        }

        public void CreateNewAddressField(Address address)
        {
            Debug.Log("CreateNewAddressField");
            StoreAddress store = new StoreAddress();
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

            if (addAddressButton.GetComponentInChildren<TMPro.TMP_Text>().text == "Update Address")
            {
                if (TabView.Instace.currentIndex == 0)
                {
                    store.address_type = "shipping";
                    store.address_id = m_ShippingAddressBook[editIndexID].m_ID;
                }
                else
                {
                    store.address_type = "billing";
                    store.address_id = m_BillingAddressBook[editIndexID].m_ID;
                }
                UpdateAddresses(store);
                addAddressButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Add Address";
            }
            else
            {
                if (TabView.Instace.currentIndex == 0)
                {
                    UpdateAddressList(address, AddressType.Shipping);
                    store.address_type = "shipping";
                }
                else
                {
                    UpdateAddressList(address, AddressType.Billing);
                    store.address_type = "billing";
                }
                StoreAddresses(store);
            }

            //close create address panel
            m_AddressListView.GetComponent<Animator>().Play(InAnimation);
            m_AddressCreateView.GetComponent<Animator>().Play(OutAnimation);
        }

        public void UpdateAddressList(Address address, AddressType type)
        {

            if (type == AddressType.Shipping)
            {
                GameObject obj = Instantiate(addressPrefab, m_ShippingAddressParent, false);
                obj.GetComponent<AddressCell>().SetData(address, type, m_ShippingAddressBook.Count, address.is_default);
                obj.GetComponent<AddressCell>().IsAddressEdit();
                m_ShippingAddressBook.Add(address);
            }
            else
            {
                GameObject obj = Instantiate(addressPrefab, m_BillingAddressParent, false);
                obj.GetComponent<AddressCell>().SetData(address, type, m_BillingAddressBook.Count, address.is_default);
                obj.GetComponent<AddressCell>().IsAddressEdit();
                m_BillingAddressBook.Add(address);
            }
            Debug.Log("Address Added " + type.ToString());
        }

        private void OnEnable()
        {
            AddressCell.OnEdited += EditAddress;
            AddressCell.OnDeleted += DeleteAddress;
        }

        private void OnDisable()
        {
            AddressCell.OnEdited -= EditAddress;
            AddressCell.OnDeleted -= DeleteAddress;
        }
        
        public void OpenCreateAddress()
        {
            m_AddressListView.GetComponent<Animator>().Play(OutAnimation);
            m_AddressCreateView.GetComponent<Animator>().Play(InAnimation);
        }

        private void EditAddress(int index, AddressType _type)
        {
            Debug.Log("Edit address: " + index);
            OpenCreateAddress();

            editIndexID = index;
            addAddressButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Update Address";
            if (TabView.Instace.currentIndex == 0)
                updateField.setDataTofield(m_ShippingAddressBook[index]);
            else
                updateField.setDataTofield(m_BillingAddressBook[index]);

        }

        private void RefreshList()
        {
            GameManager.Instance.callWindowManagerAfterDeleteAddress();
            cleanAddressList();
            GetAddressesData();
        }

        private void cleanAddressList()
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
    }
}
