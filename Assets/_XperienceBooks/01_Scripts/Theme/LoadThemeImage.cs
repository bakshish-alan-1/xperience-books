using UnityEngine;
using UnityEngine.UI;

public class LoadThemeImage : MonoBehaviour
{
    // used in arscene for provides info about hashtag data

    [Header("Set Theme")]
    public Image backIcon;
    public Image cameraIcon, cameraSwapIcon;
    public Image printfulIcon;

    [Header("Info Popup")]
    public Image info_DialogBox;
    public Image info_DialogButton, infoIcon;
    public TMPro.TMP_Text info_DialogTitle;
    public TMPro.TMP_Text info_DialogText;
    public TMPro.TMP_Text info_ButtonText;

    [Header("Help Popup")]
    public Image DialogBox;
    public Image DialogButton, helpIcon;
    public TMPro.TMP_Text DialogTitle;
    public TMPro.TMP_Text DialogText;
    public TMPro.TMP_Text ButtonText;

    private void Start()
    {
        string path = GameManager.Instance.GetThemePath();

        if (backIcon != null)
            backIcon.sprite = ThemeManager.Instance.backBtn;

        if (cameraIcon != null)
            cameraIcon.sprite = ThemeManager.Instance.cameraBtn;

        if (cameraSwapIcon != null)
            cameraSwapIcon.sprite = ThemeManager.Instance.cameraSwapBtn;

        if (printfulIcon != null)
            printfulIcon.sprite = ThemeManager.Instance.printfulBtn;

        if (info_DialogTitle != null)
        {
            if (string.IsNullOrEmpty(GameManager.Instance._ModuleData[0].title))
                info_DialogTitle.text = "";
            else
                info_DialogTitle.text = GameManager.Instance._ModuleData[0].title;
        }

        if (info_DialogText != null)
        {
            if (string.IsNullOrEmpty(GameManager.Instance._ModuleData[0].description))
                info_DialogText.text = "No data found.";
            else
                info_DialogText.text = GameManager.Instance._ModuleData[0].description;
        }

        // Help Popup data
        if (DialogBox != null)
            DialogBox.sprite = ThemeManager.Instance.dialoguebox;

        if (DialogButton != null)
            DialogButton.sprite = ThemeManager.Instance.commonBtn;

        if (helpIcon != null)
            helpIcon.sprite = ThemeManager.Instance.helpBtn;

        // Info Popup data
        if (infoIcon != null)
            infoIcon.sprite = ThemeManager.Instance.infoBtn;

        if (info_DialogBox != null)
            info_DialogBox.sprite = ThemeManager.Instance.dialoguebox;

        if (info_DialogButton != null)
            info_DialogButton.sprite = ThemeManager.Instance.commonBtn;

        if (GameManager.Instance.TitleFont != null)
        {
            if (DialogTitle != null)
                DialogTitle.font = GameManager.Instance.TitleFont;

            if (info_DialogTitle != null)
                info_DialogTitle.font = GameManager.Instance.TitleFont;
        }

        if (GameManager.Instance.DetailFont != null)
        {
            if (ButtonText != null)
                ButtonText.font = GameManager.Instance.DetailFont;
            if (DialogText != null)
                DialogText.font = GameManager.Instance.DetailFont;

            if (info_ButtonText != null)
                info_ButtonText.font = GameManager.Instance.DetailFont;
            if (info_DialogText != null)
                info_DialogText.font = GameManager.Instance.DetailFont;
        }
        Color newCol;
        if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
        {
            if (DialogTitle != null)
                DialogTitle.color = newCol;
            if (DialogText != null)
                DialogText.color = newCol;
            if (ButtonText != null)
                ButtonText.color = newCol;

            if (info_DialogTitle != null)
                info_DialogTitle.color = newCol;
            if (info_DialogText != null)
                info_DialogText.color = newCol;
            if (info_ButtonText != null)
                info_ButtonText.color = newCol;
        }
    }
}
