using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Vector3 lightOffset;

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = transform.parent.GetChild(1).transform.position;

        transform.position = playerPosition + lightOffset;
        transform.LookAt(playerPosition);
    }
}
