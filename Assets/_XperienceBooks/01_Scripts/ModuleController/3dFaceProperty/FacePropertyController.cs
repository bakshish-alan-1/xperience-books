using System;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class FacePropertyController : MonoBehaviour
{
    public static FacePropertyController Instance = null;
    private ARFaceManager faceManager;
    private bool faceTrackingOn = true;

    bool isInventoryApiCall = false;
    bool isLocalFile = false;
    string filePath;
    string fileName;
    int currentIndex = 0;
    public string m_WebURL;
    public string m_LocalURL;

    public string HeadPoint = "root/HEAD";

    //NeedToUpdate Trilib
    [SerializeField]
    LoadModelFromURL m_ModelDownloader;
    string finalPath = "", localModelFilePath = "";

    [SerializeField]
    public GameObject InfoBox, m_RootObject;
    [SerializeField]
    public GameObject m_ScreenShotCameraBtn, m_ScreenSpaceUI;


    GameObject Facemodel, go;

    public GameObject m_Preloader;

    public List<ContentModel> FacePropertyData = new List<ContentModel>();

    bool isModelAvailable = false;
    bool isModelSet = false;
    ARFace face;
    bool isBackBtn = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        faceManager = GetComponent<ARFaceManager>();
        currentIndex = 0;
        go = GameObject.Find("Trackables");
        Init();
    }

    private void Update()
    {
        if (isBackBtn)
            return;
        if (FaceFound() && !isModelAvailable)
        {
            if (!m_Preloader.activeSelf)
                m_Preloader.SetActive(true);
        }
        else {
            if (m_Preloader.activeSelf)
                m_Preloader.SetActive(false);
        }

#if !UNITY_EDITOR //&& UNITY_ANDROID
        if (FaceFound() && m_ScreenSpaceUI.activeSelf)
            m_ScreenSpaceUI.SetActive(false);
#endif
    }

    public void onBackBtnClick()
    {
        isBackBtn = true;
        StopAllCoroutines();
        OnDisable();
        CancelInvoke();
    }

    void OnEnable()
    {
        AssetDownloaderBehaviour.OnDownloadFinished += ModelLoadFinished;
        faceManager.facesChanged += FaceUpdated;
    }

    void OnDisable()
    {
        AssetDownloaderBehaviour.OnDownloadFinished -= ModelLoadFinished;
        faceManager.facesChanged -= FaceUpdated;
    }

    public void Init()
    {
        FacePropertyData.AddRange(GameManager.Instance.GetModuleData());

        //passing index 0 because there is only 1 file in plane tracking model , if multiple then need to pass acordingly value
        filePath = GameManager.Instance.GetLocalPath(StaticKeywords.Face3DProperty); //GameManager.Instance.currentBook.BasePath()+ "/" + StaticKeywords.Face3DProperty + "/";
        fileName = FacePropertyData[currentIndex].filename;

        FileHandler.ValidateFolderStructure(filePath); // Create folder if not exist. 

        if (FileHandler.ValidateFile(filePath + fileName))
        {
            isLocalFile = true;
            finalPath = FileHandler.FinalPath(filePath, fileName);
        }
        else
        {
            isLocalFile = false;
            finalPath = FacePropertyData[currentIndex].ar_content;
        }

        localModelFilePath = FileHandler.FinalPath(filePath, fileName);
        m_LocalURL = localModelFilePath; 
        m_WebURL = FacePropertyData[currentIndex].ar_content;
       
    }

    private void Start()
    {
        Debug.Log("Model Loading Start");
        m_ModelDownloader.StartLoadObject(finalPath, isLocalFile, ModelLoaded, OnMaterialsLoad);
        GameManager.Instance.safetyWindow.OpenWindow();// call Safetywindow popup
    }

    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        if (isBackBtn)
            return;
        Debug.Log("Materials loaded. Model fully loaded.");
        LoadModelOnFace();
    }

    private void ModelLoadFinished(UnityWebRequest request)
    {
        if (isBackBtn)
            return;
        Debug.Log("Model Finished loading");
        if (isLocalFile)
        {
            return;
        }
        var data = request.downloadHandler.data;
        Debug.Log("Model data: " + data);
        if (!isLocalFile && localModelFilePath != "")
        {
            Debug.Log("Save File at : " + localModelFilePath);
            File.WriteAllBytes(localModelFilePath, data);
        }
    }

    public void OnInfoBtnHite(bool value)
    {
        InfoBox.SetActive(value);
    }

    public void ModelLoaded(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("ModelLoaded: ");
    }
    
    public void LoadModelOnFace() {

        isModelAvailable = true;

#if UNITY_IOS
        if (face != null) {
            UpdateModel(face);
        }
#endif
    }
    
    public void FaceUpdated(ARFacesChangedEventArgs fc)
    {
        if (isBackBtn)
            return;
#if UNITY_ANDROID
        if (isModelAvailable && FaceFound() && Facemodel == null && go.transform.GetChild(0).GetComponent<ARFace>().trackableId != null)
        {
            if (Facemodel == null)
            {
                Debug.Log("moldel nulled : " + m_RootObject.transform.childCount);
                GameObject face1 = null;
                if (m_RootObject.transform.GetChild(0).childCount > 0)
                {
                    face1 = m_RootObject.transform.GetChild(0).gameObject;

                    Debug.Log("face..1 : " + face1.transform.childCount);

                    if (face1 != null && go.transform.childCount > 0)
                    {
                        Facemodel = Instantiate(face1, go.transform.GetChild(0).gameObject.transform);
                        Facemodel.SetActive(true);
                    }
                }           
            }
        }
        else if (FaceFound() == false && Facemodel != null)
        {
            if (m_RootObject.transform.GetChild(0).childCount > 0)
            {
                if (m_RootObject.transform.GetChild(0).transform.GetChild(0) != null)
                {
                    isModelAvailable = false;
                    Destroy(m_RootObject.transform.GetChild(0).transform.GetChild(0).gameObject);
                }
            }
            isLocalFile = true;
            m_ModelDownloader.StartLoadObject(localModelFilePath, isLocalFile, ModelLoaded, OnMaterialsLoad);
        }
#endif

#if UNITY_IOS
        if (fc.added.Count > 0)
        {
            face = fc.added[0];

            if (isModelAvailable)
            {
                Debug.Log("<color=Green>Set Model here </color>");
                isModelSet = true;
                UpdateModel(face);
            }
        }

        if (fc.updated.Count > 0)
        {
            if (isModelAvailable)
            {
                if (!isModelSet)
                {
                    face = fc.updated[0];
                    isModelSet = true;
                    Debug.Log("<color=blue>Set Model here </color>");
                    UpdateModel(face);
                }
            }
        }        
#endif

        if (FaceFound())
        {
            m_ScreenShotCameraBtn.SetActive(true);
            if (!isInventoryApiCall)
            { isInventoryApiCall = true; GameManager.Instance.OnCheckToUnlockModule(2); }
        }
        else
            m_ScreenShotCameraBtn.SetActive(false);
    }

    void UpdateModel(ARFace face) {
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
        catch (Exception ex) {
            Debug.Log("AR Face issue : " + ex);
        }
    }

    bool FaceFound()
    {
        return faceManager?.trackables.count > 0;
    }

    public void ResetGameObject(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
    }
}
