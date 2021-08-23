using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using UnityEngine;

public class Extras : MonoBehaviour
{
    [SerializeField] LoadModelFromURL m_ModelDownloader;
    [SerializeField] GameObject doorRoot;

    string path = "/Users/macbook/Library/Application Support/continuum/Xperience Books/LocalStorage/Door.zip";

    public void OnDownload()
    {
        m_ModelDownloader.StartLoadObject(path, true, ModelLoaded, OnMaterialsLoad);
    }

    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Materials loaded. Model fully loaded.");
    }

    public void ModelLoaded(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("ModelLoaded: ");

        doorRoot.transform.GetChild(0).GetComponent<Animation>().wrapMode = WrapMode.Once;
    }

    public void OnPlayAnimation()
    {
        doorRoot.transform.GetChild(0).GetComponent<Animation>().Play("Door");
    }
}
