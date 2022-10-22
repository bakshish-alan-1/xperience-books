using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class FaceNeckController : MonoBehaviour
{
    public static FaceNeckController Instance = null;

    //Defining Delegate
    public delegate void OnFaceNeckImageDownloaded(Texture2D texture, Vector3 pos);
    public static OnFaceNeckImageDownloaded builerDataDownloaded;// implemented in face neck builder script

    public CanvasGroup CaptureBtnUI;
    public GameObject InfoBox, screenSpaceUI, cameraBtn, LoadingUI;
    public ARFaceManager arFaceManager;
    [SerializeField] GameObject m_RootObject;
    public Text progressText;
    public Image progressBar;

    public List<ContentModel> ModuleContent = new List<ContentModel>();

    string AssetURI;
    public bool isTextureAvailable = false;
    bool isInventoryApiCall = false;

    public Vector3 builderPosition = Vector3.zero;

    Texture2D webTexture = null;
    public bool isBackBtn = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        ModuleContent.AddRange(GameManager.Instance.GetModuleData());
    }

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = 0.0f;
            progressText.text = "00 %";
        }

        GameManager.Instance.safetyWindow.OpenWindow();// call Safetywindow popup
        LoadData();//change

        if (!isInventoryApiCall)
        { isInventoryApiCall = true; GameManager.Instance.OnCheckToUnlockModule(6); }
    }

    public void onBackBtnClick()
    {
        isBackBtn = true;
        StopAllCoroutines();
        CancelInvoke();
        ModuleContent.Clear();
        ModuleContent.TrimExcess();
        webTexture = null;
    }

    public void OnInfoBtnHite(bool value)
    {
        InfoBox.SetActive(value);
    }

    private void LoadData()
    {
        if (ModuleContent.Count <= 0)
            return;
        try
        {
            string localPath;

            localPath = GameManager.Instance.GetLocalPath(StaticKeywords.FaceNeckHole);

            string fileName = "";
            bool isLocalFile;

            for (int i = 0; i < ModuleContent.Count; i++)
            {
                fileName = ModuleContent[i].filename;
                if (FileHandler.ValidateFile(localPath + fileName))
                {
                    isLocalFile = true;
                    AssetURI = "file://" + GameManager.Instance.LocalStoragePath + localPath + fileName;
                }
                else
                {
                    isLocalFile = false;
                    AssetURI = ModuleContent[i].ar_content;
                }
                builderPosition = new Vector3(ModuleContent[i].position.x, ModuleContent[i].position.y, ModuleContent[i].position.z);
                StartCoroutine(LoadTexture(isLocalFile, AssetURI, localPath, fileName));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("FaceNeckHole : LoadData " + ex.Message);
        }
    }

    public Texture2D getBuilderTexture()
    {
        if (isTextureAvailable)
            return webTexture;
        else
            return null;
    }

    IEnumerator LoadTexture(bool isLocalFile, string URL, string localPath, string fileName)
    {
        Debug.Log("FaceNeckHole Img URL: " + URL);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(URL))
        {
            uwr.SendWebRequest();
            while (!uwr.isDone)
            {
                //Debug.Log(uwr.downloadProgress);
                if (progressText != null)
                {
                    float progress = uwr.downloadProgress;
                    progressBar.fillAmount = progress;
                    progressText.text = (progress * 100).ToString("00") + " %";
                }
                if (!isTextureAvailable && FaceFound())
                    LoadingUI.SetActive(true);

                yield return null;
            }
            
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error:- " + uwr.error);
            }
            else
            {
                webTexture = DownloadHandlerTexture.GetContent(uwr);
                isTextureAvailable = true;
                LoadingUI.SetActive(false);
                if (!isLocalFile)
                {
                    FileHandler.SaveFile(localPath, fileName, uwr.downloadHandler.data);
                }
                if (builerDataDownloaded != null)
                    builerDataDownloaded(webTexture, builderPosition);
            }
            uwr.Dispose();
        }
    }

    private void Update()
    {
        if (isBackBtn)
            return;

        if (CaptureBtnUI.alpha == 1)
        {
            screenSpaceUI.SetActive(false);
        }


        if (FaceFound())
            cameraBtn.SetActive(true);
        else
            cameraBtn.SetActive(false);
#if UNITY_ANDROID
#endif
    }

    public void ResetGameObject(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
    }

    bool FaceFound()
    {
        return arFaceManager?.trackables.count > 0;
    }
}
