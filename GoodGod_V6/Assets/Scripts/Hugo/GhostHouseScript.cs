using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class GhostHouseScript : MonoBehaviour
{


    private GameObject temple;

    public float raising_height;
    public float building_speed;
    private GameObject house_mesh;
    private GameObject blocks_mesh;

    public GameObject house_prefab;

    private bool is_building = false;

    private float building_completion;

    [HideInInspector] public List<GameObject> active_builder;
    private float total_work_efficiency = 0;

    [HideInInspector] public int building_row = 99;
    [HideInInspector] public int building_placement;


    void Start()
    {

        house_mesh = this.transform.GetChild(0).gameObject;
        blocks_mesh = this.transform.GetChild(1).gameObject;

        temple = TempleScript.instance.gameObject;

        //temple.GetComponent<TempleScript>().building_que.Add(this.gameObject);

    }

    public void _getting_to_build(GameObject builder)
    {
        active_builder.Add(builder);
        if (!is_building)
        {
            is_building = true;
            StartCoroutine(building());
        }

    }

    IEnumerator building()
    {
        while (active_builder.Count > 0)
        {

            if (building_completion >= 1)
            {

                temple.GetComponent<TempleScript>().building_que.Remove(this.transform.gameObject);

                foreach (GameObject villager in temple.GetComponent<TempleScript>().idle_workers)
                {
                    if (villager.GetComponent<VillagerScript>().go_build_target != null)
                    {

                        villager.GetComponent<VillagerScript>().go_build_target = null;

                        villager.GetComponent<VillagerScript>().chill_out();


                    }
                }

                temple.GetComponent<TempleScript>()._reorganize_builders();

                temple.GetComponent<TempleScript>().building_que.Remove(this.gameObject);

                temple.GetComponent<TempleScript>()._spawn_building(this.gameObject, house_prefab);

                yield break;
            }

            total_work_efficiency = 0;

            foreach (GameObject builder in active_builder)
            {

                total_work_efficiency += builder.GetComponent<VillagerScript>().boost;
                //Debug.Log(boost)
            }

            building_completion += 0.015f * building_speed * total_work_efficiency * Time.deltaTime;

            house_mesh.transform.localPosition = new Vector3(house_mesh.transform.localPosition.x, -raising_height + (raising_height * building_completion), house_mesh.transform.localPosition.z);
            blocks_mesh.transform.localPosition = new Vector3(house_mesh.transform.localPosition.x, -(raising_height * building_completion), house_mesh.transform.localPosition.z);


            yield return new WaitForEndOfFrame();

        }

        is_building = false;

    }

    public void _ghosthouse_destroyed()
    {

        temple.GetComponent<TempleScript>().building_que.Remove(this.gameObject);

        foreach (GameObject villager in temple.GetComponent<TempleScript>().idle_workers)
        {
            if (villager.GetComponent<VillagerScript>().go_build_target != null)
            {

                villager.GetComponent<VillagerScript>().go_build_target = null;

                villager.GetComponent<VillagerScript>()._awareness_trigger(this.transform.position,false);


            }
        }

        temple.GetComponent<TempleScript>()._reorganize_builders();


        temple.GetComponent<TempleScript>()._destroy_building(this.gameObject);


    }



}
