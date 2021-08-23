using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;


namespace Ecommerce.address
{
    public class CreateNewAddress : MonoBehaviour
    {

        [SerializeField]
        TMP_InputField m_FirstName;

        [SerializeField]
        TMP_InputField m_LastName;

        [SerializeField]
        TMP_InputField m_BlockName;

        [SerializeField]
        TMP_InputField m_Locality;

        [SerializeField]
        TMP_Dropdown m_CountryCode;

        [SerializeField]
        TMP_InputField m_PhoneNumber;

        [SerializeField]
        TMP_InputField m_city;

        [SerializeField]
        TMP_InputField m_state;

        [SerializeField]
        TMP_Dropdown m_Country;

        [SerializeField]
        TMP_InputField m_zipCode;

        [SerializeField]
        Color errorColor, defaultColor;

        public CustomerManager addressBook;

        void Start()
        {
            ResetField();
        }        

        public void CreateNew()
        {
            try
            {
                if (ValidateInput())
                {
                    Address address = new Address();
                    address.m_ID = -1; // -1 Means Default ID OR New ID that not registerd in Database
                    address.m_FirstName = m_FirstName.text;
                    address.m_LastName = m_LastName.text;
                    address.m_address_1 = m_BlockName.text;
                    address.m_address_2 = m_Locality.text;
                    address.m_CountryCode = "+1";
                    address.m_PhoneNumber = m_PhoneNumber.text;
                    address.is_default = true;
                    address.m_City = m_city.text;
                    address.m_State = m_state.text;
                    address.m_Country = "US";
                    address.m_PostalCode = m_zipCode.text;

                    if (addressBook != null)
                        addressBook.CreateNewAddress(address);
                    else
                        AddressManager.Instance.CreateNewAddressField(address);

                    ResetField();
                }

            }
            catch (Exception ex)
            {

                Debug.LogError("Register : " + ex.Message);
            }
        }

        public bool ValidateInput()
        {
            bool confirm = true;

            if (string.IsNullOrEmpty(m_FirstName.text))
            {
                SetError(m_FirstName);

                return false;
            }

            if (string.IsNullOrEmpty(m_LastName.text))
            {
                SetError(m_LastName);

                return false;
            }

            if (string.IsNullOrEmpty(m_BlockName.text))
            {
                SetError(m_BlockName);

                return false;
            }

            if (string.IsNullOrEmpty(m_Locality.text))
            {
                SetError(m_Locality);
                return false;
            }

          /*  if (string.IsNullOrEmpty(m_CountryCode.text))
            {
                SetError(m_CountryCode);
                return false;
            }*/

            if (string.IsNullOrEmpty(m_PhoneNumber.text))
            {
                SetError(m_PhoneNumber);
                return false;
            }

            if (m_PhoneNumber.text.Length < 10)
            {
                SetError(m_PhoneNumber,true);
                return false;
            }

            if (string.IsNullOrEmpty(m_city.text))
            {
                SetError(m_city);
                return false;
            }

            if (string.IsNullOrEmpty(m_state.text))
            {
                SetError(m_state);
                return false;
            }


          /*  if (string.IsNullOrEmpty(m_Country.text))
            {
                SetError(m_Country);
                return false;
            }*/


            if (string.IsNullOrEmpty(m_zipCode.text))
            {
                SetError(m_zipCode);
                return false;
            }

            if (m_zipCode.text.Length < 6)
            {
                SetError(m_zipCode, true);
                return false;
            }

            Debug.Log("Valid Data: " + confirm);
#if UNITY_EDITOR
            Debug.Log("<color=green>Valid Input</green>");
#endif

            return confirm;
        }

        public void SetError(TMP_InputField field,bool length = false)
        {
            field.text = "";
            field.placeholder.GetComponent<TextMeshProUGUI>().color = errorColor;
            field.placeholder.GetComponent<TextMeshProUGUI>().text = "Can not be empty.";

            if(length)
                field.placeholder.GetComponent<TextMeshProUGUI>().text = "Invalid.";
        }

        public void ResetError(TMP_InputField field)
        {
            field.text = "";
            field.placeholder.GetComponent<TextMeshProUGUI>().color = defaultColor;
            field.placeholder.GetComponent<TextMeshProUGUI>().text = "Enter text...";

        }

        public void ResetField()
        {
            ResetError(m_FirstName);
            ResetError(m_LastName);
            ResetError(m_BlockName);
            ResetError(m_Locality);

           // ResetError(m_CountryCode);
            ResetError(m_PhoneNumber);
            ResetError(m_city);
            ResetError(m_state);
          //  ResetError(m_Country);
            ResetError(m_zipCode);

        }
    }
}