using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderMains : MonoBehaviour
{
    public Material MainsMat;
    public TempleScript TempleScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Changement lerp des mains en fonction de l'alignement
    void FixedUpdate()
    {
        
        MainsMat.SetFloat("_Blend", TempleScript.village_terror * 0.01f);
    }
}
