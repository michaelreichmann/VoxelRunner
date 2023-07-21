using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] beats;
    public float volume;
    private AudioSource source;
    private int clipIndex = 0;
    private Coroutine playMusicCoroutine;
    public float fadeTime;
   
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        playMusicCoroutine = StartCoroutine(PlayOnLoop());
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOut(fadeTime));
        StopCoroutine(playMusicCoroutine);
    }


    private IEnumerator PlayOnLoop()
    {
        while(true)
        {
            //random index
            if (Random.Range(0, 2) == 0)
            {
                clipIndex = Random.Range(0, beats.Length);
            }

            source.volume = volume;

            //get and play clip
            AudioClip clip = beats[clipIndex];
            source.clip = clip;
            source.Play();
            
            yield return new WaitForSeconds(clip.length);
        }

    }

    private IEnumerator FadeOut(float fadeTime)
    {
        float time = 0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;

            source.volume = Mathf.Lerp(volume, 0f, time / fadeTime);

            yield return null;
        }
    }
}
