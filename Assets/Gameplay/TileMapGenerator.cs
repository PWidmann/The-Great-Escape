using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileMapGenerator : MonoBehaviour
{
    [Header("Map Tiles")]
    public Tile[] tileArray = new Tile[10];
    public Tilemap groundTilemap;
    public float mapWidth;
    public float mapHeight;
    public int borderHeight;

    private int[,] mapArray;
    private int riverAmplitude = 5;

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
                        groundTilemap.SetTile(new Vector3Int(x, y, 0), tileArray[1]);
                        break;
                    default:
                        break;
                }
            }
        }
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
        //Vector2Int riverStart = new Vector2Int(10, (int)Math.Round(mapHeight/2, 0));

        Vector2Int riverStart = new Vector2Int(10, 4);

        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                mapArray[riverStart.x + x, riverStart.y + y] = 1;
            }
        }
    }
}
