using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] Image BG;
    [SerializeField] Image BackIcon;

    bool isThemeSet = false;

    public void OnSetThem()
    {
        if (!isThemeSet || GameManager.Instance.isNewThemeDownload)
        {
            isThemeSet = true;
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BGTheme, BG);
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BackBtnTheme, BackIcon);
        }
    }
}
