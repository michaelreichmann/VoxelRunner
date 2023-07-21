using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoController : MonoBehaviour
{
    public GameObject logoPrefab;
    private GameObject logo;
    public Vector3 logoPos;

    private GameObject lightObject;
    private Light light;
    public float lightStartIntensity;
    public float lightFadeTime;


    public void CreateLogo()
    {
        //instantiate
        logo = Instantiate(logoPrefab);

        //set parent
        logo.transform.parent = transform;

        //set position
        logo.transform.position = logoPos;

        lightObject = logo.transform.GetChild(0).gameObject;
        light = lightObject.GetComponent<Light>();
        light.intensity = lightStartIntensity;
    }

    public void DestroyLogo()
    {
        Destroy(logo);

    }


    public void AddGravity()
    { 
        foreach (Transform child in logo.transform)
        {
            //add rigidbody
            Rigidbody logoRigidBody;

            if (child.gameObject.GetComponent<Rigidbody>() == null && child.gameObject.name != "Light")
            {
                logoRigidBody = child.gameObject.AddComponent<Rigidbody>();
            }
        }

        StartCoroutine(Light(lightFadeTime));

    }


    private IEnumerator Light(float fadeTime)
    {
        float time = 0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;

            light.intensity = Mathf.Lerp(lightStartIntensity, 0f, time / fadeTime);

            yield return null;
        }
    }

}
