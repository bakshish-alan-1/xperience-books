using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DynamicTeture : MonoBehaviour
{

    [SerializeField]
    RawImage image;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SetData(Texture2D texture , int i) {
        image.texture = texture;
        index = i;
    }


    public void Onclick() {

        Debug.Log("Click on Object : " + index);
        FaceController.Instance.SwapFaces(index);
    }

}
