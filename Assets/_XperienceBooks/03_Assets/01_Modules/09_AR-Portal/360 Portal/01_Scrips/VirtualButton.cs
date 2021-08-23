using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualButton : MonoBehaviour
{


    Ray ray;
    RaycastHit hit;

    [SerializeField] GameObject doorRoot;
    //public Animator anim;

    bool isObjectPlced = true;

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
                PlacedObject();
        #endif
    }

    private void OnEnable()
    {
        PlacementController.onPlacedObject += PlacedObject;
    }


    private void OnDisable()
    {
        PlacementController.onPlacedObject -= PlacedObject;
    }

    void PlacedObject()
    {

        isObjectPlced = true;

    }

    void OnMouseDown()
    {
        if (isObjectPlced) {
            isObjectPlced = false;
            Debug.Log("Click");
            doorRoot.transform.GetChild(0).GetComponent<Animation>().Play("Door");
            //anim.SetTrigger("DoorOpen");
            //GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
