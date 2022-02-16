using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebPageController : MonoBehaviour
{

    public ScrollRect scrollview;
    public GameObject scrollContent;
    public GameObject scrollItemPrefab;
    public Image seriesLogo, Bg;
    public List<ContentModel> ModuleContent = new List<ContentModel>();

    bool isInventoryApiCall = false;
    private void Awake()
    {
        ModuleContent.AddRange(GameManager.Instance.GetModuleData());
    }

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < ModuleContent.Count; i++)
        {
            GenratePrefab(i, ModuleContent[i].webpagetitle, ModuleContent[i].webpagelink);
        }
       
        scrollview.verticalNormalizedPosition = 1;
        Bg.sprite = ThemeManager.Instance.background;
        seriesLogo.sprite = ThemeManager.Instance.seriesLogo;

        if (!isInventoryApiCall)
        { isInventoryApiCall = true; GameManager.Instance.OnCheckToUnlockModule(11); }
    }


    public void GenratePrefab(int index, string title , string url) {

        GameObject scrollItem = Instantiate(scrollItemPrefab);
        scrollItem.transform.SetParent(scrollContent.transform, false);

        scrollItem.transform.GetComponent<WebPageData>().setData(index, title, url);
    }
}
