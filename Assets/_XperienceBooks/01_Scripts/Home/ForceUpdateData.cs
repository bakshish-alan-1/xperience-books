using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForceUpdateData : MonoBehaviour
{
    public static ForceUpdateData Instance = null;

    [SerializeField] GameObject forceUpdate;
    [SerializeField] TMP_Text Title;
    [SerializeField] TMP_Text msg;
    [SerializeField] GameObject androidBtn;
    [SerializeField] GameObject iosBtn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {

#if UNITY_ANDROID
        androidBtn.SetActive(true);
        iosBtn.SetActive(false);
#elif UNITY_IOS
        androidBtn.SetActive(false);
        iosBtn.SetActive(true);
#endif
    }

    public void onSetdata(string message)
    {
        Debug.Log("Inside force update onSetdata");
        msg.text = message;
        forceUpdate.SetActive(true);
    }

    public void onGoogleBtn()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.continuummedia.xperiencebooks");
    }

    public void onIOSBtn()
    {
        Application.OpenURL("https://apps.apple.com/us/app/xperience-books/id1571297291");
    }
}
