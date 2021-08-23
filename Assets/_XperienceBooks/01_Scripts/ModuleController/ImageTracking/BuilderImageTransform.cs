using UnityEngine;

public class BuilderImageTransform : MonoBehaviour
{
    [SerializeField]
    Transform X, Y, Z;

    public void setTransform(Vector3 positionModel, Vector3 rotateModel, Vector3 scaleModel)
    {
        Debug.Log("Builder position: " + positionModel);
        Debug.Log("Builder rotation: " + rotateModel);
        Debug.Log("Builder scale: " + scaleModel);

        //For position
        transform.localPosition = new Vector3(positionModel.x, positionModel.y, -positionModel.z);

        //For rotation 
        X.localEulerAngles = new Vector3(-rotateModel.x, 0f, 0f);
        Y.localEulerAngles = new Vector3(0f, -rotateModel.y, 0f);
        Z.localEulerAngles = new Vector3(0f, 0f, rotateModel.z);

        //For scale
        transform.localScale = scaleModel;

    }
}
