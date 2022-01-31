using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce;
using Ecommerce.checkout;
using Intellify.core;
using UnityEngine;
using UnityEngine.Networking;

public enum Method
{

    GET,
    POST,
    DELETE

}

#region Login || Register Response

[System.Serializable]
public class Message {

    public string key;
    public string value;
}

public class ResendParentEmail
{
    public int id;
    public string parent_email;
}

#endregion

#region ForgotPassword
[Serializable]
public class GetOTP
{
    public string email;
}
#endregion

#region AR Module List Response

[System.Serializable]
public struct ARModulesData
{
    public int id;
    public string ar_module_name;
    public bool isMapped;
}

[System.Serializable]
public class BookDetails
{
    public string chapter_name;
    public string book_name;
    public string series_name;
}


[Serializable]
public class MapModuleData {
    public Book book_details;
    public List<ARModulesData> mapped_data;
}


[System.Serializable]
public struct ARMappedModuleList
{
    public bool success;
    public int code;
    public MapModuleData data;
    public string message;
}

#endregion

#region Module Content

[System.Serializable]
public struct ModuleContent
{
    public bool success;
    public int code;
    public List<ContentModel> data;
    public string message;
}

#endregion

#region Ecommerce Data

[System.Serializable]
public struct Products
{
    public bool success;
    public int code;
    public List<Product> data;
    public string message;
}

#endregion

#region Checkout Order

[System.Serializable]
public struct OrderSystemID
{
    public string system_order_id;
}

[System.Serializable]
public struct NewOrder
{
    public bool success;
    public int code;
    public OrderSystemID data;
    public string message;
}
#endregion

#region ErrorResponse

public struct ErrorReponse {
    public int code;
    public string message;
    public User data;
}

#endregion

#region SeriesList

[System.Serializable]
public class Series
{
    public int id;
    public string name;
    public int user_id;
    public int author_id;
    public int skin_id;
    public string logo_url;
    public string image_path;
    public string domain;
    public string facebook_link;
    public string instagram_link;
    public string website_link;
    public string twitter_link, youtube_link;
    public string background_image;
    public int created_at_timestamp, updated_at_timestamp;
    public Authorname author;
    public Skin theme;
}

[System.Serializable]
public struct SeriesList
{
    public int code;
    public bool success;
    public string message;
    public List<Series> data;
}

#endregion

#region BookDetails

[System.Serializable]
public class Skin
{
    public int id;
    public string name, dialog_box, dialog_box_btn, scan_qr_bg, fan_art_img_glry;
    public string color_code, notif_icon, notif_thumbnail, notif_next, notif_tile;
    public string background_image, scan_button, back_button, inventory_placeholder;
    public string logo, facebook_icon, twitter_icon, youtube_icon, instagram_icon, website_icon, profile_icon, inventory_icon;
    public string module_1, module_2, module_3, module_4, module_5, module_6, module_7, module_8, module_9, module_10, module_11, module_12;
    public string font_h1, font_h2, font_h3, font_h4;
    public int created_at_timestamp, updated_at_timestamp;
}

[System.Serializable]
public class SeriesBooks
{
    public int id;
    public string name;
    public string image;
    public int created_at_timestamp, updated_at_timestamp;
    public int series_id, author_id;
}

[System.Serializable]
public class Authorname
{
    public int id;
    public string firstname;
    public string middlename, lastname;
    public string email;
    public string address_1, address_2;
    public string country, state, city;
    public int country_code, zip, phone_number;
}

[System.Serializable]
public class SeriesDetailsList
{
    public int code;
    public bool success;
    public string message;
    public List<SeriesBooks> data = new List<SeriesBooks>();
}

#endregion

#region New
[Serializable]
public class NewToken
{
    public string firebase_token;
    public string device_token;
}

[Serializable]
public class Notification
{
    public bool success;
    public int code;
    public List<NotificationData> data;
}

[Serializable]
public class NotificationData
{
    public int id;
    public int book_id;
    public string sc_qr_title;
    public string body;
    public string title;
}
#endregion

#region Inventory
[Serializable]
public class InventoryList
{
    public int code;
    public bool success;
    public string message;
    public List<InventoryDetails> data;
}

[Serializable]
public class InventoryDetails
{
    public int id;
    public int qrcode_id;
    public int armodule_id;
    public int inventory_id;
    public int unlocked;
    public int viewed;
    public Inventory inventory;
}

[Serializable]
public class Inventory
{
    public int id;
    public string title;
    public string description;
    public string image_url; 
}

[Serializable]
public class UnlockInventoryData
{
    public int inv_map_id;
}

[Serializable]
public class OpenInventoryData
{
    public int inv_map_id;
}
#endregion

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance;

    public Properties properties;

    public GameObject m_RayCastBlocker;
    public ErrorWindow errorWindow;
    public List<String> ErrorMessage = new List<string>();

    private bool IsValid(long code) => (int)code / 100 == 2;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        properties = Resources.Load<Properties>("Properties");
    }

    public void Start()
    {
        /*StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                RaycastUnblock();
            }
        }));*/
    }

    public void APIResponseFailPopup(long statusCode, string errorTitle, string responseData, bool defaultTheme)
    {
        if (!IsValid(statusCode))
        {
            ErrorMessage.Clear();
            Debug.Log("statusCode : --- " + statusCode);
            ErrorMessage.Add(Utility.HTTPStatusError(statusCode));
            errorWindow.SetErrorMessage("Something Went Wrong", Validator.GetValidateErrorMessage(ErrorMessage), "CLOSE", ErrorWindow.ResponseData.JustClose, defaultTheme);
        }
        else
        {
            ErrorReponse error = JsonUtility.FromJson<ErrorReponse>(responseData);
            ErrorMessage.Clear();
            string errorMessage = error.message;
            string[] errorList = errorMessage.Split(',');
            foreach (string token in errorList)
            {
                ErrorMessage.Add(token);
            }
            errorWindow.SetErrorMessage(errorTitle, Validator.GetValidateErrorMessage(ErrorMessage), "CLOSE", ErrorWindow.ResponseData.JustClose, defaultTheme);
        }
    }

    //Checking Intenet Connection before Every API Call & Handle Internet Connection Error 
    #region CheckConnection
    IEnumerator CheckInternetConnection(Action<bool> action)
    {
        RayCastBlock();

        UnityWebRequest request = new UnityWebRequest("https://google.com");
        yield return request.SendWebRequest();
        if (request.isNetworkError)
        {
            Debug.Log("net error: " + request.error);
            errorWindow.SetErrorMessage("Internet Connection Failed !", "Please, Check you internet connection and try again.", "TRY AGAIN",ErrorWindow.ResponseData.InternetIssue, true);
            RaycastUnblock();
            action(false);
        }
        else
        {
            action(true);
        }
    }
    #endregion

    //Login API :- 1
    #region Login Apis

    public void NewLogin(User user) {

        Debug.Log("User data: " + user.email + " " + user.password);
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                Debug.Log(properties.LoginAPI);
                APIClient.CallWebAPI(Method.POST.ToString(),properties.LoginAPI,JsonUtility.ToJson(user),string.Empty,LoginUser);
            }
        }));
    }

    private void LoginUser(bool success, object data,long statusCode) {

        RaycastUnblock();
        if (success)
        {
            GameManager.Instance.isMarkerDetailInfoOpen = false;
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(data.ToString());
            response.data.user.password = PlayerPrefs.GetString("Password", "");
            FileHandler.SaveUserData(response.data);
            GameManager.Instance.m_UserData = response.data;//Store User Data on Instance
            Profile.Instance.SetProfileData(GameManager.Instance.m_UserData); // Set Data in Profile view (" In Future need to remove this method and Merge with SetUserData")
            PlayerPrefs.SetInt(StaticKeywords.Login, 1);//User  Login sucess playerfab set
            if (response.data.user.first_login)
                WindowManager.Instance.OpenPanel(StaticKeywords.ChangePassword);   // first time login user then redirect to change password screen
            else if(GameManager.Instance.selectedSeries.id == -1 || GameManager.Instance.selectedBooks.id == -1)
            {
                GetSeriesList();
            }
            else
            {
                GameManager.Instance.OpenMarkerDetailsWindow();
                WindowManager.Instance.OpenPanel(StaticKeywords.HomePanel); // Redirect to Home Panel
            }

            if (PlayerPrefs.GetInt("firebaseTokenSaved") == 0 && GameManager.Instance.FirebaseToken != "")
            {
                SetNotificationToken(GameManager.Instance.FirebaseToken);
            }
        }
        else {

            if (!IsValid(statusCode))
            {
                ErrorMessage.Clear();
                Debug.Log("statusCode : --- " + statusCode);
                ErrorMessage.Add(Utility.HTTPStatusError(statusCode));

                errorWindow.SetErrorMessage("Something Went Wrong", Validator.GetValidateErrorMessage(ErrorMessage), "CLOSE", ErrorWindow.ResponseData.JustClose, true);

            }
            else {

                ErrorReponse error = JsonUtility.FromJson<ErrorReponse>(data.ToString());
                if (error.code == 402)//parent email are not varified reset if need
                {
                    Debug.Log("dataID: " + error.data.id);
                    Login.Instance.OnParentEmailVarifiedFail(error.message, error.data);
                }

                ErrorMessage.Clear();
                ErrorMessage.Add(error.message);
                errorWindow.SetErrorMessage("Something Went Wrong", Validator.GetValidateErrorMessage(ErrorMessage), "CLOSE", ErrorWindow.ResponseData.JustClose, true);
            }
        }
    }
    #endregion

    //Register API :- 2
    #region Register Apis

    public void NewRegister(User user)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.POST.ToString(), properties.Register, JsonUtility.ToJson(user), string.Empty, RegisterUser);
            }            
        }));
    }

    private  void RegisterUser(bool success, object data, long statusCode) {
        if (success)
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(data.ToString());
            WindowManager.Instance.OpenErrorPanel("AccountRegisterd");
        }
        else {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }

    public void UpdateUserProfile(User user)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.POST.ToString(), properties.UpdateUserProfile, JsonUtility.ToJson(user), GameManager.Instance.m_UserData.token, UpdateUser);
            }
        }));
    }

    private void UpdateUser(bool success, object data, long statusCode)
    {
        if (success)
        {
            UpdateUserProfileResponse response = JsonUtility.FromJson<UpdateUserProfileResponse>(data.ToString());

            UserData userData = new UserData();
            userData.token = GameManager.Instance.m_UserData.token;
            userData.user = response.data;
            userData.user.password = PlayerPrefs.GetString("Password", "");
            
            FileHandler.SaveUserData(userData);    // save updated user details on local file
            Profile.Instance.SetProfileData(GameManager.Instance.m_UserData);
            errorWindow.SetErrorMessage("Congratulations", response.message, "OKAY", ErrorWindow.ResponseData.JustClose, true);
            WindowManager.Instance.BackToPreviousWindow();
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        Profile.Instance.OnResetButtonText("response");
        RaycastUnblock();
    }
    #endregion

    // Get Mapped Module List :- 3
    #region Mapped ModuleList

    public void GetMappedModules(int bookID, int chapterID,int qrCodeID, float latitude, float longitude)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                RayCastBlock();
                APIClient.CallWebAPI(Method.GET.ToString(), string.Format(properties.GetMappModuleList, bookID, chapterID, qrCodeID, latitude, longitude) , string.Empty, GameManager.Instance.m_UserData.token, GetMappedModuleList);
            }
        }));
    }

    public void GetMappedModuleList(bool success, object data, long statusCode) {

        if (success)
        {
            ARMappedModuleList response = JsonUtility.FromJson<ARMappedModuleList>(data.ToString());
            
            List<int> mappedModules = new List<int>();
            for (int i = 0; i < response.data.mapped_data.Count; i++)
            {
                if (response.data.mapped_data[i].isMapped)
                {
                    mappedModules.Add(response.data.mapped_data[i].id);
                }
            }
            response.data.book_details.b_MapModules.AddRange(mappedModules);
            GameManager.Instance.UpdateBook(response.data.book_details);
            WindowManager.Instance.OpenPanel(StaticKeywords.ContentList);
        }
        else {
            if (!IsValid(statusCode))
            {
                ErrorMessage.Clear();
                Debug.Log("statusCode : --- " + statusCode);
                ErrorMessage.Add(Utility.HTTPStatusError(statusCode));
                errorWindow.SetErrorMessage("Something Went Wrong", Validator.GetValidateErrorMessage(ErrorMessage), "CLOSE", ErrorWindow.ResponseData.JustClose, false);
            }
            else
            {
                ErrorReponse error = JsonUtility.FromJson<ErrorReponse>(data.ToString());
                ErrorMessage.Clear();
                string errorMessage = error.message;
                string[] errorList = errorMessage.Split(',');
                foreach (string token in errorList)
                {
                    ErrorMessage.Add(token);
                }

                switch (error.code)
                {

                    case 401:
                        errorWindow.SetErrorMessage("Server Problem !", Validator.GetValidateErrorMessage(ErrorMessage), "TRY AGAIN", ErrorWindow.ResponseData.InternetIssue, false);
                        break;
                    case 403:
                        errorWindow.SetErrorMessage("Session Expire", Validator.GetValidateErrorMessage(ErrorMessage), "RE-LOGIN", ErrorWindow.ResponseData.SessionExpire, false);
                        break;
                    default:
                        errorWindow.SetErrorMessage("Error Found !", Validator.GetValidateErrorMessage(ErrorMessage), "CLOSE", ErrorWindow.ResponseData.JustClose, false);
                        break;
                }
            }
        }
        RaycastUnblock();
    }

    #endregion

    // Get Module Data on base of Selected Modules from Content 4 & 5 both handle here
    #region Module Content Apis

    public void GetModuleData(int chapterID, int moduleID)
    {
        // This need to remove after Server Update : Merge API 4 & 5 In single because both api Response is Same !! just API Method is different !!
        // Created by Srushti (Server side)
        string method;

        if (moduleID == 2)
        {
            method = string.Format(properties.GetMarkerImages, chapterID, moduleID, GameManager.Instance.currentBook.qr_code_id);
        }
        else
        {
            method = string.Format(properties.GetModuleContent, chapterID, moduleID, GameManager.Instance.currentBook.qr_code_id);
        }
        
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                RayCastBlock();
                APIClient.CallWebAPI(Method.GET.ToString(), method, string.Empty, GameManager.Instance.m_UserData.token, GetModuleContent);
            }
        }));

    }

    public void GetModuleContent(bool success, object data, long statusCode) {
        if (success)
        {
            ModuleContent response = JsonUtility.FromJson<ModuleContent>(data.ToString());

            // set clipboard data received from api depending on chapter scaned
            if (response.data[0].clipboard_data != "" && response.data[0].clipboard_data != null)
            {
                TextEditor te = new TextEditor();
                te.text = response.data[0].clipboard_data;
                te.SelectAll();
                te.Copy();

                Debug.Log("Clipboard data: " + te.text);
            }

            if (!string.IsNullOrEmpty(response.data[0].watermark))
            {
                GameManager.Instance.WatermarkURL = response.data[0].watermark;
            }

            GameManager.Instance.SetModuleData(response.data, response.data[0].ar_module_id + 1);
        }
        else {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
        }
        RaycastUnblock();
    }

    #endregion
    
    #region Order_API_CALL
    public void NewOrder(OrderDetails order)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.POST.ToString(), properties.Order, JsonUtility.ToJson(order), GameManager.Instance.m_UserData.token, RegisterOrder);
            }
        }));
    }

    private void RegisterOrder(bool success, object data, long statusCode)
    {
        if (success)
        {
            Debug.Log("RegisterOrder: " + data.ToString());
            NewOrder response = JsonUtility.FromJson<NewOrder>(data.ToString());
            Checkout.Instance.NewOrderRegisteSuccess(response.data.system_order_id);
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), false);
        }
        RaycastUnblock();
    }

    public class UpdateOrderDetails
    {
        public string system_order_id;
        public string payment_status;
    }

    public void UpdateOrder(string system_order_ID, string status)
    {
        UpdateOrderDetails updateOrderDetails = new UpdateOrderDetails();
        updateOrderDetails.system_order_id = system_order_ID;
        updateOrderDetails.payment_status = status;

        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.POST.ToString(), properties.OrderUpdate, JsonUtility.ToJson(updateOrderDetails), GameManager.Instance.m_UserData.token, OrderUpdated);
            }
        }));
    }

    private void OrderUpdated(bool success, object data, long statusCode)
    {
        if (success)
        {
            Debug.Log("OrderUpdated: " + data.ToString());
            //UpdateUserProfileResponse response = JsonUtility.FromJson<UpdateUserProfileResponse>(data.ToString());            
        }
        else
        {
            APIResponseFailPopup(statusCode, "Error Found !", data.ToString(), false);
        }
        RaycastUnblock();
    }

    #endregion
    
    // Update password when user login first time in application
    #region UpdatePassword Apis

    public void UpdatePassword(PasswordChange password)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.POST.ToString(), properties.UpdateUserPassword, JsonUtility.ToJson(password), GameManager.Instance.m_UserData.token, PasswordUpdated);
            }
        }));
    }

    private void PasswordUpdated(bool success, object data, long statusCode)
    {
        if (success)
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(data.ToString());
            response.data.user.password = PlayerPrefs.GetString("Password", "");
            FileHandler.SaveUserData(response.data);
            GameManager.Instance.m_UserData = response.data;//Store User Data on Instance
            Profile.Instance.SetProfileData(GameManager.Instance.m_UserData); // Set Data in Profile view (" In Future need to remove this method and Merge with SetUserData")
            PlayerPrefs.SetInt(StaticKeywords.Login, 1);//User  Login sucess playerfab set
            if (WindowManager.Instance.currentWindowIndex == 11)// show if current screen are change password screen
                GameManager.Instance.OpenMarkerDetailsWindow();

            GetSeriesList();
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }
    #endregion

    // Get otp for forgot password
    #region GetOTPForForgotPassword Apis

    public void GetForgotPasswordOTP(string m_EmailID)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                GetOTP emailOTP = new GetOTP();
                emailOTP.email = m_EmailID;

                APIClient.CallWebAPI(Method.POST.ToString(), properties.GetOTPforForgotPassword, JsonUtility.ToJson(emailOTP), string.Empty, ForgotOTPSuccess);
            }
        }));
    }

    private void ForgotOTPSuccess(bool success, object data, long statusCode)
    {
        if (success)
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(data.ToString());
            ForgotPassword.Instance.GetOTPSuccess(response.message);
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }

    public void ForgotUserPassword(Forgot forgot)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.POST.ToString(), properties.ForgotPassword, JsonUtility.ToJson(forgot), string.Empty, ForgotPasswordSuccess);
            }
        }));
    }

    private void ForgotPasswordSuccess(bool success, object data, long statusCode)
    {
        if (success)
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(data.ToString());
            ForgotPassword.Instance.ResetField();
            errorWindow.SetErrorMessage("Congratulations", response.message, "OKAY", ErrorWindow.ResponseData.JustClose, true);
            WindowManager.Instance.OpenPanel("Login");
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }

    #endregion

    // Update parent email id
    #region UpdateParentEmail Apis

    public void UpdateParentEmail(int id, string email)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                ResendParentEmail parentEmail = new ResendParentEmail();
                parentEmail.id = id;
                parentEmail.parent_email = email;
                APIClient.CallWebAPI(Method.POST.ToString(), properties.UpdateParentEmail, JsonUtility.ToJson(parentEmail), string.Empty, ParentEmailUpdated);
            }
        }));
    }

    private void ParentEmailUpdated(bool success, object data, long statusCode)
    {
        if (success)
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(data.ToString());
            FileHandler.SaveUserData(response.data);
            GameManager.Instance.m_UserData = response.data;//Store User Data on Instance
            Profile.Instance.SetProfileData(GameManager.Instance.m_UserData);// Set Data in Profile view (" In Future need to remove this method and Merge with SetUserData")
            Login.Instance.parentEmailSendSuccess();
            WindowManager.Instance.OpenPanel(StaticKeywords.LoginPanel); // Redirect to Home Panel
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }
    #endregion

    // Series Apis
    #region Series Apis
    public void GetSeriesList()
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.GET.ToString(), properties.seriesList, string.Empty, GameManager.Instance.m_UserData.token, OnSeriesList);
            }
        }));
    }

    private void OnSeriesList(bool success, object data, long statusCode)
    {
        if (success)
        {
            SeriesList response = JsonUtility.FromJson<SeriesList>(data.ToString());
            GameManager.Instance.m_Series = response.data;//Store List of series
            WindowManager.Instance.OpenPanel("Series"); // Redirect to Series list panel
            HomeScreen.Instance.OnSetSeriesData();  // set or download all series image and name 
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }

    public void GetSeriesDetails(int id)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.GET.ToString(), properties.seriesDetails + "/" + id.ToString() + "/books", string.Empty, GameManager.Instance.m_UserData.token, OnSeriesDetail);
            }
        }));
    }

    private void OnSeriesDetail(bool success, object data, long statusCode)
    {
        if (success)
        {
            SeriesDetailsList response = JsonUtility.FromJson<SeriesDetailsList>(data.ToString());
            GameManager.Instance.m_SeriesDetails = response.data;//store details of series
            WindowManager.Instance.OpenPanel("Book"); // Redirect to book selection panel
            HomeScreen.Instance.OnSetBooksData(); // set or download all book image and name 
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }

    #endregion

    #region Notification
    public void SetNotificationToken(string myToken)
    {
        callNotification(myToken);
    }

    public async void callNotification(string myToken)
    {
        Debug.Log("inside callNotification: " + myToken);
        PlayerPrefs.SetInt("firebaseTokenSaved", 1);
        UnityWebRequest request = new UnityWebRequest("https://google.com");
        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.isNetworkError)
        {
            Debug.Log("callNotification: " + request.error);
        }
        else
        {
            NewToken token = new NewToken();
            token.firebase_token = myToken;
            RaycastUnblock();
            Debug.Log("token: " + myToken);
            APIClient.CallWebAPI(Method.POST.ToString(), properties.sendToken, JsonUtility.ToJson(token), GameManager.Instance.m_UserData.token, OnTokenSet);

        }
    }

    private void OnTokenSet(bool success, object data, long statusCode)
    {
        Debug.Log("OnTokenSet: " + data.ToString());
        if (success)
            Debug.Log("Notification token set successfully: "+ statusCode);
        else
            Debug.Log("Notification token set fail: " + statusCode);
    }

    public void GetNotificationList()
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.GET.ToString(), properties.GetNotification, string.Empty, GameManager.Instance.m_UserData.token, OnNotificationList);
            }
        }));
    }

    private void OnNotificationList(bool success, object data, long statusCode)
    {
        if (success)
        {
            NotificationPanel.Instance.OnReleaseData();
            Notification response = JsonUtility.FromJson<Notification>(data.ToString());
            NotificationPanel.Instance.setNotificationData(response.data);
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }

    public void SetNotificationView(int id)
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                RaycastUnblock();
                string url = properties.NotificationViewed + id + "/view";
                APIClient.CallWebAPI(Method.POST.ToString(), url, string.Empty, GameManager.Instance.m_UserData.token, OnViewSuccess);
            }
        }));
    }

    private void OnViewSuccess(bool success, object data, long statusCode)
    {
        if (success)
            Debug.Log("Notification viewed successfully.");
        else
            Debug.Log("Notification viewed fail.");
    }

    #endregion

    // Inventory Apis
    #region Inventory
    public void GetInventoryList(int chapter_id, int qrCode_id)
    {
        string method = string.Format(properties.Inventory, chapter_id, qrCode_id);

        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.GET.ToString(), method, string.Empty, GameManager.Instance.m_UserData.token, OnInventoryList);
            }
        }));
    }

    private void OnInventoryList(bool success, object data, long statusCode)
    {
        if (success)
        {
            InventoryList response = JsonUtility.FromJson<InventoryList>(data.ToString());
            InventoryManager.Instance.OnSetInventory(response);
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }

    public void UnlockInventory(int mapID)
    {
        UnlockInventoryData data = new UnlockInventoryData();
        data.inv_map_id = mapID;

        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.POST.ToString(), properties.UnlockInventory, JsonUtility.ToJson(data), GameManager.Instance.m_UserData.token, UnlockInventoryDone);
            }
        }));
    }

    private void UnlockInventoryDone(bool success, object data, long statusCode)
    {
        if (success)
        {
            // call notification panel for inform user to unlock inventory
            GameManager.Instance.ShowToastPanel("Congratulations!\nNew Inventory item unlocked.");
        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }

    public void OpenInventory(int mapID)
    {
        OpenInventoryData data = new OpenInventoryData();
        data.inv_map_id = mapID;

        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                APIClient.CallWebAPI(Method.POST.ToString(), properties.OpenInventory, JsonUtility.ToJson(data), GameManager.Instance.m_UserData.token, OpenInventoryDone);
            }
        }));
    }

    private void OpenInventoryDone(bool success, object data, long statusCode)
    {
        if (success)
        {

        }
        else
        {
            APIResponseFailPopup(statusCode, "Something Went Wrong", data.ToString(), true);
        }
        RaycastUnblock();
    }
    #endregion

    public void RayCastBlock() {
        m_RayCastBlocker.SetActive(true);
    }

    public void RaycastUnblock() {
        m_RayCastBlocker.SetActive(false);
    }

    public void WrongQR() {
        WindowManager.Instance.OpenErrorPanel("QRError");
    }
}
