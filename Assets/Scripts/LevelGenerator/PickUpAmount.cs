using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpAmount : MonoBehaviour
{
    public static PickUpAmount instance = null;

    public int pickupAmount;
    [SerializeField] Slider pickUpSlider;
    [SerializeField] Text pickUpText;

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
