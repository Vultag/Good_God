using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class skybox_manager : MonoBehaviour
{
    public Material Skybox, FenetresJour, FenetresNuit;
    public float ChangeMoment, speed, timeR, timeV, timeB;
    public Light PrincipalLight;
    public GameObject[] Fenetre;
    public Color CouleurLightJour, CouleurLightCoucher, CouleurLightNuit;
    // Start is called before the first frame update
    void Start()
    {
        //CouleurLightJour = new Color(0.7924528f, 0.7924528f, 0.7924528f);
        //CouleurLightCoucher = new Color(0.9811321f, 0.4073782f, 0f);
        //CouleurLightNuit = new Color(0f, 0f, 0.8f);
        Fenetre = GameObject.FindGameObjectsWithTag("fenetre");
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void AllumerFenetres()
    {
        foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresNuit;
    }
    public void EteindreFenetres()
    {
        foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresJour;
    }

    //public void SetJour()
    //{
    //   ChangeMoment = 1;
    //    StartCoroutine(ChangeSkybox(true));
    //    foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresJour;
        
    //}
    //public void SetCoucherSoleil()
    //{
    //    ChangeMoment = 2;
    //    StartCoroutine(ChangeSkybox(true));
    //    foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresJour;

    //}
    //public void SetNuit()
    //{
    //    ChangeMoment = 3;
    //    StartCoroutine(ChangeSkybox(true));
    //    foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresNuit;

    //}
    //public void SetLeverSoleil()
    //{
    //    ChangeMoment = 0;
    //   StartCoroutine(ChangeSkybox(true));
    //    foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresNuit;
        
    //}

    //public IEnumerator ChangeSkybox(bool IsChanged)
    //{
    //    while (IsChanged == true)
    //    {
    //        Skybox.SetColor("_Tint", new Color(timeR, timeV, timeB, 1));

    //        //lever soleil
    //       if(PrincipalLight.color != CouleurLightCoucher && ChangeMoment == 0)  PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurLightCoucher, Time.deltaTime/5f);
           
    //        //jour = 1 1 1
    //        if (PrincipalLight.color != CouleurLightJour && ChangeMoment == 1)PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurLightJour, Time.deltaTime /2f);

    //        //coucher soleil = R:1 V:0.6 B: 0.46
    //        if (PrincipalLight.color != CouleurLightCoucher && ChangeMoment == 2) PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurLightCoucher, Time.deltaTime/5f);

    //        //night : R =0.3 V =0.35 B = 0.5
    //        if (PrincipalLight.color != CouleurLightNuit && ChangeMoment == 3) PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurLightNuit, Time.deltaTime / 5f);

    //        IsChanged = false;
    //        yield return new WaitForSeconds(0.01f);
  
            
    //    }
    //    yield return null;

    //}
}
