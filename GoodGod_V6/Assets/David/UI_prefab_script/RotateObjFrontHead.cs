using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem;
//using UnityEngine.XR.Interaction.Toolkit;


public class RotateObjFrontHead : MonoBehaviour
{
    //public static bool activateGenerique = false;
    public GameObject ipad;

    [SerializeField] private Transform head;
    [SerializeField] private float spawnDistance = 2f;
    //[SerializeField] private float offsetX = 0f;
    [SerializeField] private float offsetY = -20f;
    //[SerializeField] private float offsetZ = -20f;


    private void Start()
    {
       // SetObjectFrontHead(ipad);
    }
    void Update()
    {
        RotateObjectFrontHead(ipad);
    }

    public void SetObjectFrontHead(GameObject ipadGo)
    {
        ipadGo.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
    }
    public void RotateObjectFrontHead(GameObject ipadGameObject)
    {
        ipadGameObject.transform.LookAt(new Vector3(head.position.x, ipadGameObject.transform.position.y + offsetY, head.position.z));
        ipadGameObject.transform.forward *= 1;
    }
}
