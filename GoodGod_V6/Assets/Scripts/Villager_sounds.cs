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
        VillagerSource.loop = false;
        VillagerSource.clip = VillagerSounds[5];
        //VillagerSource.PlayDelayed(Random.Range(1,5));
        VillagerSource.Play();
    }

    public void SonVillageoisPeur()
    {
        // Quand ils courent partout en hurlant
        VillagerSource.loop = true;
        VillagerSource.clip = VillagerSounds[Random.Range(3,5)];
        VillagerSource.Play();

    }
    public void SonVillageoisDance()
    {
        // Quand ils courent partout en hurlant
        VillagerSource.loop = true;
        VillagerSource.clip = VillagerSounds[6];
        VillagerSource.PlayDelayed(Random.Range(0, 2));

    }

    public void SonVillageoisTriste()
    {
        // Quand ils courent partout en hurlant
        VillagerSource.loop = false;
        VillagerSource.clip = VillagerSounds[2];
        VillagerSource.Play();

    }
    /*
    public void SonVillageoisGrabbed()
    {
        //Quand un jour le prend
        VillagerSource.clip = VillagerSounds[3];
        VillagerSource.loop = true;
        VillagerSource.Play();
    }
    */
    public void SonVillageoisHappy()
    {
        //Quand le villageois fait sa vie (à appeler quand il sort de la maison, ou quand le joueur le relache sur le sol)
        VillagerSource.loop = false;
        VillagerSource.clip = VillagerSounds[Random.Range(0, 2)];
        VillagerSource.Play();
    }

    public void StopSon()
    {
        VillagerSource.Stop();
    }


}
