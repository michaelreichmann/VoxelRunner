using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsController : MonoBehaviour
{
    public GameObject player;

    private void Update()
    {
        //distance
        float distance = player.GetComponent<PlayerController>().playerCubePosZ;
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "DISTANCE " + (int)Mathf.Floor(distance);

        if(transform.childCount > 2)
        {
            //velocity
            float velocity = player.GetComponent<PlayerController>().playerVelocity;
            transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "VELOCITY " + (int)Mathf.Floor(velocity);

            //size
            int size = player.GetComponent<PlayerController>().numObstaclesAttached;
            transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = "SIZE     " + size;
        }

    }

}
