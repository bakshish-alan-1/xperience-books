using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TBEasyWebCam;
using System.Text.RegularExpressions;
using System.Collections;

public class QRScanController : MonoBehaviour
{
    public static QRScanController Instance = null;

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

    [Header("Theme")]
    [SerializeField] Image BG;
    [SerializeField] Image BackIcon;
    [SerializeField] TMPro.TMP_Text TitleTxt;
    [SerializeField] TMPro.TMP_Text InfoTxt;

    private float latitude, longitude;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

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
            Debug.Log("No theme selected");
            isThemeSet = true;
            if(ThemeManager.Instance.scanBackground != null)
                BG.sprite = (ThemeManager.Instance.scanBackground);
            BackIcon.sprite = (ThemeManager.Instance.backBtn);
            if (GameManager.Instance.TitleFont != null)
                TitleTxt.font = GameManager.Instance.TitleFont;

            if (GameManager.Instance.DetailFont != null)
                InfoTxt.font = GameManager.Instance.DetailFont;

            Color newCol;
            if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            {
                TitleTxt.color = newCol;
                InfoTxt.color = newCol;
            }
        }
    }

    public void OnsetScanQRInfo(string msg)
    {
        InfoTxt.text = msg;
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
                Debug.Log("QRScanController: " + latitude.ToString() + " , " + longitude.ToString());
                StopQRCode();

                GameManager.Instance.FanArtEmailSubject = "Fan Art Submission for " + GameManager.Instance.selectedBooks.name + " (Chapter-" + chapterID + " - QR Code-" + qrCodeID + ")";

                ApiManager.Instance.GetMappedModules(bookID, chapterID, qrCodeID, latitude, longitude, OnScanSuccess);
                //Download theme for this QR code
                GlobalControl.Instance.scanComplete = true;
            }
        }
        else {

            ApiManager.Instance.WrongQR();

        }
    }

    private void OnScanSuccess(bool success, object data, long respCode)
    {
        if (success)
        {
            ARMappedModuleList response = JsonUtility.FromJson<ARMappedModuleList>(data.ToString());
            ApiManager.Instance.DownloadSkinAuto(response.data.book_details.genre_id, response.data.book_details.series_id, response.data.book_details.book_id);
        }
    }

    public void StopQRCode() {

        Stop();

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
        GlobalControl.Instance.scanComplete = false;
        if (this.e_qrController != null)
        {
            this.e_qrController.Reset();
        }
       
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(true);
        }
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        isTorchOn = false;
        torchImage.sprite = torchOffSprite;
        EasyWebCam.setTorchMode(TBEasyWebCam.Setting.TorchMode.Off);
#endif
    }

    public void Play()
    {
        OnSetThem();
        Reset();
        if (this.e_qrController != null)
        {
            Debug.Log("Check 1");
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
        Debug.Log("===================================== All Download compete goto next scene");
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

    void OnApplicationFocus(bool focus)
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
		torchImage.sprite = torchOffSprite;
		EasyWebCam.setTorchMode (TBEasyWebCam.Setting.TorchMode.Off);
        isTorchOn = false;
#endif
    }
}
