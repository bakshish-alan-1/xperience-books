using System.Collections;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using UnityEngine;
using UnityEngine.Networking;

public class PlaneTrackingController : MonoBehaviour
{
    public static PlaneTrackingController Instance = null;

    bool isLocalFile = false;
    [SerializeField] GameObject m_SpawnObject;
    public bool isMaterialLoaded = false;
    string filePath;
    string fileName;
    int currentIndex = 0;
    public string m_WebURL;
    public string m_LocalURL;

    //NeedToUpdate Trilib
    [SerializeField]
    LoadModelFromURL m_ModelDownloader;
    string finalPath = "", localModelFilePath = "";
    string audioPath = "";

    public List<ContentModel> PlaneTrackingData = new List<ContentModel>();

    [Header("Audio Player")]
    [SerializeField] AudioSource audioSource;

    bool isBackBtnClick = false;
    AudioClip temp = null;

    private void Awake()
    {
        // Chnage value if need to chanhe runtime multiple list
        currentIndex = 0;
        LoadModel();
    }

    private void OnEnable()
    {
        AssetDownloaderBehaviour.OnDownloadFinished += ModelLoadFinished;
    }

    void OnDisable()
    {
        AssetDownloaderBehaviour.OnDownloadFinished -= ModelLoadFinished;
    }

    public void LoadModel() {

        PlaneTrackingData.AddRange(GameManager.Instance.GetModuleData());

        //passing index 0 because there is only 1 file in plane tracking model , if multiple then need to pass acordingly value
        filePath = GameManager.Instance.GetLocalPath(StaticKeywords.PlaneTracking);
        fileName = PlaneTrackingData[currentIndex].filename;

        FileHandler.ValidateFolderStructure(filePath); // Create folder if not exist. 

        if (FileHandler.ValidateFile(filePath + fileName)){
            isLocalFile = true;
            finalPath = FileHandler.FinalPath(filePath, fileName);
        }
        else{
            isLocalFile = false;
            finalPath = PlaneTrackingData[currentIndex].ar_content;
        }

        localModelFilePath = FileHandler.FinalPath(filePath, fileName);
        m_LocalURL = FileHandler.FinalPath(filePath, fileName); // For Example :- "file://" + Application.persistentDataPath + "/" + filePath + fileName;
        m_WebURL = PlaneTrackingData[currentIndex].ar_content;
        audioPath = GameManager.Instance.LocalStoragePath + filePath;
        Debug.Log("audioPath: " + audioPath);
        Debug.Log("Audio Loop: " + PlaneTrackingData[currentIndex].audio_play_in_loop);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;

        m_ModelDownloader.StartLoadObject(finalPath, isLocalFile, ModelLoaded, OnMaterialsLoad);
        GameManager.Instance.safetyWindow.OpenWindow();// call Safetywindow popup
    }

    public void onBackBtnClick()
    {
        isBackBtnClick = true;
        AssetDownloaderBehaviour.OnDownloadFinished -= ModelLoadFinished;
        StopAllCoroutines();
        CancelInvoke();
        m_ModelDownloader.onStopDownload();
        if (audioSource.isPlaying)
            audioSource.Stop();

        PlaneTrackingData.Clear();
        PlaneTrackingData.TrimExcess();
        Destroy(temp);
    }

    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        if (isBackBtnClick)
            return;

        Debug.Log("Materials loaded. Model fully loaded: ");
        isMaterialLoaded = true;

        audioPath = GetSubDirectory(audioPath);
        if (!string.IsNullOrEmpty(audioPath))
        {
            string strAudio = GetAudioPath(audioPath);// getting audio file path
            if (!string.IsNullOrEmpty(strAudio))
            {
                string str = "file://" + strAudio;
                StartCoroutine(LoadAudioFile(str));
            }
        }
    }

    private void ModelLoadFinished(UnityWebRequest request)
    {
        if (isBackBtnClick)
            return;

        Debug.Log("Model Finished loading");
        if (isLocalFile)
        {
            PlacementController.Instance.UpdatePlacementState();
            return;
        }
        var data = request.downloadHandler.data;
        if (!isLocalFile && localModelFilePath != "")
        {
            Debug.Log("Save File at : " + localModelFilePath);
            File.WriteAllBytes(localModelFilePath, data);
            FileHandler.ExtractFiles(localModelFilePath, audioPath, true);
        }
    }

    public void ModelLoaded(AssetLoaderContext assetLoaderContext)
    {
        if (isBackBtnClick)
            return;

        Debug.Log("ModelLoaded: ");
        PlacementController.Instance.UpdatePlacementState();
    }

    string GetFBXModelPath(string DirPath)// Return path of the fbx file at given directory path
    {
        string[] dir = Directory.GetDirectories(DirPath);
        string str = "";

        if (dir.Length != 0)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir[0].ToString());
            FileInfo[] file = dirInfo.GetFiles("*.fbx");
            if (file.Length != 0)
                str = file[0].ToString();
        }
        return str;
    }

    #region PlayAudio
    string GetSubDirectory(string path)// Return first subdirectory path of extract directory from .zip file
    {
        string dir = m_LocalURL.Substring(0, m_LocalURL.Length - 4);
        return dir;
    }

    string GetAudioPath(string DirPath)// Return path of the audio file at given directory path
    {
        DirectoryInfo dirInfo = new DirectoryInfo(DirPath);
        FileInfo[] audioFile = dirInfo.GetFiles("*.mp3");
        if (audioFile.Length != 0)
            return audioFile[0].ToString();
        else
            return "";
    }

    private IEnumerator LoadAudioFile(string fullpath)
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
                temp = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("audio_play_in_loop: " + PlaneTrackingData[currentIndex].audio_play_in_loop);
                audioSource.loop = PlaneTrackingData[currentIndex].audio_play_in_loop;
                audioSource.clip = temp;
                Debug.Log("temp.length: " + temp.length);
                Debug.Log("m_SpawnObject: " + m_SpawnObject.activeSelf);
                if (temp.length <= 0)
                {
                    string msg = "Unsupported file or audio format.";
                    ApiManager.Instance.errorWindow.SetErrorMessage("Something Went Wrong", msg, "OKAY", ErrorWindow.ResponseData.JustClose, false);
                }
                else if (m_SpawnObject.activeSelf)
                {
                    audioSource.Play();
                }
            }
        }
    }
    #endregion
}
