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
        if (isDefault)
        {
            Debug.Log("set them for error msg default");
            BG.sprite = Resources.Load<Sprite>("DialogBox");
            ButtonImg.sprite = Resources.Load<Sprite>("StoneBack");

            Message.font = GameManager.Instance.DefaultFont;
            TitleText.font = GameManager.Instance.DefaultFont;
            ButtonText.font = GameManager.Instance.DefaultFont;

            Message.color = Color.white;
            TitleText.color = Color.white;
            ButtonText.color = new Color32(95, 93, 170, 255);
        }
        else
        {
            Debug.Log("set them for error msg");
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.DialogBox, BG);
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.DialogBoxBtn, ButtonImg);
            Color newCol;
            if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            {
                TitleText.color = newCol;
                Message.color = newCol;
                ButtonText.color = newCol;
            }

            if (GameManager.Instance.TitleFont != null)
            {
                TitleText.font = GameManager.Instance.TitleFont;
                ButtonText.font = GameManager.Instance.TitleFont;
            }

            if (GameManager.Instance.DetailFont != null)
                Message.font = GameManager.Instance.DetailFont;
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

        if (response == ResponseData.SessionExpire) {
            WindowManager.Instance.LogOut();
        }

        if (response == ResponseData.InternetIssue) {
            ApiManager.Instance.Start();
        }
    }
}
