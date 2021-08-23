using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils 
{


    static public bool WasTouchStartDetected()
    {
        #if UNITY_EDITOR
                return Input.GetKeyDown(KeyCode.Space);
        #else
                return Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began;
        #endif
    }

    static public bool IsTouchOnUI()
    {

        #if UNITY_EDITOR
                return EventSystem.current.IsPointerOverGameObject();
        #else
                return Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject (Input.GetTouch(0).fingerId);
        #endif
    }
}
