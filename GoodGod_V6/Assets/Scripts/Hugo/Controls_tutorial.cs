using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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


    [SerializeField] GameObject XR_ray;
    [SerializeField] GameObject XR_interaction;

    // Start is called before the first frame update
    void Start()
    {
        //XR_interaction.SetActive(false);
        XR_ray.SetActive(true);
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

            GameManager.instance.GetComponent<GameManager>().Language = "FR";
            langue_pick_gb.SetActive(false);
            control_tuto_FR_gb.SetActive(true);

        }
        if(langue == "EN")
        {
            GameManager.instance.GetComponent<GameManager>().Language = "EN";
            langue_pick_gb.SetActive(false);
            control_tuto_EN_gb.SetActive(true);
        }

        XR_ray.SetActive(false);
        XR_interaction.SetActive(true);

    }

    public void tuto_completion_check()
    {




        if(move_action_check && meditate_action_check && turn_action_check && grab_action_check && shoot_action_check && pause_action_check)
        {
            control_tuto_EN_gb.SetActive(false);
            control_tuto_FR_gb.SetActive(false);
            //XR_interaction.SetActive(false);
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("GoodGod_Fusion");
        }
        else
        {
            this.GetComponent<AudioSource>().Play();
        }

    }


}
