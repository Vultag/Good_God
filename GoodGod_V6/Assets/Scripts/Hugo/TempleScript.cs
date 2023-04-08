//using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using System.Xml.Schema;
using TMPro;
using Unity.AI.Navigation;
//using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEditor;
//using UnityEditor.PackageManager.UI;
//using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
//using static UnityEditor.FilePathAttribute;
using static UnityEngine.GraphicsBuffer;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class TempleScript : MonoBehaviour
{

    [SerializeField] NavMeshSurface NavSurface;

    [SerializeField] int NumberOfStartingHouse;


    //[SerializeField] GameObject Reserve;


    [SerializeField] GameObject SmallHousePrefab;
    [SerializeField] GameObject SmallGhostHousePrefab;
    [SerializeField] GameObject MediumHousePrefab;
    [SerializeField] GameObject MediumGhostHousePrefab;
    [SerializeField] GameObject BigHousePrefab;
    [SerializeField] GameObject BigGhostHousePrefab;
    [SerializeField] GameObject GhostDiscoPrefab;


    [SerializeField] GameObject villagerPrefab;


    //[SerializeField] GameObject Mineral_prefab;
    //[SerializeField] GameObject Tree_prefab;

    [SerializeField] public GameObject[] Minerals_prefab;
    [SerializeField] public GameObject[] Minerals_destroy_prefab;
    [SerializeField] public GameObject[] Trees_prefab;


    //[SerializeField] GameObject WoodcutterCampPrefab;
    //[SerializeField] GameObject WoodcutterPrefab;

    //[SerializeField] GameObject GarthererCampPrefab;
    //[SerializeField] GameObject GarthererPrefab;


    [HideInInspector] public List<GameObject> building_que;

    [HideInInspector] public List<GameObject> idle_workers;

    [HideInInspector] public List<GameObject> working_workers;

    [HideInInspector] public List<GameObject> homeless;

    public Transform[] building_row_0;
    private List<int> building_row_0_free;
    public Transform[] building_row_1;
    private List<int> building_row_1_free;
    [HideInInspector] public List<GameObject> current_building;
    [HideInInspector] public List<GameObject> small_building;
    [HideInInspector] public List<GameObject> medium_building;
    public Transform disco_spawn;
    [HideInInspector] public GameObject DiscoGB;

    public GameObject sol;


    private float village_happiness = 50f;
    [HideInInspector] public float village_terror = 50f;
    private int workers_ready_to_worship;
    [HideInInspector] public bool disco_builed = false;
    [HideInInspector] public bool worship_time = false;
    [HideInInspector] public bool sleep_time = false;
    //[HideInInspector] public int max_population;
    [HideInInspector] public int current_population;


    public TextMeshProUGUI text_essence;
    public int essence_num = 0;

    public TextMeshProUGUI current_population_slide;
    public TextMeshProUGUI text_time;
    public TextMeshProUGUI text_day;
    public GameObject sun_light_GB;
    public float time_speed;
    private float time;

    float minutes = 0;
    public float heures = 0;
    [HideInInspector] public float journees = 0;
    int hour_tick = 0;

    public Slider terror_slide;

    public Tutoriel tutoriel;

    public static TempleScript instance;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogError("2 temple");
        }
        else
            instance = this;

    }

    void Start()
    {


        time = minutes * 60 + heures * 3600;
        hour_tick = (int) heures;


        building_row_0_free = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7};
        building_row_1_free = new List<int> { 0, 1, 2, 3, 4, 5, 6};

        if (NumberOfStartingHouse > building_row_0_free.Count)
            Debug.LogError("2 row starting not implemented");
            

        for (int i = 0; i < NumberOfStartingHouse; i++)
        {

            int pos_id = Random.Range(0, building_row_0_free.Count);

            GameObject new_building = Instantiate(SmallHousePrefab, building_row_0[building_row_0_free[pos_id]].position, building_row_0[building_row_0_free[pos_id]].rotation, TempleScript.instance.transform.parent);


            new_building.GetComponent<HouseScript>().building_row = 0;
            new_building.GetComponent<HouseScript>().building_placement = building_row_0_free[pos_id];

            current_building.Add(new_building);
            small_building.Add(new_building);

            building_row_0_free.Remove(building_row_0_free[pos_id]);
            /*
            Vector3 new_pos = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(Random.Range(6, 12), 0, 0);
            Quaternion new_rot = Quaternion.FromToRotation(Vector3.back, new_pos.normalized);

            GameObject new_villager = Instantiate(villagerPrefab, new_pos, new_rot);

            idle_workers.Add(new_villager); 
            */
        }

        //current_population = NumberOfStartingVillagers;

        
        current_population_slide.text = current_population.ToString();

        NavSurface.BuildNavMesh();

    }

    // Update is called once per frame
    void Update()
    {

        update_timer();

        terror_slide.value = 100f - village_terror;


        if (Input.GetKeyDown(KeyCode.S))
        {

            _spawn_ghost_building();

            /*

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {

                //_get_to_work(idle_workers[0], "Woodcutter", hit);

            }

            */
        }

        //if(Input.GetMouseButtonDown(0))
        //{
        //    /*
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 10000,6))
        //    {

        //        if (hit.collider.gameObject.tag == "Villager")
        //        {
        //            Debug.Log("hit_villager");
        //            hit.collider.gameObject.GetComponent<VillagerScript>().die();
        //        }

        //    }
        //    */
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit[] first_hits;

        //    first_hits = Physics.RaycastAll(ray,1000, LayerMask.GetMask("awareness"));


        //    if (first_hits.Length > 0)
        //    {

        //        RaycastHit second_hit;


        //        if (Physics.Raycast(ray, out second_hit, 10000, (LayerMask.GetMask("Villager") | LayerMask.GetMask("Default"))))
        //        {

        //            //Debug.Log(second_hit.collider.tag);


        //            for (int i = 0; i < first_hits.Length; i++)
        //            {
        //                RaycastHit hit = first_hits[i];

        //                hit.collider.transform.parent.GetComponent<VillagerScript>()._awareness_trigger(second_hit.point);


        //            }

        //            if (second_hit.collider.tag == "Villager")
        //            {

        //               second_hit.collider.GetComponent<VillagerScript>().grabbed_by_player(false);
        //               //second_hit.collider.GetComponent<VillagerScript>().die();
        //            }



                    






        //        }


        //    /*
        //    if (Physics.RaycastAll(ray, out first_hits[0], 10000, LayerMask.GetMask("awareness")))
        //    {

        //        RaycastHit second_hit;


        //        if (Physics.Raycast(ray, out second_hit, 10000, (LayerMask.GetMask("Villager") | LayerMask.GetMask("Default"))))
        //        {

        //            //Debug.Log(second_hit.collider.tag);

        //            if (second_hit.collider.tag == "Villager")
        //            {
        //                first_hit.collider.transform.parent.GetComponent<VillagerScript>().die();
        //            }
        //            else
        //                first_hit.collider.transform.parent.GetComponent<VillagerScript>()._awareness_trigger(second_hit.point);


        //        }

        //    */

        //}

        //}


        
    }

    public void _spawn_ghost_building()//(Vector3 location,string building)
    {
        
        float total_value = 0;
        float ratio = 2;

        foreach (GameObject house in current_building)
        {
            //Debug.Log(house.gameObject.name);
            total_value += house.GetComponent<HouseScript>().house_value;

        }


        ratio = total_value / current_building.Count();
        //Debug.Log(total_value);
        //Debug.Log(ratio);


        if(Random.Range(1f,2f) > ratio || building_row_0_free.Union(building_row_1_free).Count() == 0)
        {
            //Debug.Log("upgrade");

            if(small_building.Count() > medium_building.Count())
            {

                GameObject building_to_remove = small_building[0];

                GameObject new_upgraded_building = Instantiate(MediumGhostHousePrefab, building_to_remove.transform.position, building_to_remove.transform.rotation, TempleScript.instance.transform.parent);

                foreach (GameObject habitant in building_to_remove.GetComponent<HouseScript>().habitant)
                {
                    //Debug.Log("new_homeless");
                    homeless.Add(habitant);
                    habitant.GetComponent<VillagerScript>().home = null;

                }


                new_upgraded_building.GetComponent<GhostHouseScript>().building_row = building_to_remove.GetComponent<HouseScript>().building_row;
                new_upgraded_building.GetComponent<GhostHouseScript>().building_placement = building_to_remove.GetComponent<HouseScript>().building_placement;

                current_building.Remove(building_to_remove);
                small_building.Remove(building_to_remove);

                Destroy(building_to_remove);

                building_que.Add(new_upgraded_building);

                _reorganize_builders();

            }
            else
            {

                GameObject building_to_remove = medium_building[0];

                GameObject new_upgraded_building = Instantiate(BigGhostHousePrefab, building_to_remove.transform.position, building_to_remove.transform.rotation, TempleScript.instance.transform.parent);

                foreach (GameObject habitant in building_to_remove.GetComponent<HouseScript>().habitant)
                {
                    //Debug.Log("new_homeless");
                    homeless.Add(habitant);
                    habitant.GetComponent<VillagerScript>().home = null;

                }


                new_upgraded_building.GetComponent<GhostHouseScript>().building_row = building_to_remove.GetComponent<HouseScript>().building_row;
                new_upgraded_building.GetComponent<GhostHouseScript>().building_placement = building_to_remove.GetComponent<HouseScript>().building_placement;


                current_building.Remove(building_to_remove);
                medium_building.Remove(building_to_remove);

                Destroy(building_to_remove);

                building_que.Add(new_upgraded_building);

                _reorganize_builders();

            }



        }
        else
        {
            //Debug.Log("build new");

            if (building_row_0_free.Count != 0)
            {

                int pos_id = Random.Range(0, building_row_0_free.Count);

                Debug.Log(building_row_0[building_row_0_free[pos_id]], gameObject);

                if (building_row_0[building_row_0_free[pos_id]].transform.childCount>0)
                    Destroy(building_row_0[building_row_0_free[pos_id]].transform.GetChild(0).gameObject);

                GameObject new_building = Instantiate(SmallGhostHousePrefab, building_row_0[building_row_0_free[pos_id]].position, building_row_0[building_row_0_free[pos_id]].rotation, TempleScript.instance.transform.parent);

                new_building.GetComponent<GhostHouseScript>().building_row = 0;
                new_building.GetComponent<GhostHouseScript>().building_placement = building_row_0_free[pos_id];

                building_row_0_free.Remove(building_row_0_free[pos_id]);

                building_que.Add(new_building);

                _reorganize_builders();

            }
            else
            {

                int pos_id = Random.Range(0, building_row_1_free.Count);

                if (building_row_1[building_row_1_free[pos_id]].transform.childCount > 0)
                    Destroy(building_row_1[building_row_1_free[pos_id]].transform.GetChild(0).gameObject);

                GameObject new_building = Instantiate(SmallGhostHousePrefab, building_row_1[building_row_1_free[pos_id]].position, building_row_1[building_row_1_free[pos_id]].rotation, TempleScript.instance.transform.parent);

                new_building.GetComponent<GhostHouseScript>().building_row = 1;
                new_building.GetComponent<GhostHouseScript>().building_placement = building_row_1_free[pos_id];

                building_row_1_free.Remove(building_row_1_free[pos_id]);

                building_que.Add(new_building);

                _reorganize_builders();

            }
        }






        NavSurface.BuildNavMesh();
        
    }


    public void _destroy_building(GameObject building)
    {

        if (building.GetComponent<HouseScript>() != null)
        {
            current_building.Remove(building.gameObject);
            small_building.Remove(building.gameObject);
            medium_building.Remove(building.gameObject);


            if (building.GetComponent<HouseScript>().building_row == 0)
            {
                building_row_0_free.Add(building.GetComponent<HouseScript>().building_placement);
                //building_row_0_free.Sort();
            }
            else if (building.GetComponent<HouseScript>().building_row == 1)
            {
                building_row_1_free.Add(building.GetComponent<HouseScript>().building_placement);
                //building_row_1_free.Sort();
            }

        }
        else if (building.GetComponent<GhostHouseScript>() != null)
        {

            if (building.GetComponent<GhostHouseScript>().building_row == 0)
            {
                building_row_0_free.Add(building.GetComponent<GhostHouseScript>().building_placement);
                //building_row_0_free.Sort();
            }
            else if (building.GetComponent<GhostHouseScript>().building_row == 1)
            {
                building_row_1_free.Add(building.GetComponent<GhostHouseScript>().building_placement);
                //building_row_1_free.Sort();
            }
            else
            {
                disco_builed = false;
            }

        }
        else
        {
            disco_builed = false;
        }


       Destroy(building.gameObject);
        
    }

    private void _spawn_disco()
    {

        if (disco_spawn.transform.childCount > 0)
            Destroy(disco_spawn.transform.GetChild(0).gameObject);

        GameObject new_disco = Instantiate(GhostDiscoPrefab, disco_spawn.position, disco_spawn.rotation, TempleScript.instance.transform.parent);

        building_que.Add(new_disco);

        tutoriel.TropBonheur();

        _reorganize_builders();

    }

    private void building_job()
    {

        foreach (GameObject villager in idle_workers)
        {
            if (!villager.GetComponent<VillagerScript>().is_dancing && !villager.GetComponent<VillagerScript>().is_grabbed)
            {
                villager.GetComponent<VillagerScript>().targetGB.SetActive(false);
                villager.GetComponent<VillagerScript>().StopAllCoroutines();
                villager.GetComponent<VillagerScript>().go_build(building_que[0]);
            }
        }

    }


    public void _spawn_building(GameObject old_building, GameObject building)
    {

        GameObject new_building = Instantiate(building, old_building.transform.position, old_building.transform.rotation, TempleScript.instance.transform.parent);

        //Destroy(gb);

        if (new_building.GetComponent<HouseScript>() != null)
        {

            new_building.GetComponent<HouseScript>().building_row = old_building.GetComponent<GhostHouseScript>().building_row;
            new_building.GetComponent<HouseScript>().building_placement = old_building.GetComponent<GhostHouseScript>().building_placement;

            current_building.Add(new_building);

            if (new_building.GetComponent<HouseScript>().house_value == 1)
                small_building.Add(new_building);
            if (new_building.GetComponent<HouseScript>().house_value == 2)
                medium_building.Add(new_building);
        }
        else
        { 
            DiscoGB = new_building;
            disco_builed = true;
            _disco_time();
        }

        DestroyImmediate(old_building, false);

        /*
        if (building_row_0_free.Count != 0)
        {

            int pos_id = Random.Range(0, building_row_0_free.Count);

                       GameObject new_building = Instantiate(SmallHousePrefab, building_row_0[building_row_0_free[pos_id]].position, building_row_0[building_row_0_free[pos_id]].rotation);

            building_row_0_free.Remove(building_row_0_free[pos_id]);

            building_row_0_free.Remove(building_row_0_free[pos_id]);
    
        }
        else
        {

            int pos_id = Random.Range(0, building_row_1_free.Count);

            GameObject new_building = Instantiate(SmallHousePrefab, building_row_1[building_row_1_free[pos_id]].position, building_row_1[building_row_1_free[pos_id]].rotation);

            building_row_1_free.Remove(building_row_1_free[pos_id]);

        }
        */

        NavSurface.BuildNavMesh();

    }

    public void _spawn_villager(GameObject house)
    {

        current_population++;

        current_population_slide.text = current_population.ToString();

        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(house.transform.position, out closestHit, 500, 1))
        {

            GameObject new_villager = Instantiate(villagerPrefab, closestHit.position, house.transform.rotation, TempleScript.instance.transform.parent);

            house.GetComponent<HouseScript>().habitant.Add(new_villager.transform.GetChild(0).gameObject);

            new_villager.transform.GetChild(0).GetComponent<VillagerScript>().home = house;

            idle_workers.Add(new_villager.transform.GetChild(0).gameObject);

        }
        else
            Debug.LogError("coudnt create villager");

            //Debug.Log(current_population);

            //if (worship_time == true) new_villager.GetComponent<VillagerScript>().get_ready_to_worship();
        
    }



    /*
    private void _get_to_work(GameObject villager,string job, Vector3 location)
    {

        switch(job)
        {
            case "Woodcutter":
            {
                    GameObject new_building = Instantiate(WoodcutterCampPrefab, location, Quaternion.identity);

                    Transform new_worker_transform = villager.transform;

                    GameObject new_worker = Instantiate(WoodcutterPrefab, new_worker_transform.position, new_worker_transform.rotation);

                    new_worker.transform.GetChild(0).GetComponent<WoodCutterScript>().Working_building = new_building;
                    new_worker.transform.GetChild(0).GetComponent<WoodCutterScript>().Reserve = Reserve;


                    idle_workers.Remove(villager);

                    Destroy(villager);
                    break;
            }
            case "Gartherer":
                {

                    GameObject new_building = Instantiate(GarthererCampPrefab, location, Quaternion.identity);

                    Transform new_worker_transform = villager.transform;

                    GameObject new_worker = Instantiate(GarthererPrefab, new_worker_transform.position, new_worker_transform.rotation);

                    new_worker.transform.GetChild(0).GetComponent<GarthererScript>().Working_building = new_building;
                    new_worker.transform.GetChild(0).GetComponent<GarthererScript>().Reserve = Reserve;


                    idle_workers.Remove(villager);

                    Destroy(villager);
                    break;
                }
        }


        NavSurface.BuildNavMesh();

    }
    */

    public void respawn_ressource()
    {
        if (Random.Range(0, 2) == 0)
        {
            Instantiate(Trees_prefab[Random.Range(0,Trees_prefab.Count())], Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(Random.Range(60, 70), this.transform.position.y, 0), Quaternion.Euler(0, Random.Range(0, 360), 0), TempleScript.instance.transform.parent);
        }
        else
        {
            Instantiate(Minerals_prefab[Random.Range(0,Minerals_prefab.Count())], Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(Random.Range(60, 70), this.transform.position.y, 0), Quaternion.Euler(0, Random.Range(0, 360), 0), TempleScript.instance.transform.parent);
        }
    }


    public void consume_crystal()
    {

        update_essence(Random.Range(100,800));
    }


    private void update_essence(int number)
    {

        essence_num += number;

        text_essence.text = essence_num.ToString("0000000");

       // text_essence.text = String.Format("00000", essence_num);

    }

    public void update_timer()
    {

        time += Time.deltaTime * time_speed;


        minutes = Mathf.FloorToInt(time/60);
        heures = Mathf.FloorToInt(time/3600);
        journees = Mathf.FloorToInt(time / 86400);


        if (heures > hour_tick)
        {
            _hour_passed();
            hour_tick++;
        }

        if (minutes >= 60)
        {
            minutes = minutes - (60*heures);
        }

        if (heures >= 24)
        {
            heures = heures - (24 * journees);
            if (7 - journees == 0)
                StartCoroutine(tutoriel.DernierJour());
        }


        /*
        if (heures < 12)
            text_time.text = String.Format("{00:00} : {01:00}  AM", heures,minutes);
        else
            text_time.text = String.Format("{00:00} : {01:00} PM", heures-12, minutes);
        */

        text_time.text = String.Format("{00:00}:{01:00}", heures, minutes);



        //text_day.text = ("JOURS : "+ journees);

        text_day.text = ((7-journees).ToString());

        //a mofidier (bug rotation soleil ici je pense)
        sun_light_GB.transform.rotation = Quaternion.Euler((time / 86400) * 360 - 90, 0, 0);

    }


    void _hour_passed()
    {

        _reorganize_workers();


        village_terror = Mathf.Clamp(village_terror -1,0,100);

        if (heures >= 24)
        {
            heures = heures - (24 * journees);
        }

        //if (heures == 21)
        //_sleeping_time();

        if (heures == 18)
            _worship_time();
        else if (heures == 6)
        {
            _wakeup_time();
            if (disco_builed == false && idle_workers.Count >= 10)
                _spawn_disco();
            else
                _spawn_ghost_building();
        }
        else if (heures == 14)
        {
            _spawn_ghost_building();
        }
        else if (heures == 22 && workers_ready_to_worship!=0)
        {
            StartCoroutine(worship_cycle());
            workers_ready_to_worship = 0;
        }


    }

    public void _reorganize_builders()
    {
        if (heures>5 && heures < 18)
        {
            if (building_que.Count != 0)
            {
                building_job();
            }
        }
    }

    public void _reorganize_workers()
    {
        float village_terror_ratio = village_terror / 100;

        int new_worker_count = Mathf.FloorToInt(village_terror_ratio * current_population);

        float current_worker_count = working_workers.Count;

        while(current_worker_count != new_worker_count)
        {

            if (current_worker_count<new_worker_count)
            {
                if (idle_workers[0].GetComponent<VillagerScript>().is_sleeping == false && idle_workers[0].GetComponent<VillagerScript>().is_worshiping == false)
                {
                    if(idle_workers[0].GetComponent<VillagerScript>().go_build_target != null)
                    {
                        idle_workers[0].GetComponent<VillagerScript>().go_build_target.GetComponent<GhostHouseScript>().active_builder.Remove(idle_workers[0]);
                    }
                    idle_workers[0].GetComponent<VillagerScript>()._get_to_work();
                }
                idle_workers[0].transform.tag = "Worker";
                working_workers.Add(idle_workers[0]);
                idle_workers.Remove(idle_workers[0]);
                current_worker_count++;
            }
            else
            {
                if (working_workers[0].GetComponent<VillagerScript>().is_going_to_sleep == false && working_workers[0].GetComponent<VillagerScript>().is_worshiping == false)
                {



                    if (building_que.Count != 0)
                    {
                        if (Random.value > 0.5f)
                            working_workers[0].GetComponent<VillagerScript>().go_build(building_que[0]);
                        else
                        {
                            if(disco_builed)
                                working_workers[0].GetComponent<VillagerScript>().is_dancing = true;
                            working_workers[0].GetComponent<VillagerScript>().chill_out();
                        }
                    }
                    else
                    {
                        if (Random.value > 0.5f)
                        {
                            if (disco_builed)
                                working_workers[0].GetComponent<VillagerScript>().is_dancing = true;
                        }

                        working_workers[0].GetComponent<VillagerScript>().chill_out();
                    }

                }

                working_workers[0].transform.tag = "Villager";
                idle_workers.Add(working_workers[0]);
                working_workers.Remove(working_workers[0]);
                current_worker_count--;
            }

            _reorganize_builders();

        }


    }

    void _worship_time()
    {

        worship_time = true;

        foreach (GameObject villager in working_workers.Union(idle_workers))
        {
            if(!villager.GetComponent<VillagerScript>().is_grabbed)
                villager.GetComponent<VillagerScript>().get_ready_to_worship();
        }


    }

    public void start_worshiping()
    {

        workers_ready_to_worship++;

        //Debug.Log(workers_ready_to_worship);
        //Debug.Log(current_population);
        //Debug.Log(working_workers.Union(idle_workers).Count());

        if (workers_ready_to_worship >= current_population)
        {

            foreach (GameObject villager in working_workers.Union(idle_workers))
            {
                if (villager.GetComponent<VillagerScript>().is_worshiping)
                {
                    villager.GetComponent<VillagerScript>().targetGB.SetActive(false);
                    villager.GetComponent<VillagerScript>().animator.Play("PNJ_rig_pray");
                }
                //villager.transform.GetChild(0).GetComponent<VillagerScript>().get_ready_to_worship();
            }

            //Debug.Log("start");

            StartCoroutine(worship_cycle());
            workers_ready_to_worship = 0;
        }

    }

    IEnumerator worship_cycle()
    {

        int pray_cycle = 4;

        const float pray_countdown = 2000;
        float counter = 0f;

        int worshiping_villagers = 0;

        while (pray_cycle > 0)
        {

            while (counter < pray_countdown)
            {

                counter += Time.deltaTime * time_speed;

                yield return null;

            }

            foreach (GameObject villager in working_workers.Union(idle_workers))
            {
                if (villager.GetComponent<VillagerScript>().is_worshiping)
                {
                    worshiping_villagers++;
                    villager.GetComponent<VillagerScript>().animator.Play("PNJ_rig_pray");
                }
            }

            update_essence(Mathf.FloorToInt(80 * (village_happiness/50)) * worshiping_villagers);

            //Debug.Log(Mathf.FloorToInt(10 * (village_happiness / 50)) * current_population);

            counter = 0f;
            worshiping_villagers = 0;

            pray_cycle--;

        }

        worship_time = false;
        sleep_time = true;


        //Debug.Log(homeless.Count());


        foreach (GameObject villager in working_workers.Union(idle_workers))
        {
            villager.GetComponent<VillagerScript>().animator.Play("PNJ_rig_walk");
            villager.GetComponent<VillagerScript>()._sleep();
        }

    }

    /*
    void _sleeping_time()
    {

        Debug.Log(homeless.Count());

        foreach (GameObject villager in working_workers.Union(idle_workers))
        {
            villager.transform.GetChild(0).GetComponent<VillagerScript>()._sleep();
        }


    }
    */
    void _wakeup_time()
    {

        foreach (GameObject house in current_building)
        {
            house.GetComponent<HouseScript>().light_switch(false);
        }


        sleep_time = false;

        if (disco_builed)
        {
            _disco_time();
        }

        foreach (GameObject villager in working_workers.Union(idle_workers))
        {
            villager.GetComponent<VillagerScript>()._wakeup();
        }

        _reorganize_builders();

    }

    void _disco_time()
    {

        int gota_dance = (int)MathF.Floor(idle_workers.Count / 2);
        //Debug.Log(gota_dance);


        foreach (GameObject villager in idle_workers)
        {
            if (gota_dance > 0)
            {
                villager.GetComponent<VillagerScript>().is_dancing = true;
                villager.GetComponent<VillagerScript>().chill_out();
                gota_dance--;
            }
        }
    }
    public void _disco_switch(GameObject old_dancer)
    {
        if(!worship_time && !sleep_time && disco_builed )
        { 
            GameObject new_dancer = new GameObject();
            bool more_dance = false;

            foreach (GameObject villager in idle_workers)
            {
                if (villager.GetComponent<VillagerScript>().can_dance)
                {
                    new_dancer = villager;
                    more_dance = true;
                }
            }
            if (more_dance == true)
            {

                new_dancer.GetComponent<VillagerScript>().StopAllCoroutines();
                new_dancer.GetComponent<VillagerScript>().path_complete = true;
                new_dancer.GetComponent<VillagerScript>().is_dancing = true;
                new_dancer.GetComponent<VillagerScript>().chill_out();
            }

        }

        old_dancer.GetComponent<VillagerScript>().is_dancing = false;
        old_dancer.GetComponent<VillagerScript>().chill_out();

    }


    //testing:
    public void update_time_speed(System.Single speed)
    {
        //Debug.Log("erere");
        //time_speed = speed;
        Time.timeScale = speed;
    }
    public void update_terror(System.Single terror)
    {
        Debug.Log(terror);
        village_terror = terror;
        village_happiness = math.abs(terror - 100);

    }
    public void break_rock(Transform location, int type)
    {
        GameObject mine_breaking = Instantiate(Minerals_destroy_prefab[type], location.position, location.rotation, TempleScript.instance.transform.parent);
        Destroy(mine_breaking, 20f);
    }

    public void _check_remaining_villager()
    {
        //Debug.Log(current_population);
        if (current_population <= 0)
        {
            tutoriel.PlusdeVillageois();
        }
    }


}
