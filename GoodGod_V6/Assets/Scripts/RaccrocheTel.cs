using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccrocheTel : MonoBehaviour
{
    public Tutoriel Tuto;
    public bool TelDeccroche;
    public Collider SocleTel;

     void Start()
    {
        TelDeccroche = false;
    }
    //Si le joueur a décroché le téléphone et qu'il le remet sur le socle alors il raccroche
    private void OnTriggerEnter(Collider other)
    {
        if (other == SocleTel) if (Tuto.tuto > 0 && TelDeccroche == true) Tuto.RaccrocheTelephone();
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
