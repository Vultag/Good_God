using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class inputs_tuto_detection : MonoBehaviour
{


    [SerializeField] GameObject control_tuto_gb;

    XRIDefaultInputActions inputs_action;

    [SerializeField] GameObject move_test_gb;
    [SerializeField] GameObject meditate_test_gb;
    [SerializeField] GameObject turn_test_gb;
    [SerializeField] GameObject grab_test_gb;
    [SerializeField] GameObject shoot_test_gb;
    [SerializeField] GameObject pause_test_gb;


    [SerializeField] ParticleSystem meditate_fx;


    private float Ltrigger, Lgrab, Rtrigger, Rgrab;

    private static bool LPoingFerme, RPoingFerme;


    [SerializeField] InputActionProperty RpinchAnimationAction;
    [SerializeField] InputActionProperty RgripAnimationAction;
    [SerializeField] InputActionProperty LpinchAnimationAction;
    [SerializeField] InputActionProperty LgripAnimationAction;
    //[SerializeField] InputActionProperty button_Y;
    //[SerializeField] InputActionProperty button_B;

    private void OnEnable()
    {

        inputs_action = new XRIDefaultInputActions();


        inputs_action.Enable();


        inputs_action.XRBUTTONS.Y.started += Y_activate;

        inputs_action.XRBUTTONS.B.started += B_activate;


        inputs_action.XRBUTTONS.MENU.started += menu_start;

        inputs_action.XRIRightHandLocomotion.Move.performed += R_stick_perform;

        inputs_action.XRIRightHandInteraction.Activate.started += R_Trigger_activate;
        inputs_action.XRIRightHandInteraction.Activate.canceled += R_Trigger_deactivate;
        inputs_action.XRIRightHandInteraction.ActivateValue.performed += R_Trigger_performed;

        inputs_action.XRILeftHandInteraction.Activate.started += L_Trigger_activate;
        inputs_action.XRILeftHandInteraction.Activate.canceled += L_Trigger_deactivate;
        inputs_action.XRILeftHandInteraction.ActivateValue.performed += L_Trigger_performed;

        inputs_action.XRIRightHandInteraction.SelectValue.performed += R_Trigger_performed;
        inputs_action.XRILeftHandInteraction.SelectValue.performed += L_Trigger_performed;

        //inputs_action.XRIRightHandInteraction.SelectValue.started += R_Select_started;
        //inputs_action.XRIRightHandInteraction.SelectValue.canceled += R_Select_canceled;


        //inputs_action.XRILeftHandInteraction.SelectValue.started += L_Select_started;
        //inputs_action.XRILeftHandInteraction.SelectValue.canceled += L_Select_canceled;

        inputs_action.XRBUTTONS.X.performed += Lance_eclair_performed_L;
        inputs_action.XRBUTTONS.A.performed += Lance_eclair_performed_R;

    }

    void Start()
    {


        Rtrigger = Mathf.Clamp(Rtrigger, 0, 1);
        Ltrigger = Mathf.Clamp(Ltrigger, 0, 1);
        Lgrab = Mathf.Clamp(Lgrab, 0, 1);
        Rgrab = Mathf.Clamp(Rgrab, 0, 1);

    }



    private void OnDisable()
    {
        inputs_action.XRBUTTONS.Y.started -= Y_activate;

        inputs_action.XRBUTTONS.B.started -= B_activate;

        inputs_action.XRBUTTONS.MENU.started -= menu_start;

        inputs_action.XRIRightHandLocomotion.Move.performed -= R_stick_perform;

        inputs_action.XRIRightHandInteraction.Activate.started -= R_Trigger_activate;
        inputs_action.XRIRightHandInteraction.Activate.canceled -= R_Trigger_deactivate;
        inputs_action.XRIRightHandInteraction.ActivateValue.performed += R_Trigger_performed;

        inputs_action.XRILeftHandInteraction.Activate.started -= L_Trigger_activate;
        inputs_action.XRILeftHandInteraction.Activate.canceled -= L_Trigger_deactivate;
        inputs_action.XRILeftHandInteraction.ActivateValue.performed -= L_Trigger_performed;

        inputs_action.XRIRightHandInteraction.SelectValue.performed -= R_Trigger_performed;
        inputs_action.XRILeftHandInteraction.SelectValue.performed -= L_Trigger_performed;
        inputs_action.XRIRightHandInteraction.SelectValue.canceled -= R_grip_deactivate;
        inputs_action.XRILeftHandInteraction.SelectValue.canceled -= L_grip_deactivate;

        inputs_action.XRBUTTONS.X.performed -= Lance_eclair_performed_L;
        inputs_action.XRBUTTONS.A.performed -= Lance_eclair_performed_R;


        inputs_action.XRIRightHandInteraction.Activate.canceled -= R_Trigger_deactivate;
        inputs_action.XRILeftHandInteraction.Activate.canceled -= L_Trigger_deactivate;


        inputs_action.Disable();
    }


    private void Y_activate(CallbackContext obj)
    {
        tuto_input_check("move");

    }
    private void B_activate(CallbackContext obj)
    {

        //Debug.Log("move");
        tuto_input_check("move");
    }
    private void menu_start(CallbackContext obj)
    {

        tuto_input_check("pause");

    }
    private void R_stick_perform(CallbackContext obj)
    {

        tuto_input_check("turn");

    }


    private void R_Trigger_deactivate(CallbackContext obj)
    {
        meditate_fx.Stop();
        Time.timeScale = 1;

    }


    private void L_Trigger_deactivate(CallbackContext obj)
    {
        meditate_fx.Stop();
        Time.timeScale = 1;

    }

    private void R_Trigger_activate(CallbackContext obj)
    {

        Rtrigger = RpinchAnimationAction.action.ReadValue<float>();
        Ltrigger = LpinchAnimationAction.action.ReadValue<float>();

        Rgrab = RgripAnimationAction.action.ReadValue<float>();
        Lgrab = LgripAnimationAction.action.ReadValue<float>();

        if (Rgrab < 0.2f)
        {

            if (Lgrab < 0.2f && Ltrigger > 0.99f)
            {
                meditate_fx.Play();
                tuto_input_check("medite");
                Time.timeScale = 6;
            }
            else
            {
                meditate_fx.Stop();
                Time.timeScale = 1;
            }
        }
        else
        {
            meditate_fx.Stop();
            Time.timeScale = 1;
        }

    }


    private void L_Trigger_activate(CallbackContext obj)
    {

        Rtrigger = RpinchAnimationAction.action.ReadValue<float>();
        Ltrigger = LpinchAnimationAction.action.ReadValue<float>();

        Rgrab = RgripAnimationAction.action.ReadValue<float>();
        Lgrab = LgripAnimationAction.action.ReadValue<float>();

        if (Lgrab < 0.2f)
        {
            if (Rgrab < 0.2f && Rtrigger > 0.99f)
            {
                meditate_fx.Play();
                tuto_input_check("medite");
                Time.timeScale = 6;
            }
            else
            {
                meditate_fx.Stop();
                Time.timeScale = 1;
            }

        }
        else
        { 
            meditate_fx.Stop();
            Time.timeScale = 1;
        }

    }

    private void Lance_eclair_performed_L(CallbackContext obj)
    {

        if (Ltrigger < 0.2f) 
            if (Lgrab > 0.9f)
            {
                tuto_input_check("shoot");
            }
    }
    private void Lance_eclair_performed_R(CallbackContext obj)
    {


        if (Rtrigger < 0.2f) 
            if (Rgrab > 0.9f)
            {
                tuto_input_check("shoot");
            }
    }

    private void R_Trigger_performed(CallbackContext obj)
    {



        Rtrigger = RpinchAnimationAction.action.ReadValue<float>();

        Rgrab = RgripAnimationAction.action.ReadValue<float>();

        if (Rtrigger > 0.9f && Rgrab > 0.9f)
        {
            meditate_fx.Stop();
            Time.timeScale = 1;

            //poing gauche fermé
            RPoingFerme = true;
        }

    }


    private void L_Trigger_performed(CallbackContext obj)
    {



        Ltrigger = LpinchAnimationAction.action.ReadValue<float>();

        Lgrab = LgripAnimationAction.action.ReadValue<float>();

        if (Ltrigger > 0.9f && Lgrab > 0.9f)
        {
            meditate_fx.Stop();
            Time.timeScale = 1;

            //poing gauche fermé
            LPoingFerme = true;
        }

    }

    //test pour fix le bug de tuto medit
    
    private void R_grip_deactivate(CallbackContext obj)
    {



        Rtrigger = RpinchAnimationAction.action.ReadValue<float>();
        Ltrigger = LpinchAnimationAction.action.ReadValue<float>();

        Rgrab = RgripAnimationAction.action.ReadValue<float>();
        Lgrab = LgripAnimationAction.action.ReadValue<float>();

        if (Ltrigger > 0.9f && Rtrigger > 0.9f && Lgrab < 0.2f)
        {
            tuto_input_check("medite");
        }

    }
    private void L_grip_deactivate(CallbackContext obj)
    {



        Rtrigger = RpinchAnimationAction.action.ReadValue<float>();
        Ltrigger = LpinchAnimationAction.action.ReadValue<float>();

        Rgrab = RgripAnimationAction.action.ReadValue<float>();
        Lgrab = LgripAnimationAction.action.ReadValue<float>();

        if (Ltrigger > 0.9f && Rtrigger > 0.9f && Rgrab < 0.2f)
        {
            tuto_input_check("medite");
        }

    }
    


    public void tuto_input_check(string action)
    {
        
        switch (action)
        {
            case "move":
                
                if(control_tuto_gb.GetComponent<Controls_tutorial>().move_action_check != true)
                {
                    control_tuto_gb.GetComponent<Controls_tutorial>().move_action_check = true;
                    move_test_gb.GetComponent<Animator>().enabled = true;
                    control_tuto_gb.GetComponent<Controls_tutorial>().tuto_completion_check();
                }
                break;
            case "shoot":
                if (control_tuto_gb.GetComponent<Controls_tutorial>().shoot_action_check != true)
                {
                    control_tuto_gb.GetComponent<Controls_tutorial>().shoot_action_check = true;
                    shoot_test_gb.GetComponent<Animator>().enabled = true;
                    control_tuto_gb.GetComponent<Controls_tutorial>().tuto_completion_check();
                }
                break;
            case "medite":
                if (control_tuto_gb.GetComponent<Controls_tutorial>().meditate_action_check != true)
                {
                    control_tuto_gb.GetComponent<Controls_tutorial>().meditate_action_check = true;
                    meditate_test_gb.GetComponent<Animator>().enabled = true;
                    control_tuto_gb.GetComponent<Controls_tutorial>().tuto_completion_check();
                }
                break;
            case "grab":
                if (!grab_test_gb.GetComponent<Animator>().enabled)
                {
                    control_tuto_gb.GetComponent<Controls_tutorial>().grab_action_check = true;
                    grab_test_gb.GetComponent<Animator>().enabled = true;
                    control_tuto_gb.GetComponent<Controls_tutorial>().tuto_completion_check();
                }
                break;
            case "turn":
                if (control_tuto_gb.GetComponent<Controls_tutorial>().turn_action_check != true)
                {
                    control_tuto_gb.GetComponent<Controls_tutorial>().turn_action_check = true;
                    turn_test_gb.GetComponent<Animator>().enabled = true;
                    control_tuto_gb.GetComponent<Controls_tutorial>().tuto_completion_check();
                }
                break;
            case "pause":
                if (control_tuto_gb.GetComponent<Controls_tutorial>().pause_action_check != true)
                {
                    control_tuto_gb.GetComponent<Controls_tutorial>().pause_action_check = true;
                    pause_test_gb.GetComponent<Animator>().enabled = true;
                    control_tuto_gb.GetComponent<Controls_tutorial>().tuto_completion_check();
                }
                break;

        }


    }

}
