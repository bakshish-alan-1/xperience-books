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

            productName.text = data.name;

            string path = "";
            if (product.image.Count > 0)
                path = product.image[0];

            if (data.attributes.Count > 0)
            {
                if (data.attributes[0].id == 0)
                {
                    if (data.attributes[0].sizes.Count > 0)// && !string.IsNullOrEmpty(data.attributes[0].sizes[0].size_image))
                    {
                        path = data.attributes[0].sizes[0].size_image;
                        priceText.text = "$" + data.attributes[0].sizes[0].size_price;
                    }
                }
                else
                {
                    path = data.attributes[0].color_image;
                    if (data.attributes[0].sizes.Count > 0)
                    {
                        priceText.text = "$" + data.attributes[0].sizes[0].size_price;
                    }
                    else
                        priceText.text = "$" + data.attributes[0].color_price;
                }
            }


            if (string.IsNullOrWhiteSpace(path))
                return;

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
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
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
