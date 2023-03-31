using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XR_UI_RayInteractor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {

        RaycastHit hit;


        Debug.DrawLine(transform.position, transform.position + (100 * transform.forward));

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10000, LayerMask.GetMask("Default")))
        {
            Debug.Log(hit.collider.gameObject.name);
            //hit.collider.GetComponent<Button>().onClick.Invoke();
        }





    }

}
