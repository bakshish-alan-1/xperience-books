
using UnityEngine;
using UnityEngine.UI;

public class NotificationCellController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text title;
    [SerializeField] Image BG;
    [SerializeField] Image Icon;
    [SerializeField] Image NextBtnIcon;

    int notificationId;
    string scanInfo = "";

    public void SetData(int id, string titleName, string info)
    {
        setTheme();
        notificationId = id;
        scanInfo = info;// this message shown on QRScan page
        title.text = titleName;
    }

    public void onNextBtnClick()
    {
        ApiManager.Instance.SetNotificationView(notificationId);
        QRScanController.Instance.OnsetScanQRInfo(scanInfo);
        WindowManager.Instance.OpenPanel("QRScan");
        QRScanController.Instance.Play();
    }

    private void setTheme()
    {
        ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.NotificationCellBG, BG);
        ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.NotificationCellIcon, Icon);
        ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.NotificationNextBtn, NextBtnIcon);
        if (GameManager.Instance.TitleFont != null)
        {
            title.font = GameManager.Instance.TitleFont;
        }
        Color newCol;
        if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
        {
            title.color = newCol;
        }
    }
}
