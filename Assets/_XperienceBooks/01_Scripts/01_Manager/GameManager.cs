using System.Collections.Generic;
using System.IO;
using System.Text;
using Intellify.core;
using KetosGames.SceneTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class Book
{
    public int book_id;
    public int chapter_id;
    public int qr_code_id;
    public string book_name;
    public string chapter_name;
    public string series_name;
    public List<int> b_MapModules = new List<int>();

   
    public string BasePath() {

        StringBuilder path = new StringBuilder();

            path.Append(series_name);
            path.Append("/");
            path.Append(book_name);
            path.Append("/");
            path.Append(chapter_name);

        return path.ToString();
    }
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    /// <summary>
    /// List of all series
    /// </summary>
    public List<Series> m_Series = new List<Series>();

    /// <summary>
    /// Series Details
    /// </summary>
    public List<SeriesBooks> m_SeriesDetails = new List<SeriesBooks>();

    /// <summary>
    /// User Property that will use in all Project
    /// </summary>
    public UserData m_UserData;


    /// <summary>
    /// Book Details That will used in all Project
    /// </summary>
    public Book currentBook;

    /// <summary>
    /// Hold Last selected module data
    /// </summary>
    public List<ContentModel> _ModuleData = new List<ContentModel>();

    public Series selectedSeries;
    public SeriesBooks selectedBooks;

    public bool isNewThemeDownload = false;

    /// <summary>
    /// Store local storage path
    /// </summary>
    [SerializeField] string m_LocalPath;
    public Texture2D SeriesImageTexture; // hold selected series Image texture
    public Texture2D SeriesLogoTexture; // hold selected series Logo texture

    /// <summary>
    /// Store warning window info
    /// </summary>
    [Header("Safety Info")]
    public bool isSafetyWindowOpen = false;
    public SafetyWindow safetyWindow;

    [Header("Logout Confirm")]
    [SerializeField] Animator confrimWindowAnimator;

    [Header("Preparing Theme")]
    [SerializeField] Animator prepareThemeAnimator;

    public string FanArtEmailSubject = "";

    public TMPro.TMP_FontAsset DefaultFont;
    public TMPro.TMP_FontAsset TitleFont;
    public TMPro.TMP_FontAsset DetailFont;

    [Header("ARMarker Info")]
    public bool isMarkerDetailInfoOpen = false;
    [SerializeField] Animator markerWindowAnimator;
    [SerializeField] UnityEngine.UI.Image DialogBox;
    [SerializeField] UnityEngine.UI.Image CloseBoxBtn;
    [SerializeField] UnityEngine.UI.Image WebsiteBoxBtn;
    [SerializeField] TMPro.TMP_Text TitleText;
    [SerializeField] TMPro.TMP_Text message;
    [SerializeField] TMPro.TMP_Text closeBtnText;
    [SerializeField] TMPro.TMP_Text websiteBtnText;

    public string LocalStoragePath
    {
        get => m_LocalPath;
        set => m_LocalPath = value;
    }

    private void Awake()
    {
        PlayerPrefs.SetString("IsThemeSaved", PlayerPrefs.GetString("IsThemeSaved", "false"));
        LocalStoragePath = Application.persistentDataPath + "/LocalStorage/";
        Debug.Log("GameManager: " + LocalStoragePath);
        if (PlayerPrefs.GetInt("IsFreshInstall", 0) == 0)
        {
            Debug.Log("GameManager localstorage folder delete or created ");
            if (Directory.Exists(LocalStoragePath)) {
                Debug.Log("Deleting Old Data");
                Directory.Delete(LocalStoragePath, true);
            }
            Directory.CreateDirectory(LocalStoragePath);

            if (!Directory.Exists(LocalStoragePath + "Theme"))
                Directory.CreateDirectory(LocalStoragePath + "Theme");

        }
        if (Instance == null) {
            Instance = this;
        }

        if (PlayerPrefs.GetInt(StaticKeywords.Login, 0) == 1)
        {
            m_UserData = FileHandler.GetUser();
            if (File.Exists(LocalStoragePath + "Theme/SeriesData.json"))
                selectedSeries = FileHandler.GetSeries();
            if (File.Exists(LocalStoragePath + "Theme/BooksData.json"))
                selectedBooks = FileHandler.GetSeriesBook();
        }
    }

    public string GetThemePath()
    {
        string str = "";
        if (selectedSeries != null)
            str = LocalStoragePath + "Theme/" + selectedSeries.theme.id + "/" + selectedSeries.theme.updated_at_timestamp;

        return str;
    }

    public void UpdateBook(Book book) {
        currentBook = new Book();
        currentBook = book;
        UpdateMappedModuleList();
    }

    public void UpdateMappedModuleList() {
        ModuleList.Instance.ActivateModules(currentBook.b_MapModules);
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void GetModuleData(int moduleID)
    {
        // Call Module Data Api On base of selected ModuleID

        if (moduleID == 4)
        {
            LoadModule(moduleID + 1);
        }
        else {
            ApiManager.Instance.GetModuleData(currentBook.chapter_id, moduleID);
            //ApiManager.Instance.GetModuleData(selectedBooks.id, moduleID);
        }

    }


    /// <summary>
    /// setter for Module content 
    /// </summary>
    /// <param name="content"> Get data from server and Hold here when switch Scene </param>
    public void SetModuleData(List<ContentModel> content , int moduleID) {
        _ModuleData.Clear();
        _ModuleData.AddRange(content);
        LoadModule(moduleID);
    }
    /// <summary>
    /// Getter method 
    /// </summary>
    /// <returns>Return Module data list</returns>
    public List<ContentModel> GetModuleData() {
        return _ModuleData;
    }


   /// <summary>
   /// Load Scene Related to Module
   /// </summary>
   /// <param name="SceneIndex">Same as Load Module + 1 </param>
    public void LoadModule(int SceneIndex)
    {
        if (SceneIndex != 5 || SceneIndex != 8 || SceneIndex != 9 || SceneIndex != 11)
            LoaderUtility.Initialize(); // start arsession subsystem (https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/api/UnityEngine.XR.ARFoundation.LoaderUtility.html)

        Debug.Log("SceneIndex: " + SceneIndex);
        SceneLoader.LoadScene(SceneIndex);
    }

    public string GetLocalPath(string ModuleName) {
        return currentBook.BasePath() + "/" + ModuleName + "/";
    }

    public void callWindowManagerAfterDeleteAddress()
    {
        Debug.Log("callWindowManagerAfterDeleteAddress");
        //WindowManager.Instance.OpenPanel("AddressDetails");Akash
    }

    #region markerDetailInfo
    private void setMarkerInfoTheme()
    {
        ThemeManager.Instance.OnLoadImage(GetThemePath(), StaticKeywords.DialogBox, DialogBox);
        ThemeManager.Instance.OnLoadImage(GetThemePath(), StaticKeywords.DialogBoxBtn, CloseBoxBtn);
        ThemeManager.Instance.OnLoadImage(GetThemePath(), StaticKeywords.DialogBoxBtn, WebsiteBoxBtn);

        Color newCol;
        if (!string.IsNullOrEmpty(selectedSeries.theme.color_code) && ColorUtility.TryParseHtmlString(selectedSeries.theme.color_code, out newCol))
        {
            TitleText.color = newCol;
            message.color = newCol;
            closeBtnText.color = newCol;
            websiteBtnText.color = newCol;
        }

        if (TitleFont != null)
        {
            TitleText.font = TitleFont;
        }

        if (DetailFont != null)
            message.font = DetailFont;
    }

    public void OpenMarkerDetailsWindow()
    {
        if (!isMarkerDetailInfoOpen)
        {
            isMarkerDetailInfoOpen = true;
            setMarkerInfoTheme();
            markerWindowAnimator.Play("Window In");
        }
    }

    public void OpenMarkerWebsite()
    {
        Application.OpenURL("http://www.theworldofagartha.com/");
    }

    public void CloseMarkerWindow()
    {
        markerWindowAnimator.Play("Window Out");
    }
    #endregion

    public Sprite Texture2DToSprite(Texture2D texture)
    {
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        return sprite;
    }

    // Open logout Confirm box 
    public void OpenConfirmWindow()
    {
        confrimWindowAnimator.Play("Window In");
    }

    public void OnConfirmResponseBtn(bool value)
    {
        if (value)
        {
            Debug.Log("<color=red>I am ready to delete all</color>");
            WindowManager.Instance.LogOut();
            SeriesController.Instance.OnRemoveChield();

            Debug.Log(LocalStoragePath);

            if (File.Exists(LocalStoragePath + "Theme/SeriesData.json"))
                File.Delete(LocalStoragePath + "Theme/SeriesData.json");

            if (File.Exists(LocalStoragePath + "Theme/BooksData.json"))
                File.Delete(LocalStoragePath + "Theme/BooksData.json");

            //Todo : HD
            ClearAllData();
            SceneManager.LoadSceneAsync(0);
        }
        confrimWindowAnimator.Play("Window Out");

    }

    // Open Prepare theme box
    bool isPrepared = false;
    public void OpenPrepareThemWindow(bool value)
    {
        isPrepared = value;
        if (value)
            prepareThemeAnimator.Play("Window In");
        else
            prepareThemeAnimator.Play("Window Out");
    }

    public bool isPrepareThemeOpen()
    {
        return isPrepared;
    }



    //Todo : create by hardik shah
    // Clear all data before logout so can load new data on login 
    public void ClearAllData() {

        if (GameManager.Instance) {
            Destroy(this.gameObject);
        }

    }
}
