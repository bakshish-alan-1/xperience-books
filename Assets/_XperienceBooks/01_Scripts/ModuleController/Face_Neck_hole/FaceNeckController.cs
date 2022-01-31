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
    public CanvasGroup CaptureBtnUI;
    public GameObject InfoBox, screenSpaceUI, cameraBtn, LoadingUI;
    public ARFaceManager arFaceManager;
    public Text progressText;
    public Image progressBar;

    public List<ContentModel> ModuleContent = new List<ContentModel>();

    string AssetURI;
    public bool isTextureAvailable = false;
    bool isTextureSet = false;
    bool isInventoryApiCall = false;

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
                                                       //LoadData();//change

        if (!isInventoryApiCall)
        { isInventoryApiCall = true; GameManager.Instance.OnCheckToUnlockModule(6); }
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
                StartCoroutine(LoadTexture(isLocalFile, AssetURI, localPath, fileName));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("FaceNeckHole : LoadData " + ex.Message);
        }
    }

    Texture2D webTexture = null;
    IEnumerator LoadTexture(bool isLocalFile, string URL, string localPath, string fileName)
    {
        Debug.Log("FaceNeckHole Img URL: " + URL);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(URL))
        {
            uwr.SendWebRequest();
            while (!uwr.isDone)
            {
                Debug.Log(uwr.downloadProgress);
                if (progressText != null)
                {
                    float progress = uwr.downloadProgress;
                    progressBar.fillAmount = progress;
                    progressText.text = (progress * 100).ToString("00") + " %";
                }
                yield return null;
            }
            
            if (uwr.isNetworkError || uwr.isHttpError)
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
            }
        }
    }

    private void Update()
    {
        if (CaptureBtnUI.alpha == 1)
        {
            screenSpaceUI.SetActive(false);
        }
        /*
#if UNITY_ANDROID
        if (FaceFound() && isTextureAvailable == false)
            LoadingUI.SetActive(true);
        else
            LoadingUI.SetActive(false);
#endif
        */ 
    }

    void OnEnable()
    {
        arFaceManager.facesChanged += FaceUpdated;
    }

    void OnDisable()
    {
        arFaceManager.facesChanged -= FaceUpdated;
    }

    ARFace face;
    public void FaceUpdated(ARFacesChangedEventArgs fc)
    {
        /*
#if UNITY_ANDROID
        if (webTexture != null && !isTextureSet)
        {
            face = fc.added[0];
            isTextureSet = true;
            face.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Utility.Texture2DToSprite(webTexture);
            Debug.Log("SpriteRenderer set: " + isTextureSet);
        }
        else if (FaceFound() == false)
        {
            isTextureSet = false;
        }
#endif

#if UNITY_IOS
        if (fc.added.Count > 0 && webTexture != null)
        {
            face = fc.added[0];

            if (isTextureAvailable)
            {
                UpdateModel(face);
            }
        }
        if (fc.updated.Count > 0)
        {
            if (isTextureAvailable)
            {
                face = fc.updated[0];
                UpdateModel(face);
            }
        }
#endif
        */
        if (FaceFound())
            cameraBtn.SetActive(true);
        else
            cameraBtn.SetActive(false);
    }

    void UpdateModel(ARFace arFace)
    {
        try
        {
            Debug.Log("UpdateModel: " + arFace.transform.childCount);
            if (arFace.transform.childCount > 0 && !isTextureSet)
            {
                isTextureSet = true;
                arFace.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Utility.Texture2DToSprite(webTexture);
                Debug.Log("SpriteRenderer set: " + isTextureSet);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("AR Face issue : " + ex);
        }
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
