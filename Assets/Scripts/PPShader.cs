using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class PPShader : MonoBehaviour
{
    private int pixelate;

    //materials
    private Material defaultMaterial;
    private Material pixelMaterial;

    //pick up obstacle
    private float time;

    //choose chader
    string shaderType;

    private Coroutine pixelCoroutine;

    private void Start()
    {
        //create shader materials
        defaultMaterial = new Material(Shader.Find("Hidden/DefaultShader"));
        pixelMaterial = new Material(Shader.Find("Hidden/PixelShader"));
    }


    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (shaderType == "pixelate")
        {
            pixelMaterial.SetFloat("_pixelate", pixelate);
            Graphics.Blit(source, destination, pixelMaterial);
        }

        //default shader
        else
        {
            Graphics.Blit(source, destination, defaultMaterial);
        }
    }

    public void PickUpObstacle(float pixTime, int minPix, int maxPix)
    {
        if (pixelCoroutine != null)
        {
            StopCoroutine(pixelCoroutine);
        }
        pixelCoroutine = StartCoroutine(ChangePixelate(pixTime, minPix, maxPix));
    }

    //change pixelate
    IEnumerator ChangePixelate(float pixTime, int minPix, int maxPix)
    {
        shaderType = "pixelate";

        time = 0f;
        while (time < pixTime)
        {
            time += Time.deltaTime;

            pixelate = (int)Mathf.Lerp(minPix, maxPix, time / pixTime);

            yield return null;
        }

        shaderType = "default";
    }
}