
using UnityEngine;

public class BookController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text seriesName;
    [SerializeField] GameObject bookObj;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject nodataFound;

    public void OnBackBtn()
    {
        ApiManager.Instance.GetSeriesList(); //WindowManager.Instance.BackToPreviousWindow();
        Invoke("OnRemoveChield", 1f);
    }

    public void OnRemoveChield()
    {
        for (int i = 0; i < parent.transform.childCount; i++)
            Destroy(parent.transform.GetChild(i).gameObject);
    }

    public void SetBookIcons()
    {
        if (GameManager.Instance.m_SeriesDetails.Count == 0)
            nodataFound.SetActive(true);
        else
            nodataFound.SetActive(false);

        int seriesID = GameManager.Instance.selectedSeries.id;
        seriesName.text = GameManager.Instance.selectedSeries.name;
        for (int i = 0; i < GameManager.Instance.m_SeriesDetails.Count; i++)
        {
            GameObject obj = Instantiate(bookObj, parent.transform);
            obj.GetComponent<BookData>().SetData(seriesID, i, GameManager.Instance.m_SeriesDetails[i].name, GameManager.Instance.m_SeriesDetails[i].image);
        }
    }
}
