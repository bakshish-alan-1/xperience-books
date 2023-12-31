﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;
using KetosGames.SceneTransition;
using System.Collections;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance = null;

    [SerializeField] TMP_Text progressText;
    public Sprite background, scanBackground, scanBtn, seriesLogo, seriesIcon;
    public Sprite dialoguebox, commonBtn, backBtn, profileIcon, newIcon, inventoryIcon, inventoryPlaceholder, fanArtHeaderBg;
    public Sprite cameraBtn, infoBtn, helpBtn, printfulBtn, cameraSwapBtn;
    public Sprite facebook, insta, youtube, website, twitter;
    public Sprite notificationNextBtn, notificationIcon, notificationTill;

    Sprite textureToSprite = null;
    Texture2D t = null;
    string theme = "";

    public bool isThemeAvailable = true;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

    }

    List<string> urls = new List<string>();
    List<string> name = new List<string>();
    int no = 0;
    float per = 0f;

    // Download theame of the selected series if not avialable
    public void SaveSeriesTheame()
    {
        no = 0;
        urls.Clear();
        name.Clear();
        theme = GameManager.Instance.GetThemePath();
        progressText.text = "0 %";
        GameManager.Instance.TitleFont = null;
        GameManager.Instance.DetailFont = null;

        Debug.Log("Download theme: "+ theme);
        // create directory
        if (!Directory.Exists(theme))
        {
            Directory.CreateDirectory(theme);
        }
        theme = GameManager.Instance.GetThemePath();

        /*if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.image_path))
        { urls.Add(GameManager.Instance.selectedSeries.image_path); name.Add(GameManager.Instance.LocalStoragePath + "/Theme/" + StaticKeywords.SeriesImage); }*/
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.logo))
        { urls.Add(GameManager.Instance.selectedSeries.theme.logo); name.Add(theme + "/" + StaticKeywords.LogoTheme); }

        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.camera_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.camera_icon); name.Add(theme + "/" + StaticKeywords.CameraBtn); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.camera_swap_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.camera_swap_icon); name.Add(theme + "/" + StaticKeywords.CameraSwapBtn); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.info_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.info_icon); name.Add(theme + "/" + StaticKeywords.InfoBtn); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.help_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.help_icon); name.Add(theme + "/" + StaticKeywords.HelpBtn); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.printful_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.printful_icon); name.Add(theme + "/" + StaticKeywords.PrintfulBtn); }

        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.dialog_box))
        { urls.Add(GameManager.Instance.selectedSeries.theme.dialog_box); name.Add(theme + "/" + StaticKeywords.DialogBox); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.dialog_box_btn))
        { urls.Add(GameManager.Instance.selectedSeries.theme.dialog_box_btn); name.Add(theme + "/" + StaticKeywords.DialogBoxBtn); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.scan_qr_bg))
        { urls.Add(GameManager.Instance.selectedSeries.theme.scan_qr_bg); name.Add(theme + "/" + StaticKeywords.Scan_BGTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.fan_art_img_glry))
        { urls.Add(GameManager.Instance.selectedSeries.theme.fan_art_img_glry); name.Add(theme + "/" + StaticKeywords.Fan_art_Img_Bg); }

        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.background_image))
        {  urls.Add(GameManager.Instance.selectedSeries.theme.background_image); name.Add(theme + "/" + StaticKeywords.BGTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.scan_button))
        { urls.Add(GameManager.Instance.selectedSeries.theme.scan_button); name.Add(theme + "/" + StaticKeywords.ScanTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.facebook_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.facebook_icon); name.Add(theme + "/" + StaticKeywords.FacebookTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.youtube_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.youtube_icon); name.Add(theme + "/" + StaticKeywords.YoutubeTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.twitter_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.twitter_icon); name.Add(theme + "/" + StaticKeywords.TwitterTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.instagram_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.instagram_icon); name.Add(theme + "/" + StaticKeywords.InstaTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.website_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.website_icon); name.Add(theme + "/" + StaticKeywords.WebsiteTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.back_button))
        { urls.Add(GameManager.Instance.selectedSeries.theme.back_button); name.Add(theme + "/" + StaticKeywords.BackBtnTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.profile_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.profile_icon); name.Add(theme + "/" + StaticKeywords.ProfileTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.inventory_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.inventory_icon); name.Add(theme + "/" + StaticKeywords.InventoryTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.inventory_placeholder))
        {
            urls.Add(GameManager.Instance.selectedSeries.theme.inventory_placeholder);
            name.Add(theme + "/" + StaticKeywords.InventoryPlaceholderImage);
        }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.notif_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.notif_icon); name.Add(theme + "/" + StaticKeywords.NotificationTheme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.notif_thumbnail))
        { urls.Add(GameManager.Instance.selectedSeries.theme.notif_thumbnail); name.Add(theme + "/" + StaticKeywords.NotificationCellIcon); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.notif_tile))
        { urls.Add(GameManager.Instance.selectedSeries.theme.notif_tile); name.Add(theme + "/" + StaticKeywords.NotificationCellBG); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.notif_next))
        { urls.Add(GameManager.Instance.selectedSeries.theme.notif_next); name.Add(theme + "/" + StaticKeywords.NotificationNextBtn); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_1))
        { urls.Add(GameManager.Instance.selectedSeries.theme.module_1); name.Add(theme + "/" + StaticKeywords.Module1Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_2))
        {   urls.Add(GameManager.Instance.selectedSeries.theme.module_2); name.Add(theme + "/" + StaticKeywords.Module2Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_3))
        {   urls.Add(GameManager.Instance.selectedSeries.theme.module_3); name.Add(theme + "/" + StaticKeywords.Module3Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_4))
        {   urls.Add(GameManager.Instance.selectedSeries.theme.module_4); name.Add(theme + "/" + StaticKeywords.Module4Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_5))
        {    urls.Add(GameManager.Instance.selectedSeries.theme.module_5); name.Add(theme + "/" + StaticKeywords.Module5Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_6))
        {  urls.Add(GameManager.Instance.selectedSeries.theme.module_6); name.Add(theme + "/" + StaticKeywords.Module6Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_7))
        {  urls.Add(GameManager.Instance.selectedSeries.theme.module_7); name.Add(theme + "/" + StaticKeywords.Module7Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_8))
        { urls.Add(GameManager.Instance.selectedSeries.theme.module_8); name.Add(theme + "/" + StaticKeywords.Module8Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_9))
        {   urls.Add(GameManager.Instance.selectedSeries.theme.module_9); name.Add(theme + "/" + StaticKeywords.Module9Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_10))
        {  urls.Add(GameManager.Instance.selectedSeries.theme.module_10); name.Add(theme + "/" + StaticKeywords.Module10Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_11))
        {   urls.Add(GameManager.Instance.selectedSeries.theme.module_11); name.Add(theme + "/" + StaticKeywords.Module11Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.module_12))
        {   urls.Add(GameManager.Instance.selectedSeries.theme.module_12); name.Add(theme + "/" + StaticKeywords.Module12Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.font_h1))
        {  urls.Add(GameManager.Instance.selectedSeries.theme.font_h1); name.Add(theme + "/" + StaticKeywords.Font1Theme); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.font_h2))
        {   urls.Add(GameManager.Instance.selectedSeries.theme.font_h2); name.Add(theme + "/" + StaticKeywords.Font2Theme); }

        if (urls.Count > 0)
        {
            Debug.Log(urls.Count);

            isThemeAvailable = true;

            per = 100 / urls.Count;
            GameManager.Instance.isNewThemeDownload = true;
            GameManager.Instance.OpenPrepareThemWindow(true);

            if (Directory.Exists(theme))
            {
                for (int i = 0; i < urls.Count; i++)
                {
                    OnDownloadTheme(urls[i], name[i]);// async call for download data
                }
            }
        }
        else if (urls.Count == 0)
        {
            PlayerPrefs.SetString("IsThemeSaved", "true");
            isThemeAvailable = false;
            Debug.Log("isThemeAvailable is false now");
            LoadSkinTheme(true);
        }
    }

    //Texture2D texture;
    public async void OnDownloadTheme(string url, string imageName)
    {
        Debug.Log("url: " + url + ", name: " + imageName);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("error: " + request.error + ", code: " + request.responseCode);
            GameManager.Instance.OpenPrepareThemWindow(false);
            ErrorWindow.Instance.SetErrorMessage("Something Went Wrong", request.error, "TRY AGAIN", ErrorWindow.ResponseData.InternetIssue, true);
        }
        else
        {
            File.WriteAllBytes(imageName, request.downloadHandler.data);
            
            if ((no + 1) >= urls.Count)
            {
                PlayerPrefs.SetString("IsThemeSaved", "true");
                StartCoroutine(LoadSkin());
                //LoadSkinTheme();
            }
            else
            {
                no += 1;
                progressText.text = (no * per) + " %";
            }
        }
        request.Dispose();
    }

    public async void SaveSeriesIcon(string url, string path)
    {
        Debug.Log("url: " + url + ", name: " + path);
        if (string.IsNullOrEmpty(url))
        {
            seriesIcon = Resources.Load<Sprite>("NoImage");
            progressText.text = "100 %";
            GameManager.Instance.OpenPrepareThemWindow(false);
            SceneLoader.LoadScene(1);
        }
        else
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                float per1 = request.downloadProgress * 100f;
                int a = (int)per1;
                //Debug.Log(per1.ToString() + ", " + a.ToString());
                progressText.text = a.ToString() + " %";
                await Task.Yield();
            }
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                File.WriteAllBytes(path, request.downloadHandler.data);
                seriesIcon = getTexture(GameManager.Instance.LocalStoragePath + "/Theme/", StaticKeywords.SeriesImage);
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
                    HomeScreen.Instance.onSetBookImage(seriesIcon);
            }
            progressText.text = "100 %";
            request.Dispose();
            GameManager.Instance.OpenPrepareThemWindow(false);
            SceneLoader.LoadScene(1);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.TitleFont == null)
        {
            string myFontPath = GameManager.Instance.GetThemePath() + "/" + StaticKeywords.Font1Theme;
            if (!File.Exists(myFontPath))
                return;

            GameManager.Instance.TitleFont = TMP_FontAsset.CreateFontAsset(new Font(myFontPath));
        }

        if (GameManager.Instance.DetailFont == null)
        {
            string myFontPath1 = GameManager.Instance.GetThemePath() + "/" + StaticKeywords.Font2Theme;
            if (!File.Exists(myFontPath1))
                return;

            GameManager.Instance.DetailFont = TMP_FontAsset.CreateFontAsset(new Font(myFontPath1));
        }
    }

    Sprite getTexture(string path, string name)
    {
        t = new Texture2D(100, 100);
        byte[] b = File.ReadAllBytes(path + "/" + name);
        t.LoadImage(b);

        textureToSprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), 100.0f);

        return textureToSprite;
    }

    // call after download theme
    IEnumerator LoadSkin()
    {
        yield return new WaitForEndOfFrame();
        theme = GameManager.Instance.GetThemePath();

        if (!File.Exists(theme + "/" + StaticKeywords.BGTheme))
            background = Resources.Load<Sprite>("Main_BG");
        else
            background = getTexture(theme, StaticKeywords.BGTheme);
        yield return new WaitForEndOfFrame();

        if (!File.Exists(theme + "/" + StaticKeywords.Scan_BGTheme))
            scanBackground = Resources.Load<Sprite>("MainBG_New");
        else
            scanBackground = getTexture(theme, StaticKeywords.Scan_BGTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.ScanTheme))
            scanBtn = Resources.Load<Sprite>("scan_button");
        else
            scanBtn = getTexture(theme, StaticKeywords.ScanTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.LogoTheme))
            seriesLogo = Resources.Load<Sprite>("transparentImage");
        else
            seriesLogo = getTexture(theme, StaticKeywords.LogoTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(GameManager.Instance.LocalStoragePath + "/Theme/" + StaticKeywords.SeriesImage) || string.IsNullOrEmpty(GameManager.Instance.selectedBooks.image))
            seriesIcon = Resources.Load<Sprite>("NoImage");
        else
            seriesIcon = getTexture(GameManager.Instance.LocalStoragePath + "/Theme/", StaticKeywords.SeriesImage);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.DialogBox))
            dialoguebox = Resources.Load<Sprite>("DialogBox");
        else
            dialoguebox = getTexture(theme, StaticKeywords.DialogBox);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.Fan_art_Img_Bg))
            fanArtHeaderBg = Resources.Load<Sprite>("transparentImage");
        else
            fanArtHeaderBg = getTexture(theme, StaticKeywords.Fan_art_Img_Bg);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.DialogBoxBtn))
            commonBtn = Resources.Load<Sprite>("StoneBack");
        else
            commonBtn = getTexture(theme, StaticKeywords.DialogBoxBtn);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.BackBtnTheme))
            backBtn = Resources.Load<Sprite>("btn_back_white");
        else
            backBtn = getTexture(theme, StaticKeywords.BackBtnTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.CameraBtn))
            cameraBtn = Resources.Load<Sprite>("btn_camera");
        else
            cameraBtn = getTexture(theme, StaticKeywords.CameraBtn);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.CameraSwapBtn))
            cameraSwapBtn = Resources.Load<Sprite>("btn_camera_swap");
        else
            cameraSwapBtn = getTexture(theme, StaticKeywords.CameraSwapBtn);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.InfoBtn))
            infoBtn = Resources.Load<Sprite>("InfoBtn");
        else
            infoBtn = getTexture(theme, StaticKeywords.InfoBtn);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.HelpBtn))
            helpBtn = Resources.Load<Sprite>("help");
        else
            helpBtn = getTexture(theme, StaticKeywords.HelpBtn);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.PrintfulBtn))
            printfulBtn = Resources.Load<Sprite>("printful");
        else
            printfulBtn = getTexture(theme, StaticKeywords.PrintfulBtn);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.ProfileTheme))
            profileIcon = Resources.Load<Sprite>("Profile");
        else
            profileIcon = getTexture(theme, StaticKeywords.ProfileTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.NotificationTheme))
            newIcon = Resources.Load<Sprite>("Notification");
        else
            newIcon = getTexture(theme, StaticKeywords.NotificationTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.InventoryTheme))
            inventoryIcon = Resources.Load<Sprite>("Inventory");
        else
            inventoryIcon = getTexture(theme, StaticKeywords.InventoryTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.InventoryPlaceholderImage))
            inventoryPlaceholder = Resources.Load<Sprite>("InventoryPlaceHolder");
        else
            inventoryPlaceholder = getTexture(theme, StaticKeywords.InventoryPlaceholderImage);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.FacebookTheme))
            facebook = Resources.Load<Sprite>("Facebook");
        else
            facebook = getTexture(theme, StaticKeywords.FacebookTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.InstaTheme))
            insta = Resources.Load<Sprite>("Insta");
        else
            insta = getTexture(theme, StaticKeywords.InstaTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.YoutubeTheme))
            youtube = Resources.Load<Sprite>("Youtube");
        else
            youtube = getTexture(theme, StaticKeywords.YoutubeTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.WebsiteTheme))
            website = Resources.Load<Sprite>("Website");
        else
            website = getTexture(theme, StaticKeywords.WebsiteTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.TwitterTheme))
            twitter = Resources.Load<Sprite>("Twitter");
        else
            twitter = getTexture(theme, StaticKeywords.TwitterTheme);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.NotificationNextBtn))
            notificationNextBtn = Resources.Load<Sprite>("NotificationNext");
        else
            notificationNextBtn = getTexture(theme, StaticKeywords.NotificationNextBtn);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.NotificationCellIcon))
            notificationIcon = Resources.Load<Sprite>("New_Icon");
        else
            notificationIcon = getTexture(theme, StaticKeywords.NotificationCellIcon);

        yield return new WaitForEndOfFrame();
        if (!File.Exists(theme + "/" + StaticKeywords.NotificationCellBG))
            notificationTill = Resources.Load<Sprite>("StoneBack");
        else
            notificationTill = getTexture(theme, StaticKeywords.NotificationCellBG);

        progressText.text = "100 %";

        urls.Clear();
        name.Clear();
        urls.TrimExcess();
        name.TrimExcess(); 
        SaveSeriesIcon(GameManager.Instance.selectedBooks.image, GameManager.Instance.LocalStoragePath + "Theme/" + StaticKeywords.SeriesImage);
        //SceneLoader.LoadScene(1);
    }

    // call from loadMainScene, BookData script and current script as well
    public void LoadSkinTheme(bool isDownloadSeriesIcon)
    {
        Debug.Log("ThemeManager LoadTheme: " + isDownloadSeriesIcon);
        theme = GameManager.Instance.GetThemePath();
        if (isDownloadSeriesIcon)
            GameManager.Instance.OpenPrepareThemWindow(true);
        progressText.text = "10 %";

        if (!File.Exists(theme + "/" + StaticKeywords.BGTheme))
            background = Resources.Load<Sprite>("Main_BG");
        else
            background = getTexture(theme, StaticKeywords.BGTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.Scan_BGTheme))
            scanBackground = Resources.Load<Sprite>("MainBG_New");
        else
            scanBackground = getTexture(theme, StaticKeywords.Scan_BGTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.ScanTheme))
            scanBtn = Resources.Load<Sprite>("scan_button");
        else
            scanBtn = getTexture(theme, StaticKeywords.ScanTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.CameraBtn))
            cameraBtn = Resources.Load<Sprite>("btn_camera");
        else
            cameraBtn = getTexture(theme, StaticKeywords.CameraBtn);

        if (!File.Exists(theme + "/" + StaticKeywords.CameraSwapBtn))
            cameraSwapBtn = Resources.Load<Sprite>("btn_camera_swap");
        else
            cameraSwapBtn = getTexture(theme, StaticKeywords.CameraSwapBtn);

        if (!File.Exists(theme + "/" + StaticKeywords.InfoBtn))
            infoBtn = Resources.Load<Sprite>("InfoBtn");
        else
            infoBtn = getTexture(theme, StaticKeywords.InfoBtn);

        if (!File.Exists(theme + "/" + StaticKeywords.HelpBtn))
            helpBtn = Resources.Load<Sprite>("help");
        else
            helpBtn = getTexture(theme, StaticKeywords.HelpBtn);

        if (!File.Exists(theme + "/" + StaticKeywords.PrintfulBtn))
            printfulBtn = Resources.Load<Sprite>("printful");
        else
            printfulBtn = getTexture(theme, StaticKeywords.PrintfulBtn);

        if (!File.Exists(theme + "/" + StaticKeywords.LogoTheme))
            seriesLogo = Resources.Load<Sprite>("transparentImage");
        else
            seriesLogo = getTexture(theme, StaticKeywords.LogoTheme);

        progressText.text = "50 %";

        if (!File.Exists(GameManager.Instance.LocalStoragePath + "/Theme/" + StaticKeywords.SeriesImage) || string.IsNullOrEmpty(GameManager.Instance.selectedBooks.image))
            seriesIcon = Resources.Load<Sprite>("NoImage");
        else
            seriesIcon = getTexture(GameManager.Instance.LocalStoragePath + "/Theme/", StaticKeywords.SeriesImage);

        if (!File.Exists(theme + "/" + StaticKeywords.DialogBox))
            dialoguebox = Resources.Load<Sprite>("DialogBox");
        else
            dialoguebox = getTexture(theme, StaticKeywords.DialogBox);

        if (!File.Exists(theme + "/" + StaticKeywords.Fan_art_Img_Bg))
            fanArtHeaderBg = Resources.Load<Sprite>("transparentImage");
        else
            fanArtHeaderBg = getTexture(theme, StaticKeywords.Fan_art_Img_Bg);

        if (!File.Exists(theme + "/" + StaticKeywords.DialogBoxBtn))
            commonBtn = Resources.Load<Sprite>("StoneBack");
        else
            commonBtn = getTexture(theme, StaticKeywords.DialogBoxBtn);

        if (!File.Exists(theme + "/" + StaticKeywords.BackBtnTheme))
            backBtn = Resources.Load<Sprite>("btn_back_white");
        else
            backBtn = getTexture(theme, StaticKeywords.BackBtnTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.ProfileTheme))
            profileIcon = Resources.Load<Sprite>("Profile");
        else
            profileIcon = getTexture(theme, StaticKeywords.ProfileTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.NotificationTheme))
            newIcon = Resources.Load<Sprite>("Notification");
        else
            newIcon = getTexture(theme, StaticKeywords.NotificationTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.InventoryTheme))
            inventoryIcon = Resources.Load<Sprite>("Inventory");
        else
            inventoryIcon = getTexture(theme, StaticKeywords.InventoryTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.InventoryPlaceholderImage))
            inventoryPlaceholder = Resources.Load<Sprite>("InventoryPlaceHolder");
        else
            inventoryPlaceholder = getTexture(theme, StaticKeywords.InventoryPlaceholderImage);

        if (!File.Exists(theme + "/" + StaticKeywords.FacebookTheme))
            facebook = Resources.Load<Sprite>("Facebook");
        else
            facebook = getTexture(theme, StaticKeywords.FacebookTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.InstaTheme))
            insta = Resources.Load<Sprite>("Insta");
        else
            insta = getTexture(theme, StaticKeywords.InstaTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.YoutubeTheme))
            youtube = Resources.Load<Sprite>("Youtube");
        else
            youtube = getTexture(theme, StaticKeywords.YoutubeTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.WebsiteTheme))
            website = Resources.Load<Sprite>("Website");
        else
            website = getTexture(theme, StaticKeywords.WebsiteTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.TwitterTheme))
            twitter = Resources.Load<Sprite>("Twitter");
        else
            twitter = getTexture(theme, StaticKeywords.TwitterTheme);

        if (!File.Exists(theme + "/" + StaticKeywords.NotificationNextBtn))
            notificationNextBtn = Resources.Load<Sprite>("NotificationNext");
        else
            notificationNextBtn = getTexture(theme, StaticKeywords.NotificationNextBtn);

        if (!File.Exists(theme + "/" + StaticKeywords.NotificationCellIcon))
            notificationIcon = Resources.Load<Sprite>("New_Icon");
        else
            notificationIcon = getTexture(theme, StaticKeywords.NotificationCellIcon);

        if (!File.Exists(theme + "/" + StaticKeywords.NotificationCellBG))
            notificationTill = Resources.Load<Sprite>("StoneBack");
        else
            notificationTill = getTexture(theme, StaticKeywords.NotificationCellBG);


        urls.Clear();
        name.Clear();

        if (isDownloadSeriesIcon)
            SaveSeriesIcon(GameManager.Instance.selectedBooks.image, GameManager.Instance.LocalStoragePath + "Theme/" + StaticKeywords.SeriesImage);
        else
        {
            progressText.text = "100 %";
            SceneLoader.LoadScene(1);
        }
    }
}
