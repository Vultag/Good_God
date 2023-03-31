using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class bonhomme_sable : MonoBehaviour
{
    public float boostLimit;
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    private bonhomme_boost Bonhomme;
    public quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
       part = GetComponent<ParticleSystem>();
       collisionEvents = new List<ParticleCollisionEvent>();
        rotation= new Quaternion(0,0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnParticleCollision(GameObject other)
    {

        if (other.tag == "Villager" || other.tag == "VillageoisTuto" || other.tag == "Worker")
        {

            other.GetComponent<VillagerScript>().Boosted();
            /*
            other.transform.rotation = rotation;
            Bonhomme = other.GetComponent<bonhomme_boost>();
            Bonhomme.Boost = Mathf.Clamp(Bonhomme.Boost, 0, 10);
            Bonhomme.Boost -= 10*Time.deltaTime ;
            Bonhomme.brilleMat.SetFloat("_alpha", 0);
            */
        }
     }

    

    }
