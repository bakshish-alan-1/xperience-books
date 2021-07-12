
/*
 * NOTE:
 * 
 * This scripts are handle two popup 
 * - Safety Window
 * 
 * Which is show only one time when application open
 * 
 * */

using UnityEngine;

public class SafetyWindow : MonoBehaviour
{
    [SerializeField] Animator currentWindowAnimator;

    [SerializeField] string windowFadeIn = "Window In";
    [SerializeField] string windowFadeOut = "Window Out";

    public void OpenWindow()
    {
        //if (!GameManager.Instance.isSafetyWindowOpen)Akash
        {
            currentWindowAnimator.Play(windowFadeIn);
        }
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
