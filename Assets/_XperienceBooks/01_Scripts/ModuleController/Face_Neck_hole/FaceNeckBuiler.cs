using UnityEngine;

public class FaceNeckBuiler : MonoBehaviour
{
    [SerializeField] MeshRenderer myRenderer;
    [SerializeField] Transform myTransform;

    private void Start()
    {
        onCheckData();

#if UNITY_ANDROID
        transform.localScale = new Vector3(-1f, 1f, 1f);
#endif
    }

    private void OnEnable()
    {
        FaceNeckController.builerDataDownloaded += setBuilderData;
    }

    private void OnDisable()
    {
        FaceNeckController.builerDataDownloaded -= setBuilderData;
    }

    void onCheckData()
    {
        Texture2D texture = FaceNeckController.Instance.getBuilderTexture();
        if (texture != null)
        {
            myRenderer.material.mainTexture = texture;
            myTransform.localPosition = FaceNeckController.Instance.builderPosition;
            myRenderer.enabled = true;
        }
        else
            myRenderer.enabled = false;
    }

    void setBuilderData(Texture2D texture2D, Vector3 pos)
    {
        Debug.Log("check data: " + pos);
        myRenderer.material.mainTexture = texture2D;
        myTransform.localPosition = new Vector3(pos.x, pos.y, pos.z);
        myRenderer.enabled = true;
    }
}
