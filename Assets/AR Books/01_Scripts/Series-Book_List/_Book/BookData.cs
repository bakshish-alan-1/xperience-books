using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BookData : MonoBehaviour
{
    [SerializeField] Image seriesImg;
    [SerializeField] TMP_Text seriesName;

    int index = 0;
    public void SetData(int no, string name, string imgURL)
    {
        index = no;
        seriesName.text = name;
        if (imgURL != "")
            StartCoroutine(setImage(imgURL));
    }

    public void OnBookSelected()
    {
        GameManager.Instance.selectedBooks = GameManager.Instance.m_SeriesDetails[index];
        FileHandler.SaveBooksData(GameManager.Instance.selectedBooks);
        // if theme is not available then save new theme and set
        if (!Directory.Exists(GameManager.Instance.GetThemePath()))
            ThemeManager.Instance.SaveSeriesTheame();
        else
            HomeScreen.Instance.OnSetHomePanelData();// already theme available then direct set to the home panel
    }

    IEnumerator setImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            seriesImg.sprite = GameManager.Instance.Texture2DToSprite(((DownloadHandlerTexture)request.downloadHandler).texture);
    }
}
