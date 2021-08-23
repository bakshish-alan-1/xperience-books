using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    public static HomeScreen Instance = null;

    [Header("Login Panel Social Media Link")]
    [SerializeField] string instaLink = "";
    [SerializeField] string websiteLink = "";
    [SerializeField] string twitterLink = "";
    [SerializeField] string youtubeLink = "";
    [SerializeField] string facebookLink = "";

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OnBackBtnClick()
    {
        if (WindowManager.Instance.windows[WindowManager.Instance.currentWindowIndex].windowName == StaticKeywords.HomePanel)
            ApiManager.Instance.GetSeriesDetails(GameManager.Instance.selectedSeries.id);//ApiManager.Instance.GetSeriesList();
        else if (WindowManager.Instance.windows[WindowManager.Instance.currentWindowIndex].windowName == "Series")
            WindowManager.Instance.LogOut();
        else if (WindowManager.Instance.windows[WindowManager.Instance.currentWindowIndex].windowName == "Register")
            WindowManager.Instance.OpenPanel("Login");
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

    // set selected series logo and title in home panel
    public void OnSetHomePanelData()
    {
        Debug.Log("inside OnSetHomePanelData");
        if (GameManager.Instance.TitleFont == null)
            GameManager.Instance.TitleFont = ThemeManager.Instance.onCreateFontAsset(GameManager.Instance.GetThemePath() + "/" + StaticKeywords.Font1Theme);

        if (GameManager.Instance.DetailFont == null)
            GameManager.Instance.DetailFont = ThemeManager.Instance.onCreateFontAsset(GameManager.Instance.GetThemePath() + "/" + StaticKeywords.Font2Theme);

        if (GameManager.Instance.SeriesImageTexture != null)
            seriesImage.sprite = GameManager.Instance.Texture2DToSprite(GameManager.Instance.SeriesImageTexture);
        else if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.image_path))
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.SeriesImage, seriesImage);

        if (GameManager.Instance.SeriesLogoTexture != null)
            seriesLogo.sprite = GameManager.Instance.Texture2DToSprite(GameManager.Instance.SeriesLogoTexture);
        else if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.logo))
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.LogoTheme, seriesLogo);
        
        Title.text = "";
        OnSetHomeScreenSocialMediaObject();
        setHomeTheameOfSeries();
    }

    public void setHomeTheameOfSeries()
    {
        Debug.Log("Inside setTheameOfSeries");
        string theme = GameManager.Instance.GetThemePath();

        ThemeManager.Instance.OnLoadImage(theme, StaticKeywords.BGTheme, Bg);
        ThemeManager.Instance.OnLoadImage(theme, StaticKeywords.ScanTheme, scaneIcon);
        ThemeManager.Instance.OnLoadImage(theme, StaticKeywords.BackBtnTheme, backIcon);
        ThemeManager.Instance.OnLoadImage(theme, StaticKeywords.ProfileTheme, profileIcon);
        ThemeManager.Instance.OnLoadImage(theme, StaticKeywords.NotificationTheme, notificationIcon);

        ScanBtnTxt.font = GameManager.Instance.TitleFont;
        Color newCol;
        if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            ScanBtnTxt.color = newCol;

        GameManager.Instance.OpenPrepareThemWindow(false);
        WindowManager.Instance.OpenPanel(StaticKeywords.HomePanel);
    }

    // set the position of button as per the no of links
    public void OnSetHomeScreenSocialMediaObject()
    {
        int nooflinks = 0;
        socialMedia.Clear();
        string themePath = GameManager.Instance.GetThemePath();

        ThemeManager.Instance.OnLoadImage(themePath, StaticKeywords.FacebookTheme, facebookIcon);
        ThemeManager.Instance.OnLoadImage(themePath, StaticKeywords.TwitterTheme, twitterIcon);
        ThemeManager.Instance.OnLoadImage(themePath, StaticKeywords.WebsiteTheme, websiteIcon);
        ThemeManager.Instance.OnLoadImage(themePath, StaticKeywords.InstaTheme, instaIcon);

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
}
