using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<PlayerController>().soundManager.PlayCubeSound();
    }
}
