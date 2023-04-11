using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class PauseScript : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject reglageMenu;
    public GameObject controlMenu;

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                ContinuerLaPartie();
                QuitterReglage();
                QuitterControl();
            }
            else
            {
                Pause();
            }
        }
        */
    }
    
    //pause switch sur XR interraction
    /*
    public void ContinuerLaPartie()
    {
        Debug.Log("continue");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        //AudioManager.Instance.musicSource.Stop();
        gameIsPaused = true;
    }
    */
    public void Reglages()
    {
        reglageMenu.SetActive(true);
        pauseMenuUI.SetActive(false);
        gameIsPaused = true;
    }
    public void Control()
    {
        controlMenu.SetActive(true);
        pauseMenuUI.SetActive(false);
        reglageMenu.SetActive(false);
        gameIsPaused = true;
    }
    public void QuitterReglage()
    {
        reglageMenu.SetActive(false);
        gameIsPaused = false;
    }
    public void QuitterControl()
    {
        controlMenu.SetActive(false);
        gameIsPaused = false;
    }
    public void QuitterGame()
    {
        Debug.Log(" Leave the game ");
        Application.Quit();
    }
    public void RestartGame()
    {
        Debug.Log("Reload the scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    
    public void Retour()
    {
        //Pause();
        pauseMenuUI.SetActive(true);
        QuitterReglage();
        QuitterControl();
    }
    // -----------------------------------------------------------------------------
    // -----------------------------------------------------------------------------
}
