using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class skybox_manager_v2 : MonoBehaviour
{
    public Material Skybox, FenetresJour, FenetresNuit, endGameSkybox;
    public float ChangeMoment, speed, timeR, timeV, timeB;
    public Light PrincipalLight;
    public GameObject[] Fenetre;
    public Color CouleurLightJour, CouleurLightCoucher, CouleurLightNuit, CouleurEngGame;

    [SerializeField] private SwitchSkybox refScriptSwitchSkybox;
    

    // Start is called before the first frame update
    void Start()
    {
        CouleurLightJour = new Color(0.7924528f, 0.7924528f, 0.7924528f);
        CouleurLightCoucher = new Color(0.9811321f, 0.4073782f, 0f);
        CouleurLightNuit = new Color(0f, 0f, 0.8f);
        CouleurEngGame = new Color(0.92f, 0.92f, 0.92f);

        Fenetre = GameObject.FindGameObjectsWithTag("fenetre");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

  public void SetJour()
    {
        ChangeMoment = 1;
        StartCoroutine(ChangeSkybox(true));
        foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresJour;
    }
    public void SetCoucherSoleil()
    {
        ChangeMoment = 2;
        StartCoroutine(ChangeSkybox(true));
        foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresJour;

    }
    public void SetNuit()
    {
        ChangeMoment = 3;
        StartCoroutine(ChangeSkybox(true));
        foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresNuit;
    }
    public void SetLeverSoleil()
    {
        ChangeMoment = 0;
        StartCoroutine(ChangeSkybox(true));
        foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresNuit;
    }
    public void SetEndGame()
    {
       // ChangeMoment = 4;
       // StartCoroutine(ChangeSkybox(true));
        //foreach (GameObject f in Fenetre) f.GetComponent<MeshRenderer>().material = FenetresJour;
     
        Debug.Log("bwah");
    }

    public IEnumerator ChangeSkybox(bool IsChanged)
    {
        while (IsChanged == true)
        {
            Skybox.SetColor("_Tint", new Color(timeR, timeV, timeB, 1));

            //lever soleil
            if (ChangeMoment == 0)
            {
                //change RVB skybox
                if (timeR < 1) timeR += 0.2f * Time.deltaTime;
                else timeR -= 0.1f * Time.deltaTime;
                if (timeV < 0.72f) timeV += 0.1f * Time.deltaTime;
                else timeV += 0.1f * Time.deltaTime;
                if (timeB < 0.75f) timeB += 0.1f * Time.deltaTime;
                else timeB += 0.1f * Time.deltaTime;
                if (timeR >= 1 && timeV >= 0.72f && timeB >= 0.75f) IsChanged = false;

                PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurLightCoucher, Time.deltaTime/5f);
            }
            //jour = 1 1 1
            if (ChangeMoment == 1)
            {
                //change RVB skybox
                if (timeR < 1) timeR += 0.1f * Time.deltaTime;
                else timeR -= 0.1f * Time.deltaTime;
                if (timeV < 1) timeV += 0.1f * Time.deltaTime;
                else timeV -= 0.1f * Time.deltaTime;
                if (timeB < 1) timeB += 0.1f * Time.deltaTime;
                else timeB -= 0.1f * Time.deltaTime;
                if (timeR >= 1 && timeV >= 1 && timeB >= 1) IsChanged = false;
                PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurLightJour, Time.deltaTime /2f);
            }
            //coucher soleil = R:1 V:0.6 B: 0.46
            if (ChangeMoment == 2)
            {
                //change RVB skybox

                if (timeR < 1) timeR += 0.1f * Time.deltaTime;
                else timeR -= 0.3f * Time.deltaTime;
                if (timeV > 0.6f) timeV -= 0.1f * Time.deltaTime;
                else timeV += 0.06f * Time.deltaTime;
                if (timeB > 0.46f) timeB -= 0.1f * Time.deltaTime;
                else timeB += 0.1f * Time.deltaTime;
                if (timeR <= 1 && timeV <= 0.6f && timeB <= 0.46f) IsChanged = false;

                PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurLightCoucher, Time.deltaTime/5f);
            }
            //night : R =0.3 V =0.35 B = 0.5
            if (ChangeMoment == 3)
            {
                //change RVB skybox
                if (timeR > 0.2f) timeR -= 0.3f * Time.deltaTime;
                else timeR += 0.1f * Time.deltaTime;
                if (timeV > 0.3f) timeV -= 0.1f * Time.deltaTime;
                else timeV += 0.1f * Time.deltaTime;
                if (timeB > 0.45f) timeB -= 0.1f * Time.deltaTime;
                else timeB += 0.1f * Time.deltaTime;
                if (timeR <= 0.2f && timeV <= 0.3f && timeB <= 0.45f) IsChanged = false;
                PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurLightNuit, Time.deltaTime / 5f);
            }

            //end : R = 0.92 V = 0.92 B =0.92
            if (ChangeMoment == 4)
            {
                //change RVB skybox
                if (timeR > 0.92f) timeR -= 0.3f * Time.deltaTime;
                else timeR += 0.1f * Time.deltaTime;
                if (timeV > 0.92f) timeV -= 0.1f * Time.deltaTime;
                else timeV += 0.1f * Time.deltaTime;
                if (timeB > 0.92f) timeB -= 0.1f * Time.deltaTime;
                else timeB += 0.1f * Time.deltaTime;
                if (timeR <= 0.92f && timeV <= 0.92f && timeB <= 0.92f) IsChanged = false;

                PrincipalLight.color = Color.Lerp(PrincipalLight.color, CouleurEngGame, Time.deltaTime / 5f);

               
            }

            yield return new WaitForSeconds(0.01f);
        }
        yield return null;

    }
}
