using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Langage_initialize : MonoBehaviour
{
    [SerializeField] GameObject Igod_screen;
    [SerializeField] Material EN_screen;
    [SerializeField] Material FR_screen;



    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.GetComponent<GameManager>().Language == "FR")
        {
            Igod_screen.GetComponent<MeshRenderer>().material = FR_screen;
        }
        else if (GameManager.instance.GetComponent<GameManager>().Language == "EN")
        {
            Igod_screen.GetComponent<MeshRenderer>().material = EN_screen;
        }
    }


}
