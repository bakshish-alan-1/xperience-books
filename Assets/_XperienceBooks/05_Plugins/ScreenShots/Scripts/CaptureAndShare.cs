using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CaptureAndShare : MonoBehaviour
{
    public static CaptureAndShare Instance = null;

    public GameObject previewUI;
    public Image previewImage;
    public GameObject needToHide, backBtnUI;
    public GameObject watermark;
    public bool ssInProgress = false;

    public RectTransform container;
    Image watermarkIcon;
    Texture2D tempTexture;

    private void Awake()
    {
        container.sizeDelta = new Vector2(Screen.width / 1.2f, Screen.height / 1.2f);
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        if (watermark != null)
            watermarkIcon = watermark.GetComponent<Image>();

        if (!string.IsNullOrEmpty(GameManager.Instance.WatermarkURL))
        {
            OnDownloadWatermark(GameManager.Instance.WatermarkURL);
        } 
        else if (watermark != null)
            watermark.SetActive(false);
    }

    public void TakeScreenshot() {

        if (!ssInProgress) {
            ssInProgress = true;
            StartCoroutine(TakeSSAndPreview());
        }
    }

    private IEnumerator TakeSSAndPreview()
    {
        needToHide.SetActive(false);
        if (backBtnUI != null)
            backBtnUI.SetActive(false);

        yield return new WaitForEndOfFrame();

        tempTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tempTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tempTexture.Apply();

        //  previewImage.texture = tempTexture;
        previewImage.sprite = Utility.Texture2DToSprite(tempTexture);
        previewImage.preserveAspect = true;

        previewUI.SetActive(true);
        needToHide.SetActive(true);
        if (backBtnUI != null)
            backBtnUI.SetActive(true);
    }

    public void ShareSS() {

        string filePath = Path.Combine(Application.temporaryCachePath, "ss.png");
        File.WriteAllBytes(filePath, tempTexture.EncodeToPNG());
        new NativeShare()
            .AddFile(filePath)
            .SetSubject("Continuum Multimedia").SetText("")
            .Share();

        //SaveSS();
    }

    public void SaveSS() {

        NativeGallery.SaveImageToGallery(tempTexture, "ARBook", "Image.png");
        ClosePreview();
    }

    public void ClosePreview(bool destroy = false) {

        if (destroy) {

        }
        StopAllCoroutines();
        previewUI.SetActive(false);
        ssInProgress = false;
        Destroy(tempTexture);
    }

    private async void OnDownloadWatermark(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("watermark download error: " + request.error);
        }
        else
        {
            watermark.SetActive(true);
            Texture2D texture2D = DownloadHandlerTexture.GetContent(request);
            watermarkIcon.sprite = GameManager.Instance.Texture2DToSprite(texture2D);
        }
    }
}
