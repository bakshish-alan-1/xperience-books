using System.Collections;
using System.Collections.Generic;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine;


public class FeatureProductList : MonoBehaviour, IEnhancedScrollerDelegate
{
    public List<Product> _FeatureProductList = new List<Product>();


    #region ScrollData

    private SmallList<Product> _data;
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;
    public int numberOfCellsPerRow = 3;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        scroller.Delegate = this;


#if UNITY_EDITOR
        // InitListDummy();
        InitList();
#else
                     InitList();
#endif


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitList() {
        if (_FeatureProductList.Count >= 0) {
            _FeatureProductList.Clear();
        }
     //   _FeatureProductList.AddRange(ContentManager.Instance._FeatureProductList);

        LoadData();
    }

    public void InitListDummy()
    {
        if (_FeatureProductList.Count >= 0)
        {
            _FeatureProductList.Clear();
        }
       
        for (int i = 0; i < 10; i++)
        {
            _FeatureProductList.Add(new Product() { id = i, name = "Hello" });
        }
        LoadData();
    }


    private void LoadData()
    {
        // set up some simple data
        _data = new SmallList<Product>();
        for (var i = 0; i < _FeatureProductList.Count; i++)
        {
             Debug.Log(_FeatureProductList[i].id + " Added in list");
            _data.Add(_FeatureProductList[i]);
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
