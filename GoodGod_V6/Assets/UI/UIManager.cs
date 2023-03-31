using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }
    public void MusicVolume()
    {
        _musicSlider.maxValue = AudioManager.Instance.musicChangeMultiplier * 2f;
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }
    public void SFXVolume()
    {
        _sfxSlider.maxValue = AudioManager.Instance.volumeChangeMultiplier * 2f;
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }
}
