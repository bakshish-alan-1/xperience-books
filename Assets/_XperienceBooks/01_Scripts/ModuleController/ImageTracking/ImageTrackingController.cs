using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TriLibCore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ImageTargetData {

    public string m_TargetName;
    public string m_LocalURL;
    public string m_WebURL;
    public bool audio_loop;
    public Vector3 modelPosition;
    public Vector3 modelRotation;
    public Vector3 modelScale;
}

public class ImageTrackingController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("from project window")]
    private XRReferenceImageLibrary runtimeImageLibrary;
    
    public ARTrackedImageManager m_ImageManager;

    [SerializeField]
    private GameObject m_ArObjectsToPlace;
    
    [SerializeField]
    public List<Texture2D> m_ImageTargetTexture = new List<Texture2D>();

    //NeedToUpdate Trilib
    [SerializeField]
    private LoadModelFromURL ModelLoader;

    [SerializeField]
    private Transform rootObject;

    GameObject m_SpawnedPrefab;
    public GameObject spawnedPrefab
    {
        get => m_SpawnedPrefab;
        set => m_SpawnedPrefab = value;
    }

    [SerializeField]
    GameObject m_ScreenSpaceUI;

    [SerializeField]
    BuilderImageTransform builderTransform;

    private Vector3 Builder_Position = Vector3.zero, Builder_Rotation = Vector3.zero, Builder_Scale = Vector3.one;

    public List<ContentModel> ModuleContent = new List<ContentModel>();

    public GameObject m_Preloader;
    public string m_PreviousTrackedImage = "n";
    public string m_LastTrackedImage = "none";

    [Header("Audio Player")]
    [SerializeField] AudioSource audioSource;

    bool isInventoryApiCall = false;
    bool isBackBtnClick = false;
    bool audioFinish = false;

    AudioClip tempClip = null;
    string audioPath = "";
    void Start()
    {
        Init();
        GameManager.Instance.safetyWindow.OpenWindow();// call Safetywindow popup
    }

    void Init() {


        ModuleContent.AddRange(GameManager.Instance.GetModuleData());

#if !UNITY_EDITOR

        m_ImageManager = gameObject.AddComponent<ARTrackedImageManager>();
        m_ImageManager.referenceLibrary = m_ImageManager.CreateRuntimeLibrary(runtimeImageLibrary);
        m_ImageManager.maxNumberOfMovingImages = 1;
        //m_ImageManager.trackedImagePrefab = placedObject;

        m_ImageManager.enabled = true;

        m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;

#endif
        LoadPaintTexture();
    }

    private void OnEnable()
    {
        AssetDownloaderBehaviour.OnDownloadFinished += ModelLoadFinished;
    }

    void OnDisable()
    {
        AssetDownloaderBehaviour.OnDownloadFinished -= ModelLoadFinished;

        if (m_ImageManager != null)
            m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
    }

    public void LoadPaintTexture()
    {
        if (ModuleContent.Count <= 0) return;

        foreach (ContentModel model in ModuleContent)
        {
            StartCoroutine(LoadContent(model));
        }

        //StartCoroutine(LoadAudioFile(""));
    }

    Dictionary<string, ImageTargetData> m_TargetLib = new Dictionary<string, ImageTargetData>();

    bool isLocalFile = false;
    bool isLocalModleFile = false;
    string localModelFilePath = "";
    public IEnumerator LoadContent(ContentModel data)
    {
        Vector3 myPosition = new Vector3((data.position.x), (data.position.y), (data.position.z));
        Vector3 myRotation = new Vector3(data.rotation.x, data.rotation.y, data.rotation.z);
        Vector3 myScale = new Vector3(data.scale.x, data.scale.y, data.scale.z);

        string m_TargetLocalPath = GameManager.Instance.GetLocalPath(StaticKeywords.ImageTracking) + data.id + "/"+ data.c_code_image_id + "/";
        string m_TargetImageName = data.id+"_"+data.c_code_image_name;
        string prefix = GameManager.Instance.LocalStoragePath;
        audioPath = GameManager.Instance.LocalStoragePath + m_TargetLocalPath;
        Debug.Log("audio_play_in_loop: " + data.audio_play_in_loop);
        audioSource.loop = data.audio_play_in_loop;
        var TaregtURI = "";

        if (FileHandler.ValidateFile(m_TargetLocalPath + m_TargetImageName))
        {
            isLocalFile = true;
            TaregtURI = "file://" + prefix + m_TargetLocalPath + m_TargetImageName;
        }
        else
        {
             isLocalFile = false;
             TaregtURI = data.c_code_image;
        }

        Texture2D webTexture = null;

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(TaregtURI))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log("Error:- " + uwr.error);
            }
            else
            {
                webTexture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                // m_ImageTargetTexture.Add(webTexture);

                if (!isLocalFile)
                {
                    FileHandler.SaveFile(m_TargetLocalPath, m_TargetImageName, uwr.downloadHandler.data);
                }
            }
            uwr.Dispose();
        }       

        #if !UNITY_EDITOR
                StartCoroutine(AddImageJob(webTexture, m_TargetImageName));
        #endif
       
        ImageTargetData targetData = new ImageTargetData();

        targetData.m_TargetName = data.filename;
        targetData.m_WebURL = data.ar_content;
        targetData.m_LocalURL = GameManager.Instance.LocalStoragePath + m_TargetLocalPath + data.filename;
        targetData.modelPosition = myPosition;
        targetData.modelRotation = myRotation;
        targetData.modelScale = myScale;
        targetData.audio_loop = data.audio_play_in_loop;

        m_TargetLib.Add(m_TargetImageName, targetData);

    }

    IEnumerator GetAllTexture(string filePath)
    {

        Debug.Log("File Available now Loading Texture : " + filePath);
        yield return new WaitForSeconds(0.1f);
        FileInfo m_FileInfo = new FileInfo(filePath);

        var extractDir = Path.Combine(m_FileInfo.Directory.FullName, m_FileInfo.Name.Substring(0, m_FileInfo.Name.Length - m_FileInfo.Extension.Length));
        DirectoryInfo dirInfo = new DirectoryInfo(extractDir);
        FileInfo[] fileNames = dirInfo.GetFiles("*.*");

        Debug.Log(dirInfo.Attributes.ToString());

        foreach (FileInfo info in fileNames)
        {
            Debug.Log(info.FullName);

            // var AssetURI = "file://" + info.FullName;

            StartCoroutine(LoadTexture("file://" + info.FullName, info.Name));
        }

    }

    IEnumerator LoadTexture(string AssetURI,string name)
    {
        Texture2D webTexture;
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(AssetURI))
        {
            // www.downloadHandler = new DownloadHandlerBuffer();
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log("Error:- " + uwr.error);
            }
            else
            {
                webTexture = DownloadHandlerTexture.GetContent(uwr); //((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
                m_ImageTargetTexture.Add(webTexture);

                webTexture.LoadImage(uwr.downloadHandler.data);

                AddImageJob(webTexture, name);
            }
            uwr.Dispose();

        }
    }

    public IEnumerator AddImageJob(Texture2D texture2D , string imageTargetName)
    {
        yield return null;

        Debug.Log("<color=yellow>" + texture2D.width + " " + texture2D.height + "</color>");

        Debug.Log("Job Starting...");

        var firstGuid = new SerializableGuid(0, 0);
        var secondGuid = new SerializableGuid(0, 0);



        // XRReferenceImage newImage = new XRReferenceImage(firstGuid, secondGuid, new Vector2(0.1f, 0.1f), Guid.NewGuid().ToString(), texture2D);
        XRReferenceImage newImage = new XRReferenceImage(firstGuid, secondGuid, Utility.PixelToMeter(texture2D.width, texture2D.height), Guid.NewGuid().ToString(), texture2D);
        try
        {
            MutableRuntimeReferenceImageLibrary mutableRuntimeReferenceImageLibrary = m_ImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;

            var jobHandle = mutableRuntimeReferenceImageLibrary.ScheduleAddImageJob(texture2D, imageTargetName, 1f);

            while (!jobHandle.IsCompleted)
            {
                //Debug.Log("Job Running...");
            }

            Debug.Log("Job Completed...");

        }
        catch (Exception e)
        {
            if (texture2D == null)
            {
                Debug.Log("texture2D is null");
            }
            Debug.LogError($"Error: {e.ToString()}");
        }
    }

    IEnumerator removeRootChild()
    {
        for (int i = 0; i < rootObject.transform.childCount; i++)
        {
            DestroyImmediate(rootObject.transform.GetChild(i).transform.gameObject);
            yield return new WaitForEndOfFrame();
        }
    }

    bool isPreloaderPositionSet = false;
    int n = 0;
    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        // updated, set prefab position and rotation
        foreach (ARTrackedImage image in obj.updated)
        {
            //Debug.Log("image name: " + image.referenceImage.name);
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            if (image.trackingState == TrackingState.Tracking)
            {
                CancelInvoke("stopMyAudio");
                if (m_LastTrackedImage.Equals("none") || !m_LastTrackedImage.Equals(image.referenceImage.name))
                {
                    if (audioSource.isPlaying)
                        audioSource.Stop();

                    audioSource.clip = null;
                    m_PreviousTrackedImage = m_LastTrackedImage;
                    m_LastTrackedImage = image.referenceImage.name;
                    
                    Debug.Log("TrackedImagesChanged: " + m_LastTrackedImage);
                    if (rootObject.transform.childCount > 0)
                    {
                        for (int i = 0; i < rootObject.transform.childCount; i++)
                        {
                            DestroyImmediate(rootObject.transform.GetChild(i).transform.gameObject);
                        }
                    }

                    if (File.Exists(m_TargetLib[image.referenceImage.name].m_LocalURL))
                    {
                        isLocalModleFile = true;
                        isModelAvailable = false;
                        localModelFilePath = m_TargetLib[image.referenceImage.name].m_LocalURL;
                        Debug.Log("localModelFilePath: " + localModelFilePath);
                        ModelLoader.StartLoadObject(localModelFilePath, true, ModelLoaded, OnMaterialsLoad);
                    }
                    else
                    {
                        isLocalModleFile = false;
                        isModelAvailable = false;
                        localModelFilePath = m_TargetLib[image.referenceImage.name].m_LocalURL;
                        Debug.Log("webURL: " + m_TargetLib[image.referenceImage.name].m_WebURL);
                        ModelLoader.StartLoadObject(m_TargetLib[image.referenceImage.name].m_WebURL, false, ModelLoaded, OnMaterialsLoad);
                    }
                    n = 0;
                    audioSource.loop = m_TargetLib[image.referenceImage.name].audio_loop;
                    Builder_Position = m_TargetLib[image.referenceImage.name].modelPosition;
                    Builder_Rotation = m_TargetLib[image.referenceImage.name].modelRotation;
                    Builder_Scale = m_TargetLib[image.referenceImage.name].modelScale;
                    m_ArObjectsToPlace.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                    Debug.Log("Audio loop: " + audioSource.loop);
                }

                if (n == 0)
                {
                    n = 1;
                    builderTransform.setTransform(Builder_Position, Builder_Rotation, Builder_Scale);

                    if (!isInventoryApiCall)
                    { isInventoryApiCall = true; GameManager.Instance.OnCheckToUnlockModule(3); }
                }

                if (audioSource.clip != null && !audioSource.isPlaying && audioFinish)
                {
                    audioFinish = false;
                    audioSource.Play();
                }

                rootObject.gameObject.SetActive(true);
                
                if (n == 1)
                    n = 2;

                isPreloaderPositionSet = true;
                m_Preloader.transform.position = image.transform.position;

                m_ArObjectsToPlace.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
            }
            else if (image.trackingState != TrackingState.Tracking && m_LastTrackedImage.Equals(image.referenceImage.name))
            {
                Debug.Log("state: " + image.trackingState + ", imageName: " + m_LastTrackedImage);
                StopAllCoroutines();
                ModelLoader.onStopDownload();
                rootObject.gameObject.SetActive(false);
                isPreloaderPositionSet = false;
                /*if (audioSource.clip != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }*/
                if (!IsInvoking("stopMyAudio"))
                    Invoke("stopMyAudio", 0.25f);
            }
        }

        // removed, destroy spawned instance
        foreach (ARTrackedImage image in obj.removed)
        {
            Destroy(rootObject.gameObject);
        }
    }

    void stopMyAudio()
    {
        audioSource.Stop();
        audioFinish = true;
    }

    private void ModelLoadFinished(UnityWebRequest request)
    {
        Debug.Log("Model Finished loading");
        if (!isLocalModleFile && localModelFilePath != null)
        {
            var data = request.downloadHandler.data;
            Debug.Log("Model data: " + data);
            Debug.Log("Save File at : " + localModelFilePath);
            if (data != null)
            {
                File.WriteAllBytes(localModelFilePath, data);
                FileHandler.ExtractFiles(localModelFilePath, audioPath, true);// extract zip at path
            }
        }
        Debug.Log("Root obj active: " + rootObject.gameObject.activeSelf);
        if (!rootObject.gameObject.activeSelf)
            rootObject.gameObject.SetActive(true);
    }

    bool isModelAvailable = false;

    bool ImageFound()
    {
        return m_ImageManager?.trackables.count > 0;
    }

    private void Update()
    {
        if (isBackBtnClick)
            return;

        /*if (!ImageFound() && audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (audioSource.isPlaying && audioLength > 0f)
            audioLength -= Time.deltaTime;
        else if (!audioSource.isPlaying && audioLength <= 0f && !audioSource.loop)
        {
            audioSource.Stop();
        }*/

        if (ImageFound() && !isModelAvailable)
        {
            if (!m_Preloader.gameObject.activeInHierarchy)
                m_Preloader.SetActive(true);
        }
        else
        {
            m_ScreenSpaceUI.SetActive(true);
            if (m_Preloader.gameObject.activeInHierarchy)
                m_Preloader.SetActive(false);
        }

        if (ImageFound() && m_ScreenSpaceUI.activeSelf == true)
        {
            m_ScreenSpaceUI.SetActive(false);
        }
    }

    public void ModelLoaded(AssetLoaderContext assetLoaderContext) {
        //isModelAvailable = true;
    }

    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        if (isBackBtnClick)
            return;

        Debug.Log("Materials loaded. Model fully loaded: " + rootObject.transform.childCount.ToString());
        isModelAvailable = true;

        string mp3Path = GetAudioPath();
        Debug.Log("Mp3Path: " + mp3Path);
        if (!string.IsNullOrEmpty(mp3Path))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(mp3Path);
            FileInfo[] audioFile = dirInfo.GetFiles("*.mp3");
            if (audioFile.Length > 0)
            {
                string str = "file://" + audioFile[0].ToString();
                OnDownloadAudio(str);//StartCoroutine(LoadAudioFile(str));
            }
        }

        Debug.Log("1 Root obj active: " + rootObject.gameObject.activeSelf);
        if (!rootObject.gameObject.activeSelf)
            rootObject.gameObject.SetActive(true);
    }

    public void ResetGameObject(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        obj.transform.localScale = Vector3.one;
    }

    public void onBackBtnClick()
    {
        isBackBtnClick = true;
        AssetDownloaderBehaviour.OnDownloadFinished -= ModelLoadFinished;
        StopAllCoroutines();
        CancelInvoke();
        ModelLoader.onStopDownload();
        if (audioSource.isPlaying)
            audioSource.Stop();

        m_TargetLib.Clear();
        ModuleContent.Clear();
        ModuleContent.TrimExcess();
        Destroy(tempClip);
    }

    #region PlayAudio
    string GetAudioPath()
    {
        // return path of the mp3 file directory
        string path = localModelFilePath.Substring(0, localModelFilePath.Length - 4);
        Debug.Log("inside GetAudioPath: " + path);
        
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] audioFile = dirInfo.GetFiles("*.mp3");
        if (audioFile.Length != 0)
        { return path; }
        else
            return "";
    }

    public async void OnDownloadAudio(string url)
    {
        Debug.Log("OnDownloadAudio url: " + url);

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Get Audio Error: " + request.error);
        }
        else
        {
            tempClip = DownloadHandlerAudioClip.GetContent(request);
             
            Debug.Log("Audio length: " + tempClip.length);
            if (tempClip.length <= 0)
            {
                string msg = "Unsupported file or audio format.";
                ApiManager.Instance.errorWindow.SetErrorMessage("Something Went Wrong", msg, "OKAY", ErrorWindow.ResponseData.JustClose, false);
            }
            else
            {
                if (audioSource == null)
                    return;

                audioSource.clip = tempClip;
                while (audioSource.clip.loadState != AudioDataLoadState.Loaded)
                {
                    Debug.Log("Waiting for clip to ready");
                }
                Debug.Log("Clip loaded success for marker: " + m_LastTrackedImage);
                Debug.Log("Audio name: " + audioSource.clip.name);
                Debug.Log("Audio volume: " + audioSource.volume);
                if (!string.IsNullOrEmpty(m_LastTrackedImage))
                {
                    audioSource.Play();
                }
            }
        }
        request.Dispose();
    }

    private IEnumerator LoadAudioFile(string fullpath)
    {
        if (!string.IsNullOrEmpty(fullpath))
        {
            Debug.Log("LOADING CLIP: " + fullpath);
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullpath, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("Get Audio Error: " + www.error);
                }
                else
                {
                    tempClip = DownloadHandlerAudioClip.GetContent(www);
                    Debug.Log("Audio length: " + tempClip.length);
                    if (tempClip.length <= 0)
                    {
                        string msg = "Unsupported file or audio format.";
                        ApiManager.Instance.errorWindow.SetErrorMessage("Something Went Wrong", msg, "OKAY", ErrorWindow.ResponseData.JustClose, false);
                    }
                    else
                    {
                        audioSource.clip = tempClip;
                        while (audioSource.clip.loadState != AudioDataLoadState.Loaded)
                        {
                            Debug.Log("Waiting for clip to ready");
                        }
                        Debug.Log("Clip loaded success");
                        Debug.Log("Audio name: " + audioSource.clip.name);
                        Debug.Log("Audio volume: " + audioSource.volume);
                        audioSource.Play();
                    }
                }
                www.Dispose();
            }
        }
    }
    #endregion
}
