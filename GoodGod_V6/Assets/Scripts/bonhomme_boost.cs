using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class bonhomme_boost : MonoBehaviour
{
    public float Boost, brille;
    public Material brilleMat;
    public GameObject Mec, Villageois;
    public Rigidbody rb;
    private XRGrabInteractable Grab;
    public VillagerScript VillagerScript;
    // Start is called before the first frame update
    private void Awake()
    {
        //brilleMat = Mec.GetComponent<Renderer>().materials[0];
        //brilleMat = new Material(brilleMat);
    }
    void Start()
    {

        brilleMat = new Material(brilleMat);
        //Boost = 30f;
        //brilleMat = Mec.GetComponent<Renderer>().materials[0];
       
        //brille = Mathf.Clamp(brille, 0.15f, 4);
    }
    private void FixedUpdate()
    {
        //brilleMat.SetFloat("_contour", brille);
        //if (Boost <= 0)
        //{
        //    Grab = this.GetComponent<XRGrabInteractable>();
        //    Grab.enabled = false;
        //    transform.Translate(0, 2 * Time.deltaTime, 0);
        //    rb = Villageois.GetComponent<Rigidbody>();
        //    rb.useGravity = false;
        //    brille = Boost * 0.1f;
        //}
    }
    /*
    // Update is called once per frame
    void Update()
    {
        brille = Mathf.Clamp(brille, 0.15f, 4);
        brille = Boost * 0.1f;
        brilleMat = Mec.GetComponent<Renderer>().materials[0];
        brilleMat.SetFloat("_contour", brille);
        if (Boost <= 0)
        {
           
            Grab = this.GetComponent<XRGrabInteractable>();
            Grab.enabled = false;
           // transform.Translate(0, 2 * Time.deltaTime, 0);
            transform.position= new Vector3(0, transform.position.y + 2 * Time.deltaTime, 0);
            rb = Villageois.GetComponent<Rigidbody>();
            rb.useGravity = false;
            brille = Boost * 0.1f;
        }
       

    }
    */
    public void update_boost_render(float brille)
    {
        brille = Mathf.Clamp(brille, 0.15f, 4);


        brilleMat = Mec.GetComponent<Renderer>().materials[0];
        brilleMat.SetFloat("_alpha", (brille>0?0:-1));
        brilleMat.SetFloat("_contour", 1.6f-brille);

    }

    public void disable_boost_render()
    {
        brille = 0;

        brilleMat = Mec.GetComponent<Renderer>().materials[0];
        brilleMat.SetFloat("_alpha", -1);
        brilleMat.SetFloat("_contour", 1.6f);

    }



}
