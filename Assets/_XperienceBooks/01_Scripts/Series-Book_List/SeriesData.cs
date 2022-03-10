
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;

public class SeriesData : MonoBehaviour
{
    [SerializeField] Image seriesImg;
    [SerializeField] TMP_Text seriesName;
    string url = "";
    Texture2D texture;

    int index = 0;
    public void SetData(int no, string name, string imgURL)
    {
        index = no;
        seriesName.text = name;
        url = imgURL;
        if (url != "")
            setImage(url);
    }

    public void OnSeriesSelected()
    {
        GameManager.Instance.selectedSeries = GameManager.Instance.m_Series[index];

        GameManager.Instance.TitleFont = null;
        GameManager.Instance.DetailFont = null;

        string theme = GameManager.Instance.GetThemePath();
        // create directory, remove different time stamp theme save on same series
        if (!Directory.Exists(theme))
        {
            if (Directory.Exists(GameManager.Instance.LocalStoragePath + "Theme/" + GameManager.Instance.selectedSeries.theme.id))
                Directory.Delete(GameManager.Instance.LocalStoragePath + "Theme/" + GameManager.Instance.selectedSeries.theme.id + "/", true);
        }

        FileHandler.SaveSeriesData(GameManager.Instance.selectedSeries);
        ApiManager.Instance.GetSeriesDetails(GameManager.Instance.m_Series[index].id);
    }

    public async void setImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            if (seriesImg)
                seriesImg.sprite = GameManager.Instance.Texture2DToSprite(texture);
        }
    }

    /*IEnumerator setImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            seriesImg.sprite = GameManager.Instance.Texture2DToSprite(texture);
        }
    }*/
}
