using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnObject : MonoBehaviour
{
    public Vector3 StartPos;
    public float  Distance, Troploin;
    public Rigidbody rb;
    public Quaternion StartRot;
    // Start is called before the first frame update
    void Start()
    {
        StartRot = transform.localRotation;
        rb = GetComponent<Rigidbody>();
        StartPos = transform.localPosition;



    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(StartPos, transform.localPosition);
        if (Distance > Troploin) Respawn();
       
    }

    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        this.transform.localPosition = StartPos;
        this.transform.localRotation = StartRot;
    }
}
