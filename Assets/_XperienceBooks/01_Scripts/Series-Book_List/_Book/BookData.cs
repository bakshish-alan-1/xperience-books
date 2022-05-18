using System.Collections;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BookData : MonoBehaviour
{
    [SerializeField] Image seriesImg;
    [SerializeField] TMP_Text seriesName;

    int selectedSeriesID = 0;
    int index = 0;
    public void SetData(int id, int no, string name, string imgURL)
    {
        selectedSeriesID = id;
        index = no;
        seriesName.text = name;
        if (imgURL != "")
            setImage(imgURL);// StartCoroutine(setImage(imgURL));
    }

    void checkDownloadedThemeSeries()
    {
        /*
            First check selected series path exists if yes then cross check stored skin updated time and new updated time
            if both are different then delete that skin folder, that means need to download new updated skin
        */
        string seriesPath = GameManager.Instance.LocalStoragePath + "Theme/" + GameManager.Instance.selectedSeries.theme.id;
        Debug.Log("Index: " + selectedSeriesID);
        if (Directory.Exists(seriesPath))
        {
            Debug.Log("Skin Path exists");
            if (GameManager.Instance.m_Series.Count != 0 && selectedSeriesID <= GameManager.Instance.m_Series.Count)
            {
                for (int i = 0; i < GameManager.Instance.m_Series.Count; i++)
                {
                    if(selectedSeriesID == GameManager.Instance.m_Series[i].id)
                    {
                        if (GameManager.Instance.selectedSeries.theme.updated_at_timestamp != GameManager.Instance.m_Series[i].theme.updated_at_timestamp)
                        {
                            Debug.Log("Deleted Skin Folder: " + seriesPath);
                            Directory.Delete(seriesPath, true);// delete previous series skin downloaded theme folder
                        }
                        break;
                    }
                }
            }
        }
    }

    public void OnBookSelected()
    {
        checkDownloadedThemeSeries();
        GameManager.Instance.selectedBooks = GameManager.Instance.m_SeriesDetails[index];
        FileHandler.SaveBooksData(GameManager.Instance.selectedBooks);
        // if theme is not available then save new theme and set
        if (!Directory.Exists(GameManager.Instance.GetThemePath()))
        {
            PlayerPrefs.SetString("IsThemeSaved", "false");
            ThemeManager.Instance.SaveSeriesTheame();
        }
        else
        {
            Debug.Log("Call LoadSkinTheme from BookData");
            ThemeManager.Instance.LoadSkinTheme();// already theme available then direct set to the home panel
        }
        ThemeManager.Instance.SaveSeriesIcon(GameManager.Instance.selectedBooks.image, GameManager.Instance.LocalStoragePath + "Theme/" + StaticKeywords.SeriesImage);
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
            if (seriesImg != null)
                seriesImg.sprite = GameManager.Instance.Texture2DToSprite(((DownloadHandlerTexture)request.downloadHandler).texture);
        }
    }
}
