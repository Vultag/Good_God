using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class VillagerDetectorScript : MonoBehaviour
{

    public GameObject carring_crystal;
    //public bool is_looking_for_tree;
    //public bool is_looking_to_unload;
    [HideInInspector] public bool building_detected = false;


    private void OnTriggerEnter(Collider other)
    {

        //desactiver le collider si pas en train de travailler

        //if (other.gameObject.tag == "Crystal_targeted")
        {

            if (this.transform.parent.GetComponent<VillagerScript>().target_ressource == other.gameObject)
            {

                this.transform.parent.GetComponent<VillagerScript>()._chop_ressource();
                /*
                if (other.gameObject.tag == "Mineral_targeted")
                    this.transform.parent.GetComponent<VillagerScript>().animator.Play("PNJ_rig_miner");
                if (other.gameObject.tag == "Tree_targeted")
                    this.transform.parent.GetComponent<VillagerScript>().animator.Play("PNJ_rig_bucheron");
                */

                /*
                carring_crystal = other.gameObject;

                carring_crystal.tag = "Untagged";

                this.transform.parent.GetComponent<VillagerScript>().is_looking_for_ressouce = false;

                this.transform.parent.GetComponent<VillagerScript>().is_looking_to_unload = true;

                transform.parent.GetComponent<VillagerScript>()._ReturnToTemple();
                
                other.transform.parent = this.transform.parent;
                */

            }
        }
        if (other.gameObject.tag == "Temple")
        {

            if (this.transform.parent.GetComponent<VillagerScript>().is_looking_to_unload)
                transform.parent.GetComponent<VillagerScript>()._UnloadCargo();
            //transform.parent.GetComponent<VillagerScript>()._UnloadCargo(carring_crystal);

        }
        /*
        if (other.gameObject == this.transform.parent.GetComponent<VillagerScript>().targetGB)
        {
            this.transform.parent.GetComponent<VillagerScript>().target_reached();
        }
        */
        if (other.gameObject == this.transform.parent.GetComponent<VillagerScript>().go_build_target && building_detected == false)
        {
            building_detected = true;
            this.transform.parent.GetComponent<VillagerScript>().start_building();
        }

    }

}
