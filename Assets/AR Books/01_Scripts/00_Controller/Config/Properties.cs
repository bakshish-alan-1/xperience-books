﻿

using UnityEngine;

public enum DevEnvironment
{
    Testing,
    Local,
    Production
}

/// <summary>
/// This file is use to create Properties file that will use as scriptable object and store data of database configration 
/// </summary>


[CreateAssetMenu(fileName = "Properties", menuName = "BookSystem/DatabaseConfig", order = 1)]
public class Properties : ScriptableObject
{
    #region AR Book System Database

    public DevEnvironment m_CurrentEnvirnment;

    [Header("Testing Server")]
    public string TestingBaseURL = "https://arbooks.theintellify.net/api/";

    //[Header("New Production")]
    //public string ProductionNew = "http://192.168.1.3:8000/api/";

    [Header("Client Server")]
    public string ProductionBaseURL = "https://admin.continuummultimedia.com/api/";  //"https://arbooks-prod.theintellify.net/api/";

    [Space(15)]
    [Header("API List")]
    [Space(15)]

    [Tooltip("1) Login Method")]
    public string LoginAPI = "login";

    [Tooltip("2) Register Method")]
    public string Register = "register";

    public string GetOTPforForgotPassword = "forget-password/get-otp";
    public string ForgotPassword = "forget-password";
    public string UpdateParentEmail = "user/parent-email/update";

    [Tooltip("3) Mapped Module List - Call when QR Scan : Required 3 Param : Book ID , Chapter ID , Index")]
    public string GetMappModuleList = "chapter/getarmoduleschapters?book_id={0}&chapter_id={1}&qr_code_id={2}&latitude={3}&longitude={4}";

    [Tooltip("4) Get Selected Module Data")]
    public string GetModuleContent = "chapter/getchapterqrcodealldata";

    [Tooltip("5) Get Image Target List :- Module 2")]
    public string GetMarkerImages = "chapter/getchapterccodealldata";

    [Tooltip("6) Update User Profile")]
    public string UpdateUserProfile = "profile/update";

    [Tooltip("7) Update User Password")]
    public string UpdateUserPassword = "change-password";

    [Tooltip("Get Series list")]
    public string seriesList = "series";

    [Tooltip("Get Series Details with theame")]
    public string seriesDetails = "series";

    [Space(15)]
    [Header("ECommerce API")]
    [Space(15)]

    [Tooltip("8) Get Feature Product List")]
    public string GetFeatureProductList = "product/getfeaturedproducts";
    [Tooltip("9) Get All Category List")]
    public string GetAllCategory = "category/show";
    [Tooltip("10) Get Product List By Category ID")]
    public string GetProductByCategory = "product/getproductsbycategory?category_id={0}";
    [Tooltip("11) Get Address List added By User")]
    public string GetAddressList = "address";
    [Tooltip("12) Delete Address ")]
    public string DeleteAddress = "address/delete";
    [Tooltip("13) Store Address ")]
    public string StoreAddress = "address/store";
    [Tooltip("14) Update Address ")]
    public string UpdateAddress = "address/update";


    [Space(15)]
    [Header("Purchase Order API")]
    [Space(15)]


    [Tooltip("15) Post purchased order detail by user")]
    public string Order = "order";
    [Tooltip("16) Post order update with payment status")]
    public string OrderUpdate = "order/update";


    #endregion
}
