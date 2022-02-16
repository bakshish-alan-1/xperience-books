using UnityEngine;

public class FaceNeckBuiler : MonoBehaviour
{
    [SerializeField] MeshRenderer myRenderer;
    [SerializeField] Transform myTransform;

    private void OnEnable()
    {
        onCheckData();
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
            myRenderer.enabled = true;
            myRenderer.material.mainTexture = texture;
            myTransform.localPosition = FaceNeckController.Instance.builderPosition;
        }
    }

    void setBuilderData(Texture2D texture2D, Vector3 pos)
    {
        Debug.Log("check data: " + pos);
        myRenderer.enabled = true;
        myRenderer.material.mainTexture = texture2D;
        myTransform.localPosition = new Vector3(pos.x, pos.y, pos.z);
    }
}
