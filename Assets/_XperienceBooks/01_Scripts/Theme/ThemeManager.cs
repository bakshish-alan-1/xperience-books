using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance = null;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    List<string> urls = new List<string>();
    List<string> name = new List<string>();
    int no = 0;
    // Download theame of the selected series if not avialable
    public void SaveSeriesTheame()
    {
        urls.Clear();
        name.Clear();
        string theme = GameManager.Instance.GetThemePath();
        Debug.Log("Download theme: "+ theme);
        // create directory, remove different time stamp theme save on same series
        if (!Directory.Exists(theme))
        {
            Directory.CreateDirectory(theme);
        }
        theme = GameManager.Instance.GetThemePath();

        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.image_path))
        { urls.Add(GameManager.Instance.selectedSeries.image_path); name.Add(theme + "/" + StaticKeywords.SeriesImage); }
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.logo))
        { urls.Add(GameManager.Instance.selectedSeries.theme.logo); name.Add(theme + "/" + StaticKeywords.LogoTheme); }

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
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.notif_icon))
        { urls.Add(GameManager.Instance.selectedSeries.theme.notif_icon); name.Add(theme + "/" + StaticKeywords.NotificationTheme); }
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
            GameManager.Instance.isNewThemeDownload = true;
            GameManager.Instance.OpenPrepareThemWindow(true);

            if (Directory.Exists(theme))
            {
                StartCoroutine(saveTheame(urls[no], name[no]));
            }
            //else
            //    setTheameOfSeries();
        }
    }

    IEnumerator saveTheame(string url, string imageName)
    {
        Debug.Log("url: " + url + ", name: " + imageName);
        
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            File.WriteAllBytes(imageName, request.downloadHandler.data);

            if ((no + 1) < urls.Count)
            {
                no += 1;
                Debug.Log("saveTheame image: " + no);
                StartCoroutine(saveTheame(urls[no], name[no]));
            }
            else
            {
                if (imageName.Equals(StaticKeywords.Font1Theme))
                    GameManager.Instance.TitleFont = onCreateFontAsset(GameManager.Instance.GetThemePath() + "/" + StaticKeywords.Font1Theme);

                if (imageName.Equals(StaticKeywords.Font2Theme))
                    GameManager.Instance.DetailFont = onCreateFontAsset(GameManager.Instance.GetThemePath() + "/" + StaticKeywords.Font2Theme);

                HomeScreen.Instance.OnSetHomePanelData();
            }
        }
    }

    // return TMP_FontAsset object for TextMesh pro text
    public TMP_FontAsset onCreateFontAsset(string myFontPath)
    {
        string fontPaths = myFontPath;
        //Font font = new Font(fontPaths);

        //TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(font);
        return TMP_FontAsset.CreateFontAsset(new Font(myFontPath));
    }

    public void OnLoadImage(string path, string name, Image image)
    {
        if (!File.Exists(path + "/" + name))
            return;

        Texture2D thisTexture = new Texture2D(100, 100);
        byte[] bytes = File.ReadAllBytes(path +"/" + name);
        thisTexture.LoadImage(bytes);
        image.sprite = GameManager.Instance.Texture2DToSprite(thisTexture);
    }
}
