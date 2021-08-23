using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{


    float rotatespeed = 10f;
    private Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    private void Update()
    {

        
        // Handle a single touch
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // store the initial touch position
                    lastPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    // get the moved difference and convert it to an angle
                    // using the rotationSpeed as sensibility
                    var rotationX = (touch.position.x - lastPos.x) * rotatespeed * Time.deltaTime;
                    transform.Rotate(Vector3.up, -rotationX, Space.Self);

                    lastPos = touch.position;
                    break;
            }
        }
    }

    
}




