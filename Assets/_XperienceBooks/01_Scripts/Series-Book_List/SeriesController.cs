using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

[System.Serializable]
public class GenreData
{
    public bool success;
    public List<GenreList> data;
}

[System.Serializable]
public class GenreList
{
    public int id;
    public string name;
    public int user_id;
}

public class SeriesController : MonoBehaviour
{
    public static SeriesController Instance = null;

    [SerializeField] GameObject genreInfo;
    [SerializeField] TMP_Dropdown genreList;
    [SerializeField] GameObject seriesObj;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject nodataFoundObj;

    public GenreData genreData = new GenreData();

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void setGenreList(string data)
    {
        genreData = null;
        genreData = JsonUtility.FromJson<GenreData>(data);
        List<string> nameList = new List<string>();
        genreList.ClearOptions();
        nameList.Add("Select Genre");
        for (int i = 0; i < genreData.data.Count; i++)
        {
            nameList.Add(genreData.data[i].name);
        }
        nameList.Add("");
        genreList.AddOptions(nameList);
        if (GameManager.Instance.genreId > 0)
            genreList.value = GameManager.Instance.genreId;
        else if (GameManager.Instance.genreId != -1)
            nodataFoundObj.SetActive(true);
    }

    public void ShowGenreInfo(bool value)
    {
        genreInfo.SetActive(value);
    }

    public void onGenreSelected()
    {
        ShowGenreInfo(false);
        GameManager.Instance.genreId = genreList.value;
        PlayerPrefs.SetInt("GenreId", GameManager.Instance.genreId);
        if (GameManager.Instance.genreId > 0)
        {
            nodataFoundObj.SetActive(false);
            ApiManager.Instance.GetSeriesList();
        }
        else if (GameManager.Instance.genreId != -1)
        {
            OnRemoveChield();
            nodataFoundObj.SetActive(true);
        }
    }

    public void OnBackBtn()
    {
        GameManager.Instance.OpenConfirmWindow();
    }

    public void OnRemoveChield()
    {
        for (int i = 0; i < parent.transform.childCount; i++)
            Destroy(parent.transform.GetChild(i).gameObject);
    }

    public void SetSeriesIcons()
    {
        // Delete bookInfo stored json file
        if (File.Exists(GameManager.Instance.LocalStoragePath + "Theme/BooksData.json"))
            File.Delete(GameManager.Instance.LocalStoragePath + "Theme/BooksData.json");


        string seriesPath = GameManager.Instance.LocalStoragePath + "Theme/" + GameManager.Instance.selectedSeries.theme.id;
        if (PlayerPrefs.GetString("IsThemeSaved").Equals("false"))
        {
            Debug.Log("IsThemeSaved: false, so delete skin folder to get new skin");
            if (Directory.Exists(seriesPath))
                Directory.Delete(seriesPath, true);// delete previous series skin downloaded theme folder
        }

        if (GameManager.Instance.m_Series.Count <= 0)
            nodataFoundObj.SetActive(true);
        else
            nodataFoundObj.SetActive(false);

        for (int i = 0; i < GameManager.Instance.m_Series.Count; i++)
        {
            GameObject obj = Instantiate(seriesObj, parent.transform);
            obj.GetComponent<SeriesData>().SetData(i, GameManager.Instance.m_Series[i].name, GameManager.Instance.m_Series[i].image_path);
        }
    }
}
