using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast_eclair : MonoBehaviour
{

    public ParticleSystem eclair, boom;
    public Vector3 PositionEclair, positionRaycast;
    
    // Start is called before the first frame update
    void Start()
    {
        eclair.Stop();
        boom.Stop();
        positionRaycast = transform.position;
        positionRaycast.x -= 20;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void LanceEclair()
    { 
        
        eclair.Play();

        RaycastHit[] first_hits;

        first_hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), 1000, LayerMask.GetMask("awareness"));

        

        if (first_hits.Length > 0)
        {
          
            RaycastHit second_hit;


            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out second_hit, 10000, (LayerMask.GetMask("Villager") | LayerMask.GetMask("Default"))))
            {

                //Debug.Log(second_hit.collider.tag);


                for (int i = 0; i < first_hits.Length; i++)
                {
                    RaycastHit hit = first_hits[i];

                    Debug.Log("raycasteclair");
                    hit.collider.transform.parent.GetComponent<VillagerScript>()._awareness_trigger(second_hit.point,true);
                    

                }

                if (second_hit.collider.tag == "Villager" | second_hit.collider.tag == "Worker")
                {
                    PositionEclair = second_hit.collider.gameObject.transform.position;
                    boom.transform.position = PositionEclair;
                    boom.Play();

                    second_hit.collider.GetComponent<VillagerScript>().mesh.SetActive(true);
                    second_hit.collider.GetComponent<VillagerScript>().die();
                }

              



            }
        }
        RaycastHit hitEclair;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitEclair, Mathf.Infinity, (LayerMask.GetMask("Cristaux"))))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitEclair.distance, Color.yellow);
            if (hitEclair.collider.tag == "CristalEvent" )
            {
                PositionEclair = hitEclair.collider.gameObject.transform.position;
                boom.transform.position = PositionEclair;
                boom.Play();
                Destroy(hitEclair.collider.gameObject);

            }

        }
    }
}
