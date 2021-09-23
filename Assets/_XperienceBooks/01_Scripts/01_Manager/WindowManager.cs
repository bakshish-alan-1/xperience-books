using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowManager : MonoBehaviour
{

    public static WindowManager Instance;

    [System.Serializable]
    public class WindowItem
    {
        public string windowName = "My Window";
        public GameObject windowObject;
    }

    public List<WindowItem> windows = new List<WindowItem>();

    public List<WindowItem> notification = new List<WindowItem>();

    public int currentWindowIndex = 0;
    public int previousWindowIndex = 0;
    private int newWindowIndex;

    public TMP_Text title;
    public GameObject WhiteCommonBG, BackBtn, socialMediaCanvas;
    public string windowFadeIn = "Window In";
    public string windowFadeOut = "Window Out";

    private GameObject currentWindow;
    private GameObject nextWindow;

    private Animator currentWindowAnimator;
    private Animator nextWindowAnimator;


    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }

        if (PlayerPrefs.GetInt(StaticKeywords.Login, 0) == 1)// 1 = login, 0 = logout
        {
            if (PlayerPrefs.GetString("IsThemeSaved").Equals("false") || GameManager.Instance.selectedSeries.id == -1 || GameManager.Instance.selectedBooks.id == -1)
                currentWindowIndex = 5;// redirect to series screen
            else
                currentWindowIndex = 7;// redirect to home screen
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Back to Content screen if Already scan once 
        if (GlobalControl.Instance.scanComplete)
        {
            Debug.Log("scanComplete");
            currentWindowIndex = 11;
            GameManager.Instance.UpdateMappedModuleList();
            HomeScreen.Instance.OnSetHomeScreenSocialMediaObject();
            SocialMediaList(StaticKeywords.ContentList);
        }

        currentWindow = windows[currentWindowIndex].windowObject;
        currentWindowAnimator = currentWindow.GetComponent<Animator>();
        currentWindowAnimator.Play(windowFadeIn);

        if (currentWindowIndex != 0)
            title.text = windows[currentWindowIndex].windowName;

        if (PlayerPrefs.GetInt(StaticKeywords.Login, 0) == 1 && !GlobalControl.Instance.scanComplete)
        {
            if (PlayerPrefs.GetString("IsThemeSaved").Equals("false") || GameManager.Instance.selectedSeries.id == -1 || GameManager.Instance.selectedBooks.id == -1)
            {
                ApiManager.Instance.GetSeriesList();
            }
            else
            {
                HomeScreen.Instance.OnSetHomePanelData();// if user already login then direct set home panel theme as series selected
            }
        }
    }

    public void RegisterBtnClick()
    {
        PlayerPrefs.SetInt("IsAgreed", PlayerPrefs.GetInt("IsAgreed", 0));
        if (PlayerPrefs.GetInt("IsAgreed") == 1)
            OpenPanel("Register");
        else
            OpenPanel("PrivacyPolicy");
    }

    void SocialMediaList(string name)
    {
        if (name.Equals(StaticKeywords.HomePanel) || name.Equals(StaticKeywords.ContentList))
            socialMediaCanvas.SetActive(true);
        else
            socialMediaCanvas.SetActive(false);
    }

    void OnExtraSettings(string newPanel)
    {
        title.text = "";
        if (newPanel.Equals("QRScan"))
            WhiteCommonBG.SetActive(false);
        else if (!newPanel.Equals("Login"))
            title.text = newPanel;

        if (newPanel.Equals("Login") || newPanel.Equals("ChangePassword"))
        { BackBtn.SetActive(false); }
        else
            BackBtn.SetActive(true);

        if (windows[newWindowIndex].windowName.Equals(StaticKeywords.HomePanel))
        {
            title.text = "";
        }

        SocialMediaList(newPanel);
    }

    public void OpenPanel(string newPanel)
    {
        OnExtraSettings(newPanel);

        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].windowName == newPanel)
                newWindowIndex = i;
        }

        if (newWindowIndex != currentWindowIndex)
        {
            currentWindow = windows[currentWindowIndex].windowObject;
            previousWindowIndex = currentWindowIndex;
            currentWindowIndex = newWindowIndex;
            nextWindow = windows[currentWindowIndex].windowObject;

            currentWindowAnimator = currentWindow.GetComponent<Animator>();
            nextWindowAnimator = nextWindow.GetComponent<Animator>();

            currentWindowAnimator.Play(windowFadeOut);
            nextWindowAnimator.Play(windowFadeIn);
        }
    }

    public void BackToPreviousWindow() {

        newWindowIndex = previousWindowIndex;

        if (newWindowIndex != currentWindowIndex)
        {
            if (windows[newWindowIndex].windowName.Equals("Login") || windows[newWindowIndex].windowName.Equals("ChangePassword") || windows[newWindowIndex].windowName.Equals("QRScan"))
            {
                BackBtn.SetActive(false);
                if (windows[newWindowIndex].windowName.Equals("Login") || windows[newWindowIndex].windowName.Equals("QRScan"))
                    title.text = "";
            }
            else if (windows[newWindowIndex].windowName.Equals(StaticKeywords.HomePanel))
            {
                title.text = "";
            }
            else
            {
                BackBtn.SetActive(true);
                title.text = windows[newWindowIndex].windowName;
            }
            SocialMediaList(windows[newWindowIndex].windowName);
            currentWindow = windows[currentWindowIndex].windowObject;
            
            previousWindowIndex = currentWindowIndex;
            currentWindowIndex = newWindowIndex;
            nextWindow = windows[currentWindowIndex].windowObject;

            currentWindowAnimator = currentWindow.GetComponent<Animator>();
            nextWindowAnimator = nextWindow.GetComponent<Animator>();

            currentWindowAnimator.Play(windowFadeOut);
            nextWindowAnimator.Play(windowFadeIn);
        }
    }

    int currentErrorPanel;
    public void OpenErrorPanel(string panelName) {

        for (int i = 0; i < notification.Count; i++)
        {
            if (notification[i].windowName == panelName)
                currentErrorPanel = i;
        }

        notification[currentErrorPanel].windowObject.GetComponent<Animator>().Play(windowFadeIn);
    }

    public void CloseErrorPanel() {

        notification[currentErrorPanel].windowObject.GetComponent<Animator>().Play(windowFadeOut);
    }

    public void LogOut()
    {

        //ToDo : Clear all playerPref
        PlayerPrefs.DeleteAll();

        //Clear all user Data



        PlayerPrefs.SetInt(StaticKeywords.Login, 0);
        OpenPanel("Login");
    }
}



