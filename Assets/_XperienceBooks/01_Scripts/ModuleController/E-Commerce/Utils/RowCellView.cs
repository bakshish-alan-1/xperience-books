using System;
using System.Collections;
using System.Collections.Generic;
using Ecommerce;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RowCellView : MonoBehaviour
{
    public GameObject container;
   
    public Button button;

    public Image cellImage;
    public TMPro.TextMeshProUGUI priceText;
    public TMPro.TextMeshProUGUI productName;

    public Texture2D defaultTexture;

    public Product product;


    private void Start()
    {
        button.onClick.AddListener(() => OnClick());
    }

    /// <summary>
    /// This function just takes the Demo data and displays it
    /// </summary>
    /// <param name="data"></param>
    public void SetData(Product data)
    {
        // this cell was outside the range of the data, so we disable the container.
        // Note: We could have disable the cell gameobject instead of a child container,
        // but that can cause problems if you are trying to get components (disabled objects are ignored).



        StopCoroutine("LoadRemoteImage");
        container.SetActive(data != null);

        if (data != null)
        {
            product = data;
            // set the text if the cell is inside the data range
            // text.text = data.someText;

            priceText.text = "$"+ data.price;
            productName.text = data.name;

            string path = "";
            if (product.image.Count > 0)
                path = product.image[0];

            if (string.IsNullOrWhiteSpace(path))
                return;

            // Debug.Log("*** : "+gameObject.activeInHierarchy);
            //  if(gameObject.activeInHierarchy)

            this.path = path;
            Invoke("LateIamgeUpdate", 1f);
        }
    }
    string path;
    void LateIamgeUpdate() {
        if (gameObject.activeInHierarchy)
            StartCoroutine("LoadRemoteImage");
    }

    private void OnDisable()
    {
        product = new Product();
        ClearImage();
    }

    public IEnumerator LoadRemoteImage()
    {
        Debug.Log("Load Image from web Data");
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
            // cellImage.texture = texture;
            cellImage.sprite = Utility.Texture2DToSprite(texture);
            cellImage.preserveAspect = true;
        }
        else
        {
            ClearImage();
        }
    }

    public void ClearImage()
    {
        cellImage.sprite = Utility.Texture2DToSprite(defaultTexture);
        cellImage.preserveAspect = true;
        // cellImage.texture = defaultTexture;
    }

    public void OnClick()
    {
        // Debug.Log("Click on : " + product.name);

        ECommerceManager.Instance.ViewProductDetails(product);
    }
}
