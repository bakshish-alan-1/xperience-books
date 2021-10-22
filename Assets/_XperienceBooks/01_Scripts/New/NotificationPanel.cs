using System.Collections.Generic;
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
            BG.sprite = (ThemeManager.Instance.background);
            BackIcon.sprite = (ThemeManager.Instance.backBtn);
            HeaderBg.sprite = (ThemeManager.Instance.fanArtHeaderBg);

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

    public void OnReleaseData()
    {
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            Destroy(parentObj.transform.GetChild(i).gameObject);
        }
    }

    public void setNotificationData(List<NotificationData> data)
    {
        nodataMsg.text = "";//No Data available
        for (int i = 0; i < data.Count; i++)
        {
            GameObject obj = Instantiate(notificationCell, parentObj.transform);
            obj.GetComponent<NotificationCellController>().SetData(data[i].id, data[i].body, data[i].sc_qr_title);
        }
    }
}
