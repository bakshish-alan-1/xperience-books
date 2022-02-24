using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GalleryCell : MonoBehaviour
{

    [SerializeField] RawImage image;
    Texture2D webTexture;
    public GalleryViewData m_CellData;

    public Button button;


    public void SetGalleryTexture(bool isLocalFile, string URL, string localPath, string fileName, GalleryViewData data) {
        m_CellData = data;
        StartCoroutine(LoadTexture(isLocalFile, URL, localPath, fileName));
    }

    private void OnDisable()
    {
        Destroy(webTexture);
        Destroy(button);
    }


    IEnumerator LoadTexture(bool isLocalFile, string URL , string localPath ,string fileName) {

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(URL))
        {
            button.interactable = false;

            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error:- " + uwr.error);
            }
            else
            {
                webTexture = DownloadHandlerTexture.GetContent(uwr);

                image.texture = webTexture;

                image.gameObject.transform.GetComponent<RatioMaintainer>().SizeToParent();

                if (!isLocalFile)
                {
                    FileHandler.SaveFile(localPath, fileName, uwr.downloadHandler.data);
                }

                if (button)
                    button.interactable = true;
            }
        }
    }

    public void OnClickImage() {
        Debug.Log("GalleryCell OnClickImage");
        GalleryView.Instance.LoadFullView(webTexture, m_CellData);
    }
}
