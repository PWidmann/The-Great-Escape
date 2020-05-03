using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public Slider progressSlider;
    public Transform raftObject;
    float mapWidth;

    void Start()
    {
        progressSlider.value = 0;
        mapWidth = TileMapGenerator.instance.mapWidth;
        progressSlider.maxValue = mapWidth - 41;
    }

    // Update is called once per frame
    void Update()
    {
        progressSlider.value += RaftController.instance.change.x * RaftController.instance.moveSpeed * Time.deltaTime;
    }
}
