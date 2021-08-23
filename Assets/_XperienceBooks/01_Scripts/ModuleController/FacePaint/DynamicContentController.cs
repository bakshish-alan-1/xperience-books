using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

public class DynamicContentController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    protected GameObject m_Content, m_PagingToggle;

    private float toggleWidth;

    [SerializeField]
    private SimpleScrollSnap sss;
    #endregion

    #region Methods
    private void Awake()
    {
       // sss = GetComponent<SimpleScrollSnap>();
        toggleWidth = m_PagingToggle.GetComponent<RectTransform>().sizeDelta.x * (Screen.width / 2048f); ;
    }

    public void AddToFront()
    {
       // Add(0);
    }
    public void AddToBack()
    {
       // Add(sss.NumberOfPanels);
    }
    public void AddAtIndex()
    {
       // int index = Convert.ToInt32(addInput.GetComponent<InputField>().text);
       // Add(0);
    }
    public void Add(Texture2D texture)
    {
        int index = sss.NumberOfPanels;

        //Pagination
        Instantiate(m_PagingToggle, sss.pagination.transform.position + new Vector3(toggleWidth * (index + 1), 0, 0), Quaternion.identity, sss.pagination.transform);
        sss.pagination.transform.position -= new Vector3(toggleWidth / 2f, 0, 0);

       
       // GameObject scrollData = Instantiate(m_Content);
       // scrollData.GetComponent<DynamicTeture>().SetData(texture,index);

        sss.Add(m_Content ,texture, index);
    }

    public void RemoveFromFront()
    {
        Remove(0);
    }
    public void RemoveFromBack()
    {
        if (sss.NumberOfPanels > 0)
        {
            Remove(sss.NumberOfPanels - 1);
        }
        else
        {
            Remove(0);
        }
    }
    public void RemoveAtIndex()
    {
       // int index = Convert.ToInt32(removeInput.GetComponent<InputField>().text);
        Remove(0);
    }
    private void Remove(int index)
    {
        if (sss.NumberOfPanels > 0)
        {
            //Pagination
            DestroyImmediate(sss.pagination.transform.GetChild(sss.NumberOfPanels - 1).gameObject);
            sss.pagination.transform.position += new Vector3(toggleWidth / 2f, 0, 0);

            //Panel
            sss.Remove(index);
        }
    }
    #endregion
}
