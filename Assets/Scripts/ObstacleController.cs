using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private bool isPickedUp = false;

    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "PlayerCube" && !isPickedUp)
        {
            transform.parent = collision.gameObject.transform;
            transform.localRotation = Quaternion.Euler(0,0,0);
            float randomPosX = Random.Range(0, transform.parent.gameObject.transform.localScale.x);
            float randomPosY = Random.Range(0, transform.parent.gameObject.transform.localScale.x);
            float randomPosZ = Random.Range(0, transform.parent.gameObject.transform.localScale.x);

            transform.localPosition = new Vector3(randomPosX, randomPosY, randomPosZ);

            collision.gameObject.transform.parent.GetChild(0).GetComponent<PPShader>().PickUpObstacle(1f, 5, 150);

            //get player
            PlayerController playerController = collision.gameObject.transform.parent.GetComponent<PlayerController>();

            //play sound
            playerController.soundManager.PlayItemObstacleSound();
            playerController.soundManager.PlayPixSound();

            //increment num obstacles
            playerController.numObstaclesAttached += 1;

            //slow player down
            playerController.playerMaxVelocity *= playerController.decelerationFactor;

            //check player velocity
            playerController.CheckVelocity();

            isPickedUp = true;
        }
    }
}
