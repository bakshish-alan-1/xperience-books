using Ecommerce;
using TMPro;
using UnityEngine;

public class UpdateAddressField : MonoBehaviour
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

    public void setDataTofield(Address address)
    {
        m_FirstName.text = address.m_FirstName;
        m_LastName.text = address.m_LastName;
        m_BlockName.text = address.m_address_1;
        m_Locality.text = address.m_address_2;
        //m_CountryCode.itemText = address.m_CountryCode;
        m_PhoneNumber.text = address.m_PhoneNumber;
        m_city.text = address.m_City;
        m_state.text = address.m_State;
        //m_Country.t
        m_zipCode.text = address.m_PostalCode;
    }

}
