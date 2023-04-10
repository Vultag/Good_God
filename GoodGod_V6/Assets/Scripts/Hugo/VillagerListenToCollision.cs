using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerListenToCollision : MonoBehaviour
{

    //private bool is_dead = false;
    [SerializeField] private GameObject blood_prefab;
    [SerializeField] public Material[] dead_mat;

    [SerializeField] private GameObject sol;


    private void Start()
    {
        sol = TempleScript.instance.sol;
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (this.GetComponent<VillagerScript>().is_grabbed == true)
        {

            if (this.GetComponent<VillagerScript>().is_dead == false && (collision.collider.tag == "sol" | collision.collider.tag == "Maison"))
            {

                //Debug.Log(collision.impulse.magnitude);

                if (collision.impulse.magnitude > 10)
                {

                    GameObject blood = Instantiate(blood_prefab, new Vector3(this.transform.position.x, -5.8500f, this.transform.position.z), Quaternion.identity, TempleScript.instance.transform.parent);
                    Destroy(blood, 16f);

                    this.GetComponent<VillagerScript>().die();


                }
                else
                {
                    //Debug.Log("col_light");

                    Physics.IgnoreCollision(this.GetComponent<CapsuleCollider>(), sol.GetComponent<MeshCollider>(), true);

                    this.GetComponent<VillagerScript>().Temple.GetComponent<TempleScript>().village_terror = Mathf.Clamp(this.GetComponent<VillagerScript>().Temple.GetComponent<TempleScript>().village_terror + 1f, 0, 100);
                    this.GetComponent<Rigidbody>().isKinematic = true;
                    this.GetComponent<VillagerScript>().is_grabbed = false;
                    this.GetComponent<VillagerNavPathScript>().enabled = true;
                    this.GetComponent<VillagerScript>().NavMeshAgent.enabled = true;
                    this.GetComponent<VillagerScript>()._get_to_work();
                }
            }
        }

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "LimiteCiel" && this.GetComponent<VillagerScript>().is_dead == false)
        {
            this.GetComponent<VillagerScript>().die();
        }
    }

}
