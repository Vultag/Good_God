using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smooth_camera_follow : MonoBehaviour
{

    public Transform target;
    [Range(0f, 1f)]
    public float pos_damp;
    [Range(0f, 1f)]
    public float rot_damp;


    private void OnEnable()
    {
        this.transform.position = target.transform.position;
        this.transform.rotation = target.transform.rotation;
    }

    private void Update()
    {

        this.transform.position = Vector3.Lerp(transform.position, target.transform.position,pos_damp);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, rot_damp);
    }



}
