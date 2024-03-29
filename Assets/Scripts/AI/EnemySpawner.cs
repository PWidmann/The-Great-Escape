﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    static GameObject previousEnemy;
    public static List<GameObject> spawnedEnemies = new List<GameObject>(); 

    public static GameObject PreviousEnemy { get => previousEnemy; set => previousEnemy = value; }

    void Awake()
    {
        SpawnAiObjects();
    }

    void Update()
    {
        if (PlayerInterface.instance.gameOver || PlayerInterface.instance.win)
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                Destroy(spawnedEnemies[i]);
                spawnedEnemies.Remove(spawnedEnemies[i]);
                AIController.RaftHooked = false;
                AIController.IsMakingAction = false;
                RaftController.AllPlayersOnRaft = false;
            }
        }
    }

    void SpawnAiObjects()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (i % 2 == 0)
            {
                previousEnemy = Instantiate(enemies[i], new Vector3(6, 0, 0),
                    Quaternion.identity);
                spawnedEnemies.Add(previousEnemy);
            }
            else
            {
                previousEnemy = Instantiate(enemies[i], new Vector3(6, 48, 0),
                    Quaternion.identity);
                spawnedEnemies.Add(previousEnemy);
            }
        }
    }
}
