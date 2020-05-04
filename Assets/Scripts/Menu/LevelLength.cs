using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLength : MonoBehaviour
{
    public static LevelLength instance = null;

    public int levelLength;
    public Slider levelLengthSlider;
    [SerializeField] Text levelLengthText;

    private void Awake()
    {
        levelLength = (int)levelLengthSlider.value;
    }
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        levelLengthText.text = levelLengthSlider.value.ToString();
        levelLength = (int)levelLengthSlider.value;
    }
}
