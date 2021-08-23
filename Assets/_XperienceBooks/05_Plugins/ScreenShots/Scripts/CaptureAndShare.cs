using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CaptureAndShare : MonoBehaviour
{
    public static CaptureAndShare Instance = null;

    public GameObject previewUI;
    public Image previewImage;
    public GameObject needToHide, backBtnUI;
    public bool ssInProgress = false;

    public RectTransform container;

    Texture2D tempTexture;

    private void Awake()
    {
        Debug.Log(Screen.width / 1.3f + "   " + Screen.height / 1.3f);
        container.sizeDelta = new Vector2(Screen.width / 1.2f, Screen.height / 1.2f);

    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
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
            .SetSubject("Continuum Multimedia").SetText("#WorldofAGARTHA")
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
        previewUI.SetActive(false);
        ssInProgress = false;
    }
}
