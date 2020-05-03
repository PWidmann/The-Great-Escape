using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TileMapGenerator : MonoBehaviour
{
    public static TileMapGenerator instance = null;

    [Header("Map Tiles")]
    public Tile[] tileArray = new Tile[10];
    public AnimatedTile[] animatedTileArray = new AnimatedTile[10];
    public Tilemap groundTilemap;
    public Tilemap waterTilemap;
    public float mapWidth;
    public float mapHeight;
    public int borderHeight;

    public Transform player1;
    public Transform player2;
    public Transform player3;
    public Transform player4;

    [Header("Pickup Objects")]
    public float pickupAmount;
    public GameObject[] pickUps = new GameObject[4];
    public Transform instanceGrouping;

    [Header("Plants")]
    public GameObject[] plants = new GameObject[4];

    [Header("EndFlag")]
    public GameObject endFlag;

    int riverWidth = 15;

    [Header("Raft")]
    public GameObject raft;

    public int[,] mapArray;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        mapArray = new int[(int)Math.Round(mapWidth, 0), (int)Math.Round(mapHeight, 0)];


        pickupAmount = PickUpAmount.instance.pickupAmount;

        GenerateTileMap();
    }

    private void GenerateTileMap()
    {
        // Tilemap coordinates 
        // X: Increasing left to right
        // Y: Increasing bottom to top

        GenerateMapArray();
        

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                switch (mapArray[x, y])
                {
                    case 0:
                        // Set grass tile
                        int rnd = Random.Range(0, 3);
                        groundTilemap.SetTile(new Vector3Int(x, y, 0), tileArray[rnd]);
                        break;
                    case 1:
                        // Set water tile
                        waterTilemap.SetTile(new Vector3Int(x, y, 0), animatedTileArray[0]);
                        break;
                    default:
                        break;
                }
            }
        }
        GeneratePickups();
        GeneratePlants();
    }

    public void GenerateMapArray()
    {
        // Fill with grass
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                mapArray[x, y] = 0;
            }
        }

        // Draw river
        // Vector2Int riverStart = new Vector2Int(10, (int)Math.Round(mapHeight/2, 0));

        Vector2Int riverStart = new Vector2Int(10, borderHeight + 10);

        // Place Players
        if (player1)
            player1.position = new Vector3( -10, 2);
        if (player2)
            player2.position = new Vector3(-10, 4);
        if (player3)
            player3.position = new Vector3(-10, 6);
        if (player4)
            player4.position = new Vector3(-10, 8);

        // Place raft
        raft.transform.position = new Vector2(riverStart.x + 6, riverStart.y + 7);

        int riverHeightChange = 1;

        //End Flag Placement
        endFlag.transform.position = new Vector3(mapWidth - 5, (mapHeight / 2) + 2);


        // River
        for (int x = 0; x < mapWidth - 30; x++)
        {
            for (int y = 0; y < riverWidth; y++)
            {
                mapArray[riverStart.x + x, riverStart.y + y] = 1;   
            }

            if (x % 7 == 0)
            {
                riverStart.y += riverHeightChange;


            }
            if (x % 35 == 0 && x != 0)
            {
                riverHeightChange *= -1;
            }
        }
    }

    public void GeneratePickups()
    {
        for (int x = 21; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (mapArray[x, y] == 1) // If on water
                {
                    float rnd = Random.Range(0, 100);
                    int pickupItem = Random.Range(0, 3);
                    int rotationDegreeOption = Random.Range(1, 4);
                    float rotationChange = 0f;

                    switch (rotationDegreeOption)
                    {
                        case 1:
                            rotationChange = 0f;
                            break;
                        case 2:
                            rotationChange = 90f;
                            break;
                        case 3:
                            rotationChange = 180f;
                            break;
                        case 4:
                            rotationChange = 270f;
                            break;
                    }

                    Transform leafObject = pickUps[0].transform;
                    Transform stickObject = pickUps[1].transform;

                    leafObject.Rotate(Vector3.forward * rotationChange);
                    stickObject.Rotate(Vector3.forward * rotationChange);

                    
                    if (rnd < pickupAmount / 4)
                    {
                        switch (pickupItem)
                        {
                            case 0:
                                Instantiate(pickUps[0], new Vector3(x, y, -3f), leafObject.transform.rotation).transform.SetParent(instanceGrouping);
                                break;
                            case 1:
                                Instantiate(pickUps[1], new Vector3(x, y, -3f), stickObject.transform.rotation).transform.SetParent(instanceGrouping);
                                break;
                        }
                    }
                }
            }
        }
    }

    void GeneratePlants()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (mapArray[x, y] == 0) // If on land
                {
                    int rnd = Random.Range(0, 100);
                    int pickupItem = Random.Range(0, 4);


                    //Calc plant number
                    if (rnd > 70) 
                    {
                        switch (pickupItem)
                        {
                            case 0:
                                Instantiate(plants[0], new Vector3(x, y, -3f), plants[0].transform.rotation).transform.SetParent(instanceGrouping);
                                break;
                            case 1:
                                Instantiate(plants[1], new Vector3(x, y, -3f), plants[1].transform.rotation).transform.SetParent(instanceGrouping);
                                break;
                            case 2:
                                Instantiate(plants[2], new Vector3(x, y, -3f), plants[1].transform.rotation).transform.SetParent(instanceGrouping);
                                break;
                            case 3:
                                Instantiate(plants[3], new Vector3(x, y, -3f), plants[1].transform.rotation).transform.SetParent(instanceGrouping);
                                break;
                        }

                    }
                }
            }
        }
    }
}
