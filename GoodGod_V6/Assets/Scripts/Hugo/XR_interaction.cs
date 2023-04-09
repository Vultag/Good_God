using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.InputSystem;


public class XR_interaction : MonoBehaviour
{


    XRIDefaultInputActions inputs_action;

    public XR_drag_system drag_system;

    [SerializeField] GameObject Menu;
    [SerializeField] GameObject XR_ray;

    public DeskMouvScript Desk;

    public GameObject R_hand;
    public GameObject L_hand;

    private Vector3 body_point_start;
    private Vector3 drag_joint_point_start;
    private Vector3 drag_joint_point;

    //private InputAction right_hand_trigger;
    //private InputAction left_hand_trigger;

    [HideInInspector] public bool pause = false;

    private void OnEnable()
    {

        inputs_action = new XRIDefaultInputActions();

        inputs_action.Enable();



        inputs_action.XRBUTTONS.Y.started += Y_activate;
        inputs_action.XRBUTTONS.Y.canceled += Y_deactivate;

        inputs_action.XRBUTTONS.B.started += B_activate;
        inputs_action.XRBUTTONS.B.canceled += B_deactivate;


        inputs_action.XRBUTTONS.MENU.started += menu_start;
        //inputs_action.XRBUTTONS.MENU.canceled += menu_cancel;

        inputs_action.XRIRightHandLocomotion.Move.performed += R_stick_perform;
        //inputs_action.XRIRightHandInteraction.Activate.canceled += R_Trigger_deactivate;

        //inputs_action.XRIRightHandInteraction.Activate.started += R_Trigger_activate;
        //inputs_action.XRIRightHandInteraction.Activate.canceled += R_Trigger_deactivate;

        //inputs_action.XRILeftHandInteraction.Activate.started += L_Trigger_activate;
        //inputs_action.XRILeftHandInteraction.Activate.canceled += L_Trigger_deactivate;

        //inputs_action.XRIRightHandInteraction.ActivateValue.performed += R_Trigger_valued;
        //inputs_action.XRILeftHandInteraction.ActivateValue.performed += L_Trigger_valued;


        //right_hand_trigger = inputs_action.XRIRightHandInteraction.ActivateValue;
        //left_hand_trigger = inputs_action.XRILeftHandInteraction.ActivateValue;

    }

    void Start()
    {




    }

    private void OnDisable()
    {
        inputs_action.XRBUTTONS.Y.started -= Y_activate;
        inputs_action.XRBUTTONS.Y.canceled -= Y_deactivate;

        inputs_action.XRBUTTONS.B.started -= B_activate;
        inputs_action.XRBUTTONS.B.canceled -= B_deactivate;


        inputs_action.XRBUTTONS.MENU.started -= menu_start;
        inputs_action.XRIRightHandLocomotion.Move.performed -= R_stick_perform;
    }

    private void menu_start(CallbackContext obj)
    {
        _pause_switch();
    }

    public void _pause_switch()
    {
        if (!pause)
        {
            XR_ray.SetActive(true);
            Time.timeScale = 0;
            Menu.SetActive(true);
            pause = true;
        }
        else
        {
            XR_ray.SetActive(false);
            Time.timeScale = 1;
            Menu.SetActive(false);
            pause = false;
        }
    }
    
    private void menu_cancel(CallbackContext obj)
    {
        Time.timeScale = 1;
        Menu.SetActive(false);
    }

    private void R_stick_perform(CallbackContext obj)
    {
        //Debug.Log(obj.ReadValue<Vector2>());
        Desk.move_desk(obj.ReadValue<Vector2>().x);
    }


    private void Y_deactivate(CallbackContext obj)
    {
        if(!drag_system.right_hand_dragging)
            drag_system.set_dragging(false, false);
    }

    private void Y_activate(CallbackContext obj)
    {
        drag_system.set_dragging(true, false);
    }
    private void L_Trigger_valued(CallbackContext obj)
    {

    }

    /*
    private void R_Trigger_deactivate(CallbackContext obj)
    {
        drag_joint.GetComponent<ConfigurableJoint>().connectedBody =null;
        drag_joint.SetActive(false);
    }

    private void R_Trigger_activate(CallbackContext obj)
    {

        //drag_joint.transform.position = R_hand.transform.position;
        drag_joint_point_start = R_hand.transform.position;
        drag_joint.GetComponent<ConfigurableJoint>().connectedBody = R_hand.transform.parent.parent.GetComponent<Rigidbody>();
        drag_joint.SetActive(true);

    }
    private void R_Trigger_valued(CallbackContext obj)
    {

        Debug.Log(R_hand.transform.localPosition);
        drag_joint_point = drag_joint_point_start + (drag_joint_point - R_hand.transform.position);
        drag_joint.transform.position =  drag_joint_point;

    }
    */
    private void B_deactivate(CallbackContext obj)
    {

        if (drag_system.right_hand_dragging)
            drag_system.set_dragging(false, false);
    }

    private void B_activate(CallbackContext obj)
    {

        drag_system.set_dragging(true, true);

    }
    private void R_Trigger_valued(CallbackContext obj)
    {

    }

}
