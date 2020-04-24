using UnityEngine;
using UnityEngine.UI;

public class SliderElement : MonoBehaviour
{
    Image thisSliderImage;
    Text thisSliderText;
    bool isSelectedSlider;

    public bool IsSelectedSlider { get => isSelectedSlider; set => isSelectedSlider = value; }

    void Start()
    {
        thisSliderImage = GetComponentInChildren<Image>();
        thisSliderText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (isSelectedSlider)
            SliderColorTurnSelected();
        else
            SliderColorTurnUnselected();
              
    }

    public void SliderColorTurnSelected()
    {
        thisSliderImage.color = Color.white;

        thisSliderText.color = Color.white;
    }

    public void SliderColorTurnUnselected()
    {
        thisSliderImage.color = new Color32(114, 114, 114, 255);

        thisSliderText.color = new Color32(114, 114, 114, 255);
    }
}
