using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class VillagerScript : MonoBehaviour
{

    [HideInInspector] public bool is_working;
    [HideInInspector] public bool is_worshiping;
    [HideInInspector] public bool is_dancing;
    [HideInInspector] public bool can_dance = true;
    [HideInInspector] public bool is_ready_to_worship;
    [HideInInspector] public bool is_going_to_sleep;
    [HideInInspector] public bool is_sleeping;
    [HideInInspector] public bool is_idle;
    [HideInInspector] public bool is_terrified;
    [HideInInspector] public bool is_grabbed = false;
    [HideInInspector] public bool is_dead;

    [HideInInspector]public float boost = 1;

    [HideInInspector] public bool path_complete = true;


    public GameObject Temple;
    public Animator animator;
    private GameObject sol;

    public GameObject targetGB;
    [HideInInspector] public GameObject home; 
    [HideInInspector] public GameObject go_build_target;

    [HideInInspector] public NavMeshAgent NavMeshAgent;


    [HideInInspector]public bool is_looking_for_ressouce = false;
    [HideInInspector] public bool is_looking_to_unload = false;

    [HideInInspector] public GameObject target_ressource;

    private Tutoriel tutoriel;
    public GameObject mesh;



    void Start()
    {

        sol = TempleScript.instance.sol;

        Physics.IgnoreCollision(this.GetComponent<CapsuleCollider>(), sol.GetComponent<MeshCollider>());

        targetGB.transform.position = Vector3.zero;

        animator = GetComponent<Animator>();

        animator.Play("PNJ_rig_walk");

        NavMeshAgent = GetComponent<NavMeshAgent>();


        Temple = TempleScript.instance.gameObject;

        if (!Temple.GetComponent<TempleScript>().sleep_time)
            StartCoroutine(idle());
        else
            _sleep();

        tutoriel = Temple.GetComponent<TempleScript>().tutoriel;


    }

    private void Update()
    {
        /*
        if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
        {
            
            if(!path_complete)
            {
                if (!NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f)
                {

                }
            }

        }
        */

        /*
        if (!NavMeshAgent.pathPending)
        {
            if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
            {
                if (!NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("path_complete");
                }
            }
        }
        */
    }


    IEnumerator idle()
    {

        animator.Play("PNJ_rig|iddle");

        yield return new WaitForSeconds(Random.Range(1, 7));

        
        if (is_working == false)

            relocate();
        


    }
    IEnumerator watch_for_ressources()
    {


        animator.Play("PNJ_rig_Recherche");

        yield return new WaitForSeconds(Random.Range(1, 4));


        _SearchForRessouce();



    }
    void relocate_far_away()
    {


        animator.Play("PNJ_rig_walk");

        //targetGB.transform.position = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(Random.Range(40, 100), Temple.transform.position.y, 0);
        if(is_grabbed == false)
            NavMeshAgent.destination = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(Random.Range(40, 100), Temple.transform.position.y, 0);//targetGB.transform.position;

        path_complete = false;

    }

    void relocate()
    {


        animator.Play("PNJ_rig_walk");

        //targetGB.transform.position = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(Random.Range(6, 12), Temple.transform.position.y, 0);
        if (is_grabbed == false)
            NavMeshAgent.destination = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(Random.Range(6, 12), Temple.transform.position.y, 0);//targetGB.transform.position;

        path_complete = false;

    }



    /*
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == targetGB)
        {
            Debug.Log(targetGB.gameObject.transform.parent.name);
            target_reached();
        }

    }
    */

    public void target_reached()
    {

        //mettre au propre si le temps c un enfer

        if(is_terrified)
        {
            _get_to_work();
            is_terrified = false;
            return;
        }

        if(is_going_to_sleep)
        {
            if (home != null)
            {
                this.GetComponent<CapsuleCollider>().enabled = false;
                mesh.SetActive(false);
                if (!home.GetComponent<HouseScript>().lights_on)
                    home.GetComponent<HouseScript>().light_switch(true);
            }
            is_sleeping = true;

            //sleep
            targetGB.SetActive(false);

            is_sleeping = true;

            animator.Play("PNJ_rig_sleep");

            NavMeshAgent.updateRotation = false;
            NavMeshAgent.updatePosition = false;

            return;
        }

        //switch en fonctio du state
        if (!is_worshiping)
        { 

            if(Temple.GetComponent<TempleScript>().sleep_time)
            {

                //sleep
                /*
                targetGB.SetActive(false);

                is_sleeping = true;

                animator.Play("PNJ_rig_sleep");

                NavMeshAgent.updateRotation = false;
                NavMeshAgent.updatePosition = false;
                */
            }
            else
            {

                if (!is_working)
                {
                    if(!is_dancing)
                        StartCoroutine(idle());
                    else
                    {
                        //if(TempleScript.instance.disco_builed)
                        Debug.Log(TempleScript.instance.disco_builed);
                            StartCoroutine(dancing());
                    }

                }
                else
                    StartCoroutine(watch_for_ressources());
            }
        }
        else
        {
            targetGB.SetActive(false);

            animator.Play("PNJ_rig|iddle");

            NavMeshAgent.updateRotation = false;

            var direction = (Temple.transform.position - transform.position);

            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            Temple.GetComponent<TempleScript>().start_worshiping();

        }


    }

    public IEnumerator dancing()
    {


        if (go_build_target)
        {
            go_build_target.GetComponent<GhostHouseScript>().active_builder.Remove(this.gameObject);
            go_build_target = null;
        }

        if (target_ressource != null)
        {

            if (target_ressource.tag == "Mineral_targeted")
                target_ressource.tag = "Mineral";

            if (target_ressource.tag == "Tree_targeted")
                target_ressource.tag = "Tree";

            target_ressource = null;
        }

        int dancing_iteration = Random.Range(5, 9);

        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updatePosition = false;

        var direction = (Temple.GetComponent<TempleScript>().DiscoGB.transform.position - transform.position);

        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        while (dancing_iteration>0)
        {

            if (Random.value > 0.5f)
                animator.Play("PNJ_rig_dance_des_bras");
            else
                animator.Play("PNJ_rig_dance_tourni");

            yield return new WaitForSeconds(Random.Range(1f, 7f));

            Temple.GetComponent<TempleScript>().village_terror = Mathf.Clamp(Temple.GetComponent<TempleScript>().village_terror - 0.5f,0,100);

            dancing_iteration--;

        }

        Temple.GetComponent<TempleScript>()._disco_switch(this.gameObject);

    }

    public void _get_to_work()
    {
        if (go_build_target)
        {
            go_build_target.GetComponent<GhostHouseScript>().active_builder.Remove(this.gameObject);
            go_build_target = null;
        }

        path_complete =true;

        NavMeshAgent.updateRotation = true;
        NavMeshAgent.updatePosition = true;

        is_dancing =false;
        is_idle = false;
        is_working = true;
        is_worshiping = false;

        StopCoroutine(idle());

        targetGB.SetActive(false);
        targetGB.transform.position = Vector3.zero;

        _SearchForRessouce();

        //idle_workers.Remove(villager);

    }

    public void chill_out()
    {

        if (target_ressource != null)
        {

            if (target_ressource.tag == "Mineral_targeted")
                target_ressource.tag = "Mineral";

            if (target_ressource.tag == "Tree_targeted")
                target_ressource.tag = "Tree";

            target_ressource = null;
        }

        if (is_dancing == false)
        {
            //Debug.Log("nodance");
            is_idle = true;
            is_working = false;


            NavMeshAgent.updateRotation = true;
            NavMeshAgent.updatePosition = true;



            if (is_looking_to_unload)
            {
                _DropCargo();
            }

            targetGB.SetActive(true);

            relocate();
        }
        else
        {

            if (is_looking_to_unload)
            {
                _DropCargo();
            }

            targetGB.SetActive(false);

            //targetGB.transform.position = Temple.GetComponent<TempleScript>().DiscoGB.transform.position;

            Vector3 dance_location = new Vector3(Temple.GetComponent<TempleScript>().disco_spawn.position.x + Random.Range(-5f, 5f), Temple.GetComponent<TempleScript>().disco_spawn.position.y, Temple.GetComponent<TempleScript>().disco_spawn.position.z + Random.Range(-5f, 5f) + 2.5f);
           
            if (is_grabbed == false)
                NavMeshAgent.destination = dance_location;

            path_complete = false;


            can_dance = false;

            animator.Play("PNJ_rig_walk");

            is_idle = true;
            is_working = false;

            NavMeshAgent.updateRotation = true;
            NavMeshAgent.updatePosition = true;




        }


    }




    public void _UnloadCargo()
    {

        //cargo.transform.parent = Reserve.transform;


        is_looking_to_unload = false;

        //Destroy(cargo);

        Temple.GetComponent<TempleScript>().consume_crystal();

        _SearchForRessouce();

    }

    public void _DropCargo()
    {

        //GameObject cargo = this.transform.GetChild(0).GetComponent<VillagerDetectorScript>().carring_crystal;

        //cargo.transform.parent = null;

        is_looking_to_unload = false;


    }



    public static GameObject GetClosestRessouce(Vector3 from)
    {

        float shortest_path_to_ressource = 99999f;
        GameObject target_ressource = null;

        foreach (var ressource in GameObject.FindGameObjectsWithTag("Tree").Concat(GameObject.FindGameObjectsWithTag("Mineral")))
        {

            if ((ressource.transform.position - from).magnitude < shortest_path_to_ressource)
            {
                shortest_path_to_ressource = ressource.transform.position.magnitude;
                target_ressource = ressource;
            }

        }


        return target_ressource;

    }

    public void _SearchForRessouce()
    {

        //is_looking_for_ressouce = true;

        if (target_ressource == null)
            target_ressource = GetClosestRessouce(this.transform.position);

        if (target_ressource != null)
        {
            if (target_ressource.tag == "Mineral")
                target_ressource.tag = "Mineral_targeted";

            if (target_ressource.tag == "Tree")
                target_ressource.tag = "Tree_targeted";

            //is_looking_for_ressouce = false;

            //List<GameObject> other_workers = new List<GameObject>();

            //other_workers.AddRange(GameObject.FindGameObjectsWithTag("Worker"));

            /*
            foreach (var workers in other_workers)
            {
                if (workers.transform.GetChild(0).GetComponent<VillagerScript>().is_looking_for_ressouce == true)
                    workers.transform.GetChild(0).GetComponent<VillagerScript>()._SearchForRessouce();

            }
            */
            if (is_grabbed == false)
                NavMeshAgent.destination = target_ressource.transform.position;

            NavMeshAgent.updateRotation = true;

            animator.Play("PNJ_rig_walk");
        }
        else
        {

            relocate_far_away();

        }
    }

    public void _chop_ressource()
    {

        NavMeshAgent.updateRotation = false;

        if (is_grabbed == false)
            NavMeshAgent.destination = this.transform.position;


        if(target_ressource.tag == "Mineral_targeted")
        {

            animator.Play("PNJ_rig_miner");

        }
        if (target_ressource.tag == "Tree_targeted")
        {

           animator.Play("PNJ_rig_bucheron");

        }



    }

    public void _chop_event()
    {


        if(target_ressource!= null)
            target_ressource.GetComponent<Animator>().Play("Shake_event");

        /*
        if (target_ressource.tag == "Mineral_targeted")
        {


        }
        if (target_ressource.tag == "Tree_targeted")
        {


        }
        */


    }

    public void _awareness_trigger(Vector3 point)
    {

        mesh.SetActive(true);

        is_sleeping= false;
        is_dancing = false;
        NavMeshAgent.updatePosition = true;
        NavMeshAgent.updateRotation = true;
        is_worshiping = false;

        this.GetComponent<CapsuleCollider>().enabled= true;

        if (go_build_target!= null)
        {
            go_build_target.GetComponent<GhostHouseScript>().active_builder.Remove(this.gameObject);
            go_build_target = null;
        }

        //Debug.Log((this.transform.position - point).magnitude);
        animator.Play("PNJ_rig_Peur");
        Debug.DrawLine(new Vector3(point.x, point.y, point.z), new Vector3(this.transform.position.x, point.y, this.transform.position.z), Color.red,5f);
        if(is_working == false)
            Temple.GetComponent<TempleScript>().village_terror = Mathf.Clamp(Temple.GetComponent<TempleScript>().village_terror + (100f / Temple.GetComponent<TempleScript>().current_population),0,100);
        else
            Temple.GetComponent<TempleScript>().village_terror = Mathf.Clamp(Temple.GetComponent<TempleScript>().village_terror + (30f / Temple.GetComponent<TempleScript>().current_population), 0, 100);

        is_terrified = true;

        //NavMeshAgent.destination = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(this.transform.position.x + 12f - Vector3.Distance(new Vector3(point.x, 0, point.z), new Vector3(this.transform.position.x, 0, this.transform.position.z)), Temple.transform.position.y, 0);//targetGB.transform.position;
        if (is_grabbed == false)
            NavMeshAgent.destination = this.transform.position - ((this.transform.position - point).normalized * (-12 +(this.transform.position - point).magnitude));

        path_complete = false;

        //_get_to_work();

    }

    


    public void _ReturnToTemple()
    {

        //GameObject cargo = this.transform.GetChild(0).GetComponent<VillagerDetectorScript>().carring_crystal;
        if(target_ressource.GetComponent<Rigidbody>().angularDrag<0.1f)
            Temple.GetComponent<TempleScript>().respawn_ressource();

        if (target_ressource.tag == "Mineral_targeted")
        {
            Temple.GetComponent<TempleScript>().break_rock(target_ressource.transform, (int)target_ressource.GetComponent<Rigidbody>().mass -1);
            Temple.GetComponent<TempleScript>().break_rock(target_ressource.transform, (int)target_ressource.GetComponent<Rigidbody>().mass -1);
            Destroy(target_ressource.transform.parent.gameObject);
        }

        if (target_ressource.tag == "Tree_targeted")
        {
            target_ressource.GetComponent<Animator>().enabled = false;
            target_ressource.GetComponent<Rigidbody>().isKinematic = false;
            target_ressource.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Destroy(target_ressource.transform.parent.gameObject, 20f);

        }


        target_ressource = null;

        is_looking_to_unload = true;

        if (is_grabbed == false)
            NavMeshAgent.destination = Temple.transform.position;

        animator.Play("PNJ_rig|walk_with_cristal");
        NavMeshAgent.updateRotation = true;

    }

    public void go_build(GameObject building) 
    {
        path_complete = true;
        this.transform.GetChild(0).GetComponent<VillagerDetectorScript>().building_detected = false;

        animator.Play("PNJ_rig_walk");

        go_build_target = building;

        if (is_grabbed == false)
            NavMeshAgent.destination = building.transform.position;

        NavMeshAgent.updateRotation = true;

    }
    public void start_building()
    {

        NavMeshAgent.destination = this.transform.position;
        NavMeshAgent.updateRotation = false;


        animator.Play("PNJ_rig_build_with_2hammers");

        var direction = (go_build_target.transform.position - transform.position);

        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        go_build_target.GetComponent<GhostHouseScript>()._getting_to_build(this.gameObject);

    }

    public void die()
    {
        if (is_dead == false)
        {

            is_dead = true;

            path_complete = true;

            if (go_build_target != null)
            {
                go_build_target.GetComponent<GhostHouseScript>().active_builder.Remove(this.gameObject);
                go_build_target = null;
            }

            if (home != null)
                home.GetComponent<HouseScript>()._update_house_habitant(this.gameObject);
            else
            {
                Temple.GetComponent<TempleScript>().homeless.Remove(this.gameObject);//pas de parent ?
            }

            if (this.gameObject.tag == "Villager")
            {
                Temple.GetComponent<TempleScript>().idle_workers.Remove(this.gameObject);
            }
            else if (this.gameObject.tag == "Worker")
            {
                Temple.GetComponent<TempleScript>().working_workers.Remove(this.gameObject);
            }

            mesh.GetComponent<Renderer>().material = this.GetComponent<VillagerListenToCollision>().dead_mat;

            Temple.GetComponent<TempleScript>().current_population--;

            Temple.GetComponent<TempleScript>()._check_remaining_villager();

            Temple.GetComponent<TempleScript>().current_population_slide.text = Temple.GetComponent<TempleScript>().current_population.ToString();

            if (target_ressource != null)
            {

                if (target_ressource.tag == "Mineral_targeted")
                    target_ressource.tag = "Mineral";

                if (target_ressource.tag == "Tree_targeted")
                    target_ressource.tag = "Tree";

                target_ressource = null;
            }

            Temple.GetComponent<TempleScript>().village_terror = Mathf.Clamp(Temple.GetComponent<TempleScript>().village_terror + 5f, 0, 100);

            //this.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.GetChild(1).gameObject.SetActive(false);
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;

            animator.Play("PNJ_rig_electrification");

            Destroy(this.transform.parent.gameObject, 6f);

            this.GetComponent<VillagerScript>().enabled = false;
        }


        /*
        if (Temple.GetComponent<TempleScript>().worship_time == true)
            Temple.GetComponent<TempleScript>().start_worshiping();
        */
    }

    public void get_ready_to_worship()
    {

        //path_complete = false;

        StopAllCoroutines();
        is_dancing = false;

        if (go_build_target != null)
        {
            go_build_target.GetComponent<GhostHouseScript>().active_builder.Remove(this.gameObject);
            go_build_target = null;
        }

        animator.Play("PNJ_rig_walk");

        NavMeshAgent.updateRotation = true;
        NavMeshAgent.updatePosition = true;

        is_idle = false;
        is_working = false;
        is_worshiping = true;

        if (target_ressource != null)
        {

            if (target_ressource.tag == "Mineral_targeted")
                target_ressource.tag = "Mineral";

            if (target_ressource.tag == "Tree_targeted")
                target_ressource.tag = "Tree";

            target_ressource = null;
        }

        if (is_looking_to_unload)
        {
            _DropCargo();
        }

        targetGB.SetActive(true);

        relocate();

    }

    public void _sleep()
    {


        animator.Play("PNJ_rig_walk");

        path_complete = false;

        is_dancing = false;
        is_worshiping = false;
        is_going_to_sleep = true;


        NavMeshAgent.updateRotation = true;

        if (target_ressource != null)
        {
            if (target_ressource.tag == "Mineral_targeted")
                target_ressource.tag = "Mineral";

            if (target_ressource.tag == "Tree_targeted")
                target_ressource.tag = "Tree";
            target_ressource = null;
        }


        if (is_looking_to_unload)
        {
            _DropCargo();
        }

        if (home != null)
        {

            Physics.IgnoreLayerCollision(9, 9, true);

            this.NavMeshAgent.radius = 0.01f;


            targetGB.SetActive(false);
            targetGB.transform.position = Vector3.zero;

            if (is_grabbed == false)
                NavMeshAgent.destination = home.transform.position;
        }
        else
        {

            Physics.IgnoreLayerCollision(9, 9, false);

            relocate();

        }

    }
    public void _wakeup()
    {

        mesh.SetActive(true);

        this.GetComponent<CapsuleCollider>().enabled = true;
        Physics.IgnoreLayerCollision(9, 9, false);

        can_dance = true;

        this.NavMeshAgent.radius = 0.5f;

        NavMeshAgent.updatePosition = true;
        NavMeshAgent.updateRotation = true;

        this.GetComponent<CapsuleCollider>().enabled = true;

        is_worshiping = false;
        is_going_to_sleep = false;
        is_sleeping = false;

        if (target_ressource != null)
        {

            if (target_ressource.tag == "Mineral_targeted")
                target_ressource.tag = "Mineral";

            if (target_ressource.tag == "Tree_targeted")
                target_ressource.tag = "Tree";

            target_ressource = null;
        }

        if (this.gameObject.tag == "Worker")
        {
            _get_to_work();
        }
        else
        {
            if(Temple.GetComponent<TempleScript>().building_que.Count ==0)
            {
                chill_out();
            }
        }


        targetGB.SetActive(true);

    }


    public void grabbed_by_player(bool state)
    {
        //disable nav_path script


        this.GetComponent<CapsuleCollider>().enabled = !state;


        //this.GetComponent<Rigidbody>().isKinematic = !state;
        if (state)
        {

            path_complete = true;

            if (go_build_target != null)
            {
                go_build_target.GetComponent<GhostHouseScript>().active_builder.Remove(this.gameObject);
                go_build_target = null;
            }

            if (target_ressource != null)
            {

                if (target_ressource.tag == "Mineral_targeted")
                    target_ressource.tag = "Mineral";

                if (target_ressource.tag == "Tree_targeted")
                    target_ressource.tag = "Tree";

                target_ressource = null;
            }

            if (is_looking_to_unload)
            {
                _DropCargo();
            }

            animator.Play("PNJ_rig_fly");
            is_grabbed = true;
            this.GetComponent<VillagerNavPathScript>().enabled = false;
            NavMeshAgent.enabled = false;



        }
        else
        {
            //Debug.Log("release");

            this.GetComponent<Rigidbody>().isKinematic = false;

            Physics.IgnoreCollision(this.GetComponent<CapsuleCollider>(), sol.GetComponent<MeshCollider>(),false);
            this.GetComponent<VillagerListenToCollision>().enabled = true;

        }

    }

    public void Boosted()
    {

        boost += 0.03f;

        if(boost <= 1.03f)
        {
            StartCoroutine(boost_effect());
        }
;

        if (boost>3)
        {
            overdose();
        }


    }

    IEnumerator boost_effect()
    {


        while (boost > 1)
        {
            boost -= 0.001f;

            //Debug.Log(boost);

            animator.speed = boost;
            NavMeshAgent.speed = 3.5f * boost;

            this.GetComponent<bonhomme_boost>().update_boost_render(boost-1);


            yield return new WaitForFixedUpdate();

        }

        this.GetComponent<bonhomme_boost>().update_boost_render(0);
        boost = 1;
        animator.speed = 1;
        NavMeshAgent.speed = 3.5f;

    }

    public void overdose()
    {

        if (!is_dead)
        {

            path_complete = true;

            is_dead = true;

            this.GetComponent<CapsuleCollider>().enabled = false;

            Debug.Log("overdose");

            StopAllCoroutines();

            animator.Play("PNJ_rig_fly");
            this.GetComponent<VillagerNavPathScript>().enabled = false;
            NavMeshAgent.enabled = false;

            XRGrabInteractable Grab = this.GetComponent<XRGrabInteractable>();
            Grab.enabled = false;
            // transform.Translate(0, 2 * Time.deltaTime, 0);
            //transform.position = new Vector3(0, transform.position.y + 2 * Time.deltaTime, 0);
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;



            Temple.GetComponent<TempleScript>().current_population--;
            if (Temple.GetComponent<TempleScript>().current_population < 1 && tutoriel.tuto >= 6)
                tutoriel.PlusdeVillageois();

            if (target_ressource != null)
            {

                if (target_ressource.tag == "Mineral_targeted")
                    target_ressource.tag = "Mineral";

                if (target_ressource.tag == "Tree_targeted")
                    target_ressource.tag = "Tree";

                target_ressource = null;
            }

            Temple.GetComponent<TempleScript>().village_terror = Mathf.Clamp(Temple.GetComponent<TempleScript>().village_terror + 5f, 0, 100);

            this.transform.GetChild(1).gameObject.SetActive(false);
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;

            if (go_build_target != null)
            {
                go_build_target.GetComponent<GhostHouseScript>().active_builder.Remove(this.gameObject);
                go_build_target = null;
            }

            if (home != null)
                home.GetComponent<HouseScript>()._update_house_habitant(this.gameObject);
            else
            {
                Temple.GetComponent<TempleScript>().homeless.Remove(this.gameObject);//pas de parent ?
            }

            if (this.gameObject.tag == "Villager")
            {
                Temple.GetComponent<TempleScript>().idle_workers.Remove(this.gameObject);
            }
            else if (this.gameObject.tag == "Worker")
            {
                Temple.GetComponent<TempleScript>().working_workers.Remove(this.gameObject);
            }

            this.GetComponent<VillagerScript>().enabled = false;


            rb.AddForce(0, 500f, 0);


        }
    }


}
