using UnityEngine;
using UnityEngine.UI;

public class LoadThemeImage : MonoBehaviour
{
    // used in arscene for provides info about hashtag data

    [Header("Set Theme")]
    public Image DialogBox;
    public Image DialogButton;
    public Image backIcon;
    public Image cameraIcon, cameraSwapIcon, infoIcon;
    public TMPro.TMP_Text DialogTitle;
    public TMPro.TMP_Text DialogText;
    public TMPro.TMP_Text ButtonText;

    private void Start()
    {
        string path = GameManager.Instance.GetThemePath();
        if (DialogBox != null)
            DialogBox.sprite = ThemeManager.Instance.dialoguebox;

        if (DialogButton != null)
            DialogButton.sprite = ThemeManager.Instance.commonBtn;

        if (backIcon != null)
            backIcon.sprite = ThemeManager.Instance.backBtn;

        if (cameraIcon != null)
            cameraIcon.sprite = ThemeManager.Instance.cameraBtn;

        if (cameraSwapIcon != null)
            cameraSwapIcon.sprite = ThemeManager.Instance.cameraSwapBtn;

        if (infoIcon != null)
            infoIcon.sprite = ThemeManager.Instance.infoBtn;

        if (GameManager.Instance.TitleFont != null && DialogTitle != null)
        {
            DialogTitle.font = GameManager.Instance.TitleFont;
        }

        if (GameManager.Instance.DetailFont != null)
        {
            if (ButtonText != null)
                ButtonText.font = GameManager.Instance.DetailFont;
            if (DialogText != null)
                DialogText.font = GameManager.Instance.DetailFont;
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
        }
    }
}
