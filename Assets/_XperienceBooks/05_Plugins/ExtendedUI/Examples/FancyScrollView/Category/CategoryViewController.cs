using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Ecommerce.Category
{
    public class CategoryViewController : MonoBehaviour
    {
        [SerializeField] InputField selectIndexInputField = default;
        public GameObject Prefabs, PrefabsParent;
        public List<CategoryData> m_CategoryList = new List<CategoryData>();
        string localPath;

        public void ClearCategoryList(List<CategoryData> data)
        {
            m_CategoryList.Clear();
            m_CategoryList.AddRange(data);
        }

        public void onRemoveCategoryCells()
        {
            for (int i = 0; i < PrefabsParent.transform.childCount; i++)
            {
                PrefabsParent.transform.GetChild(i).transform.gameObject.SetActive(false);
                Destroy(PrefabsParent.transform.GetChild(i).transform.gameObject);
            }
        }

        public void GenerateCategoryCells()
        {
            try
            {
                Debug.Log("m_CategoryList: " + m_CategoryList.Count);
                if (m_CategoryList.Count <= 0)
                    return;

                for (int i = 0; i < m_CategoryList.Count; i++)
                {
                    // Param : index , localURL , ServerURL ,categoryName
                    ItemData d = new ItemData(i, localPath, m_CategoryList[i].category_image, m_CategoryList[i].category_name); // Dummy Data for testing
                    GameObject newCell = Instantiate(Prefabs, PrefabsParent.transform);
                    newCell.name = i.ToString();
                    newCell.GetComponent<CategoryCell>().setCategoryCellData(d);
                    newCell.GetComponent<Button>().onClick.AddListener(()=>{
                        ECommerceManager.Instance.GetProductByCategoryID(int.Parse(newCell.name));
                    });
                }
            }
            catch (Exception EX)
            {
                Debug.LogError("Issue with Cell : " + EX.Message);
            }
        }
    }
}
