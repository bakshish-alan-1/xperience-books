using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebPageData : MonoBehaviour
{
    public string URL;

    UniWebView webView;

    public Image btnBg;
    public TMPro.TextMeshProUGUI text;

    public void OnClickButton() {

        if (string.IsNullOrWhiteSpace(URL)) {
            return;
        }
        webView = transform.gameObject.AddComponent<UniWebView>();
        webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        webView.SetShowToolbar(true, false, false, true);
        webView.Load(URL);
        webView.Show();

        webView.OnShouldClose += (view) => {
            webView = null;
            return true;
        };

    }

    void setTheme()
    {
        btnBg.sprite = ThemeManager.Instance.commonBtn;
        text.font = GameManager.Instance.TitleFont;
        Color newCol;
        if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            text.color = newCol;
    }

    public void setData(int index, string url) {
        setTheme();
        text.text = "Link_" + (index + 1);
        URL = url;
    }
}
