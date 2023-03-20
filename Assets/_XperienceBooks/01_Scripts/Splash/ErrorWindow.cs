using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ErrorWindow : MonoBehaviour
{

    public enum ResponseData {

        JustClose,
        InternetIssue,
        SessionExpire

    }

    public static ErrorWindow Instance = null;

    [SerializeField] Image BG, ButtonImg;
    [SerializeField]
    TextMeshProUGUI TitleText, Message, ButtonText;

    public Animator currentWindowAnimator;

    public string windowFadeIn = "Window In";
    public string windowFadeOut = "Window Out";


    public ResponseData response;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    void setBGTheme(bool isDefault)
    {
        BG.sprite = (ThemeManager.Instance.dialoguebox);
        ButtonImg.sprite = (ThemeManager.Instance.commonBtn);
        Color newCol;
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.color_code))
        {
            if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            {
                TitleText.color = newCol;
                Message.color = newCol;
                ButtonText.color = newCol;
            }
        }
        else
        {
            Message.color = Color.white;
            TitleText.color = Color.white;
            ButtonText.color = new Color32(95, 93, 170, 255);
        }

        if (GameManager.Instance.TitleFont != null)
        {
            TitleText.font = GameManager.Instance.TitleFont;
        }
        else
            TitleText.font = GameManager.Instance.DefaultFont;

        if (GameManager.Instance.DetailFont != null)
        {
            ButtonText.font = GameManager.Instance.DetailFont;
            Message.font = GameManager.Instance.DetailFont;
        }
        else
        {
            Message.font = GameManager.Instance.DefaultFont;
            ButtonText.font = GameManager.Instance.DefaultFont;
        }
    }

    public void SetErrorMessage(string title, string message, string buttonText,ResponseData res, bool isDefault) {

        setBGTheme(isDefault);

        TitleText.text = title;
        Message.text = message;
        ButtonText.text = buttonText;
        currentWindowAnimator.Play(windowFadeIn);
        response = res;
    }


    public void CloseWindow() {
        TitleText.text = string.Empty;
        Message.text = string.Empty;
        ButtonText.text = string.Empty;
        currentWindowAnimator.Play(windowFadeOut);

        if (string.Equals(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, "01_Home"))
        {
            if (WindowManager.Instance.getCurrentWindowName().Equals("QRScan"))
                QRScanController.Instance.Play();
        }

        if (response == ResponseData.SessionExpire) {
            WindowManager.Instance.LogOut();
        }

        if (response == ResponseData.InternetIssue) {
            ApiManager.Instance.Start();
        }
    }
}
