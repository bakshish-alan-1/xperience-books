using System;
using System.Collections.Generic;

namespace PaymentSDK.Core
{
   [Serializable]
    public class AccessToken
    {
        public string access_token;
        public string token_type;
        public int expires_in;
        private DateTime createDate;
        public AccessToken()
        {
            this.createDate = DateTime.Now;
        }
        public bool IsExpired()
        {
            DateTime expireDate = this.createDate.Add(TimeSpan.FromSeconds(this.expires_in));
            return DateTime.Now.CompareTo(expireDate) > 0;
        }
    }

    #region Checkout Order Model

    [Serializable]
    public class Amount
    {
        public string currency_code;
        public string value;
    }

    [Serializable]
    public class Address
    {
        public string address_line_1;
        public string address_line_2 ;
        public string admin_area_2 ;
        public string admin_area_1 ;
        public string postal_code ;
        public string country_code ;
    }

    [Serializable]
    public class Shipping
    {
        public Address address ;
    }
    [Serializable]
    public class PurchaseUnit
    {
        public string reference_id ;
        public Amount amount ;
        public Shipping shipping ;
    }

    [Serializable]
    public class ApplicationContext
    {
        public string return_url ;
        public string cancel_url ;
    }

    [Serializable]
    public class CheckOut
    {
        public string intent ;
        public List<PurchaseUnit> purchase_units  = new List<PurchaseUnit>();
        public ApplicationContext application_context = new ApplicationContext();
    }
    #endregion


    [Serializable]
    public class Order
    {
        public string create_time;
        public string update_time;
        public string id;
        public string intent;
        public Payer payer;
        public PurchaseUnit[] purchase_units;
        public string status;
        public Link[] links;
    }


    [Serializable]
    public class Payer
    {
        public Name name;
        public string email_address;
        public string payer_id;
    }

    [Serializable]
    public class Link
    {
        public string href;
        public string rel;
        public string method;
    }

    [Serializable]
    public class Name
    {
        public string given_name;
        public string surname;
    }
}
