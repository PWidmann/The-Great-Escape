using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
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
    public GameObject[] pickUps = new GameObject[4];
    public Transform instanceGrouping;

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
                        groundTilemap.SetTile(new Vector3Int(x, y, 0), tileArray[0]);
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

        for (int x = 0; x < mapWidth - 20; x++)
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
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (mapArray[x, y] == 1)
                {
                    int rnd = Random.Range(0, 100);
                    int pickupItem = Random.Range(0, 3);

                    if (rnd > 98)
                    {
                        switch (pickupItem)
                        {
                            case 0:
                                Instantiate(pickUps[0], new Vector3(x, y, -3f), pickUps[0].transform.rotation).transform.SetParent(instanceGrouping);
                                break;
                            case 1:
                                Instantiate(pickUps[1], new Vector3(x, y, -3f), pickUps[1].transform.rotation).transform.SetParent(instanceGrouping);
                                break;
                        }
                        
                    }
                }
            }
        }
    }
}
