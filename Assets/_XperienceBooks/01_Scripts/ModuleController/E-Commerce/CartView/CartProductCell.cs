using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ecommerce;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CartProductCell : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI title;
    public TextMeshProUGUI m_Discription;
    public TextMeshProUGUI m_Price;
    public TextMeshProUGUI m_qty;
    public Image cellImage;
    public Texture2D defaultTexture;
    public CartProduct cartProduct;

    public int totalQtyAvailable;
    public int qty;
    public float price;

    public float finalPrice; 

    public void SetData(CartProduct data, int index) {

        this.index = index;
        cartProduct = data;
        title.text = data.m_product.name;
        StringBuilder disc = new StringBuilder();
        totalQtyAvailable = data.m_TotalQtyAvailable;

        disc.Append("");

        // if color selected by user for product
        if (data.m_SelectedAttributes[0] >= 0)
        {
            disc.Append("Color : " + data.m_product.attributes[data.m_SelectedAttributes[0]].color_name);

            if (data.m_SelectedAttributes[1] >= 0)
            {
                disc.Append("    Attribute : " + data.m_product.attributes[data.m_SelectedAttributes[0]].sizes[data.m_SelectedAttributes[1]].size_name);
            }
            StartCoroutine(LoadRemoteImage(data.m_product.attributes[data.m_SelectedAttributes[0]].color_image));
        }
        else if (data.m_SelectedAttributes[1] >= 0)
        {
            disc.Append("Attribute : " + data.m_product.attributes[0].sizes[data.m_SelectedAttributes[1]].size_name);

            StartCoroutine(LoadRemoteImage(data.m_product.attributes[0].sizes[data.m_SelectedAttributes[1]].size_image));
        }
        else if (data.m_product.image.Count >= 1) // default fillter are selected by user
        {
            StartCoroutine(LoadRemoteImage(data.m_product.image[0]));
        }
        m_Discription.text = disc.ToString();

        qty = data.m_TotalQty;
        price = data.m_FinalPrice;

        UpdatePrice();
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
            cellImage.sprite = Utility.Texture2DToSprite(texture);
            cellImage.preserveAspect = true;
        }
        else
        {
            ClearImage();
        }
    }

   void  ClearImage() {

        cellImage.sprite = Utility.Texture2DToSprite(defaultTexture);
        cellImage.preserveAspect = true;
    }

    public void IncresedQty() {

        if (!CartController.Instance.OnCheckItemQuantity(cartProduct.m_product.id, totalQtyAvailable, cartProduct.m_SelectedAttributes[0],cartProduct.m_SelectedAttributes[1]))
            return;

        qty++;
        UpdatePrice();
    }

    public void DecreasedQty() {

        if (qty <= 1) 
            return;
        
        qty--;
        UpdatePrice();
    }

    public void UpdatePrice() {

        finalPrice = price * qty;

        m_qty.text = qty.ToString();
        m_Price.text = "$ "+finalPrice.ToString();

        cartProduct.m_TotalQty = qty;
        cartProduct.m_FinalPrice = finalPrice;

        CartController.Instance.UpdateFinalPrice();
    }

    public void RemoveFromList() {
        gameObject.SetActive(false);
        cartProduct.isActive = false;
        CartController.Instance.UpdateFinalPrice();
    }
}
