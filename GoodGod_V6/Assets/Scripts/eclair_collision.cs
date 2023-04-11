using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class eclair_collision : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    //public Vector3 PositionDebris;
    public GameObject Debris, ruines;
    public GameObject temple;
    public AudioSource BoomAudio;

    //private bool has_hit = false;
    // Start is called before the first frame update
    void Start()
    {
       
        part = GetComponent<ParticleSystem>();
        //ruines = GameObject.FindGameObjectWithTag("debris");
        //Debris = GameObject.FindGameObjectWithTag("debris"); ;
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {


        RaycastHit[] first_hits;

        first_hits = Physics.RaycastAll(transform.position, Vector3.down, 1000, LayerMask.GetMask("awareness"));



        if (first_hits.Length > 0)
        {

            RaycastHit second_hit;


            if (Physics.Raycast(transform.position, Vector3.down, out second_hit, 10000, (LayerMask.GetMask("Villager") | LayerMask.GetMask("Default"))))
            {

                //Debug.Log(second_hit.collider.tag);


                for (int i = 0; i < first_hits.Length; i++)
                {
                    RaycastHit hit = first_hits[i];

                    hit.collider.transform.parent.GetComponent<VillagerScript>()._awareness_trigger(second_hit.point, false);


                }

                if (second_hit.collider.tag == "Villager" | second_hit.collider.tag == "Worker")
                {

                    second_hit.collider.GetComponent<VillagerScript>().mesh.SetActive(true);
                    second_hit.collider.GetComponent<VillagerScript>().die();
                }





            }
        }


        //if (has_hit == false)
        {

            //has_hit = true;

            if (other.tag == "Maison")
            {

                if (other.GetComponent<HouseScript>().building_row == 0)
                {
                    BoomAudio.Play();
                     GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_0[other.GetComponent<HouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_0[other.GetComponent<HouseScript>().building_placement]);
                }

                if (other.GetComponent<HouseScript>().building_row == 1)
                {
                    BoomAudio.Play();
                    GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_1[other.GetComponent<HouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_1[other.GetComponent<HouseScript>().building_placement]);
                }



                other.GetComponent<HouseScript>()._house_destroyed();

            }
            if (other.tag == "GhostBuilding")
            {

                if (other.GetComponent<GhostHouseScript>().raising_height < 7)
                {
                    if (other.GetComponent<GhostHouseScript>().building_row == 0)
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_0[other.GetComponent<GhostHouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_0[other.GetComponent<GhostHouseScript>().building_placement]);
                    }

                    if (other.GetComponent<GhostHouseScript>().building_row == 1)
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_1[other.GetComponent<GhostHouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_1[other.GetComponent<GhostHouseScript>().building_placement]);
                    }
                }
                else
                {
                    BoomAudio.Play();
                    GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().disco_spawn.position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0));
                }

                Debug.Log("ghosthouse");
                other.GetComponent<GhostHouseScript>()._ghosthouse_destroyed();
            }
            if (other.tag == "plaisir")
            {
                BoomAudio.Play();
                GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().disco_spawn.position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), TempleScript.instance.GetComponent<TempleScript>().disco_spawn);


                temple.GetComponent<TempleScript>().disco_builed = false;

                foreach (GameObject villager in TempleScript.instance.idle_workers)
                {

                    StopCoroutine(villager.GetComponent<VillagerScript>().dancing());

                    if (villager.GetComponent<VillagerScript>().is_dancing)
                    {
                        Debug.Log("eclair");
                        villager.GetComponent<VillagerScript>()._awareness_trigger(temple.GetComponent<TempleScript>().DiscoGB.transform.position, false);
                    }
                }

                temple.GetComponent<TempleScript>()._destroy_building(temple.GetComponent<TempleScript>().DiscoGB);


            }
        }

    }
}
