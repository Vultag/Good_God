using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] AudioMixer audioMixer;
    public Slider _musicSlider, _sfxSlider, _voicesSlider;
    /*
    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }
    */



    public void MusicVolume()
    {
        //_musicSlider.maxValue = AudioManager.Instance.musicChangeMultiplier * 2f;
        //AudioManager.Instance.MusicVolume(_musicSlider.value);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(_musicSlider.value) * 20) ;
    }
    public void SFXVolume()
    {
        //_sfxSlider.maxValue = AudioManager.Instance.volumeChangeMultiplier * 2f;
        //AudioManager.Instance.SFXVolume(_sfxSlider.value);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(_sfxSlider.value) * 20);
    }
    public void VoicesVolume()
    {
        audioMixer.SetFloat("VoicesVolume", Mathf.Log10(_voicesSlider.value) * 20);
    }
}
