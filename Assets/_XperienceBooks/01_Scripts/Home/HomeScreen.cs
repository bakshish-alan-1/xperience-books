using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    public static HomeScreen Instance = null;

    [SerializeField] InputField mytoken;

    [Header("Login Panel Social Media Link")]
    [SerializeField] string instaLink = "";
    [SerializeField] string websiteLink = "";
    [SerializeField] string twitterLink = "";
    [SerializeField] string youtubeLink = "";
    [SerializeField] string facebookLink = "";

    [SerializeField] GameObject backBtn;// this is common back btn for all screen before series selection screen

    [SerializeField] GameObject Series;
    [SerializeField] GameObject Books;

    [SerializeField] GameObject insta, facebook, website, twitter, youtube;
    List<GameObject> socialMedia = new List<GameObject>();

    [Header("HomePanel")]
    [SerializeField] Image seriesLogo;
    [SerializeField] Image seriesImage;
    [SerializeField] TMPro.TMP_Text Title;

    [SerializeField] Image Bg, scaneIcon, backIcon, profileIcon, notificationIcon;
    [SerializeField] Image facebookIcon, youtubeIcon, twitterIcon, websiteIcon, instaIcon;
    [SerializeField] TMPro.TMP_Text ScanBtnTxt;

    bool isThemeSet = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Debug.Log("IsThemeSaved: "+ PlayerPrefs.GetString("IsThemeSaved"));
    }

    private void Start()
    {
        
    }

    public void OnScanBtnHit()
    {
        if (GameManager.Instance.IsCameraPermissionGranted())
        {
            GameManager.Instance.buyFromPrintfull = false;
            backBtn.SetActive(false);
            QRScanController.Instance.OnsetScanQRInfo("Scan QR");
            WindowManager.Instance.OpenPanel("QRScan");
            QRScanController.Instance.Play();
        }
        else
        {
            string msg = "Camera permission is required in order to use this application.\n\nIf you have denied the camera permission then please go to settings and give the camera permission manually. Otherwise you will not be able to proceed ahead in the application.";
            ErrorWindow.Instance.SetErrorMessage("", msg, "OKAY", ErrorWindow.ResponseData.JustClose, false);
        }
    }

    public void OnBackBtnClick()
    {
        if (WindowManager.Instance.windows[WindowManager.Instance.currentWindowIndex].windowName == "ForgotPassword")
        {
            ForgotPassword.Instance.ResetField();
        }

        if (WindowManager.Instance.windows[WindowManager.Instance.currentWindowIndex].windowName == StaticKeywords.HomePanel)
        {
            isThemeSet = false;
            ApiManager.Instance.GetSeriesDetails(GameManager.Instance.selectedSeries.id);
        }
        else if (WindowManager.Instance.windows[WindowManager.Instance.currentWindowIndex].windowName == "Series")
            WindowManager.Instance.LogOut();
        else if (WindowManager.Instance.windows[WindowManager.Instance.currentWindowIndex].windowName == "Register")
        { PlayerPrefs.SetInt("IsAgreed", 0); WindowManager.Instance.OpenPanel("Login"); }
        else
            WindowManager.Instance.BackToPreviousWindow();

    }

    public void OnSetSeriesData()
    {
        Series.GetComponent<SeriesController>().OnRemoveChield();
        Series.GetComponent<SeriesController>().SetSeriesIcons();
    }

    public void OnSetBooksData()
    {
        Books.GetComponent<BookController>().OnRemoveChield();
        Books.GetComponent<BookController>().SetBookIcons();
    }

    // call from login panel social media
    public void OnSocialMediaHit(int index)
    {
        switch(index)
        {
            case 0://Insta
                {
                    Application.OpenURL(instaLink);
                }
                break;
            case 1://Website
                {
                    Application.OpenURL(websiteLink);
                }
                break;
            case 2://Twitter
                {
                    Application.OpenURL(twitterLink);
                }
                break;
            case 3://Youtube
                {
                    Application.OpenURL(youtubeLink);
                }
                break;
            case 4://facebook
                {
                    Application.OpenURL(facebookLink);
                }
                break;
        }
    }

    // call from themeManager script after download theme
    public void OnSetHomePanelData()
    {
        Debug.Log("inside OnSetHomePanelData");
        if (isThemeSet)
        {
            WindowManager.Instance.OpenPanel(StaticKeywords.HomePanel);
            return;
        }

        Title.text = "";
        OnSetHomeScreenSocialMediaObject();
        setHomeTheameOfSeries();// call to set home window images as per skin data received
        GameManager.Instance.OpenMarkerDetailsWindow();// instruction window to provide details of marker images

        NotificationPanel.Instance.OnSetThem();// set downloaded theme for notification panel
        Profile.Instance.OnSetThem();// set theme for profile screen
    }

    public void onSetBookImage(Sprite img)
    {
        seriesImage.sprite = img;
    }

    public void setHomeTheameOfSeries()
    {
        Debug.Log("Inside setTheameOfSeries");
        string theme = GameManager.Instance.GetThemePath();

        Bg.sprite = (ThemeManager.Instance.background);
        scaneIcon.sprite = (ThemeManager.Instance.scanBtn);
        backIcon.sprite = (ThemeManager.Instance.backBtn);
        profileIcon.sprite = (ThemeManager.Instance.profileIcon);
        notificationIcon.sprite = (ThemeManager.Instance.newIcon);
        seriesImage.sprite = (ThemeManager.Instance.seriesIcon);
        seriesLogo.sprite = (ThemeManager.Instance.seriesLogo);

        ScanBtnTxt.font = GameManager.Instance.DetailFont;
        Color newCol;
        if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            ScanBtnTxt.color = newCol;

        GameManager.Instance.OpenPrepareThemWindow(false);
        isThemeSet = true;
        WindowManager.Instance.OpenPanel(StaticKeywords.HomePanel);

        ApiManager.Instance.GetNotificationList();
    }

    // set the position of button as per the no of links
    public void OnSetHomeScreenSocialMediaObject()
    {
        int nooflinks = 0;
        socialMedia.Clear();
        string themePath = GameManager.Instance.GetThemePath();

        facebookIcon.sprite = (ThemeManager.Instance.facebook);
        twitterIcon.sprite = (ThemeManager.Instance.twitter);
        websiteIcon.sprite = (ThemeManager.Instance.website);
        instaIcon.sprite = (ThemeManager.Instance.insta);
        youtubeIcon.sprite = (ThemeManager.Instance.youtube);

        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.instagram_link))
        { nooflinks += 1; socialMedia.Add(insta); insta.SetActive(false); }
        
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.website_link))
        { nooflinks += 1; socialMedia.Add(website); website.SetActive(false); }
        
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.facebook_link))
        { nooflinks += 1; socialMedia.Add(facebook); facebook.SetActive(false); }
        
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.twitter_link))
        { nooflinks += 1; socialMedia.Add(twitter); twitter.SetActive(false); }
        
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.youtube_link ))
        { nooflinks += 1; socialMedia.Add(youtube); youtube.SetActive(false); }

        Debug.Log("Total Social media links of series are: " + nooflinks + ", " + socialMedia.Count);
        int gap = 0;
        if (nooflinks == 2 || nooflinks == 4)
            gap = 60;

        for (int i = 0; i < nooflinks; i++)
        {
            socialMedia[i].SetActive(true);
            if (nooflinks == 1 || nooflinks == 3 || nooflinks == 5)
            {
                if (i % 2 == 0)
                    socialMedia[i].transform.localPosition = new Vector3(gap, socialMedia[i].transform.localPosition.y, 0f);
                else
                    socialMedia[i].transform.localPosition = new Vector3(-gap, socialMedia[i].transform.localPosition.y, 0f);

                if (i == 0 || i == 2)
                    gap += 120;
            }
            else
            {
                if (i % 2 == 0)
                    socialMedia[i].transform.localPosition = new Vector3(gap, socialMedia[i].transform.localPosition.y, 0f);
                else
                    socialMedia[i].transform.localPosition = new Vector3(-gap, socialMedia[i].transform.localPosition.y, 0f);

                if (i == 1)
                    gap += 120;
            }
        }
    }

    // call from home panel social media
    public void OnHomeScreenSocialMedia(int index)
    {
        switch (index)
        {
            case 0://Insta
                {
                    Application.OpenURL(GameManager.Instance.selectedSeries.instagram_link);
                }
                break;
            case 1://Website
                {
                    Application.OpenURL(GameManager.Instance.selectedSeries.website_link);
                }
                break;
            case 2://Twitter
                {
                    Application.OpenURL(GameManager.Instance.selectedSeries.twitter_link);
                }
                break;
            case 3://Youtube
                {
                    Application.OpenURL(GameManager.Instance.selectedSeries.youtube_link);
                }
                break;
            case 4://facebook
                {
                    Application.OpenURL(GameManager.Instance.selectedSeries.facebook_link);
                }
                break;
        }
    }

    private void Update()
    {
        mytoken.text = GameManager.Instance.FirebaseToken;
    }
}
