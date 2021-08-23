
/*
 * NOTE:
 * 
 * This scripts are handle two popup 
 * - Safety Window
 * 
 * Which is show only one time when application open
 * 
 * */

using System;
using UnityEngine;
using UnityEngine.UI;

public class SafetyWindow : MonoBehaviour
{
    [SerializeField] Animator currentWindowAnimator;

    [SerializeField] string windowFadeIn = "Window In";
    [SerializeField] string windowFadeOut = "Window Out";

    [SerializeField] Image DialogBox;
    [SerializeField] Image DialogBoxBtn;
    [SerializeField] TMPro.TMP_Text TitleText;
    [SerializeField] TMPro.TMP_Text message;
    [SerializeField] TMPro.TMP_Text BtnText;

    bool isThemeSet = false;

    public void OpenWindow()
    {
        if (!isThemeSet)
        {
            setTheme();
        }

        if (!GameManager.Instance.isSafetyWindowOpen)
        {
            currentWindowAnimator.Play(windowFadeIn);
        }
    }

    private void setTheme()
    {
        ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.DialogBox, DialogBox);
        ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.DialogBoxBtn, DialogBoxBtn);

        Color newCol;
        if (!string.IsNullOrEmpty(GameManager.Instance.selectedSeries.theme.color_code) && ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
        {
            TitleText.color = newCol;
            message.color = newCol;
            BtnText.color = newCol;
        }

        if (GameManager.Instance.TitleFont != null)
        {
            TitleText.font = GameManager.Instance.TitleFont;
        }

        if (GameManager.Instance.DetailFont != null)
            message.font = GameManager.Instance.DetailFont;
    }

    public void OpenWebsite()
    {
        Application.OpenURL("https://www.theworldofagartha.com/");
    }

    public void CloseWindow()
    {
        currentWindowAnimator.Play(windowFadeOut);
    }
}
