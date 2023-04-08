using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumiereSkyboxIntensite : MonoBehaviour
{
    public TempleScript temple;
    public float Jour, Nuit;
    public bool starting;

    // Start is called before the first frame update
    void Start()
    {
        Jour = 0.6f;
        Nuit = 0.3f;
        RenderSettings.ambientIntensity = Nuit;
        starting = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (temple.heures == 19f && starting == false)
        {
            StartCoroutine(ChangeLuminositeNuit(0));
        }
        else if (temple.heures == 4f && starting == false)
        {
            StartCoroutine(ChangeLuminositeJour(0));
        }
    }
  

    public IEnumerator ChangeLuminositeNuit(float time)
    {
        starting = true;
        while (time < 0.01f)
        {
            //Debug.Log(time);
            time += 0.001f* Time.deltaTime;
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, Nuit, time);
            yield return new WaitForFixedUpdate();
        }
       
        starting = false;
        StopAllCoroutines();
        yield return null;
    }

    public IEnumerator ChangeLuminositeJour(float time)
    {
        starting = true;
        while (time < 0.05f)
        {
            //Debug.Log(time);
            time += 0.005f * Time.deltaTime;
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, Jour, time);
            yield return new WaitForFixedUpdate();
        }
     
        starting = false;
        StopAllCoroutines();
        yield return null;
    }


}
