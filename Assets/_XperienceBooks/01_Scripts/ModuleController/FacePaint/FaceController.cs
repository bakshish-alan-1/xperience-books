using System.Collections;
using System.Collections.Generic;
using System.IO;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFaceManager))]
public class FaceController : MonoBehaviour
{
    //[SerializeField]
    //private Button swapCameraToggle;

    public static FaceController Instance = null;
  
    public TMPro.TextMeshProUGUI materialName;

    private ARFaceManager arFaceManager;

    private bool faceTrackingOn = true;
    public bool isFaceFound = false;

    private int swapCounter = 0;

    public Material FaceMaterial;

    string m_FolderPath;

    [SerializeField]
    public GameObject InfoBox, loadingUI, m_ScreenSpaceUI;
    public CanvasGroup m_additionalUI;
    
    [SerializeField]
    public List<Texture2D> m_FacePaintTextures = new List<Texture2D>();

    private int currentMateria = -1;
    int totalFileCount = 0;
    List<ItemData> cellData = new List<ItemData>();
    [SerializeField] ScrollView scrollView = default;

    void Awake() 
    {
        Instance = this;
        arFaceManager = GetComponent<ARFaceManager>();

        LoadPaintTexture();
    }

    public void Start()
    {
        scrollView.OnSelectionChanged(OnSelectionChanged);
        GameManager.Instance.safetyWindow.OpenWindow();// call Safetywindow popup
    }

    public void OnInfoBtnHite(bool value)
    {
        InfoBox.SetActive(value);
    }

    public void LoadPaintTexture() {

        if (GameManager.Instance._ModuleData.Count <= 0) return;

        foreach (ContentModel model in GameManager.Instance._ModuleData) {

            StartCoroutine(LoadContent(model));
        }
    }

    bool isLocalFile = false;
    public IEnumerator LoadContent(ContentModel data) {

         string localPath = GameManager.Instance.GetLocalPath(StaticKeywords.FacePaint);  //data.series_name + "/" + data.book_name + "/" + data.chapter_name +"/" + StaticKeywords.FacePaint + "/";
         string fileName = data.filename;

        if (FileHandler.ValidateFile(localPath + fileName))
        {
            isLocalFile = true;
        }
        else
        {
            isLocalFile = false;
            using (UnityWebRequest uwr = UnityWebRequest.Get(data.ar_content))
            {
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log("Error:- " + uwr.error);
                }
                else
                {
                    if (!isLocalFile)
                    {
                        FileHandler.SaveFile(localPath, fileName, uwr.downloadHandler.data,true);
                    }
                }
            }
        }

        Debug.Log("Now all clear call  Next function");
        StartCoroutine(GetAllTexture(FileHandler.FinalPath(localPath, fileName)));
    }

    IEnumerator GetAllTexture(string filePath) {

        FileInfo m_FileInfo = new FileInfo(filePath);
        var extractDir = Path.Combine(m_FileInfo.Directory.FullName, m_FileInfo.Name.Substring(0, m_FileInfo.Name.Length - m_FileInfo.Extension.Length));

#if UNITY_ANDROID
        extractDir += "/android";
#elif UNITY_IOS
        extractDir += "/ios";
#endif
        Debug.Log("Final path: " + extractDir);

        DirectoryInfo dirInfo = new DirectoryInfo(extractDir);
        FileInfo[] fileNames = dirInfo.GetFiles("*.*");
        totalFileCount = fileNames.Length;

         foreach (FileInfo info in fileNames) {
            Debug.Log(info.FullName);
            ItemData data = new ItemData(); //new ItemData($"Cell {i}", m_FacePaintTextures[i]);
            cellData.Add(data);
        }
        scrollView.UpdateData(cellData);

        foreach (FileInfo info in fileNames)
        {
            // var AssetURI = "file://" + info.FullName;
            StartCoroutine(LoadTexture("file://" + info.FullName));
        }

        Debug.Log("Total File Count : " + totalFileCount);
        while (totalFileCount > m_FacePaintTextures.Count) {
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Load Texture count : " + m_FacePaintTextures.Count + ", cellData: " + cellData.Count);

        for (int i = 0; i < m_FacePaintTextures.Count; i++) {
            cellData[i].Message = $"Cell {i}";
            cellData[i].texture = m_FacePaintTextures[i];
        }

        scrollView.SelectCell(0);
        //SwapFaces(0);
        loadingUI.SetActive(false);
        GameManager.Instance.OnCheckToUnlockModule(7);
    }

    IEnumerator LoadTexture(string AssetURI) {

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
                Texture2D webTexture = DownloadHandlerTexture.GetContent(uwr); //((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
                m_FacePaintTextures.Add(webTexture);
               // scroll.Add(webTexture);
            }

        }

    }

    public void SwapFaces(int index) 
    {

        if (m_FacePaintTextures.Count <= 0)
            return;
        
        foreach(ARFace face in arFaceManager.trackables)
        {

            MeshRenderer renderer = face.GetComponent<MeshRenderer>();
            var mats = renderer.materials;

            if (mats.Length <= 0) {
                renderer.material = FaceMaterial;
            }
            mats = renderer.materials;

            face.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", m_FacePaintTextures[index]);
        }

        materialName.text = $"Face Material ({m_FacePaintTextures[swapCounter]})";

        Debug.Log($"Face Material ({m_FacePaintTextures[swapCounter]})");
    }

    void ToggleTrackingFaces() 
    { 
        faceTrackingOn = !faceTrackingOn;

        foreach(ARFace face in arFaceManager.trackables)
        {
            face.enabled = faceTrackingOn;
        }
    }

    public void CameraButtonHit()
    {
#if UNITY_IOS
        if (isFaceFound == true)
            CaptureAndShare.Instance.TakeScreenshot();
#endif

#if UNITY_ANDROID
        if (FaceFound() || isFaceFound == true)
            CaptureAndShare.Instance.TakeScreenshot();
#endif
    }

    private void Update()
    {
        if (FaceFound())
        {   
            m_ScreenSpaceUI.SetActive(false);
            if (m_additionalUI != null)
            {
                m_additionalUI.alpha = 1;
                m_additionalUI.interactable = true;
                m_additionalUI.blocksRaycasts = true;
            }
        }
        /*
        else
        {
            if (m_additionalUI != null)
            {
                m_additionalUI.alpha = 0;
                m_additionalUI.interactable = false;
                m_additionalUI.blocksRaycasts = false;
            }
        }*/
    }

    void OnSelectionChanged(int index)
    {
        currentMateria = index;
        //SwapFaces(index);
    }

    public void setMaterial()
    {
        Debug.Log("setMaterial currentMateria: " + currentMateria);
        if (currentMateria < 0)
            return;

        foreach (ARFace face in arFaceManager.trackables)
        {

            MeshRenderer renderer = face.GetComponent<MeshRenderer>();
            var mats = renderer.materials;

            if (mats.Length <= 0)
            {
                renderer.material = FaceMaterial;
            }
            mats = renderer.materials;

            face.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", m_FacePaintTextures[currentMateria]);
        }
    }

    bool FaceFound()
    {
        return arFaceManager?.trackables.count > 0;
    }
}

[System.Serializable]
public class FaceMaterial
{
    public Texture2D texture;
    public string Name;
}