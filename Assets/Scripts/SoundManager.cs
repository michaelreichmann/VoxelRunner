using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Player")]
    public AudioClip[] cubeSounds;
    public float cubeSoundsVolume;
    private AudioSource playerSource;

    [Header("Player Wind")]
    public GameObject playerCube;
    public PlayerController playerController;
    public AudioClip windSound;
    public float windVolume;
    private AudioSource windSource;

    [Header("item/Obstacle")]
    public AudioClip[] itemObstacleSounds;
    public float itemObstacleVolume;
    private AudioSource itemObstacleSource;

    [Header("Intro Animation")]
    public AudioClip[] animationSounds;
    public float animationSoundsVolume;
    private AudioSource animationSource;

    [Header("Pixelate")]
    public AudioClip[] pixSounds;
    public float pixVolume;
    private AudioSource pixSource;

    [Header("UI")]
    public AudioClip uiSound;
    public float uiVolume;
    private AudioSource uiSource;

    public float vel;


    public bool diageticInPause;

    private void Start()
    {
        playerSource = gameObject.AddComponent<AudioSource>();
        windSource = gameObject.AddComponent<AudioSource>();
        animationSource = gameObject.AddComponent<AudioSource>();
        itemObstacleSource = gameObject.AddComponent<AudioSource>();
        pixSource = gameObject.AddComponent<AudioSource>();
        uiSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (playerController.isMoving && !playerController.pause)
        {
            float height = playerCube.transform.position.y;
            float velocity = playerController.playerVelocity / playerController.playerMaxVelocityStart;

            height = height < 0f ? 0f : height;
            height *= 0.5f;
            height = height > 1f ? 1f : height;
            height = Mathf.Pow(height, 2f);

            windSource.volume = velocity * height * windVolume;

            playerSource.volume = cubeSoundsVolume * velocity;
            vel = velocity;

        }
        else
        {
            windSource.volume = 0;
        }

    }

    //play sound
    public void PlayWind()
    {
        windSource.clip = windSound;
        windSource.loop = true;
        windSource.Play();
    }

    //play sound
    public void PlayCubeSound()
    {
        if (!diageticInPause)
        {
            playerSource.clip = cubeSounds[Random.Range(0, cubeSounds.Length)];
            playerSource.Play();
        }
    }

    //play sound
    public void PlayItemObstacleSound()
    {
        if (!diageticInPause)
        {
            itemObstacleSource.volume = itemObstacleVolume;

            itemObstacleSource.clip = itemObstacleSounds[Random.Range(0, itemObstacleSounds.Length)];
            itemObstacleSource.Play();
        }
    }



    //play sound
    public void PlayAnimationSound()
    {
        if (!diageticInPause)
        {
            animationSource.volume = animationSoundsVolume;

            animationSource.clip = animationSounds[Random.Range(0, animationSounds.Length)];
            animationSource.Play();

        }
    }

    //play sound
    public void PlayPixSound()
    {
        if (!diageticInPause)
        {
            pixSource.volume = pixVolume;

            pixSource.clip = pixSounds[Random.Range(0, pixSounds.Length)];
            pixSource.Play();

        }
    }

    //play sound
    public void PlayUISound()
    {
        uiSource.volume = uiVolume;

        uiSource.clip = uiSound;
        uiSource.Play();
    }
}
