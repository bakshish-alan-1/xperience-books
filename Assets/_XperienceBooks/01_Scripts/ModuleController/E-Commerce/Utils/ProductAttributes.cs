﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using TMPro;
using System;

public class ProductAttributes : MonoBehaviour
{

   // public List<Attribute> attribute = new List<Attribute>();
   [Header("Color Attribute reference")]
    public int m_ColorAttributeIndex=0;
    public GameObject prefab;
    public GameObject parent;
    public ToggleGroup parentGroup;
    Color myColor = new Color();

    [Header("Size Attribute reference")]
    public int m_SizeAttributeIndex = 1;
    public GameObject sizeAttributeTitle, sizeAttribute, sizePrefab;
    public GameObject sizeParent;
    public ToggleGroup sizeParentGroup;

    public List<GameObject> objectList = new List<GameObject>();
    public List<GameObject> sizeObjectList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        sizeAttributeTitle.SetActive(false);
        sizeAttribute.SetActive(false);
    }

    // reset color object list when color list create
    public void ResetColorAttributeObject()
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

    public void LoadAttributeData(Attributes attributes, int arrayIndex, string type) {

        int i = arrayIndex;
        //for (int i = 0; i < attributes.Count; i++)
        {

            GameObject obj = Instantiate(prefab, parent.transform, false);
            obj.GetComponent<ToggleSelector>().index = i;
            obj.GetComponent<ToggleSelector>().m_AttributeIndex = m_ColorAttributeIndex;
            Toggle toggle = obj.GetComponent<Toggle>();
            toggle.group = parentGroup;
            objectList.Add(obj);

            ColorUtility.TryParseHtmlString(attributes.color_code, out myColor);
            obj.GetComponent<ProceduralImage>().color = myColor;
            obj.transform.GetChild(0).gameObject.GetComponent<ProceduralImage>().color = myColor;

            if (i == 0) {
                try
                {
                    toggle.isOn = true;
                    toggle.Select();
                }
                catch (Exception Ex) {

                    Debug.LogError("Getting issue while default color set: " + Ex);
                }
            }
        }
    }

    // reset size object list when color change
    public void ResetSizeAttributeObject()
    {
        Debug.Log("ResetSizeAttributeObject");
        for (int i = 0; i < sizeParent.transform.childCount; i++)
        {
            Destroy(sizeParent.transform.GetChild(i).gameObject);
        }
    }

    public void LoadSizeAttributesData(int index, string name)
    {
        GameObject obj = Instantiate(sizePrefab, sizeParent.transform, false);
        obj.GetComponent<ToggleSelector>().index = index;
        obj.GetComponent<ToggleSelector>().m_AttributeIndex = m_SizeAttributeIndex;
        Toggle toggle1 = obj.GetComponent<Toggle>();
        toggle1.group = sizeParentGroup;
        sizeObjectList.Add(obj);

        obj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = name;

        if (index == 0) {
            try
            {
                toggle1.isOn = true;
                toggle1.Select();
            }
            catch (Exception Ex)
            {

                Debug.LogError("Getting issue while default attribute set: " + Ex);
            }
        }
    }

    public void ResetObject() {
        foreach (GameObject obj in objectList) {
            Destroy(obj);
        }

        foreach (GameObject obj1 in sizeObjectList)
        {
            Destroy(obj1);
        }
    }
}
