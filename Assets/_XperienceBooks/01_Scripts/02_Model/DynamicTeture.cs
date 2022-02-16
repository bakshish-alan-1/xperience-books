using UnityEngine;
using UnityEngine.UI;


public class DynamicTeture : MonoBehaviour
{

    [SerializeField]
    RawImage image;
    public int index;

    public void SetData(Texture2D texture , int i) {
        image.texture = texture;
        index = i;
    }


    public void Onclick() {

        Debug.Log("Click on Object : " + index);
        FaceController.Instance.SwapFaces(index);
    }

}
