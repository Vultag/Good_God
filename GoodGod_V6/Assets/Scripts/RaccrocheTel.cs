using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RaccrocheTel : MonoBehaviour
{
    public Tutoriel Tuto;
    public bool TelDeccroche;
    public Collider SocleTel;
    public AudioSource RaccrocheSon;

    //[HideInInspector] public XRBaseInteractor grabbing_hand;

    private Transform base_parent;


     void Start()
    {
        TelDeccroche = false;
        base_parent = this.transform.parent;
    }

    //Si le joueur a décroché le téléphone et qu'il le remet sur le socle alors il raccroche
    private void OnTriggerEnter(Collider other)
    {
        if (other == SocleTel) if (Tuto.tuto > 0 && TelDeccroche == true && Tuto.fin == 0)
            {

                RaccrocheSon.Play();
                if (Tuto.monologue_actif)
                {
                    //grabbing_hand.allowSelect = false;
                    this.transform.parent = base_parent;
                    this.GetComponent<respawnObject>().Respawn();
                    Tuto.RaccrocheTelephone();
                }

            }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == SocleTel) StartCoroutine(WaitDeccroche(0));
    }

    public IEnumerator WaitDeccroche(float time)
    {
        while (time < 3)
        {
            time++;
            yield return new WaitForSeconds(1);
        }
        TelDeccroche = true;
    }
}
