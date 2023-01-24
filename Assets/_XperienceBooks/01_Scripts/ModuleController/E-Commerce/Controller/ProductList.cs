using System.Collections;
using System.Collections.Generic;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using TMPro;

namespace Ecommerce
{
    // Display list of product on base of selected Category.
    public class ProductList : MonoBehaviour,
    IEnhancedScrollerDelegate
    {
        public static ProductList Instance = null;
        [SerializeField] GameObject menuBtn;
        [SerializeField] GameObject backBtnObj;
        [SerializeField] GameObject titleObj;
        //Product List Data
        public List<Product> _ProductList = new List<Product>();
        public TextMeshProUGUI m_CategoryTitle;

        #region ScrollData

        private SmallList<Product> _data;
        public EnhancedScroller scroller;
        public EnhancedScrollerCellView cellViewPrefab;
        public int numberOfCellsPerRow = 3;

        #endregion

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (GameManager.Instance.buyFromPrintfull)
            {
                menuBtn.SetActive(false);
                backBtnObj.SetActive(true);
                titleObj.SetActive(true);
            }
            else
            {
                menuBtn.SetActive(true);
                backBtnObj.SetActive(false);
                titleObj.SetActive(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            scroller.Delegate = this;
            RefreshListData();
        }

        public void RefreshListData()
        {           
            _ProductList.Clear();
            _data = new SmallList<Product>();
            scroller.ReloadData();
           
        }

        public void InitList(List<Product> list , string title)
        {
           // RefreshListData();
            m_CategoryTitle.text = title;//Featured
            _ProductList.AddRange(list);

            LoadData();
        }

        private void LoadData()
        {
            // set up some simple data
            _data = new SmallList<Product>();
            for (var i = 0; i < _ProductList.Count; i++)
            {
//                Debug.Log(_ProductList[i].id + " Added in list");
                _data.Add(_ProductList[i]);
            }

            Debug.Log("Data Added from List : " + _data.Count);
            // tell the scroller to reload now that we have the data
            scroller.ReloadData();
        }


        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)_data.Count / (float)numberOfCellsPerRow);
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 520f;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            // first, we get a cell from the scroller by passing a prefab.
            // if the scroller finds one it can recycle it will do so, otherwise
            // it will create a new cell.
            CellView cellView = scroller.GetCellView(cellViewPrefab) as CellView;

            cellView.name = "Cell " + (dataIndex * numberOfCellsPerRow).ToString() + " to " + ((dataIndex * numberOfCellsPerRow) + numberOfCellsPerRow - 1).ToString();

            // pass in a reference to our data set with the offset for this cell
            cellView.SetData(ref _data, dataIndex * numberOfCellsPerRow);

            // return the cell to the scroller
            return cellView;
        }
    }
}