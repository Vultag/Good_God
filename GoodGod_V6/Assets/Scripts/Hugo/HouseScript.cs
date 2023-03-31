using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;

public class HouseScript : MonoBehaviour
{

    private GameObject temple;

    [SerializeField] private Material light_on_mat;
    [SerializeField] private Material light_off_mat;

    [HideInInspector] public bool lights_on = false;


    //[SerializeField] GameObject villagerPrefab;

    public int max_house_population;
    public int house_value;
    [HideInInspector]public int current_house_pop = 0;
    //private bool spawn_villager;
    private float time_speed;

    [HideInInspector] public List<GameObject> habitant;
    private List<GameObject> homeless_to_remove;
    [HideInInspector] public int building_placement;
    [HideInInspector] public int building_row;


    void Start()
    {

        homeless_to_remove = new List<GameObject>();

        temple = TempleScript.instance.gameObject;


        foreach (GameObject homeless in temple.GetComponent<TempleScript>().homeless)
        {
            if (current_house_pop < max_house_population)
            {
                habitant.Add(homeless);
                homeless.GetComponent<VillagerScript>().home = this.gameObject;
                homeless_to_remove.Add(homeless);
                current_house_pop++;
            }

        }
        if (homeless_to_remove.Count > 0)
        {
            foreach (GameObject homeless in homeless_to_remove)
            {

                temple.GetComponent<TempleScript>().homeless.Remove(homeless);

            }
        }

        homeless_to_remove.Clear();

        if (current_house_pop < max_house_population)
            StartCoroutine(spawn_villager_loop());
        

        time_speed = temple.GetComponent<TempleScript>().time_speed;


    }


    /*
    void spawn_villager()
    {

        GameObject new_villager = Instantiate(villagerPrefab, transform.position, transform.rotation);

    }
    */
    

    IEnumerator spawn_villager_loop()
    {
        while (current_house_pop < max_house_population)
        {

            const float spawn_countdown = 5000;
            float counter = 0f;


            while (counter < spawn_countdown)
            {
                counter += Time.deltaTime * time_speed;

                yield return new WaitForEndOfFrame(); //Don't freeze Unity
            }

            if (!temple.GetComponent<TempleScript>().worship_time)
            {
                temple.GetComponent<TempleScript>()._spawn_villager(this.gameObject);
                current_house_pop++;
            }
            counter = 0f;

        }
    }

    public void _update_house_habitant(GameObject leaver)
    {
        habitant.Remove(leaver);
        current_house_pop--;
        StartCoroutine(spawn_villager_loop());

    }
    public void _house_destroyed()
    {

        foreach (GameObject habitant in habitant)
        {

            habitant.GetComponent<VillagerScript>().home = null;

            habitant.GetComponent<VillagerScript>()._sleep();

            if (habitant.GetComponent<VillagerScript>().is_sleeping)
            {
                habitant.GetComponent<VillagerScript>().die();
            }
            else
            {
                temple.GetComponent<TempleScript>().homeless.Add(habitant);
            }


        }

        temple.GetComponent<TempleScript>()._destroy_building(this.gameObject);


    }


    public void light_switch(bool state)
    {
        if(state)
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material = light_on_mat;
            lights_on = true;
        }
        else
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material = light_off_mat;
            lights_on= false;
        }

    }


}
