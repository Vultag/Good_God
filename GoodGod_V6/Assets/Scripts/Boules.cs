using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boules : MonoBehaviour
{
    public GameObject player;
    private Vector3 playerPos;
    public static float speed1, speed2;
    public bool deplacement;

    public Tutoriel tutoS;
    // Start is called before the first frame update
    void Start()
    {
       speed1 = 0.5f;
       speed2 = 10f;

    }

    // Update is called once per frame
    void Update()
    {
        if (deplacement == false)
        {
            //playerPos = Vector3.LerpUnclamped(this.transform.position, player.transform.position, speed1 * Time.deltaTime);
            this.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed2 * Time.deltaTime);
        }
        
    }

}
