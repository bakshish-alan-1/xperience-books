using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExtrasNew : MonoBehaviour
{
    public GameObject hideObj;
    public GameObject previewObj;
    public Image previewIMG;

    Texture2D texture;

    public void onTackSS()
    {
        StartCoroutine(TakeSSAndPreview());
    }

    IEnumerator TakeSSAndPreview()
    {
        yield return new WaitForEndOfFrame();

        hideObj.SetActive(false);
        texture = ScreenCapture.CaptureScreenshotAsTexture(4);
        previewIMG.sprite = Utility.Texture2DToSprite(texture);
        previewObj.SetActive(true);
        hideObj.SetActive(true);
    }

    public void onSave()
    {
        NativeGallery.SaveImageToGallery(texture, "ARBook", "highResolution.png");
    }
}
