using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : MonoBehaviour
{
    public static NotificationPanel Instance = null;

    [SerializeField] Image BG;
    [SerializeField] Image HeaderBg;
    [SerializeField] Image BackIcon;
    [SerializeField] TMPro.TMP_Text title;
    [SerializeField] TMPro.TMP_Text nodataMsg;

    [Header("Notification prefabs")]
    [SerializeField] GameObject parentObj;// prefabs parent
    [SerializeField] GameObject notificationCell;// prefabs

    bool isThemeSet = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        OnSetThem();
    }

    // call from homescreen script after downloading theme
    public void OnSetThem()
    {
        if (!isThemeSet || GameManager.Instance.isNewThemeDownload)
        {
            isThemeSet = true;
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BGTheme, BG);
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BackBtnTheme, BackIcon);
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.Fan_art_Img_Bg, HeaderBg);
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

    public void setNotificationData()
    {
        nodataMsg.text = "";//No Data available
        GameObject obj = Instantiate(notificationCell, parentObj.transform);
        obj.GetComponent<NotificationCellController>().SetData("title");
    }
}
