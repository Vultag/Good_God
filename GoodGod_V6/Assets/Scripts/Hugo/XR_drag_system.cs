using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class XR_drag_system : MonoBehaviour
{

    public GameObject R_hand;
    public GameObject L_hand;

    private Vector3 body_point_start;
    private Vector3 drag_joint_point_start;
    private Vector3 drag_joint_point;

    private bool is_dragging;
    [HideInInspector]public bool right_hand_dragging;

    public float Vitesse;

    public void set_dragging(bool _is, bool _righ_hand)
    {
        if (_righ_hand)
        {
            drag_joint_point_start = R_hand.transform.position ;
            drag_joint_point = R_hand.transform.position ;
            body_point_start = this.transform.position - R_hand.transform.position;
        }
        else
        {
            drag_joint_point_start = L_hand.transform.position ;
            drag_joint_point = L_hand.transform.position ;
            body_point_start = this.transform.position - L_hand.transform.position;

        }

        is_dragging = _is;
        right_hand_dragging= _righ_hand;

    }

    private void FixedUpdate()
    {

        if (is_dragging)
        {

            if (right_hand_dragging)
            {
                drag_joint_point = drag_joint_point_start + (drag_joint_point - R_hand.transform.position) * Vitesse;

                this.transform.position = body_point_start + drag_joint_point;

            }
            else
            {
                drag_joint_point = drag_joint_point_start + (drag_joint_point - L_hand.transform.position) * Vitesse;

                this.transform.position = body_point_start + drag_joint_point ;

            }


        }

    }


}
