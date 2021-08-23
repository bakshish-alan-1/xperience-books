using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OrderData : MonoBehaviour
{
    public delegate void OnAddressSelected(int index, int pIndex);
    public static event OnAddressSelected OnMoreDetail;

    [SerializeField]
    private TMP_Text title, description, orderDate, deliveryStatus;
    [SerializeField] private Image cellImage;
    [SerializeField] private Texture2D defaultTexture;

    int orderIndex = -1;
    int productIndex = -1;

    public void setData(int OIndex, int PIndex, string name, string desc, string OrderDate, string statusDelivery, string path)
    {
        orderIndex = OIndex;
        productIndex = PIndex;
        title.text = name.ToString();
        description.text = desc.ToString();
        deliveryStatus.text = statusDelivery;
        orderDate.text = "Order Date: " + OrderDate.ToString();
        if (path != "")
            StartCoroutine(LoadRemoteImage(path));
    }

    public IEnumerator LoadRemoteImage(string path)
    {

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
            cellImage.sprite = Utility.Texture2DToSprite(texture);
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
        cellImage.preserveAspect = true;
    }

    public void ModeBtnHit()
    {
        Debug.Log("More btn hit orderIndex: " + orderIndex+ ", productIndex: " + productIndex);
        OnMoreDetail(orderIndex, productIndex);
    }
}
