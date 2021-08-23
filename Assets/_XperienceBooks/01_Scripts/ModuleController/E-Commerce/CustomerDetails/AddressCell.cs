using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Ecommerce.address
{
    public class AddressCell : MonoBehaviour
    {
        public delegate void OnAddressSelected(int index, AddressType t);
        public static event OnAddressSelected OnSelected;
        public static event OnAddressSelected OnEdited;
        public static event OnAddressSelected OnDeleted;

        public int index;

        public TextMeshProUGUI ContactPerson;
        public TextMeshProUGUI BlockNumber;
        public TextMeshProUGUI locality;
        public TextMeshProUGUI city_state;
        public TextMeshProUGUI country_zip;

        public GameObject EditOptions, toggleOptions;

        Toggle toggle;
        AddressType type;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        public void SetData(Address address, AddressType addType, int index, bool isSelected) {
            toggle = GetComponent<Toggle>();

            this.index = index;
            ContactPerson.text = address.m_FirstName + " " + address.m_LastName + " - (" + address.m_CountryCode + address.m_PhoneNumber + ")";
            BlockNumber.text = address.m_address_1;
            locality.text = address.m_address_2;
            city_state.text = address.m_City + " , " + address.m_State;
            country_zip.text = address.m_Country + "  - " + address.m_PostalCode;

            type = addType;
            Debug.Log("setData: " + index + ", " + isSelected.ToString());

            if (toggle != null)
                toggle.isOn = isSelected;

        }

        public void IsAddressEdit()
        {
            EditOptions.SetActive(true);
            toggleOptions.SetActive(false);
        }

        public void OnEdit()
        {
            OnEdited(index, type);
        }

        public void OnDelete()
        {
            OnDeleted(index, type);
        }

        public void OnSelect()
        {
            if (toggle.isOn)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "04_E-commerce")
                {
                    Debug.Log("I am Here: " + index);
                    OnSelected(index, type); 
                }
                else
                    Debug.Log("I am Here");
            }
        }

    }
}
