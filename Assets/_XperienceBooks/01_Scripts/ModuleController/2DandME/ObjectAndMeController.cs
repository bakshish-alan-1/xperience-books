using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ObjectAndMeController : MonoBehaviour
{

    public GameObject InfoBox, m_TutorialUI;


    [SerializeField]
    Image m_RawImage;

    [SerializeField]
    Texture2D m_2DTexture;

    string AssetURI;
    public int m_CurrentIndex = 0;

    public List<ContentModel> ModuleContent = new List<ContentModel>();

    bool isInventoryApiCall = false;

    public void Awake()
    {
        if (PlayerPrefs.GetInt("isTutorialOver") == 0)
        {
            m_TutorialUI.SetActive(true);
        }
        else {
            m_TutorialUI.SetActive(false);
        }

        ModuleContent.AddRange(GameManager.Instance.GetModuleData());
    }

    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.safetyWindow.OpenWindow();// call Safetywindow popup
        StartCoroutine(LoadTexture());
    }

    public void OnInfoBtnHite(bool value)
    {
        InfoBox.SetActive(value);
    }

    public IEnumerator LoadTexture()
    {
        string localPath;

        localPath = GameManager.Instance.GetLocalPath(StaticKeywords.ObjectAndMe); //ContentManager.Instance.LocalPath(m_CurrentIndex, StaticKeywords.ObjectAndMe);
        string fileName = ModuleContent[m_CurrentIndex].filename; //ContentManager.Instance._ModuleData[m_CurrentIndex].filename;
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
                m_2DTexture = DownloadHandlerTexture.GetContent(uwr); //((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;

                m_RawImage.sprite = Utility.Texture2DToSprite(m_2DTexture);
                m_RawImage.preserveAspect = true;
                m_RawImage.enabled = true;

                if (!isLocalFile)
                {
                    Debug.Log("Save At : " + localPath);
                    FileHandler.SaveFile(localPath, fileName, uwr.downloadHandler.data);
                }
            }
        }
        if (!isInventoryApiCall)
        { isInventoryApiCall = true; GameManager.Instance.OnCheckToUnlockModule(12); }
    }
}
