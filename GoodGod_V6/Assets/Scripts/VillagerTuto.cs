using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VillagerTuto : MonoBehaviour
{
    public float Boost, brille;
    public Material brilleMat;
    public GameObject Mec, Villageois;
    public Rigidbody rb;
    private XRGrabInteractable Grab;
    // Start is called before the first frame update
    void Start()
    {
        brilleMat = new Material(brilleMat);
        Boost = 30f;

    }
    private void FixedUpdate()
    {
        brilleMat = Mec.GetComponent<Renderer>().materials[0];
        brilleMat.SetFloat("_contour", brille);
        if (Boost <= 0)
        {
            Grab = this.GetComponent<XRGrabInteractable>();
            Grab.enabled = false;
            transform.Translate(0, 2 * Time.deltaTime, 0);
            rb = Villageois.GetComponent<Rigidbody>();
            rb.useGravity = false;
            brille = Boost * 0.1f;
        }
        brille = Mathf.Clamp(brille, 0.15f, 4);
        brille = Boost * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "LimiteCiel") Destroy(this.gameObject);
    }
}
