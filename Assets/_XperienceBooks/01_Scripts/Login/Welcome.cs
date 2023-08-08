using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welcome : MonoBehaviour
{
    private void Start()
    {
        //GameManager.Instance.OpenSafetyWindow();
        //WindowManager.Instance.OpenPanel(StaticKeywords.HomePanel);
    }

    public void GUI_QR_CodeDownload()
    {
        Application.OpenURL("http://xperiencebooks.net/wp-content/uploads/2022/11/TryIt3code2.png");
    }
}
