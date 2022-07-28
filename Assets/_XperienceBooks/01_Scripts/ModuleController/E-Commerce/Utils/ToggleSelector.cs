using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSelector : MonoBehaviour
{

    public int index = -1;
    Toggle toggle;
    public int m_AttributeIndex;


    public delegate void OnAttributeSelected(int index,int type);
    public static event OnAttributeSelected OnSelected;


    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    

    public void OnSelect() {

        if (toggle.isOn) {

            Debug.Log("I am Here");
            OnSelected(index,m_AttributeIndex);
        }
    }

    private void Update()
    {
        if (toggle.isOn)
            toggle.Select();
    }
}
