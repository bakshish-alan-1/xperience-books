using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleList : MonoBehaviour
{
    public static ModuleList Instance;
   
    public Sprite[] m_ButtonSprite;
    public GameObject[] m_DynamicModules;

    public List<int> m_ActiveModules = new List<int>();

    private void Awake()
    {
        if (Instance == null) {

            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ResetModule();
    }

    public void ActivateModule(int index) {

    }

    [SerializeField] Image BG;
    [SerializeField] Image BackIcon;
    [SerializeField] Image SeriesLogo;
    [SerializeField] Image InventoryIcon;

    bool isThemeSet = false;
    void OnSetThem()
    {
        if (!isThemeSet || GameManager.Instance.isNewThemeDownload)
        {
            isThemeSet = true;
            BG.sprite = (ThemeManager.Instance.background);
            BackIcon.sprite = (ThemeManager.Instance.backBtn);
            SeriesLogo.sprite = (ThemeManager.Instance.seriesLogo);
            InventoryIcon.sprite = (ThemeManager.Instance.inventoryIcon);
            InventoryManager.Instance.OnSetThem();
        }
    }

    public void ResetModule() {

        foreach (GameObject obj in m_DynamicModules) {
            obj.SetActive(false);
        }
    }

    public void OnInventoryOpen()
    {
        ApiManager.Instance.GetInventoryList(GameManager.Instance.currentBook.chapter_id, GameManager.Instance.currentBook.qr_code_id);// call for get inventory list
        WindowManager.Instance.OpenPanel("Inventory");
    }

    GameObject m_CurrentActiveModule;

    public void ActivateModules(List<int> modules) {
        OnSetThem();
        ResetModule();
        m_ActiveModules.Clear();
        m_ActiveModules.AddRange(modules);

        int m_TotalActivatedModule = m_ActiveModules.Count-1;
        Debug.Log("m_TotalActivatedModule: " + m_TotalActivatedModule);
        m_CurrentActiveModule = m_DynamicModules[m_TotalActivatedModule];
        m_CurrentActiveModule.SetActive(true);

        int i = 0;
        foreach (Transform child in m_CurrentActiveModule.transform)
        {
            child.GetComponent<ContentButton>().SetData(m_ActiveModules[i]-1, m_ButtonSprite[m_ActiveModules[i] - 1], m_ActiveModules[i]);
             i++;
        }
    }
}
