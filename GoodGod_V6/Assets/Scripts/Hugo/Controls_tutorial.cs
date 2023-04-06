using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls_tutorial : MonoBehaviour
{

    [SerializeField] GameObject desk;
    [SerializeField] GameObject langue_pick_gb;
    [SerializeField] GameObject control_tuto_EN_gb;
    [SerializeField] GameObject control_tuto_FR_gb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void langue_picking(string langue)
    {

        desk.SetActive(true);


        if(langue == "FR")
        {
            langue_pick_gb.SetActive(false);
            control_tuto_FR_gb.SetActive(true);

        }
        if(langue == "EN")
        {
            langue_pick_gb.SetActive(false);
            control_tuto_EN_gb.SetActive(true);

        }


    }




}
