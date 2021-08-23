using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GalleryCell : MonoBehaviour
{

    [SerializeField]
    RawImage image;
    Texture2D webTexture;
    public GalleryViewData m_CellData;



    public void SetGalleryTexture(bool isLocalFile, string URL, string localPath, string fileName, GalleryViewData data) {
        m_CellData = data;
        StartCoroutine(LoadTexture(isLocalFile, URL, localPath, fileName));
    }


    IEnumerator LoadTexture(bool isLocalFile, string URL , string localPath ,string fileName) {

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(URL))
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

                image.texture = webTexture;

                image.gameObject.transform.GetComponent<RatioMaintainer>().SizeToParent();

                if (!isLocalFile)
                {
                    FileHandler.SaveFile(localPath, fileName, uwr.downloadHandler.data);
                }
            }
        }
}

    public void OnClickImage() {
        Debug.Log("GalleryCell OnClickImage");
        GalleryView.Instance.LoadFullView(webTexture, m_CellData);
    }
}
