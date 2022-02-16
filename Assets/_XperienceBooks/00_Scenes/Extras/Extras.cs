using System;
using System.Collections;
using System.Collections.Generic;
using Intellify.core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Intellify.core.APIClient;

public class Extras : MonoBehaviour
{
    [SerializeField] Text progressTxt;
    [SerializeField] Text responseTxt;

    int cnt = 0;
    float per = 0f;
    List<string> urls = new List<string>();

    IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest request = new UnityWebRequest("https://google.com");
        yield return request.SendWebRequest();
        if (request.isNetworkError)
        {
            Debug.Log("net error: " + request.error);
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public void onCallAPIBtnClick()
    {
        GetSeriesList();
    }

    void GetSeriesList()
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                CallWebAPI(Method.GET.ToString(), "series", string.Empty, string.Empty, OnSeriesList);
            }
        }));
    }

    private void OnSeriesList(bool success, object data, long statusCode)
    {
        if (success)
        {
            SeriesList response = JsonUtility.FromJson<SeriesList>(data.ToString());
            responseTxt.text = data.ToString();
            SaveSeriesTheameURL(response);

        }
        else
        {
            responseTxt.text = "Something Went Wrong: " + data.ToString();
        }
    }

    void SaveSeriesTheameURL(SeriesList data)
    {
        cnt = 0;
        urls.Clear();
        Debug.Log("inside SaveSeriesTheameURL");
        if (!string.IsNullOrEmpty(data.data[0].theme.logo))
        { urls.Add(data.data[0].theme.logo); }

        if (!string.IsNullOrEmpty(data.data[0].theme.background_image))
        { urls.Add(data.data[0].theme.background_image); }

        if (!string.IsNullOrEmpty(data.data[0].theme.scan_button))
        { urls.Add(data.data[0].theme.scan_button); }

        if (!string.IsNullOrEmpty(data.data[0].theme.facebook_icon))
        { urls.Add(data.data[0].theme.facebook_icon); }

        if (!string.IsNullOrEmpty(data.data[0].theme.twitter_icon))
        { urls.Add(data.data[0].theme.twitter_icon); }

        if (!string.IsNullOrEmpty(data.data[0].theme.instagram_icon))
        { urls.Add(data.data[0].theme.instagram_icon); }

        if (!string.IsNullOrEmpty(data.data[0].theme.youtube_icon))
        { urls.Add(data.data[0].theme.youtube_icon); }

        if (!string.IsNullOrEmpty(data.data[0].theme.website_icon))
        { urls.Add(data.data[0].theme.website_icon); }

        if (!string.IsNullOrEmpty(data.data[0].theme.back_button))
        { urls.Add(data.data[0].theme.back_button); }

        if (!string.IsNullOrEmpty(data.data[0].theme.profile_icon))
        { urls.Add(data.data[0].theme.profile_icon); }

        if (!string.IsNullOrEmpty(data.data[0].theme.notif_icon))
        { urls.Add(data.data[0].theme.notif_icon); }

        if (!string.IsNullOrEmpty(data.data[0].theme.inventory_icon))
        { urls.Add(data.data[0].theme.inventory_icon); }

        if (!string.IsNullOrEmpty(data.data[0].theme.inventory_placeholder))
        { urls.Add(data.data[0].theme.inventory_placeholder); }

        if (!string.IsNullOrEmpty(data.data[0].theme.notif_thumbnail))
        { urls.Add(data.data[0].theme.notif_thumbnail); }

        if (!string.IsNullOrEmpty(data.data[0].theme.notif_next))
        { urls.Add(data.data[0].theme.notif_next); }

        if (!string.IsNullOrEmpty(data.data[0].theme.notif_tile))
        { urls.Add(data.data[0].theme.notif_tile); }

        if (!string.IsNullOrEmpty(data.data[0].theme.scan_qr_bg))
        { urls.Add(data.data[0].theme.scan_qr_bg); }

        if (!string.IsNullOrEmpty(data.data[0].theme.dialog_box))
        { urls.Add(data.data[0].theme.dialog_box); }

        if (!string.IsNullOrEmpty(data.data[0].theme.dialog_box_btn))
        { urls.Add(data.data[0].theme.dialog_box_btn); }

        if (!string.IsNullOrEmpty(data.data[0].theme.fan_art_img_glry))
        { urls.Add(data.data[0].theme.fan_art_img_glry); }

        if (!string.IsNullOrEmpty(data.data[0].theme.module_1))
        { urls.Add(data.data[0].theme.module_1); }

        if (!string.IsNullOrEmpty(data.data[0].theme.module_2))
        { urls.Add(data.data[0].theme.module_2); }

        if (!string.IsNullOrEmpty(data.data[0].theme.module_3))
        { urls.Add(data.data[0].theme.module_3); }

        if (!string.IsNullOrEmpty(data.data[0].theme.module_4))
        { urls.Add(data.data[0].theme.module_4); }

        if (!string.IsNullOrEmpty(data.data[0].theme.module_5))
        { urls.Add(data.data[0].theme.module_5); }

        if (!string.IsNullOrEmpty(data.data[0].theme.module_6))
        { urls.Add(data.data[0].theme.module_6); }
        if (!string.IsNullOrEmpty(data.data[0].theme.module_7))
        { urls.Add(data.data[0].theme.module_7); }
        if (!string.IsNullOrEmpty(data.data[0].theme.module_8))
        { urls.Add(data.data[0].theme.module_8); }
        if (!string.IsNullOrEmpty(data.data[0].theme.module_9))
        { urls.Add(data.data[0].theme.module_9); }
        if (!string.IsNullOrEmpty(data.data[0].theme.module_10))
        { urls.Add(data.data[0].theme.module_10); }
        if (!string.IsNullOrEmpty(data.data[0].theme.module_11))
        { urls.Add(data.data[0].theme.module_11); }
        if (!string.IsNullOrEmpty(data.data[0].theme.module_12))
        { urls.Add(data.data[0].theme.module_12); }

        if (!string.IsNullOrEmpty(data.data[0].theme.font_h1))
        { urls.Add(data.data[0].theme.font_h1); }

        if (!string.IsNullOrEmpty(data.data[0].theme.font_h2))
        { urls.Add(data.data[0].theme.font_h2); }

        if (!string.IsNullOrEmpty(data.data[0].theme.font_h3))
        { urls.Add(data.data[0].theme.font_h3); }

        if (!string.IsNullOrEmpty(data.data[0].theme.font_h4))
        { urls.Add(data.data[0].theme.font_h4); }

        if (urls.Count > 0)
        {
            per = 100 / urls.Count;
            Debug.Log(urls.Count + ", per: " + per.ToString());
            StartCoroutine(setImage(CallDownloadComplete));
        }
    }

    IEnumerator setImage(action onComplete = null)
    {
        Debug.Log("Start Download: " + urls[cnt]);
        yield return new WaitForEndOfFrame();
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(urls[cnt]);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            Debug.Log("Download Fail: " + request.error);
        else
        {
            Debug.Log("Download Complete");
            onComplete?.Invoke(true, request.downloadHandler.data, request.responseCode);
        }    
    }

    private void CallDownloadComplete(bool success, object data, long statusCode)
    {
        Debug.Log("URL " + cnt + " Download complete");
        progressTxt.text = ((cnt + 1) * per).ToString() + " %";
        if ((cnt + 1) >= urls.Count)
        {
            Debug.Log("All Download compete goto next scene");
            SceneManager.LoadScene(1);
        }
        else
        {
            cnt += 1;
            StartCoroutine(setImage(CallDownloadComplete));
        }
    }
}
