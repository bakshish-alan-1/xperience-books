using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using TriLibCore;

public class ARPortalController : MonoBehaviour
{
    [SerializeField]
    Material m_PortalMaterial;
    [SerializeField]
    AudioSource m_PortalAudioSource;
    [SerializeField]
    Texture2D portalTexture;
    string AssetURI, extractDir;
    public int m_CurrentIndex = 0;


    [SerializeField]
    PlacementController controller;
    [SerializeField] GameObject Portal360;

    public Text progressText;
    public Image progressBar;

    [Header("For Trilib")]
    [SerializeField] GameObject doorRootObject;
    [SerializeField] LoadModelFromURL m_ModelDownloader;

    public List<ContentModel> ModuleContent = new List<ContentModel>();

    bool loadingFinished = false;

    private void Awake()
    {
        ModuleContent.AddRange(GameManager.Instance.GetModuleData());
    }

    void Start()
    {
        StartCoroutine(LoadTexture());
    }

    private void OnEnable()
    {
        AssetDownloaderBehaviour.OnDownloadFinished += ModelLoadFinished;
    }

    void OnDisable()
    {
        AssetDownloaderBehaviour.OnDownloadFinished -= ModelLoadFinished;
    }

    private void ModelLoadFinished(UnityWebRequest request)
    {

    }

    public IEnumerator LoadTexture()
    {
        Debug.Log("LoadTexture");
        string localPath;

        localPath = GameManager.Instance.GetLocalPath(StaticKeywords.ARPortal); //ContentManager.Instance.LocalPath(m_CurrentIndex, StaticKeywords.ARPortal);
        string fileName = ModuleContent[m_CurrentIndex].filename;  //ContentManager.Instance._ModuleData[m_CurrentIndex].filename;
        bool isLocalFile;

        if (FileHandler.ValidateFile(localPath + fileName))
        {
            isLocalFile = true;
            AssetURI = "file://" + FileHandler.FinalPath(localPath, fileName); 
        }
        else
        {
            isLocalFile = false;
            AssetURI = ModuleContent[m_CurrentIndex].ar_content;
        }
        Debug.Log("LoadTexture: " + AssetURI);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(AssetURI))
        {
            // www.downloadHandler = new DownloadHandlerBuffer();
            uwr.SendWebRequest();
            while (!uwr.isDone)
            {
                float progress = uwr.downloadProgress;
                //Debug.Log(progress * 100);
                if (progressText != null)
                {
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
                if (!isLocalFile)
                {
                    Debug.Log("Save file and extract");
                    FileHandler.SaveFile(localPath, fileName, uwr.downloadHandler.data, true);
                }
                Debug.Log("localPath: " + FileHandler.FinalPath(localPath, fileName));
                m_ModelDownloader.StartLoadObject(FileHandler.FinalPath(localPath, fileName), true, ModelLoaded, OnMaterialsLoad);

                FileInfo m_FileInfo = new FileInfo(FileHandler.FinalPath(localPath, fileName));
                extractDir = Path.Combine(m_FileInfo.Directory.FullName, m_FileInfo.Name.Substring(0, m_FileInfo.Name.Length - m_FileInfo.Extension.Length));
                DirectoryInfo dirInfo = new DirectoryInfo(extractDir);
                FileInfo[] audioFile = dirInfo.GetFiles("*.mp3");
                FileInfo[] imageFile = dirInfo.GetFiles("*.jpg");

                if (imageFile.Length == 0)
                {
                    Debug.Log("No JPG Image Found trying to get JPEG image");
                    imageFile = dirInfo.GetFiles("*.jpeg"); 
                }

                StartCoroutine(LoadImageTexture("file://" + imageFile[0].ToString()));
                if (audioFile.Length > 0)
                    StartCoroutine(LoadAudioFile("file://" + audioFile[0].ToString()));
            }
        }
        controller.UpdatePlacementState();
    }

    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Materials loaded. Model fully loaded.");
    }

    public void ModelLoaded(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("ModelLoaded: ");
        doorRootObject.transform.GetChild(0).GetComponent<Animation>().playAutomatically = false;
        doorRootObject.transform.GetChild(0).GetComponent<Animation>().wrapMode = WrapMode.Once;
    }

    private IEnumerator LoadImageTexture(string path)
    {
        Debug.Log("LOADING Image: " + path);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Get Image Error: "+www.error);
        }
        else
        {
            portalTexture = DownloadHandlerTexture.GetContent(www);

            m_PortalMaterial.SetTexture("_MainTex", portalTexture);
            controller.m_Preloader.SetActive(false);
            loadingFinished = true;
        }
    }

    private void Update()
    {
        if (loadingFinished && Portal360.activeSelf == false)
        {
            if (controller.isTapHitByUser() == true)
                Portal360.SetActive(true);
        }
    }

    private IEnumerator LoadAudioFile(string fullpath)
    {
        Debug.Log("LOADING CLIP: " + fullpath);

        AudioClip temp = null;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullpath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Get Audio Error: "+www.error);
            }
            else
            {
                temp = DownloadHandlerAudioClip.GetContent(www);
                m_PortalAudioSource.clip = temp;
            }
        }
    }
}
