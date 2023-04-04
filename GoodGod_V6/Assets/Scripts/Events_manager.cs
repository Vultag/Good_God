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
        randomNum1 = Random.Range(1, 6000);
        randomNum2 = Random.Range(1, 300);
        if(randomNum2 == 150 && tutoriel.fin == 0) UnCristal();
        if (randomNum1 == EvCristaux && EventInProgress == false && tutoriel.tuto >=6 && tutoriel.fin == 0) EventCristaux();
        else if (randomNum1 == EvNuages && EventInProgress == false && tutoriel.tuto >= 6 && tutoriel.fin == 0) EventNuages();   
      
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
        if (Tutoriel.EvCristaux == 0) tutoriel.TutoCristaux();
        Debug.Log("Event des cristaaaaaaux !!!");
        EventInProgress = true;
        StartCoroutine(ApparitionCristaux());
    }

    void EventNuages()
    {
        if (Tutoriel.EvNuage == 0) tutoriel.TutorielNuage();
        foreach (GameObject n in NuageNoir)
        {
            n.transform.Rotate(0, Random.Range(100f, -100f), 0);
            n.transform.position = new Vector3(Random.Range(20f, -20f), 35f, Random.Range(20f, -20f));
          
            n.gameObject.SetActive(true);
            EventInProgress = true;
            StartCoroutine(FadeNuage());
            if (Tutoriel.EvNuage == 0) Tutoriel.EvNuage = 1;
        }
    }
    public IEnumerator AppUnCristal()
    {
        GameObject Cristaux = Instantiate(cristal, new Vector3(Random.Range(100f, -100f), Random.Range(80f, 300f), Random.Range(100f, -100f)), Quaternion.identity);
        yield return new WaitForSeconds(12f);
        Destroy(Cristaux);
    }

    public IEnumerator ApparitionCristaux()
    {
        while (NbCristaux < 20)
        {
           Instantiate(cristal, new Vector3(Random.Range(100f, -100f), Random.Range(80f, 300f), Random.Range(100f, -100f)), Quaternion.identity);
            NbCristaux++;
            yield return new WaitForSeconds(0.1f);
        }
        CristauxEvent = GameObject.FindGameObjectsWithTag("CristalEvent");
        yield return new WaitForSeconds(12f);
        NbCristaux = 0;
        EventInProgress = false;
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
        yield return new WaitForSeconds(15f);
        foreach (GameObject e in eclairs) e.gameObject.SetActive(false);
        while (alpha > 0)
        {
            alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        EventInProgress = false;
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

