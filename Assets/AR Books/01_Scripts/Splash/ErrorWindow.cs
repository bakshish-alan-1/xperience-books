using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorWindow : MonoBehaviour
{

    public enum ResponseData {

        JustClose,
        InternetIssue,
        SessionExpire

    }

    [SerializeField]
    TextMeshProUGUI TitleText, Message, ButtonText;

    public Animator currentWindowAnimator;

    public string windowFadeIn = "Window In";
    public string windowFadeOut = "Window Out";


    public ResponseData response;

    public void SetErrorMessage(string title, string message, string buttonText,ResponseData res) {
        TitleText.text = title;
        Message.text = message;
        ButtonText.text = buttonText;
        currentWindowAnimator.Play(windowFadeIn);
        response = res;
    }


    public void CloseWindow() {
        TitleText.text = string.Empty;
        Message.text = string.Empty;
        ButtonText.text = string.Empty;
        currentWindowAnimator.Play(windowFadeOut);

        if (response == ResponseData.SessionExpire) {
            WindowManager.Instance.LogOut();
        }

        if (response == ResponseData.InternetIssue) {
            ApiManager.Instance.Start();
        }
    }
}
