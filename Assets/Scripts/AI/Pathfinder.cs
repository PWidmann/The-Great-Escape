using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the AI find a path to the raft
/// </summary>
public class Pathfinder : MonoBehaviour
{
    [SerializeField] Transform raftTransform;

    /// <summary>
    /// Called in an event, finds a path on walkable tiles to the raft 
    /// </summary>
    public void Move()
    {
        //gameObject.SetActive(true);
        transform.position = Vector2.MoveTowards(transform.position, GetWalktargetLocation(), Time.deltaTime *
            AIController.instance.movementSpeed);
    }

    private Vector3 GetWalktargetLocation()
    {
        Vector3 raftTargetLocation = raftTransform.position; // Current position of raft.
        Vector3 enemyLocation = transform.position; // current enemy position

        // The vector from the raft to the enemy
        Vector3 targetToEnemy = enemyLocation - raftTargetLocation; // directional Vector

        Vector3Int targetCellLocation = Vector3Int.zero; // The cell where the enemy has to go.

        // Which cell the enemy is currently located on.
        Vector3Int enemyCellLocation = TileMapGenerator.instance.groundTilemap.WorldToCell(enemyLocation);

        bool isAboveRaft = targetToEnemy.y > 0; 

        // The postion of the raft in the cell.
        Vector3Int raftTargetCellLocation = TileMapGenerator.instance.groundTilemap.WorldToCell(raftTargetLocation);

        // mapHeight is the number of tiles in the y axis
        // Scans the map from the top to the buttom till a GrassTile is found and saves the grid position of the cell
        for (int i = 1; i <= (int)TileMapGenerator.instance.mapHeight; i++)
        {
            if (TileMapGenerator.instance.groundTilemap.HasTile(raftTargetCellLocation + i * 
                (isAboveRaft ? Vector3Int.up : Vector3Int.down)))
            {
                targetCellLocation = raftTargetCellLocation + (i+1) * (isAboveRaft ? Vector3Int.up : Vector3Int.down);
                break;
            }
        }

        // The targettile has been found. Now the enemy has to know which direction he needs to go.
        // => Pathfinding algo // backtracker
        Vector3Int walkTargetCellLocation = targetCellLocation; // The positon the enemy needs to walk to
        Vector3Int previousWalkTargetCellLocation = Vector3Int.zero; // When a new target is found, the previous target is needed

        // The vector between the target cell and the enemy cell
        Vector3Int walkTargetCellToEnemyCell = enemyCellLocation  - targetCellLocation; 

        // Dictates if enemy goes up or down and when he has to go left.
        for (int i = 0; i < 100; i++)
        {
            walkTargetCellLocation += (Mathf.Abs(walkTargetCellToEnemyCell.x) <= Mathf.Abs(walkTargetCellToEnemyCell.y) ? 
                Vector3Int.left : (walkTargetCellToEnemyCell.y >= 0 ? Vector3Int.up : Vector3Int.down));

            walkTargetCellToEnemyCell = enemyCellLocation - walkTargetCellLocation;

            if (walkTargetCellLocation == enemyCellLocation)
            {
                walkTargetCellLocation = previousWalkTargetCellLocation;
                return TileMapGenerator.instance.groundTilemap.CellToWorld(walkTargetCellLocation);
            }

            previousWalkTargetCellLocation = walkTargetCellLocation;
        }
        return enemyLocation + (TileMapGenerator.instance.groundTilemap.CellToWorld(targetCellLocation) - enemyLocation);
    }
}
