using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


public class PostProccSpeedEffect : MonoBehaviour
{
    UnityEngine.Rendering.Universal.Vignette vignette;
    UnityEngine.Rendering.Universal.ColorAdjustments colorAjustements;
    UnityEngine.Rendering.Universal.Bloom bloom;

    UnityEngine.Rendering.Universal.Vignette vignetteVFX;
    UnityEngine.Rendering.Universal.ColorAdjustments colorAjustementsVFX;
    UnityEngine.Rendering.Universal.Bloom bloomVFX;

    [SerializeField] private VolumeProfile normalVolumeProfile;

    public GameObject emptyVFXProfil;
    [SerializeField] private VolumeProfile vfxVolumeProfile;


    // ---------------------------- VARIABLES POST PROCESS --------------------------------------------------------
    // ----------- NORMAL VALUES -----------
    //Bloom
    [SerializeField] private float normalValueBloomIntensity;
    [SerializeField] private float vFXValueBloomIntensity;
    [SerializeField] private float currentValueBloomIntensity;

    [SerializeField] private float normalValueBloomThreshold;
    [SerializeField] private float vFXValueBloomThreshold;
    [SerializeField] private float currentValueBloomThreshold;

    [SerializeField] private float normalValueBloomScatter;
    [SerializeField] private float vFXValueBloomScatter;
    [SerializeField] private float currentValueBloomScatter;
    // Vignette
    [SerializeField] private Color normalValueVignetteColor = Color.white;
    [SerializeField] private Color vFXValueVignetteColor = Color.black;
    [SerializeField] private Color currentValueVignetteColor;

    [SerializeField] private float normalValueVignetteIntensity;
    [SerializeField] private float vFXValueVignetteIntensity;
    [SerializeField] private float currentValueVignetteIntensity;

    [SerializeField] private float normalValueSaturation;
    [SerializeField] private float vFXValueSaturation;
    [SerializeField] private float currentValueSaturation;

    // Color adjustments
    [SerializeField] private float normalValueExposure;
    [SerializeField] private float normalValueContrast;

    // ----------- VFX VALUES -----------
    // Color adjustments
    [SerializeField] private float vFXValueExposure;
    [SerializeField] private float vFXValueContrast;

    // ----------- CURRENT VALUES -----------
    // Color adjustments
    [SerializeField] private float currentValueExposure;
    [SerializeField] private float currentValueContrast;

    [SerializeField] float intensity;


    private void Awake()
    {
        normalVolumeProfile = GetComponent<Volume>()?.profile;
        vfxVolumeProfile = emptyVFXProfil.GetComponent<Volume>().profile;

        if (!normalVolumeProfile) throw new System.NullReferenceException(nameof(normalVolumeProfile));

        // Recuperation des settings du profil de post process normal
        if (!normalVolumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        if (!normalVolumeProfile.TryGet(out colorAjustements)) throw new System.NullReferenceException(nameof(colorAjustements));
        if (!normalVolumeProfile.TryGet(out bloom)) throw new System.NullReferenceException(nameof(bloom));

        // Recuperation des settings du profile de post process VFX
        if (!vfxVolumeProfile.TryGet(out vignetteVFX)) throw new System.NullReferenceException(nameof(vignetteVFX));
        if (!vfxVolumeProfile.TryGet(out colorAjustementsVFX)) throw new System.NullReferenceException(nameof(colorAjustementsVFX));
        if (!vfxVolumeProfile.TryGet(out bloomVFX)) throw new System.NullReferenceException(nameof(bloomVFX));
    }

    void Start()
    {
        //saveNormalSettings();
        //saveVFXSettings();
    }
    private void FixedUpdate()
    {
        //Debug.Log("The current value of scatter   :   " + currentValueBloomScatter);
    }
    void Update()
    {
        /*
        SaveCurrentValuesPostProc(); // Permet de track les valeurs dans l'inspecteur

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SPACE The current value of scatter   :   " + currentValueBloomScatter);
            PostProcessToVFX();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PostProcessToNormal();
        }
        */
    }
    public void SaveCurrentValuesPostProc() // Save default values if values are changed in inspector
    {
        currentValueBloomIntensity = bloom.intensity.value;
        currentValueBloomThreshold = bloom.threshold.value;
        currentValueBloomScatter = bloom.scatter.value;

        currentValueVignetteColor = vignette.color.value;
        currentValueVignetteIntensity = vignette.intensity.value;

        currentValueExposure = colorAjustements.postExposure.value;
        currentValueContrast = colorAjustements.contrast.value;
        currentValueSaturation = colorAjustements.saturation.value;
    }
   
    public void PostProcessToVFX() 
    {
        bloom.intensity.value = Mathf.Lerp(normalValueBloomIntensity, vFXValueBloomIntensity, 1f);
        bloom.threshold.value = -Mathf.Lerp(-vFXValueBloomThreshold , -normalValueBloomThreshold, 0.1f);
        bloom.scatter.value = Mathf.Lerp(normalValueBloomScatter, vFXValueBloomScatter, 1f);

        vignette.color.value = Color.Lerp(normalValueVignetteColor, vFXValueVignetteColor, 0.1f * Time.time);
        vignette.intensity.value = Mathf.Lerp(normalValueVignetteIntensity, vFXValueVignetteIntensity, 1f);

        colorAjustements.postExposure.value = -((Mathf.Lerp(vFXValueExposure + 1, normalValueExposure + 1, Time.time * 0.1f) - 1));
        colorAjustements.contrast.value = -Mathf.Lerp(normalValueContrast, vFXValueContrast, 0.1f * Time.time);
        colorAjustements.saturation.value = -(Mathf.Lerp(normalValueSaturation, -vFXValueSaturation, Time.time));
    }

    public void PostProcessToNormal() 
    {
        colorAjustements.postExposure.value = Mathf.Lerp(currentValueExposure, normalValueExposure, 1f);
        colorAjustements.contrast.value = Mathf.Lerp(currentValueContrast, normalValueContrast, 1f);
        colorAjustements.saturation.value = Mathf.Lerp(currentValueSaturation, normalValueSaturation, 1f);

        vignette.color.value = Color.Lerp(currentValueVignetteColor, normalValueVignetteColor, 1f);
        vignette.intensity.value = Mathf.Lerp(currentValueVignetteIntensity, normalValueVignetteIntensity, 1f);

        bloom.intensity.value = Mathf.Lerp(currentValueBloomIntensity, normalValueBloomIntensity, 1f);
        bloom.threshold.value = Mathf.Lerp(currentValueBloomThreshold, normalValueBloomThreshold, 1f);
        bloom.scatter.value = Mathf.Lerp(currentValueBloomScatter, normalValueBloomScatter, 1f);
    }

    public void saveVFXSettings()
    {
        vFXValueBloomIntensity = bloomVFX.intensity.value;
        vFXValueBloomThreshold = bloomVFX.threshold.value;
        vFXValueBloomScatter = bloomVFX.scatter.value;

        vFXValueVignetteColor = vignetteVFX.color.value;
        vFXValueVignetteIntensity = vignetteVFX.intensity.value;

        vFXValueExposure = colorAjustementsVFX.postExposure.value;
        vFXValueContrast = colorAjustementsVFX.contrast.value;
        vFXValueSaturation = colorAjustementsVFX.saturation.value;
    }
    public void saveNormalSettings()
    {
        normalValueBloomIntensity = bloom.intensity.value;
        normalValueBloomThreshold = bloom.threshold.value;
        normalValueBloomScatter = bloom.scatter.value;

        normalValueVignetteColor = vignette.color.value;
        normalValueVignetteIntensity = vignette.intensity.value;

        normalValueExposure = colorAjustements.postExposure.value;
        normalValueContrast = colorAjustements.contrast.value;
        normalValueSaturation = colorAjustements.saturation.value;
    }
}
