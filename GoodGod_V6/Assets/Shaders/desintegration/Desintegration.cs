using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desintegration : MonoBehaviour
{
    public float dissolve, deform;
    public Renderer rend;
    public GameObject Mesh;

    // Start is called before the first frame update
    void Start()
    {
        rend = Mesh.GetComponent<Renderer>();
        dissolve = Mathf.Clamp(-1f, -1f, 1);
        StartCoroutine(DissolveTimer());
        // Use the Specular shader on the material
        //rend.material.shader = Shader.Find("dissolve");

    }

    // Update is called once per frame
  

    public IEnumerator DissolveTimer()
    {
        //while (time > 0)
        //{
        //    time--;
        //    yield return new WaitForSeconds(2);
        //}
        yield return new WaitForSeconds(1);
        while (dissolve < 1)
        {
            dissolve += 0.6f*Time.deltaTime;
            rend.material.SetFloat("_dissolve", dissolve);
            yield return new WaitForFixedUpdate();
        }

    }
}
