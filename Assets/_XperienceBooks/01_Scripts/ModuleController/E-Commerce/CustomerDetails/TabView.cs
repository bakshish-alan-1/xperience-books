using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TabManager {

    public Transform tabButton;
    public GameObject tabView;

}

public class TabView : MonoBehaviour
{
    public static TabView Instace = null;
    public List<TabManager> tabList = new List<TabManager>();

    public Color selectedColor,normalColor;

    public Color selectedTextColor, normalTextColor;

    public int defaultTab = 0;
    public int currentIndex = -1;


    // Start is called before the first frame update
    void Start()
    {
        if (Instace == null)
            Instace = this;

        ActivateTab(defaultTab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ActivateTab(int newIndex) {

        if (currentIndex == newIndex) return;

        tabList[newIndex].tabButton.GetComponent<Image>().color = selectedColor;
        tabList[newIndex].tabView.SetActive(true);
        tabList[newIndex].tabButton.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedTextColor;


        if (currentIndex < 0) {
            currentIndex = newIndex;
            return;
        } 

        tabList[currentIndex].tabButton.GetComponent<Image>().color = normalColor;
        tabList[currentIndex].tabView.SetActive(false);
        tabList[currentIndex].tabButton.GetChild(0).GetComponent<TextMeshProUGUI>().color = normalTextColor;

        currentIndex = newIndex;
    }

    //Deactivate Billing Address button incase of isSame ischecked true
    public void ActivateDeactivateBillingAddress()
    {

        tabList[1].tabButton.GetComponent<Button>().interactable = !tabList[1].tabButton.GetComponent<Button>().IsInteractable();

    }
}
