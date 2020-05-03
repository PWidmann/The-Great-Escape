using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpAmount : MonoBehaviour
{
    public static PickUpAmount instance = null;

    public int pickupAmount;
    public Slider pickUpSlider;
    [SerializeField] Text pickUpText;

    private void Awake()
    {
        pickupAmount = (int)pickUpSlider.value;
    }
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        pickUpText.text = pickUpSlider.value.ToString() + "%";
        pickupAmount = (int)pickUpSlider.value;
    }
}
