using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls_tutorial : MonoBehaviour
{

    [SerializeField] GameObject desk;
    [SerializeField] GameObject langue_pick_gb;
    [SerializeField] GameObject control_tuto_EN_gb;
    [SerializeField] GameObject control_tuto_FR_gb;

    [HideInInspector] public bool move_action_check = false;
    [HideInInspector] public bool meditate_action_check = false;
    [HideInInspector] public bool turn_action_check = false;
    [HideInInspector] public bool grab_action_check = false;
    [HideInInspector] public bool shoot_action_check = false;
    [HideInInspector] public bool pause_action_check = false;

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

    public void tuto_completion_check()
    {

        if(move_action_check && meditate_action_check && turn_action_check && grab_action_check && shoot_action_check && pause_action_check)
        {
            Debug.Log("change_scene");
        }

    }


}