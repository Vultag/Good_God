using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desintegration : MonoBehaviour
{
    public float dissolve, deform;
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        dissolve = Mathf.Clamp(-1f, -1f, 1);
        deform = Mathf.Clamp(0, 0, 50);
        // Use the Specular shader on the material
        //rend.material.shader = Shader.Find("dissolve");

    }

    // Update is called once per frame
    void Update()
    {
        if (dissolve < 1)
        {
            dissolve += Time.deltaTime;
            deform += Time.deltaTime;
        }
        
        rend.material.SetFloat("_dissolve", dissolve);
        rend.material.SetFloat("_deformation", deform);
    }
}
