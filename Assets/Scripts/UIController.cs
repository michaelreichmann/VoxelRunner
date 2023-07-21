using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject pauseButton;
    public GameObject stats;
    public GameObject startScreen;
    public GameObject endScreen;
    public GameObject pauseScreen;
    public GameObject gameOverText;


    private void DisableElements()
    {
        pauseButton.SetActive(false);
        startScreen.SetActive(false);
        endScreen.SetActive(false);
        pauseScreen.SetActive(false);
        gameOverText.SetActive(false);
    }

    public void DisableStats()
    {
        stats.SetActive(false);
    }

    public void ShowPauseButton()
    {
        DisableElements();
        pauseButton.SetActive(true);
    }


    public void ShowStats()
    {
        DisableElements();
        stats.SetActive(true);
    }

    public void ShowStart()
    {
        DisableElements();
        startScreen.SetActive(true);
    }

    public void ShowEnd()
    {
        DisableElements();
        endScreen.SetActive(true);
    }

    public void ShowPause()
    {
        DisableElements();
        pauseScreen.SetActive(true);
    }

    public void ShowGameOver()
    {
        DisableElements();
        DisableStats();
        gameOverText.SetActive(true);
    }

    public void ShowNone()
    {
        DisableElements();
    }


}
