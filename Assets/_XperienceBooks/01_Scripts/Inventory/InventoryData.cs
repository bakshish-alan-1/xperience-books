using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InventoryData : MonoBehaviour
{
    [SerializeField] Image inventoryImg;
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] string url = "";
    Texture2D texture;

    bool isOpen = false;

    public int inventoryID = 0;
    public int module_ID = 0;
    public void SetData(InventoryDetails data)
    {
        inventoryID = data.id;
        module_ID = data.armodule_id;
        url = data.inventory.image_url;
        title = data.inventory.title;
        description = data.inventory.description;

        if (data.unlocked == 0)
            inventoryImg.sprite = ThemeManager.Instance.inventoryPlaceholder;
        else
        {
            if (!string.IsNullOrEmpty(url))
                setImage(url);
        }

        if (data.viewed == 0)
            isOpen = false;
        else
            isOpen = true;
    }

    public void setimageBlocker(bool isBlock)
    {
        if (isBlock)
            inventoryImg.color = new Color32(152, 152, 152, 255);
        else
            inventoryImg.color = Color.white;
    }

    public void OnInventorySelected()
    {
        InventoryManager.Instance.OnShowDetails(texture, title, description);
        if (!isOpen)
        {
            isOpen = true;
            ApiManager.Instance.OpenInventory(inventoryID);// call api to set open inventory value in database
        }
    }

    public async void setImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            if (inventoryImg)
                inventoryImg.sprite = GameManager.Instance.Texture2DToSprite(texture);
        }
    }
}
