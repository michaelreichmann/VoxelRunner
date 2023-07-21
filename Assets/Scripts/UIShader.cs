using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShader : MonoBehaviour
{
    //min max values
    [SerializeField]
    private int pixelateMin = 2;
    [SerializeField]
    private int pixelateMax = 30;

    public Material material;

    private Coroutine UICoroutine;

    private void Start()
    {
        StartUICoroutine();
    }

    public void StartUICoroutine()
    {
        UICoroutine = StartCoroutine(UIMaterialCoroutine());
    }

    public void StopUICoroutine()
    {
        StopCoroutine(UICoroutine);
    }


    IEnumerator UIMaterialCoroutine()
    {
        while(true)
        {
            //set new time
            float time = Random.Range(0.1f, 1f);

            //set material
            material.SetFloat("_Pixelate", Random.Range(pixelateMin, pixelateMax));

            yield return new WaitForSeconds(time);

        }
    }


}