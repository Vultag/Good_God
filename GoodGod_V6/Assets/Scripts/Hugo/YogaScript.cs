using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class YogaScript : MonoBehaviour
{

    [HideInInspector] public bool R_hand_active = false;
    [HideInInspector] public bool L_hand_active = false;

    public InputActionProperty RpinchAnimationAction;
    public InputActionProperty LpinchAnimationAction;
    public GameObject PostProBase, PostProYoga;
    public PostProccSpeedEffect PP_volume;
    public ParticleSystem yoga_particles;
    public AudioSource YogaAudio;

    // Start is called before the first frame update
    void Start()
    {
      
        PP_volume.saveNormalSettings();
        PP_volume.saveVFXSettings();
    }

    // Update is called once per frame
    void Update()
    {
        PP_volume.SaveCurrentValuesPostProc();
    }

    public void activate_yoga_hand(int hand_id,bool state)
    {
        if(hand_id== 0)
        {
            R_hand_active= state;
        }
        if (hand_id == 1)
        {
            L_hand_active = state;
        }

        if(R_hand_active && L_hand_active)
        {
            if (TempleScript.instance.journees<7)
                StartCoroutine(yoga_running());
        }
        else
        {
            YogaAudio.Stop();
            StopAllCoroutines();
            yoga_particles.Stop();
            PP_volume.PostProcessToNormal();
            PostProBase.gameObject.SetActive(true);
            PostProYoga.gameObject.SetActive(false);
            Time.timeScale= 1.0f;
        }


    }


     IEnumerator yoga_running()
    {
        YogaAudio.Play();
        yoga_particles.Play();

        PostProBase.gameObject.SetActive(false);
        PostProYoga.gameObject.SetActive(true);

        PP_volume.PostProcessToVFX();

        while (true)
        {
            //Debug.Log(RpinchAnimationAction.action.ReadValue<float>() + LpinchAnimationAction.action.ReadValue<float>());

            Time.timeScale = (RpinchAnimationAction.action.ReadValue<float>() + LpinchAnimationAction.action.ReadValue<float>()) * 3f;

            yoga_particles.playbackSpeed = (RpinchAnimationAction.action.ReadValue<float>() + LpinchAnimationAction.action.ReadValue<float>());

            yield return new WaitForEndOfFrame();
        }

    }


}
