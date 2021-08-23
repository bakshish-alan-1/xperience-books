using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] Image BG;
    [SerializeField] Image BackIcon;
    [SerializeField] TMPro.TMP_Text title;
    [SerializeField] TMPro.TMP_Text nodataMsg;

    bool isThemeSet = false;

    public void OnSetThem()
    {
        if (!isThemeSet || GameManager.Instance.isNewThemeDownload)
        {
            isThemeSet = true;
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BGTheme, BG);
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BackBtnTheme, BackIcon);
            if (GameManager.Instance.TitleFont != null)
            {
                title.font = GameManager.Instance.TitleFont;
                nodataMsg.font = GameManager.Instance.TitleFont;
            }
            Color newCol;
            if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            {
                title.color = newCol;
                nodataMsg.color = newCol;
            }
        }
    }
}
