using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance = null;

    [SerializeField] Image BG;
    [SerializeField] TMP_Text title;
    [SerializeField] Image BackIcon;

    [SerializeField] GameObject inventoryObj;
    [SerializeField] GameObject parent;

    [Header("Details")]
    [SerializeField] GameObject detailData;
    [SerializeField] Image detailImage;
    [SerializeField] TMP_Text titleTxt;
    [SerializeField] TMP_Text descptionTxt;

    bool isThemeSet = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OnSetThem()
    {
        // call from module list after OnSetThem
        if (!isThemeSet || GameManager.Instance.isNewThemeDownload)
        {
            isThemeSet = true;
            BG.sprite = (ThemeManager.Instance.background);
            BackIcon.sprite = (ThemeManager.Instance.backBtn);
            if (GameManager.Instance.TitleFont != null)
            {
                title.font = GameManager.Instance.TitleFont;
                titleTxt.font = GameManager.Instance.TitleFont;
            }

            if (GameManager.Instance.DetailFont != null)
            {
                descptionTxt.font = GameManager.Instance.DetailFont;
            }

            Color newCol;
            if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            {
                title.color = newCol;
            }
        }
    }

    public void OnBack()
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).transform.gameObject);
        }
        WindowManager.Instance.OpenPanel("ContentList");
    }

    // on show detailes of selected inventory, call from inventoryData 
    public void OnShowDetails(Texture2D texture, string title, string description)
    {
        detailImage.sprite = GameManager.Instance.Texture2DToSprite(texture);
        titleTxt.text = title;
        descptionTxt.text = description;
        detailData.SetActive(true);
    }

    // clear all data into inventory managers
    public void OnResetInventory()
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

    public void OnSetInventory(InventoryList response)
    {
        GameManager.Instance.m_Inventory = response;

        for (int i = 0; i < response.data.Count; i++)
        {
            GameObject obj = Instantiate(inventoryObj, parent.transform);
            InventoryData iData = obj.GetComponent<InventoryData>();
            iData.SetData(response.data[i]);
            if (response.data[i].unlocked == 0)
                obj.GetComponent<Button>().interactable = false;
        }
    }
}
