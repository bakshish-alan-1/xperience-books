using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Ecommerce
{
    public class LoadImageTexture : MonoBehaviour
    {

        [SerializeField]
        Image image;


        public string m_LocalURL, m_ServerURL;

        public bool isLocal = false;

        public Texture2D defaultTexture;
        public Texture2D loadingTexture;



        public void Load()
        {


        }

        public void LoadImage()
        {
            StopCoroutine("LoadRemoteImage");
            image.sprite = Utility.Texture2DToSprite(loadingTexture);
            image.preserveAspect = true;

            if (isLocal)
            {

            }
            else
            {
                if (string.IsNullOrWhiteSpace(m_ServerURL)) {
                //    Debug.Log("Image is nulled");
                    image.sprite = Utility.Texture2DToSprite(defaultTexture);
                    image.preserveAspect = true;
                    return;
                }
                StartCoroutine("LoadRemoteImage",m_ServerURL);
            }
        }


        public IEnumerator LoadRemoteImage(string path)
        {
//            Debug.Log("Loading Image : " + path);
            Texture2D texture = null;
         
            // Get the remote texture

#if UNITY_2017_4_OR_NEWER
            var webRequest = UnityWebRequestTexture.GetTexture(path);
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Failed to download image [" + path + "]: " + webRequest.error);
            }
            else
            {
                texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            }
#else
            WWW www = new WWW(path);
            yield return www;
            texture = www.texture;
#endif

            if (texture != null)
            {
                image.sprite = Utility.Texture2DToSprite(texture);
                image.preserveAspect = true;
            }
            else
            {
                ClearImage();
            }
        }

        public void ClearImage()
        {
            image.sprite = Utility.Texture2DToSprite(defaultTexture);
            image.preserveAspect = true;
        }


    }
}