using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    public UIController uIController;
    public LogoController logoController;
    public CreateEnvironment createEnvironment;
    public SoundManager soundManager;
    public MusicController musicController;

    public float startAnimationDuration;
    public float endAnimationDuration;

    public bool logoAnimationIsDone = false;

    private void Start()
    {
        ShowStart();
    }

    public void ShowStart()
    {
        uIController.ShowStart();
    }

    //Start the game
    public void StartGame()
    {
        logoAnimationIsDone = false;
        soundManager.diageticInPause = false;
        playerController.StartGame();
        StartCoroutine(StartAnimation(startAnimationDuration));
        createEnvironment.RestartGame();
    }


    IEnumerator StartAnimation(float duration)
    {
        playerController.DisableLight();

        logoController.CreateLogo();

        uIController.ShowNone();


        yield return new WaitForSeconds(duration);
        logoController.AddGravity();

        soundManager.PlayAnimationSound();


        yield return new WaitForSeconds(0.3f);
        playerController.AddGravity();

        playerController.isMoving = true;

        soundManager.PlayWind();

        musicController.StartMusic();

        yield return new WaitForSeconds(duration + 1f);
        logoAnimationIsDone = true;

        playerController.EnableLight();

        uIController.ShowStats();
        uIController.ShowPauseButton();


        yield return new WaitForSeconds(duration + 2f);
        logoController.DestroyLogo();
    }


    //End the game
    public void EndGame()
    {
        playerController.PauseGame();
        StartCoroutine(EndAnimation(endAnimationDuration));
        playerController.RemoveGravity();
        musicController.StopMusic();
    }

    IEnumerator EndAnimation(float duration)
    {
        uIController.ShowGameOver();

        yield return new WaitForSeconds(duration);

        uIController.ShowEnd();
        playerController.RemoveObstacles();

        createEnvironment.ResetNumObstacles();
        createEnvironment.SetNewFloorPosition();
        createEnvironment.ResetNumObstacles();
        createEnvironment.SetNewFloorPosition();
    }
}
