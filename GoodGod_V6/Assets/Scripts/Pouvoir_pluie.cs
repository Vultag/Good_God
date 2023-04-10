using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

public class Pouvoir_pluie : MonoBehaviour
{
    public float VieNuage, ScaleYMax, DureeCouleur, t, Rgrab;
    public ParticleSystem pluie, eclair;
    public Vector3 scaleChange, scaleOrigin, scaleMax;
    public Material MatBlanc, MatNoir;
    public bool Attrape;
    public InputActionProperty RgripAnimationAction;


    public List<ParticleSystem> Boom;
    // Start is called before the first frame update
    void Start()
    {
        pluie.Stop();
        eclair.Stop();
        VieNuage = 0;
        scaleOrigin = this.transform.localScale;
        ScaleYMax = scaleOrigin.y * 0.7f;
        scaleMax = new Vector3(scaleOrigin.x *1.7f, scaleOrigin.y * 0.7f, scaleOrigin.z *0.7f);
        Rgrab = Mathf.Clamp(Rgrab, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    /*
 public void Effets()
    {
        if(VieNuage >90) eclair.Play();
       else pluie.Play();
    }
    */

    public void PressNuage()
    {
      
        Attrape = true;
        if (VieNuage < 90)
        {
            pluie.Play();
            StartCoroutine(Resapwn_cycle());
        }
        StartCoroutine(PresseNuage());
        
    }
    public void SorsNuage()
    {
        Attrape = false;
        pluie.Stop();
        eclair.Stop();
        // StopCoroutine(PresseNuage());
        StopCoroutine(Resapwn_cycle());
        StartCoroutine(ExitNuage());
    }
    
    public IEnumerator ExitNuage()
    {

            while (VieNuage > 1 && Attrape == false)
            {
                DureeCouleur = VieNuage * 0.007f;
                VieNuage -= Time.deltaTime * 10f;
            
            this.GetComponent<Renderer>().material.Lerp(MatBlanc, MatNoir, DureeCouleur);
            scaleChange = Vector3.Lerp(this.transform.localScale, scaleOrigin, 0.1f);
            this.transform.localScale = scaleChange;
                yield return new WaitForSeconds(0.01f);
            }
        yield break;

    }
    public IEnumerator PresseNuage()
    {

       
            while (Attrape == true)
            {
                if (VieNuage < 150)
                {
                    DureeCouleur = VieNuage * 0.007f;
                    VieNuage += Time.deltaTime * 10f;
                    this.GetComponent<Renderer>().material.Lerp(MatBlanc, MatNoir, DureeCouleur);


                    scaleChange = Vector3.Lerp(this.transform.localScale, scaleMax, 0.1f);

                    this.transform.localScale = scaleChange;
                }
                else
                {
                    eclair.Play();
                    yield break;
                }
                

                yield return new WaitForSeconds(0.02f);
            }
            yield break;
    }

    public IEnumerator Resapwn_cycle()
    {


        while (VieNuage < 150 && Attrape == true)
        {

            yield return new WaitForSeconds(1.5f);

            if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 100f,(LayerMask.GetMask("respawn")) | LayerMask.GetMask("ressource")))
            {
                if (hit.collider.tag == "spawn_surface")
                {

                    GameObject new_ressource = new GameObject();

                    if (Random.Range(0, 2) == 0)
                    {
                        new_ressource = Instantiate(TempleScript.instance.Trees_prefab[Random.Range(0, TempleScript.instance.Trees_prefab.Length)], new Vector3(hit.point.x, -5.900002f, hit.point.z), Quaternion.Euler(0, Random.Range(0, 360),0), TempleScript.instance.transform.parent);
                    }
                    else
                    {
                        new_ressource = Instantiate(TempleScript.instance.Minerals_prefab[Random.Range(0, TempleScript.instance.Minerals_prefab.Length)], new Vector3(hit.point.x, -5.900002f, hit.point.z), Quaternion.Euler(0, Random.Range(0, 360), 0), TempleScript.instance.transform.parent);
                    }


                    new_ressource.transform.GetChild(0).GetComponent<Rigidbody>().angularDrag = 10f; //methode shnaps pour faire la diff avec ressources normales
                }
            }


        }

    }
    

}
