using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public static bool allEnemiesInstantiated = false;
    static GameObject previousEnemy;
    public static List<GameObject> spawnedEnemies = new List<GameObject>(); 

    public static GameObject PreviousEnemy { get => previousEnemy; set => previousEnemy = value; }

    void Awake()
    {
        SpawnAiObjects(AIController.instance.aiDifficulty);
    }

    void SpawnAiObjects(AiDifficulty difficulty)
    {
        previousEnemy = Instantiate(enemies[0], new Vector3(6, 0, 0), Quaternion.identity);
        spawnedEnemies.Add(previousEnemy);

        for (int i = 1; i < (int)difficulty; i++)
        {
            if (i % 2 == 0)
            {
                previousEnemy = Instantiate(enemies[i], new Vector3(previousEnemy.transform.position.x + 20, 0, 0),
                    Quaternion.identity);
                spawnedEnemies.Add(previousEnemy);
            }
            else
            {
                previousEnemy = Instantiate(enemies[i], new Vector3(previousEnemy.transform.position.x + 20, 48, 0),
                    Quaternion.identity);
                spawnedEnemies.Add(previousEnemy);
            }
        }
        allEnemiesInstantiated = true;
    }
}
