using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "PlayerCube")
        {
            //get player
            PlayerController playerController = collision.gameObject.transform.parent.GetComponent<PlayerController>();

            //if player touches item
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                collision.gameObject.GetComponent<PlayerController>().RemoveObstacles();
            }
            //if obstacle attached to player touches item
            else
            {
                collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>().RemoveObstacles();
            }

            //reset num Obstacles
            playerController.numObstaclesAttached = 1;

            //reset maximum velocity
            playerController.playerMaxVelocity = playerController.playerMaxVelocityStart;

            Destroy(gameObject);

            //shader
            collision.gameObject.transform.parent.GetChild(0).GetComponent<PPShader>().PickUpObstacle(collision.gameObject.transform.parent.GetComponent<PlayerController>().obstacleDestroyTime, 150, 10);

            //play sound
            playerController.soundManager.PlayItemObstacleSound();
            playerController.soundManager.PlayPixSound();

        }
    }
}
