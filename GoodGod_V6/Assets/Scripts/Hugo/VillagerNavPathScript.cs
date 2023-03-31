using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerNavPathScript : MonoBehaviour
{

    private NavMeshAgent NavMeshAgent;



    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

    }


    void Update()
    {
        if (!NavMeshAgent.pathPending)
        {
            if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
            {
                if (!NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    if (!this.GetComponent<VillagerScript>().path_complete)
                    {
                        //Debug.Log("path_complete");
                        this.GetComponent<VillagerScript>().target_reached();
                        this.GetComponent<VillagerScript>().path_complete = true;
                    }
                }
            }
        }

    }
}
