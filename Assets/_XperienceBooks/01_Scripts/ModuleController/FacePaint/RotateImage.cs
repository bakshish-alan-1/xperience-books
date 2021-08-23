using UnityEngine;

public class RotateImage : MonoBehaviour
{
    Vector3 rotationEuler;
    void Update()
    {
        if (transform.gameObject.activeSelf)
        { transform.Rotate(0, 0, -(float)(6.0 * 20.0f * Time.deltaTime)); }
    }
}
