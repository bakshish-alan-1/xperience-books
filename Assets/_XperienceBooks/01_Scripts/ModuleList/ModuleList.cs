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

    bool isThemeSet = false;
    void OnSetThem()
    {
        if (!isThemeSet || GameManager.Instance.isNewThemeDownload)
        {
            isThemeSet = true;
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BGTheme, BG);
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.BackBtnTheme, BackIcon);
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), StaticKeywords.LogoTheme, SeriesLogo); 
        }
    }

    public void ResetModule() {

        foreach (GameObject obj in m_DynamicModules) {
            obj.SetActive(false);
        }
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
            child.GetComponent<ContentButton>().SetData(i, m_ButtonSprite[m_ActiveModules[i] - 1], m_ActiveModules[i]);
              i++;
        }
    }
}
