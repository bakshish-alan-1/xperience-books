using System.Collections;
using System.Collections.Generic;
using System.Text;
using Ecommerce;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class CheckOutOrderCell : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI title;
    public TextMeshProUGUI m_Discription;
    public TextMeshProUGUI m_Price;
    public TextMeshProUGUI m_qty;
    public Image cellImage;
    public Texture2D defaultTexture;



    public void SetData(CartProduct data, int index)
    {

        this.index = index;
        title.text = data.m_product.name;
        StringBuilder disc = new StringBuilder();
        disc.Append("");
        if (data.m_SelectedAttributes[0] >= 0)
        {
            disc.Append("Color: " + data.m_product.attributes[data.m_SelectedAttributes[0]].color_name);

            if (data.m_SelectedAttributes[1] >= 0)
            {
                disc.Append("    Attribute: " + data.m_product.attributes[data.m_SelectedAttributes[0]].sizes[data.m_SelectedAttributes[1]].size_name);
            }
            StartCoroutine(LoadRemoteImage(data.m_product.attributes[data.m_SelectedAttributes[0]].color_image));
        }
        else if (data.m_SelectedAttributes[1] >= 0)
        {
            disc.Append("Attribute: " + data.m_product.attributes[0].sizes[data.m_SelectedAttributes[1]].size_name);

            StartCoroutine(LoadRemoteImage(data.m_product.attributes[0].sizes[data.m_SelectedAttributes[1]].size_image));
        }
        else if (data.m_product.image.Count >= 1)
        {
            StartCoroutine(LoadRemoteImage(data.m_product.image[0]));
        }

        m_Discription.text = disc.ToString();

        m_qty.text = "Qty : "+data.m_TotalQty;

        m_Price.text = "$ "+data.m_FinalPrice;
    }

    public void SetPreviousOrderData(OrderItems data, int index)
    {

        this.index = index;
        title.text = data.product_name;
        StringBuilder disc = new StringBuilder();
        disc.Append("");
        if (data.order_attributes[0] >= 0)
        {
            disc.Append("Color : " + data.product_attributes[data.order_attributes[0]].color_name);
        }

        if (data.order_attributes[1] >= 0)
        {
            disc.Append("    Size : " + data.product_attributes[data.order_attributes[0]].sizes[data.order_attributes[1]].size_name);
        }
        m_Discription.text = disc.ToString();

        m_qty.text = "Qty : " + data.order_qty;

        m_Price.text = "$ " + data.order_price;


        if (data.order_item_images.Count > 0)
        {

            StartCoroutine(LoadRemoteImage(data.order_item_images[0].image_name));
        }
    }

    public IEnumerator LoadRemoteImage(string path)
    {

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
            cellImage.sprite =  Utility.Texture2DToSprite(texture);
            cellImage.preserveAspect = true;
        }
        else
        {
            ClearImage();
        }
    }

    void ClearImage()
    {

        cellImage.sprite = Utility.Texture2DToSprite(defaultTexture);
    }


}
