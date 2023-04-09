using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager_sounds : MonoBehaviour
{
    public AudioSource VillagerSource;
    public AudioClip[] VillagerSounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    public void SonVillageoisBaille()
    {
        // a appeler quand il rentre dans la maison pour dormir
        VillagerSource.clip = VillagerSounds[5];
    }

    public void SonVillageoisPeur()
    {
        // Quand ils courent partout en hurlant
        VillagerSource.clip = VillagerSounds[Random.Range(1,3)];

    }
    public void SonVillageoisGrabbed()
    {
        //Quand un jour le prend
        VillagerSource.clip = VillagerSounds[3];
    }
    public void SonVillageoisOther()
    {
        //Quand le villageois fait sa vie (à appeler quand il sort de la maison, ou quand le joueur le relache sur le sol)
        VillagerSource.clip = VillagerSounds[Random.Range(0, 2)];
    }
}
