using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Depricated
/// </summary>
public class Movement : MonoBehaviour
{
    [SerializeField] Transform targetRaftObject;
    [SerializeField] int movementSpeed = 1;

    float closestDistanceToRaft = Mathf.Infinity;
    bool[,] isWalkable;
    bool hasCheckedTiles = false;
    Vector3 targetTile;
    
    public void Move()
    {
        gameObject.SetActive(true);
        /*transform.position = Vector2.MoveTowards(
            transform.position, targetRaftObject.position, Time.deltaTime * movementSpeed);*/
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(targetRaftObject.transform.position.x, 
                targetRaftObject.transform.position.y - closestDistanceToRaft * 2), Time.deltaTime *
                movementSpeed);
    }

    private void Update()
    {
        if (!hasCheckedTiles)
        {
            // For every tile will be placed a boolean which checks if the tile is walkable or not.
            isWalkable = new bool[(int)TileMapGenerator.instance.mapWidth, (int)TileMapGenerator.instance.mapHeight];
            for (int x = 0; x < TileMapGenerator.instance.mapWidth; x++)
            {
                for (int y = 0; y < TileMapGenerator.instance.mapHeight; y++)
                {
                    // The GrassTiles are placed on the ground tilemap
                    // -> if GetTile goes through the grid where the water should be, the ground tilemap has no tiles
                    // => So GetTile == null.
                    if (TileMapGenerator.instance.groundTilemap.GetTile(new Vector3Int(x, y, 0)) == null)
                        isWalkable[x, y] = false;
                    else if (TileMapGenerator.instance.groundTilemap.GetTile(new Vector3Int(x, y, 0)).name.Equals(
                        "GrassTile"))
                        isWalkable[x, y] = true;
                    else if (TileMapGenerator.instance.groundTilemap.GetTile(new Vector3Int(x, y, 0)).name.Equals(
                        "WaterTile"))
                        isWalkable[x, y] = false;
                }
            }
            hasCheckedTiles = true;
        }
        else
        {
            // Goes through whole map
            for (int x = 0; x < TileMapGenerator.instance.mapWidth; x++)
            {
                for (int y = 0; y < TileMapGenerator.instance.mapHeight; y++)
                {
                    if (isWalkable[x, y])
                    {
                        Vector3 currentTile = TileMapGenerator.instance.groundTilemap.CellToWorld(
                            new Vector3Int(x, y, 0));
                        float distanceToRaft = Vector2.Distance(targetRaftObject.position, currentTile);
                        if (distanceToRaft < closestDistanceToRaft)
                        {
                            closestDistanceToRaft = distanceToRaft;
                            targetTile = currentTile;
                        }
                    }




                }


            }
        }
        Debug.Log("[AI] ClosestDistanceToRaft: " + closestDistanceToRaft);
    }
}
