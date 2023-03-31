using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Impact_disparition : MonoBehaviour
{
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AdieuImpact());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator AdieuImpact()
    {
        
        while (timer > 0)
        {
            timer-=1*Time.deltaTime;
            yield return new WaitForSeconds(0.5f);
        }
       Destroy(this.gameObject);
    }
}
