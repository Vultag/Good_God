using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSkybox : MonoBehaviour
{
    //public float speed = 1.0f;
    //public Color daySkyColorTopV3, daySkyColorBottomV3, nightSkyColorTopV3, nightSkyColorBottomV3, horizonColorV3;
    //public Color daySkyColorTopEndGame, daySkyColorBottomEndGame, nightSkyColorTopEndGame, nightSkyColorBottomEndGame, horizonColorEndGame;

    //[SerializeField] private Color daySkyColorTopV3SaveColor = new Color(0.7718939f, 0.8621526f, 0.9245283f, 1f);
    //[SerializeField] private Color daySkyColorBottomV3SaveColor = new Color(0.8820755f, 0.9292453f,1f , 0f) ;
    //[SerializeField] private Color nightSkyColorTopV3SaveColor = new Color(0.8926519f, 0.885502f, 0.9339623f, 1f);
    //[SerializeField] private Color nightSkyColorBottomV3SaveColor = new Color(0.01176471f, 0.04705878f, 0.1882352f,0f);
    //[SerializeField] private Color horizonColorV3SaveColor = new Color(0.8584906f, 8306026f, 0.6762638f, 1f);

    //public Material originalSkyboxV3, endGameSkybox;

    //[SerializeField] private RenderSettings _fog;
    //[SerializeField] private Color fogColor = new Color(0.9686275f, 0.9002734f, 0.8509804f);
    //[SerializeField] private Color fogColorEndGame;
    //[SerializeField] private float fogIntensity = 0f;

    //[SerializeField] private Color customColorDaySkyTop = new Color(0.7718939f, 0.8621526f, 0.9245283f, 1f);
    //[SerializeField] private Color customColorDaySkyBottom = new Color(0.8820755f, 0.9292453f, 1f, 0f);
    //[SerializeField] private Color customColornightSkyTop = new Color(0.8926519f, 0.885502f, 0.9339623f, 1f);
    //[SerializeField] private Color customColornightSkyBottom = new Color(0.01176471f, 0.04705878f, 0.1882352f, 0f);
    //[SerializeField] private Color customColorHorizon = new Color(0.8584906f, 8306026f, 0.6762638f, 1f);

    public float ChangeTimeBase, ChangeTimeEnd;
    public Material SkyboxeBase, SkyboxActuelle, EndSkybox;

    void Start()
    {
        StartCoroutine(ResetSkybox());
        //startTime = Time.time;

        //// Récupere le material de la skybox
        //originalSkyboxV3 = RenderSettings.skybox;

        //// Récupere les couleurs de la skybox
        //daySkyColorTopV3 = originalSkyboxV3.GetColor("Color_BE31CDF2");
        //daySkyColorTopEndGame = endGameSkybox.GetColor("Color_BE31CDF2");

        //daySkyColorBottomV3 = originalSkyboxV3.GetColor("Color_68FD0CD8");
        //daySkyColorBottomEndGame = endGameSkybox.GetColor("Color_68FD0CD8");

        //nightSkyColorTopV3 = originalSkyboxV3.GetColor("Color_D230EFA1");
        //nightSkyColorTopEndGame = endGameSkybox.GetColor("Color_D230EFA1");

        //nightSkyColorBottomV3 = originalSkyboxV3.GetColor("Color_83CF459");
        //nightSkyColorBottomEndGame = endGameSkybox.GetColor("Color_83CF459");

        //horizonColorV3 = originalSkyboxV3.GetColor("Color_1EB49FED");
        //horizonColorEndGame = endGameSkybox.GetColor("Color_1EB49FED");
    }

    void Update()
    {
    }
    // Fonction qui retourne la couleur lerp entre 2 couleurs fournies
    //public Color LerpSkyboxColor(Color beforeColor, Color currentColor, Color endColor) 
    //{
    //    float t = (Time.time - startTime) * speed;
    //    currentColor = Color.Lerp(beforeColor, endColor, t);
    //    return (currentColor);
    //}
    //public void SwitchSkyboxColor()
    //{
    //    // customColor vaut lerp(skyboxColorStart , skyboxColorEnd)
    //    customColorDaySkyTop = LerpSkyboxColor(daySkyColorTopV3, daySkyColorTopV3, daySkyColorTopEndGame);
    //    customColorDaySkyBottom = LerpSkyboxColor(daySkyColorBottomV3, daySkyColorBottomV3, daySkyColorBottomEndGame);
    //    customColornightSkyTop = LerpSkyboxColor(nightSkyColorTopV3, nightSkyColorTopV3, nightSkyColorTopEndGame);
    //    customColornightSkyBottom = LerpSkyboxColor(nightSkyColorBottomV3, nightSkyColorBottomV3, nightSkyColorBottomEndGame);
    //    customColorHorizon = LerpSkyboxColor(horizonColorV3, horizonColorV3, horizonColorEndGame);

    //    // Assigne la couleur qui est lerp au shader de la skybox
    //    originalSkyboxV3.SetColor("Color_BE31CDF2", customColorDaySkyTop);
    //    originalSkyboxV3.SetColor("Color_68FD0CD8", customColorDaySkyBottom);
    //    originalSkyboxV3.SetColor("Color_D230EFA1", customColornightSkyTop);
    //    originalSkyboxV3.SetColor("Color_83CF459", customColornightSkyBottom);
    //    originalSkyboxV3.SetColor("Color_1EB49FED", customColorHorizon);
    //}

    //public void SetFogToEndGameSettings()
    //{
    //    fogColor = LerpSkyboxColor(fogColor, fogColor, fogColorEndGame);
    //    fogIntensity = Mathf.Lerp(0, 0.05f, Time.time * speed);
    //}

    public IEnumerator ResetSkybox()
    {
        while (ChangeTimeBase < 3)
        {
            ChangeTimeBase += 1f * Time.deltaTime;
            RenderSettings.skybox.Lerp(SkyboxActuelle, SkyboxeBase, 10f * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

    }

    public IEnumerator TransitionSkybox()
    {
        while (ChangeTimeEnd < 3)
        {
            ChangeTimeEnd += 1f *Time.deltaTime;
            RenderSettings.skybox.Lerp(SkyboxActuelle, EndSkybox, 1f * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

    }
}
