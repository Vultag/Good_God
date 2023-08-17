using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Events_manager : MonoBehaviour
{
    public float randomNum1, randomNum2, EvNuages, TestColor, alpha, EvCristaux, EvRandom, NbCristaux;
    public static bool EventInProgress, Desastre;
    public GameObject[] NuageNoir, eclairs, CristauxEvent;
    public Material NuageNoirMat;
    public Renderer RenderNuage;
    public Color CouleurFade;
    public GameObject cristal;
    public Tutoriel Tutoriel;
    //public Tutoriel Tutoriel;

    public Tutoriel tutoriel;


    // Start is called before the first frame update
    void Start()
    {
        EvNuages = 1666;
        EvCristaux = 166;

    }
    private void FixedUpdate()
    {
        randomNum1 = Random.Range(1, 10500);
        randomNum2 = Random.Range(1, 500);
        if(randomNum2 == 150 && tutoriel.fin == 0) UnCristal();


        if (EventInProgress == false && tutoriel.tuto >= 6 && tutoriel.fin == 0 && !tutoriel.TOO_GOOD_in_progress && TempleScript.instance.GetComponent<TempleScript>().journees > 0)
        {
            if (randomNum1 == EvCristaux)
            {
                tutoriel.EVENT_in_progress = true;
                EventCristaux();
            }
            if (randomNum1 == EvNuages)
            {
                tutoriel.EVENT_in_progress = true;
                EventNuages();
            }

        }
        /*
        if (randomNum1 == EvCristaux && EventInProgress == false && tutoriel.tuto >= 6 && tutoriel.fin == 0 && !tutoriel.TOO_GOOD_in_progress)
        {
            tutoriel.EVENT_in_progress = true;
            EventCristaux();
        }
        else if (randomNum1 == EvNuages && EventInProgress == false && tutoriel.tuto >= 6 && tutoriel.fin == 0 && !tutoriel.TOO_GOOD_in_progress)
        {
            tutoriel.EVENT_in_progress = true;
            EventNuages();
        }
        */
    }
     
    // Update is called once per frame
    void Update()
    {

        if (EventInProgress == true)
        {
            CouleurFade = new Color(0.2f, 0.2f, 0.2f, alpha);
            NuageNoirMat.color = CouleurFade;
            foreach (GameObject n in NuageNoir) n.transform.Translate(Vector3.forward * Time.deltaTime);      
        }
    }
    void UnCristal()
    {
        StartCoroutine(AppUnCristal());
    }
    void EventCristaux()
    {   

        tutoriel.EVENT_in_progress = true;
        if (Tutoriel.EvCristaux == 0) tutoriel.TutoCristaux();
        Debug.Log("Event des cristaaaaaaux !!!");
        EventInProgress = true;
        TempleScript.instance.GetComponent<YogaScript>().activate_yoga_hand(0, false);
        StartCoroutine(ApparitionCristaux());

    }

    void EventNuages()
    {
        tutoriel.EVENT_in_progress = true;
        if (Tutoriel.EvNuage == 0) tutoriel.TutorielNuage();
        Debug.Log("Event des nuages !!!");
        foreach (GameObject n in NuageNoir)
        {
            n.transform.Rotate(0, Random.Range(100f, -100f), 0);
            n.transform.position = new Vector3(Random.Range(20f, -20f), 35f, Random.Range(20f, -20f));
          
            n.gameObject.SetActive(true);
            EventInProgress = true;
            TempleScript.instance.GetComponent<YogaScript>().activate_yoga_hand(0, false);
            StartCoroutine(FadeNuage());
            if (Tutoriel.EvNuage == 0) Tutoriel.EvNuage = 1;
        }
    }
    public IEnumerator AppUnCristal()
    {
        GameObject Cristaux = Instantiate(cristal, new Vector3(Random.Range(100f, -100f), Random.Range(250f, 400f), Random.Range(100f, -100f)), Quaternion.identity);
        yield return new WaitForSeconds(12f);
        Destroy(Cristaux);
    }

    public IEnumerator ApparitionCristaux()
    {
        while (NbCristaux < 40)
        {
            Instantiate(cristal, new Vector3(Random.Range(100f, -100f), Random.Range(400f, 650f), Random.Range(100f, -100f)), Quaternion.identity);
            NbCristaux++;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
        }
        CristauxEvent = GameObject.FindGameObjectsWithTag("CristalEvent");
        yield return new WaitForSeconds(12f);
        NbCristaux = 0;
        EventInProgress = false;
        tutoriel.EVENT_in_progress = false;
        foreach (GameObject c in CristauxEvent) Destroy(c);
       if(Tutoriel.EvCristaux ==0) Tutoriel.EvCristaux = 1;
        yield return null;
    }


        public IEnumerator FadeNuage()
    {
        while (alpha < 1)
        {
            alpha+= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        alpha = 1;
        Desastre = true;
        foreach (GameObject e in eclairs) e.gameObject.SetActive(true);
        //duree de l event ?
        yield return new WaitForSeconds(30f);
        foreach (GameObject e in eclairs) e.gameObject.SetActive(false);
        while (alpha > 0)
        {
            alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        EventInProgress = false;
        tutoriel.EVENT_in_progress = false;
        alpha = 0;
        foreach (GameObject n in NuageNoir)
        {
            n.gameObject.SetActive(false);
            n.transform.rotation = new Quaternion(0, 0, 0, 0);
            var rb = n.GetComponent<Rigidbody>();
            rb.velocity= Vector3.zero;
            rb.angularVelocity= Vector3.zero;
        }

        yield return null;  
    }

    

}

