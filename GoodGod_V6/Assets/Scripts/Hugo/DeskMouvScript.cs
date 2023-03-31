using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskMouvScript : MonoBehaviour
{



    public void move_desk(float horizontal_vel)
    {

        //this.transform.rotation += horizontal_vel * 

        this.transform.Rotate(0, horizontal_vel, 0);

    }


}
