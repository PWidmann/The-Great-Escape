using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    GameObject raftTransform;
    public static Pathfinder instance;
    Vector3Int targetCellLocation = Vector3Int.zero;
    Vector3Int enemyCellLocation;
    bool isAboveRaft; 

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        raftTransform = AIController.instance.raftTransform;
    }

    public void Move()
    {
        //gameObject.SetActive(true);
        transform.position = Vector2.MoveTowards(transform.position, GetWalktargetLocation(), Time.deltaTime *
            AIController.instance.movementSpeed);
    }

    private Vector3 GetWalktargetLocation()
    {
        Vector3 raftTargetWorldLocation = raftTransform.transform.position;
        Vector3 enemyWorldLocation = transform.position; 

        Vector3 targetToEnemy = enemyWorldLocation - raftTargetWorldLocation; // directional Vector

        enemyCellLocation = TileMapGenerator.instance.groundTilemap.WorldToCell(enemyWorldLocation);

        isAboveRaft = targetToEnemy.y > 0; 

        Vector3Int raftTargetCellLocation = TileMapGenerator.instance.groundTilemap.WorldToCell(raftTargetWorldLocation);

        SearchForGroundTiles(raftTargetCellLocation);
        return TileMapGenerator.instance.groundTilemap.CellToWorld(targetCellLocation);
    }

    void SearchForGroundTiles(Vector3Int raftTargetCellLocation)
    {
        for (int i = 1; i <= (int)TileMapGenerator.instance.mapHeight; i++)
        {
            if (TileMapGenerator.instance.groundTilemap.HasTile(raftTargetCellLocation + i *
                (isAboveRaft ? Vector3Int.up : Vector3Int.down)))
            {
                EnemySpawner.spawnedEnemies[0].GetComponent<Pathfinder>().targetCellLocation = 
                    raftTargetCellLocation + (i + 1) * (isAboveRaft ? Vector3Int.up : Vector3Int.down);

                EnemySpawner.spawnedEnemies[1].GetComponent<Pathfinder>().targetCellLocation =
                    raftTargetCellLocation + (i + 1) * (isAboveRaft ? Vector3Int.up : Vector3Int.down);
                break;
            }
        }
        KeepDistanceBetweenAI();
    }

    void KeepDistanceBetweenAI()
    {
        for (int i = 0; i < EnemySpawner.spawnedEnemies.Count - 2; i++)
        {
            Vector3Int thisEnemyCell = TileMapGenerator.instance.groundTilemap.WorldToCell(
                EnemySpawner.spawnedEnemies[i].transform.position);

            Vector3Int otherEnemyCell = TileMapGenerator.instance.groundTilemap.WorldToCell(
                EnemySpawner.spawnedEnemies[i + 2].transform.position);

            Vector3 thisEnemyPosWorld = TileMapGenerator.instance.groundTilemap.CellToWorld(thisEnemyCell);

            Vector3 otherEnemyPosWorld = TileMapGenerator.instance.groundTilemap.CellToWorld(otherEnemyCell);

            Vector3Int thisEnemyCellToOtherEnemyCell = otherEnemyCell - thisEnemyCell;

            if (Vector2.Distance(thisEnemyPosWorld, otherEnemyPosWorld) < AIController.instance.distanceBetweenAI)
            {
                EnemySpawner.spawnedEnemies[i + 2].GetComponent<Pathfinder>().targetCellLocation =
                    EnemySpawner.spawnedEnemies[i].GetComponent<Pathfinder>().targetCellLocation + (8 *
                    Vector3Int.right + (1 * (isAboveRaft ? Vector3Int.up : Vector3Int.down)));
            }
        }
    }
}
