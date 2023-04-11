using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cristal_Collision : MonoBehaviour
{
    public GameObject Debris, Impact;
    public Vector3 PosImpact;
    public Impact_disparition impactScript;
    private GameObject temple;
    public AudioSource BoomAudio;
    // Start is called before the first frame update
    [SerializeField] private GameObject blood_prefab;

    private bool has_hit = false;

    void Start()
    {
        BoomAudio = this.GetComponent<AudioSource>();
        temple = TempleScript.instance.gameObject;

        this.GetComponent<Rigidbody>().velocity = new Vector3(0,-15f,0);
        this.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-5,5), Random.Range(-5, 5), Random.Range(-5, 5));


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Debug.Log(collision.collider.gameObject.name, gameObject);
        RaycastHit[] first_hits;

        first_hits = Physics.RaycastAll(transform.position + new Vector3(0,150,0), Vector3.down, 1000, LayerMask.GetMask("awareness"));



        if (first_hits.Length > 0)
        {

            for (int i = 0; i < first_hits.Length; i++)
            {
                RaycastHit hit = first_hits[i];
                //Debug.Log("scared_b meteor");
                hit.collider.transform.parent.GetComponent<VillagerScript>()._awareness_trigger(this.transform.position,false);


            }

        }


        if (collision.collider.tag == "Villager")
        {

            GameObject blood = Instantiate(blood_prefab, new Vector3(this.transform.position.x, -5.8500f, this.transform.position.z), Quaternion.identity, TempleScript.instance.transform.parent);
            Destroy(blood, 16f);

            collision.collider.GetComponent<VillagerScript>().mesh.SetActive(false);
            collision.collider.GetComponent<VillagerScript>().die();

        }


        if (has_hit == false)
        {

            if(collision.collider.tag != "phy_hand")
                has_hit = true;

            //Debug.Log(collision.collider.tag,gameObject);

            if (collision.collider.tag == "Maison")
            {

                if (collision.collider.transform.GetComponent<HouseScript>() == null)
                {

                    if (collision.collider.transform.parent.parent.GetComponent<HouseScript>().building_row == 0)
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.parent.parent.GetComponent<HouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.parent.parent.GetComponent<HouseScript>().building_placement]);
                    }

                    if (collision.collider.transform.parent.parent.GetComponent<HouseScript>().building_row == 1)
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.parent.parent.GetComponent<HouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.parent.parent.GetComponent<HouseScript>().building_placement]);
                    }

                    //Debug.Log("destroy_maison");
                    collision.collider.transform.parent.parent.GetComponent<HouseScript>()._house_destroyed();
                }
                else 
                {
                    if (collision.collider.transform.GetComponent<HouseScript>().building_row == 0)
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.GetComponent<HouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.GetComponent<HouseScript>().building_placement]);
                    }

                    if (collision.collider.transform.GetComponent<HouseScript>().building_row == 1)
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.GetComponent<HouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.GetComponent<HouseScript>().building_placement]);
                    }

                    //Debug.Log("destroy_maison2");
                    collision.collider.transform.GetComponent<HouseScript>()._house_destroyed();

                }


            }
            if (collision.collider.tag == "GhostBuilding")
            {


                if (collision.collider.transform.GetComponent<GhostHouseScript>() == null)
                {

                    if (collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().raising_height < 7)
                    {
                        if (collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_row == 0)
                        {
                            //Debug.Log("row0");
                            //Debug.Log(collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement);
                            BoomAudio.Play();
                            GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement]);
                        }

                        if (collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_row == 1)
                        {
                            //Debug.Log("row1");
                            //Debug.Log(collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement);
                            BoomAudio.Play();
                            GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement]);
                        }
                    }
                    else
                    {
                        //Debug.Log(collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().raising_height);
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().disco_spawn.position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), TempleScript.instance.disco_spawn);
                    }
                
                    collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>()._ghosthouse_destroyed();

                }
                else
                {
                    if (collision.collider.transform.GetComponent<GhostHouseScript>().raising_height < 7)
                    {
                        if (collision.collider.transform.GetComponent<GhostHouseScript>().building_row == 0)
                        {
                            Debug.Log("row0");
                            Debug.Log(collision.collider.transform.GetComponent<GhostHouseScript>().building_placement);
                            BoomAudio.Play();
                            GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.GetComponent<GhostHouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.GetComponent<GhostHouseScript>().building_placement]);
                        }

                        if (collision.collider.transform.GetComponent<GhostHouseScript>().building_row == 1)
                        {
                            Debug.Log("row1");
                            Debug.Log(collision.collider.transform.GetComponent<GhostHouseScript>().building_placement);
                            BoomAudio.Play();
                            GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.GetComponent<GhostHouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.GetComponent<GhostHouseScript>().building_placement]);
                        }
                    }
                    else
                    {
                        //Debug.Log(collision.collider.transform.GetComponent<GhostHouseScript>().raising_height);
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().disco_spawn.position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), TempleScript.instance.disco_spawn);
                    }


                    collision.collider.transform.GetComponent<GhostHouseScript>()._ghosthouse_destroyed();


                    /*
                    //if (collision.collider.GetComponent<GhostHouseScript>().raising_height < 6)
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().disco_spawn.position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), TempleScript.instance.transform.parent);
                        collision.collider.GetComponent<GhostHouseScript>()._ghosthouse_destroyed();
                    }
                    */
                }



            }
            if (collision.collider.tag == "plaisir")
            {
                BoomAudio.Play();
                GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().disco_spawn.position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), TempleScript.instance.GetComponent<TempleScript>().disco_spawn);


                temple.GetComponent<TempleScript>().disco_builed = false;

                foreach (GameObject villager in TempleScript.instance.idle_workers)
                {

                    StopCoroutine(villager.GetComponent<VillagerScript>().dancing());

                    if (villager.GetComponent<VillagerScript>().is_dancing)
                    {
                        villager.GetComponent<VillagerScript>()._awareness_trigger(collision.collider.transform.parent.parent.transform.position, false);
                    }
                }

                temple.GetComponent<TempleScript>()._destroy_building(collision.collider.transform.parent.parent.gameObject);

            }



        }
        if (collision.collider.tag == "sol")
        {
            PosImpact = this.gameObject.transform.position;
           // PosImpact.y -= 1;
            GameObject clone = Instantiate(Impact, new Vector3(PosImpact.x, temple.transform.position.y + 0.01f, PosImpact.z), Quaternion.identity, TempleScript.instance.transform.parent); ;
            impactScript = clone.gameObject.GetComponent<Impact_disparition>();
            clone.gameObject.SetActive(true);
            Destroy(this.gameObject);
           
        }




    }

    public void destroyed_mid_air()
    {

        GameObject clone = Instantiate(Impact, this.gameObject.transform.position, Quaternion.identity, TempleScript.instance.transform.parent);
        clone.gameObject.GetComponent<Impact_disparition>()._is_destroyed_mid_air();
        Destroy(this.gameObject);

    }


    
}
