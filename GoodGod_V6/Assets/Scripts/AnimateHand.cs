using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class AnimateHand : MonoBehaviour
{

    XRIDefaultInputActions inputs_action;

    public InputActionProperty RpinchAnimationAction;
    public InputActionProperty RgripAnimationAction;
    public InputActionProperty LpinchAnimationAction;
    public InputActionProperty LgripAnimationAction;
    public Animator RhandAnimator, LhandAnimator;
    public float Ltrigger, Lgrab, Rtrigger, Rgrab;
    public bool eclair;
    public Raycast_eclair RayEclairR, RayEclairL;
    public InputActionProperty buttonB;
    public static bool LPoingFerme, RPoingFerme;
    private GameObject temple;
    public AudioSource EclairTir;

    private void OnEnable()
    {

        inputs_action = new XRIDefaultInputActions();


        Rtrigger = Mathf.Clamp(Rtrigger, 0, 1);
        Ltrigger = Mathf.Clamp(Ltrigger, 0, 1);
        Lgrab = Mathf.Clamp(Lgrab, 0, 1);
        Rgrab = Mathf.Clamp(Rgrab, 0, 1);


        inputs_action.Enable();

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

    // Start is called before the first frame update
    void Start()
    {
        
        if(TempleScript.instance != null)
            temple = TempleScript.instance.gameObject;


    }
    private void OnDisable()
    {
        inputs_action.XRIRightHandInteraction.Activate.started -= R_Trigger_activate;
        inputs_action.XRIRightHandInteraction.Activate.canceled -= R_Trigger_deactivate;
        inputs_action.XRIRightHandInteraction.ActivateValue.performed += R_Trigger_performed;

        inputs_action.XRILeftHandInteraction.Activate.started -= L_Trigger_activate;
        inputs_action.XRILeftHandInteraction.Activate.canceled -= L_Trigger_deactivate;
        inputs_action.XRILeftHandInteraction.ActivateValue.performed -= L_Trigger_performed;

        inputs_action.XRIRightHandInteraction.SelectValue.performed -= R_Trigger_performed;
        inputs_action.XRILeftHandInteraction.SelectValue.performed -= L_Trigger_performed;


        inputs_action.XRBUTTONS.X.performed -= Lance_eclair_performed_L;
        inputs_action.XRBUTTONS.A.performed -= Lance_eclair_performed_R;
    }

    // Update is called once per frame
    void Update()
    {
 
     
      

    }
    /*
    private void R_Select_started(CallbackContext obj)
    {

    }
    private void L_Select_started(CallbackContext obj)
    {

    }
    private void R_Select_canceled(CallbackContext obj)
    {

    }
    private void L_Select_canceled(CallbackContext obj)
    {

    }
    */


    private void R_Trigger_activate(CallbackContext obj)
    {

        Rgrab = RgripAnimationAction.action.ReadValue<float>();
        
        if (Events_manager.EventInProgress == false && TempleScript.instance != null)
        {
            if (Rgrab < 0.2f)
            {

                temple.GetComponent<YogaScript>().activate_yoga_hand(0, true);



            }

        }

    }
    private void R_Trigger_deactivate(CallbackContext obj)
    {

        if (TempleScript.instance != null)
            temple.GetComponent<YogaScript>().activate_yoga_hand(0, false);
    }


    private void L_Trigger_deactivate(CallbackContext obj)
    {

        if (TempleScript.instance != null)
            temple.GetComponent<YogaScript>().activate_yoga_hand(1, false);
    }

    private void L_Trigger_activate(CallbackContext obj)
    {

        Lgrab = LgripAnimationAction.action.ReadValue<float>();

        if (Events_manager.EventInProgress == false && TempleScript.instance != null)
        {
            if (Lgrab < 0.2f)
            {

                temple.GetComponent<YogaScript>().activate_yoga_hand(1, true);



            }

        }

    }

    private void Lance_eclair_performed_L(CallbackContext obj)
    {

        if (Ltrigger < 0.2f) 
            if (Lgrab > 0.9f)
            {
                EclairTir.Play();
                RayEclairL.LanceEclair();

            }
    }
    private void Lance_eclair_performed_R(CallbackContext obj)
    {

        if (Rtrigger < 0.2f) 
            if (Rgrab > 0.9f)
            {
                EclairTir.Play();
                RayEclairR.LanceEclair();
            }
    }

    private void R_Trigger_performed(CallbackContext obj)
    {

        Rtrigger = RpinchAnimationAction.action.ReadValue<float>();
        RhandAnimator.SetFloat("Trigger", Rtrigger);

        Rgrab = RgripAnimationAction.action.ReadValue<float>();
        RhandAnimator.SetFloat("Grip", Rgrab);

        if (Rtrigger > 0.9f && Rgrab > 0.9f)
        {
            //poing gauche fermé
            RPoingFerme = true;
        }
        else RPoingFerme = false;

        /*
        if (Events_manager.EventInProgress == false)
        {
            if (Ltrigger > 0.9f && Rtrigger > 0.9f && Lgrab < 0.2f && Rgrab < 0.2f)
            {
                //Debug.Log("Yogaaaaa");





            }
            //else
                //Debug.Log("Stop yoga");
        }

        if (Rtrigger > 0.9f && Rgrab > 0.9f)
        {
            poing gauche fermé
            RPoingFerme = true;
        }
        */
    }


    private void L_Trigger_performed(CallbackContext obj)
    {
        Ltrigger = LpinchAnimationAction.action.ReadValue<float>();
        LhandAnimator.SetFloat("Trigger", Ltrigger);

        Lgrab = LgripAnimationAction.action.ReadValue<float>();
        LhandAnimator.SetFloat("Grip", Lgrab);

        if (Ltrigger > 0.9f && Lgrab > 0.9f)
        {
            //poing gauche fermé
            LPoingFerme = true;
        }
        else LPoingFerme = false;
    }

}
