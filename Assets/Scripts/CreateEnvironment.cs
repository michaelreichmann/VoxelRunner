using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnvironment : MonoBehaviour
{
    [Header("Floors")]
    private GameObject[] tiles;
    public float zPosThresh = 2;
    public float tileZPosition;
    private float floorLength;
    private float floorWidth;
    private int activeTile;

    [Header("tileCubes")]
    private GameObject tileCubes;
    public GameObject tileCubePrefab;
    public int numtileCubes;
    public float tileCubeScaleMinimum;
    public float tileCubeScaleMaximum;
    public float tileCubeStickOutMinimum;
    public float tileCubeStickOutMaximum;

    [Header("Obstacle")]
    public GameObject obstaclePrefab;
    public int initialNumObstacles;
    public int numObstacles;
    public int numObstalcesIncrement;
    public float obstacleScaleMinimum;
    public float obstacleScaleMaximum;

    [Header("Item")]
    public GameObject itemPrefab;
    public int numItems;
    public Material itemMaterial;

    private void Awake()
    {
        //get floors
        tiles = new GameObject[2];
        tiles[0] = transform.GetChild(0).gameObject;
        tiles[1] = transform.GetChild(1).gameObject;

        //set active floor
        activeTile = 0;

        //get floor size
        GameObject floor = tiles[activeTile].transform.GetChild(0).gameObject;
        floorLength = floor.transform.localScale.z;
        floorWidth = floor.transform.localScale.x;

        //initialize position
        tileZPosition = tiles[activeTile].transform.position.z;
    }

    private void Start()
    {
        //reset num obstacles
        ResetNumObstacles();

        //tile 1
        CreateTileCubes(0);
        //tile 2
        CreateTileCubes(1);

        PrepareTile();

        SpawnObstacles();

        SpawnItems();

        //item material
        StartCoroutine(ItemMaterialCoroutine(1));
    }

    public void RestartGame()
    {
        //reset the transform
        tiles[1 - activeTile].transform.position = new Vector3(0, 0, 0);
        tiles[activeTile].transform.position = new Vector3(0, 0, 100);

        //initialize position
        tileZPosition = tiles[activeTile].transform.position.z;
    }

    private void CreateTileCubes(int tileNum)
    {
        //get tilecubes
        tileCubes = tiles[tileNum].transform.GetChild(4).gameObject;

        //Create tilecubes
        for (int i = 0; i < numtileCubes; i++)
        {
            //create tileCube
            GameObject tilecube = Instantiate(tileCubePrefab);

            //set parent
            tilecube.transform.parent = tileCubes.transform;

            //set obstacle name
            tilecube.name = "tilecube_" + i;
        }
    }

    public void ResetNumObstacles()
    {
        numObstacles = initialNumObstacles;
    }


    public void PrepareTile()
    {
        //get tilecubes
        tileCubes = tiles[activeTile].transform.GetChild(4).gameObject;

        //set tilecubeParams
        for (int i = 0; i < numtileCubes; i++)
        {
            GameObject child = tileCubes.transform.GetChild(i).gameObject;

            bool isWall = false;

            if (Random.Range(0, 100) > 33)
            {
                isWall = true;
            }

            //set color
            Color randomColor = new Color(Random.Range(0.1f,1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
            child.GetComponent<Renderer>().material.SetColor("_Color", randomColor);

            //set tileCube scale
            float randomtileCubeScale = Random.Range(tileCubeScaleMinimum, tileCubeScaleMaximum);
            child.transform.localScale = new Vector3(randomtileCubeScale, randomtileCubeScale, randomtileCubeScale);

            //set tileCube position
            if (!isWall)
            {
                float yPos = Random.Range(tileCubeStickOutMinimum, tileCubeStickOutMaximum) - (randomtileCubeScale / 2);
                child.transform.position = new Vector3(Random.Range(-floorWidth / 2, floorWidth / 2), yPos, Random.Range(-floorLength / 2, floorLength / 2) + tileZPosition);
            }
            else
            {
                float xPosLeft = Random.Range(tileCubeStickOutMinimum, tileCubeStickOutMaximum) - (randomtileCubeScale / 2);
                float xPosRight = (randomtileCubeScale / 2) - Random.Range(tileCubeStickOutMinimum, tileCubeStickOutMaximum);

                //50% chance for either wall
                if (Random.Range(0, 2) == 0)
                {
                    child.transform.position = new Vector3(xPosLeft - (floorWidth / 2), Random.Range(0, floorWidth), Random.Range(-floorLength / 2, floorLength / 2) + tileZPosition);
                }
                else
                {
                    child.transform.position = new Vector3(xPosRight + (floorWidth / 2), Random.Range(0, floorWidth), Random.Range(-floorLength / 2, floorLength / 2) + tileZPosition);
                }
            }
        }
    }

    public void SetNewFloorPosition()
    {
        //set other floor active
        if (activeTile == 0)
        {
            activeTile = 1;
        }
        else
        {
            activeTile = 0;
        }

        //set new position
        tileZPosition += floorLength;
        tiles[activeTile].transform.position = new Vector3(0, 0, tileZPosition);

        //remove obstacles
        foreach (Transform child in tiles[activeTile].transform.GetChild(3).transform)
        {
            Destroy(child.gameObject);
        }

        //create obstacles
        SpawnObstacles();

        //delete items
        foreach (Transform child in tiles[activeTile].transform.GetChild(5).transform)
        {
            Destroy(child.gameObject);
        }

        //create item
        SpawnItems();

        //prepare tile
        PrepareTile();
    }

    public void SpawnObstacles()
    {
        for (int i = 0; i < numObstacles; i++)
        {
            //create obstacle
            GameObject obstacle = Instantiate(obstaclePrefab);

            //set obstacle scale
            float randomObstacleScale = Random.Range(obstacleScaleMinimum, obstacleScaleMaximum);
            obstacle.transform.localScale = new Vector3(randomObstacleScale, randomObstacleScale, randomObstacleScale);

            //set parent
            obstacle.transform.parent = tiles[activeTile].transform.GetChild(3).transform;

            //set obstacle position
            obstacle.transform.position = new Vector3(Random.Range(-floorWidth / 2, floorWidth / 2), randomObstacleScale / 2 + tileCubeStickOutMaximum, Random.Range(-floorLength / 2, floorLength / 2) + tileZPosition);


            //set color
            Color randomColor = new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));
            obstacle.GetComponent<Renderer>().material.SetColor("_Color", randomColor);

            //set obstacle name
            obstacle.name = "obstacle_" + i;
        }

        numObstacles += numObstalcesIncrement;
    }

    public void SpawnItems()
    {
        for(int i = 0; i < numItems; i++)
        {
            //create item
            GameObject item = Instantiate(itemPrefab);

            //scale
            float randomObstacleScale = Random.Range(obstacleScaleMinimum * 2, obstacleScaleMaximum / 0.75f);
            item.transform.localScale = new Vector3(randomObstacleScale, randomObstacleScale, randomObstacleScale);

            //set parent
            item.transform.parent = tiles[activeTile].transform.GetChild(5).transform;

            //set obstacle position
            item.transform.position = new Vector3(Random.Range(-floorWidth / 2, floorWidth / 2), 1, Random.Range(-floorLength / 2, floorLength / 2) + tileZPosition);

            //set obstacle name
            item.name = "item";
        }
    }

    IEnumerator ItemMaterialCoroutine(float time)
    {
        while (true)
        {
            //set material
            itemMaterial.SetInt("_pixelate", Random.Range(2, 25));

            yield return new WaitForSeconds(time);

            //set new time
            time = Random.Range(0.1f, 1f);
        }
    }
}
