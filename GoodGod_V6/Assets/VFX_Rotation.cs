using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Rotation : MonoBehaviour
{
    public GameObject speedVFX;
    void FixedUpdate()
    {
        speedVFX.transform.Rotate(0,0,15*Time.deltaTime);
    }
}
