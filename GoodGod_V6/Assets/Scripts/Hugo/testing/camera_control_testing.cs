using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control_testing : MonoBehaviour
{

    private Vector3 lastPos = Vector3.zero;
    private Vector3 delta = Vector3.zero;
    private Vector3 new_angle_rotation;
    private Quaternion prev_angle_rotation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            lastPos = Input.mousePosition;
            prev_angle_rotation = Camera.main.transform.rotation;
        }
        else if (Input.GetMouseButton(1))
        {

            delta = Input.mousePosition - lastPos;

            new_angle_rotation.Set(delta.y, delta.x, 0);
           // new_angle_rotation = new_angle_rotation.normalized;
            //Debug.Log(new_angle_rotation);
            Camera.main.transform.rotation = prev_angle_rotation * Quaternion.Euler(new_angle_rotation*0.5f);

        }


    }
}
