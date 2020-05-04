using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public Slider progressSlider;
    public Transform raftObject;
    float mapWidth;

    bool isWidthUpdated = false;

    void Start()
    {
        progressSlider.value = 0;
        mapWidth = TileMapGenerator.instance.mapWidth;
        progressSlider.maxValue = mapWidth - 41;
    }

    void Update()
    {
        if (!isWidthUpdated)
        {
            mapWidth = TileMapGenerator.instance.mapWidth;
            progressSlider.maxValue = mapWidth - 41;
            isWidthUpdated = true;
        }
        
        progressSlider.value += RaftController.instance.change.x * RaftController.instance.moveSpeed * Time.deltaTime;
    }
}
