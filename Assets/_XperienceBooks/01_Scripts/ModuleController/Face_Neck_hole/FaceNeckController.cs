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
    bool isTextureSet = false;
    bool isInventoryApiCall = false;

    public Vector3 builderPosition = Vector3.zero;

    GameObject trackablesObj, FaceModelDummy;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        ModuleContent.AddRange(GameManager.Instance.GetModuleData());
        trackablesObj = GameObject.Find("Trackables");
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
        return webTexture;
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
                if (builerDataDownloaded != null)
                    builerDataDownloaded(webTexture, builderPosition);
            }
        }
    }

    private void Update()
    {
        if (CaptureBtnUI.alpha == 1)
        {
            screenSpaceUI.SetActive(false);
        }

#if UNITY_ANDROID
        if (FaceFound())
            cameraBtn.SetActive(true);
        else
            cameraBtn.SetActive(false);
#endif
    }

    void OnEnable()
    {
        //arFaceManager.facesChanged += FaceUpdated;
    }

    void OnDisable()
    {
        //arFaceManager.facesChanged -= FaceUpdated;
    }

    ARFace face;
    public void FaceUpdated(ARFacesChangedEventArgs fc)
    {

#if UNITY_ANDROID
        if (FaceFound() && trackablesObj.transform.GetChild(0).GetComponent<ARFace>().trackableId != null)
        {
            if (FaceModelDummy == null)
            {
                Debug.Log("moldel nulled : " + m_RootObject.transform.childCount);
                GameObject face1 = null;
                if (m_RootObject.transform.GetChild(0).childCount > 0)
                {
                    face1 = m_RootObject.transform.GetChild(0).gameObject;

                    Debug.Log("face..1 : " + face1.transform.childCount);

                    if (face1 != null && trackablesObj.transform.childCount > 0)
                    {
                        FaceModelDummy = Instantiate(face1, trackablesObj.transform.GetChild(0).gameObject.transform);
                        FaceModelDummy.SetActive(true);
                    }
                }
            }
            else if (!FaceModelDummy.activeSelf)
            {
                FaceModelDummy.SetActive(false);
            }
        }
        else if (FaceFound() == false && FaceModelDummy != null)
        {
            if (m_RootObject.transform.GetChild(0).childCount > 0)
            {
                FaceModelDummy.SetActive(false);
            }
        }
#endif
    }

    void UpdateModel(ARFace arFace)
    {
        try
        {
            Debug.Log("UpdateModel: " + face.transform.childCount);
            GameObject obj = face.transform.GetChild(0).gameObject;
            if (obj != null)
            {
                m_RootObject.transform.SetParent(obj.transform, false);
                ResetGameObject(m_RootObject);
                m_RootObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
                Debug.Log("Object not found");
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
