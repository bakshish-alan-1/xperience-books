using System.Collections;
using System.IO;
using TriLibCore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadModelFromURL : MonoBehaviour
{
    public bool ShowProgress;
    public GameObject _RootObject;
    public Text progressText;
    public Image progressBar;

    UnityWebRequest webRequest;

    public void DownloadFile(string _url, string localFilePath, System.Action<AssetLoaderContext> OnLoad, System.Action<AssetLoaderContext> OnMaterialsLoad)
    {
        StartCoroutine(LoadTexture(_url, localFilePath, OnLoad, OnMaterialsLoad));
    }

    public void onStopDownload()
    {
        StopAllCoroutines();
        webRequest = null;
    }

    IEnumerator LoadTexture(string URL, string localFilePath, System.Action<AssetLoaderContext> OnLoad, System.Action<AssetLoaderContext> OnMaterialsLoad)
    {
        Debug.Log("LoadTexture Img URL: " + URL);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(URL))
        {
            uwr.SendWebRequest();
            while (!uwr.isDone)
            {
                float progress = uwr.downloadProgress;
                Debug.Log(progress * 100);
                if (ShowProgress && progressText != null)
                {
                    progressBar.fillAmount = progress;
                    progressText.text = (progress * 100).ToString("00") + " %";
                }

                yield return null;
            }

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error:- " + uwr.error);
            }
            else
            {
                Debug.Log("File downloaded");
                File.WriteAllBytes(localFilePath, uwr.downloadHandler.data);

                StartLoadObject(localFilePath, true, OnLoad, OnMaterialsLoad);
            }
            uwr.Dispose();
        }
    }

    public void StartLoadObject(string _url, bool isLocal, System.Action<AssetLoaderContext> OnLoad, System.Action<AssetLoaderContext> OnMaterialsLoad)
    {
        if (progressBar != null)
            progressBar.fillAmount = 0f;

        Debug.Log("StartLoadObject path/url: " + _url);
        webRequest = AssetDownloader.CreateWebRequest(_url);
        if (isLocal)
            AssetDownloader.LoadModelFromZip(_url, OnLoad, OnMaterialsLoad, OnProgress, OnError, _RootObject);//, assetLoaderOptions
        else
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, _RootObject);//, assetLoaderOptions
        
    }

    private void OnError(IContextualizedError obj)
    {
        Debug.LogError($"An error ocurred while loading your Model: {obj.GetInnerException()}");
    }

    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
        if (!ShowProgress)
        {
            return;
        }

        if (progressText != null)
        {
            progressBar.fillAmount = progress;
            progressText.text = (progress * 100).ToString("00") + " %";
        }
    }
}
