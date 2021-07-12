﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TBEasyWebCam;
using System.Text.RegularExpressions;
using System.Collections;

public class QRScanController : MonoBehaviour
{
    public QRCodeDecodeController e_qrController;

    //  public GameObject resetBtn;

   public const string pattern = @"(?=.*bk=(\d+))(?=.*chptr=(\d+))(?=.*code=(\d+))";

    public GameObject scanLineObj;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
	bool isTorchOn = false;
#endif
    public Sprite torchOnSprite;
    public Sprite torchOffSprite;
    public Image torchImage;
    /// <summary>
    /// when you set the var is true,if the result of the decode is web url,it will open with browser.
    /// </summary>
    public bool isOpenBrowserIfUrl;

    public GameObject HomeScreenBG;

    [SerializeField] Image BackIcon;

    private float latitude, longitude;

    private void Start()
    {
        StartCoroutine(GetLocation());
        if (this.e_qrController != null)
        {
            this.e_qrController.onQRScanFinished += new QRCodeDecodeController.QRScanFinished(this.qrScanFinished);
        }
    }

    bool isThemeSet = false;
    void OnSetThem()
    {
        if (!isThemeSet || GameManager.Instance.isNewThemeDownload)
        {
            isThemeSet = true;
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BackBtnTheme, BackIcon);
        }
    }

    private IEnumerator GetLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    private void qrScanFinished(string dataText)
    {
        if (isOpenBrowserIfUrl)
        {
            if (Utility.CheckIsUrlFormat(dataText))
            {
                if (!dataText.Contains("http://") && !dataText.Contains("https://"))
                {
                    dataText = "http://" + dataText;
                }
                Application.OpenURL(dataText);
            }
        }
        
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(false);
        }

        if (dataText.Contains("https://www.continuummultimedia.com"))
        {

            if (dataText != null)
            {
                MatchCollection matches = Regex.Matches(dataText, pattern);
                int bookID = int.Parse(matches[1].Groups[1].Value);
                int chapterID = int.Parse(matches[1].Groups[2].Value);
                int qrCodeID = int.Parse(matches[1].Groups[3].Value);
                Debug.Log(latitude.ToString() + " , " + longitude.ToString());
                StopQRCode();

                ApiManager.Instance.GetMappedModules(bookID, chapterID, qrCodeID, latitude, longitude);
                GlobalControl.Instance.scanComplete = true;
            }
        }
        else {

            ApiManager.Instance.WrongQR();

        }
    }


    public void StopQRCode() {

        Stop();
        if (HomeScreenBG != null)
            HomeScreenBG.SetActive(true);

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
		if (isTorchOn) {
                isTorchOn=false;
				torchImage.sprite = torchOffSprite;
				EasyWebCam.setTorchMode (TBEasyWebCam.Setting.TorchMode.Off);
			}
#endif
    }


    public void Reset()
    {
        if (HomeScreenBG != null)
            HomeScreenBG.SetActive(false);
        GlobalControl.Instance.scanComplete = false;
        if (this.e_qrController != null)
        {
            this.e_qrController.Reset();
        }
       
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(true);
        }
    }

    public void Play()
    {
        OnSetThem();
        Reset();
        if (this.e_qrController != null)
        {
            this.e_qrController.StartWork();
        }
    }

    public void Stop()
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.StopWork();
        }

        
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(false);
        }
    }

    public void GotoNextScene(string scenename)
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.StopWork();
        }
        //Application.LoadLevel(scenename);
        SceneManager.LoadScene(scenename);
    }

    /// <summary>
    /// Toggles the torch by click the ui button
    /// note: support the feature by using the EasyWebCam Component 
    /// </summary>
    public void toggleTorch()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
		if (EasyWebCam.isActive) {
			if (isTorchOn) {
				torchImage.sprite = torchOffSprite;
				EasyWebCam.setTorchMode (TBEasyWebCam.Setting.TorchMode.Off);
			} else {
				torchImage.sprite = torchOnSprite;
				EasyWebCam.setTorchMode (TBEasyWebCam.Setting.TorchMode.On);
			}
			isTorchOn = !isTorchOn;
		}
#endif
    }

}
