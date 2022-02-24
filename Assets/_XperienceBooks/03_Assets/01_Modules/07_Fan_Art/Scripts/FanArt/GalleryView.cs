using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class GalleryViewData {
    public string m_ArtistName;
    public string m_ArtDiscription;
}

public class GalleryView : MonoBehaviour
{

    public static GalleryView Instance;

    public int m_CurrentIndex = 0;

    public Transform m_Container;
    public GameObject m_CellPrefab;

    public bool isFanART = false;

    [Header("Fan-Art Theme")]
    [SerializeField] Image HeaderBg;
    [SerializeField] Image emailBtnIcon;
    [SerializeField] TMPro.TMP_Text EmailBtnTxt;

    [Header("EmailBox Theme")]
    [SerializeField] Image BoxBg;
    [SerializeField] Image boxBtnIcon;
    [SerializeField] TMPro.TMP_Text boxBtnTxt;

    string AssetURI;

    public List<ContentModel> ModuleContent = new List<ContentModel>();

    [Header("Email Box")]
    [SerializeField] GameObject emailBox;
    //[SerializeField] Button sendEmailBtn;
    //[SerializeField] Toggle agreeBtn;

    bool isInventoryApiCall = false;
    bool isBackBtn = false;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }

        ModuleContent.AddRange(GameManager.Instance.GetModuleData());
    }

    private void Start()
    {
        LoadData();
    }

    public void onBackBtnClick()
    {
        isBackBtn = true;
        StopAllCoroutines();
    }

    void loadTheme()
    {
        // fan art theme
        HeaderBg.sprite = ThemeManager.Instance.fanArtHeaderBg;
        if (isFanART)
        {
            //Todo : Manage Email theme for Fan ART from here only
            emailBtnIcon.sprite = ThemeManager.Instance.commonBtn;
            
            EmailBtnTxt.font = GameManager.Instance.TitleFont;
            Color newCol;
            if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
                EmailBtnTxt.color = newCol;

            // email box theme
            BoxBg.sprite = ThemeManager.Instance.dialoguebox;
            boxBtnIcon.sprite = ThemeManager.Instance.commonBtn;

            boxBtnTxt.font = GameManager.Instance.TitleFont;
            boxBtnTxt.color = newCol;
        }
    }

    public void AgreeToggleChange()
    {
        //if (agreeBtn.isOn)
        //    sendEmailBtn.interactable = true;
        //else
        //    sendEmailBtn.interactable = false;
    }

    public void OnSendEmailClick()
    {
        string msg = "Add body message and attach Fan Art to this email.";
        string emailTo = "submitfanart@" + GameManager.Instance.selectedSeries.domain;
        
        string email = emailTo;
        string subject = MyEscapeURL(GameManager.Instance.FanArtEmailSubject);
        string body = MyEscapeURL(msg);
        Debug.Log("MSG: " + body);
        Debug.Log("EmailTo: " + email);
        Debug.Log("Subject: " + subject);
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

        //Application.OpenURL("mailto:" + emailTo + "?subject=" + GameManager.Instance.FanArtEmailSubject + "&body=" + msg);
    }

    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }

    public void LoadData() {

        loadTheme();

        if (ModuleContent.Count <= 0)
            return;

        try
        {
            string localPath;

            if (isFanART)
                localPath = GameManager.Instance.GetLocalPath(StaticKeywords.FanArt);//ContentManager.Instance.LocalPath(m_CurrentIndex, StaticKeywords.FanArt);
            else
                localPath = GameManager.Instance.GetLocalPath(StaticKeywords.ImageGallery);//ContentManager.Instance.LocalPath(m_CurrentIndex, StaticKeywords.ImageGallery);

            string fileName = "";
            bool isLocalFile;

            for (int i = 0; i < ModuleContent.Count; i++)
            {

                fileName = ModuleContent[i].filename;
                if (FileHandler.ValidateFile(localPath + fileName))
                {
                    isLocalFile = true;
                    AssetURI = "file://" + GameManager.Instance.LocalStoragePath + localPath + fileName;
                }
                else
                {
                    isLocalFile = false;
                    AssetURI = ModuleContent[i].ar_content;
                }

                GalleryViewData data = new GalleryViewData();
                data.m_ArtistName = ModuleContent[i].artist_name;
                data.m_ArtDiscription = ModuleContent[i].description;

                GameObject cell = Instantiate(m_CellPrefab, m_Container, false);

                cell.GetComponent<GalleryCell>().SetGalleryTexture(isLocalFile, AssetURI, localPath, fileName, data);
            }

            if (!isInventoryApiCall)
            {
                isInventoryApiCall = true;

                if (isFanART)
                    GameManager.Instance.OnCheckToUnlockModule(8);
                else
                    GameManager.Instance.OnCheckToUnlockModule(9);
            }

        }
        catch (Exception ex) {

            Debug.LogError("GalleryView : LoadData " + ex.Message);
        }
    }

    public GameObject fullCardView;
    public CardView fullView;

    public void LoadFullView(Texture2D texture, GalleryViewData data)
    {
        fullCardView.SetActive(true);
        fullView.ActivateFullView(texture, data);
    }
}
