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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Debug.Log(collision.collider.gameObject.name, gameObject);

        Debug.Log(collision.collider.transform.parent.gameObject);


        if (collision.collider.tag == "Villager")
        {

            GameObject blood = Instantiate(blood_prefab, new Vector3(this.transform.position.x, -5.8500f, this.transform.position.z), Quaternion.identity, TempleScript.instance.transform.parent);
            Destroy(blood, 16f);

            collision.collider.GetComponent<VillagerScript>().mesh.SetActive(false);
            collision.collider.GetComponent<VillagerScript>().die();

        }


        if (has_hit == false)
        {

            has_hit = true;


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

                    collision.collider.transform.parent.parent.GetComponent<HouseScript>()._house_destroyed();
                }

            }
            if (collision.collider.tag == "GhostBuilding")
            {


                if (collision.collider.transform.GetComponent<GhostHouseScript>() == null)
                {

                    if (collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().raising_height < 6)
                    {
                        if (collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_row == 0)
                        {
                            BoomAudio.Play();
                            GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_0[collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement]);
                        }

                        if (collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_row == 1)
                        {
                            BoomAudio.Play();
                            GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement].position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), temple.GetComponent<TempleScript>().building_row_1[collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>().building_placement]);
                        }
                    }
                    else
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().disco_spawn.position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), TempleScript.instance.transform.parent);
                    }
                
                    collision.collider.transform.parent.parent.GetComponent<GhostHouseScript>()._ghosthouse_destroyed();

                }
                else
                {
                    if (collision.collider.GetComponent<GhostHouseScript>().raising_height < 6)
                    {
                        BoomAudio.Play();
                        GameObject clone = Instantiate(Debris, temple.GetComponent<TempleScript>().disco_spawn.position, Quaternion.Euler(0, 180 * Random.Range(0, 2), 0), TempleScript.instance.transform.parent);
                        collision.collider.GetComponent<GhostHouseScript>()._ghosthouse_destroyed();
                    }
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
                        villager.GetComponent<VillagerScript>()._awareness_trigger(collision.collider.transform.parent.parent.transform.position);
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

    
}
