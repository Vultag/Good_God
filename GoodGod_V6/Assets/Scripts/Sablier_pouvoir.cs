using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;


//[RequireComponent(typeof(ParticleSystem))]
public class Sablier_pouvoir : MonoBehaviour
{
    public GameObject DetectUp, DetectDown, Audio;
    public float UP, DOWN,valeur;
    public ParticleSystem Sable;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // VALEUR ENTRE 0 ET 4
        UP = DetectUp.transform.position.y;
        DOWN = DetectDown.transform.position.y; 
        //Debug.Log("valeur : "+ valeur);

        valeur = (DOWN - UP);
        var em = Sable.emission;
        if (valeur > 0)
        {
            Audio.gameObject.SetActive(true);
            em.rateOverTime = valeur * 10;
        }
        else
        {
            Audio.gameObject.SetActive(false);
            em.rateOverTime = 0;
        }
    

    }




}
