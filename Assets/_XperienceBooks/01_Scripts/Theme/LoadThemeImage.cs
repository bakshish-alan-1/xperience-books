using UnityEngine;
using UnityEngine.UI;

public class LoadThemeImage : MonoBehaviour
{
    [Header("Set Theme")]
    public Image DialogBox;
    public Image DialogButton;
    public TMPro.TMP_Text DialogTitle;
    public TMPro.TMP_Text DialogText;
    public TMPro.TMP_Text ButtonText;

    private void Start()
    {
        string path = GameManager.Instance.GetThemePath();
        if (DialogBox != null)
            ThemeManager.Instance.OnLoadImage(path, StaticKeywords.DialogBox, DialogBox);

        if (DialogButton != null)
            ThemeManager.Instance.OnLoadImage(path, StaticKeywords.DialogBoxBtn, DialogButton);

        if (GameManager.Instance.TitleFont != null)
        {
            DialogTitle.font = GameManager.Instance.TitleFont;
            ButtonText.font = GameManager.Instance.TitleFont;
        }

        if (GameManager.Instance.DetailFont != null)
            DialogText.font = GameManager.Instance.DetailFont;

        Color newCol;
        if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
        {
            DialogTitle.color = newCol;
            DialogText.color = newCol;
            ButtonText.color = newCol;
        }
    }
}
