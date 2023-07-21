using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;


public class PlayerController : MonoBehaviour
{
    //input system
    private PlayerInput playerInput;
    private InputAction leftRight;
    private float leftRightValue;

    //movement
    [Header("Movement")]
    public bool isMoving;
    public float speed;
    private GameObject playerCube;
    private Rigidbody cubeRigidbody;
    public float playerMaxVelocityStart;
    public float playerMaxVelocity;
    public float playerVelocity;
    public float decelerationFactor;
    public float playerMinVelocity;
    public float steeringForce;

    //camera
    public GameObject cam;
    public Vector3 camPositionOffset;
    private Vector3 camStartPositon;
    public float smoothSpeed;

    //environment
    [Header("Environment")]
    public GameObject environment;
    public float playerCubePosZ;
    private float zPosThresh;
    private float tileZPosition;

    //obstacles
    [Header("Obstacles")]
    public float obstacleDestroyTime = 2;
    public float randomForceRange = 1;
    public int numObstaclesAttached = 1;

    //game logic
    private int numAttempt = 0;
    public GameController gameController;
    public bool pause;

    //sound
    public SoundManager soundManager;
    public MusicController MusicController;

    //light
    private GameObject light;

    private void Awake()
    {
        //input system
        playerInput = new PlayerInput();

        //get playercube
        playerCube = transform.GetChild(1).gameObject;

        //get rigidbody
        cubeRigidbody = playerCube.GetComponent<Rigidbody>();

        //get camera
        cam = transform.GetChild(0).gameObject;
        camStartPositon = cam.transform.position;

        //get light
        light = transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        //environment
        zPosThresh = environment.GetComponent<CreateEnvironment>().zPosThresh;
    }


    public void StartGame()
    {
        pause = false;

        //environment
        zPosThresh = environment.GetComponent<CreateEnvironment>().zPosThresh;

        //movement
        playerMaxVelocity = playerMaxVelocityStart;

        //obstacles
        numObstaclesAttached = 1;

        numAttempt += 1;

        //player position
        if (numAttempt != 1)
        {
            playerCube.transform.position = new Vector3(0, 8.5f, 0);
            playerCube.transform.eulerAngles = new Vector3(30, 0, 0);
        }

        cam.transform.position = camStartPositon;
    }

    public void EnableLight()
    {
        light.SetActive(true);
    }

    public void DisableLight()
    {
        light.SetActive(false);
    }

    public void PauseGame()
    {
        if(pause)
        {
            pause = false;
            //turn on sound
            soundManager.diageticInPause = false;
            //stop music
            MusicController.StartMusic();


            //add force
            cubeRigidbody.AddForce(transform.forward * 500);
        }
        else
        {
            pause = true;
            //turn off sound
            soundManager.diageticInPause = true;
            //stop music
            MusicController.StopMusic();
        }
    }

    private void OnEnable()
    {
        //input system
        leftRight = playerInput.Player.LeftRight;
        leftRight.Enable();

        playerInput.Player.Jump.performed += DoJump;
        playerInput.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        //input system
        leftRight.Disable();
        playerInput.Player.Jump.Disable();
    }


    private void Update()
    {
        //screen tap controll
        if (Input.GetMouseButton(0) && isMoving && !pause)
        {
            if (Input.mousePosition.x < Screen.width * 0.5)
            {
                leftRightValue = -1;
            }
            else
            {
                leftRightValue = 1;
            }
        }

        //Input System
        else
        {
            leftRightValue = leftRight.ReadValue<float>();
        }
    }


    private void FixedUpdate()
    {
        if (gameController.logoAnimationIsDone)
        {
            //cam follows player (x and y is restricted)
            Vector3 targetPos = new Vector3(camPositionOffset.x, camPositionOffset.y, playerCube.transform.position.z + camPositionOffset.z);
            Vector3 smoothFollow = Vector3.Lerp(cam.transform.position, targetPos, smoothSpeed);
            Vector3 smoothFollow2 = Vector3.Lerp(smoothFollow, targetPos, smoothSpeed);

            cam.transform.position = smoothFollow2;
        }

        //move player 
        if (isMoving && !pause)
        {
            //move player forward
            playerVelocity = cubeRigidbody.velocity.magnitude;
            if (playerVelocity < playerMaxVelocity)
            {
                cubeRigidbody.AddForce(transform.forward * speed);
            }

            //move player sideways
            cubeRigidbody.AddForce(transform.right * leftRightValue * steeringForce);

            CheckVelocity();
        }

        //environment
        playerCubePosZ = playerCube.transform.position.z;
        tileZPosition = environment.GetComponent<CreateEnvironment>().tileZPosition;

        if (playerCubePosZ > tileZPosition - zPosThresh)
        {
            environment.GetComponent<CreateEnvironment>().SetNewFloorPosition();
        }
    }

    //remove all obstacles
    public void RemoveObstacles()
    {
        foreach (Transform child in playerCube.transform)
        {
            //add rigidbody
            Rigidbody obstacleRigidBody;

            if (child.gameObject.GetComponent<Rigidbody>() == null)
            {
                obstacleRigidBody = child.gameObject.AddComponent<Rigidbody>();

                //add force
                StartCoroutine(AddForceToObstacle(obstacleRigidBody, obstacleDestroyTime));

                //destroy obstacle
                StartCoroutine(DestroyObstacle(child.gameObject, obstacleDestroyTime));
            }
        }
    }

    //add force to obstacle
    IEnumerator AddForceToObstacle(Rigidbody obstacleToAddForce, float destroyTime)
    {
        //create random force
        Vector3 randomForce = new Vector3(Random.Range(-randomForceRange, randomForceRange), Random.Range(-randomForceRange, randomForceRange), Random.Range(-randomForceRange, randomForceRange));

        float forceTime = 0f;
        while (forceTime < (destroyTime / 2))
        {
            forceTime += Time.deltaTime;

            obstacleToAddForce.AddForce(randomForce);

            yield return null;
        }
    }


    //destroy obstacle
    IEnumerator DestroyObstacle(GameObject obstacleToDetroy, float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(obstacleToDetroy);
    }

    //input system
    private void DoJump(InputAction.CallbackContext obj)
    {

        Debug.Log("jump");
    }

    //check velocity
    public void CheckVelocity()
    {
        if (playerMaxVelocity < playerMinVelocity)
        {
            isMoving = false;
            gameController.EndGame();
        }
    }

    public void AddGravity()
    {
        if (playerCube.GetComponent<Rigidbody>() == null)
        {
            cubeRigidbody = playerCube.AddComponent<Rigidbody>();
        }
    }

    public void RemoveGravity()
    {
        if (playerCube.GetComponent<Rigidbody>() != null)
        {
            cubeRigidbody = playerCube.GetComponent<Rigidbody>();
            Destroy(cubeRigidbody);
        }
    }
}
