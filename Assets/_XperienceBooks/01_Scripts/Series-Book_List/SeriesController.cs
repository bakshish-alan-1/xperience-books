﻿using System.IO;
using UnityEngine;

public class SeriesController : MonoBehaviour
{
    public static SeriesController Instance = null;

    [SerializeField] GameObject seriesObj;
    [SerializeField] GameObject parent;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
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

        for (int i = 0; i < GameManager.Instance.m_Series.Count; i++)
        {
            GameObject obj = Instantiate(seriesObj, parent.transform);
            obj.GetComponent<SeriesData>().SetData(i, GameManager.Instance.m_Series[i].name, GameManager.Instance.m_Series[i].image_path);
        }
    }
}
