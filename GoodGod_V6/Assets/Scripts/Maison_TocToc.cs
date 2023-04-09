using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Maison_TocToc : MonoBehaviour
{
    public AudioSource TocAudio;
    //public int TocToc;
    // Start is called before the first frame update
   
    void Start()
    {
     TocAudio = this.GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        //if (TocToc >=1) VillageoisReveil(); 
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Rhand" && AnimateHand.RPoingFerme)
        {
            TocAudio.Play();
            foreach (GameObject villager in this.GetComponent<HouseScript>().habitant)
            {
                if (villager.GetComponent<VillagerScript>().is_sleeping)
                {
                    villager.GetComponent<VillagerScript>()._wakeup();
                    TempleScript.instance.GetComponent<TempleScript>().village_terror = Mathf.Clamp(TempleScript.instance.village_terror + 3, 0, 100);
                }
            }

        }
        if (other.tag == "Lhand" && AnimateHand.LPoingFerme)
        {
            TocAudio.Play();
            foreach (GameObject villager in this.GetComponent<HouseScript>().habitant)
            {
                if (villager.GetComponent<VillagerScript>().is_sleeping)
                {
                    villager.GetComponent<VillagerScript>()._wakeup();
                    TempleScript.instance.GetComponent<TempleScript>().village_terror = Mathf.Clamp(TempleScript.instance.village_terror + 3, 0, 100);
                }
            }
        }
    }
    public void VillageoisReveil()
    {
        //le villageois se reveille
        Debug.Log("Le villageois se réveille pas content");
    }

}
