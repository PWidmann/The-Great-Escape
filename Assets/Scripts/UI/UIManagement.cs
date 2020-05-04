using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    // UIElements that have to be enabled/disabled and/or used with the Menuinteraction

    public static UIManagement instance = null;

    [Header("Menu Panels")]
    public GameObject mainMenu;
    public GameObject gameOptionsMenu;
    public GameObject soundOptionsMenu;

    [Header("Sound Options Menu")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundFxSlider;
    public Text masterVolSliderTextCount;
    public Text musicVolSliderTextCount;
    public Text sfxVolSliderTextCount;
    public Toggle randomMusicPlayToggle;

    [Header("Game Options Menu")]
    public Text difficultyText;
    public Slider pickUpAmountSlider;
    public Text pickUpAmountText;

    public Slider levelLengthSlider;
    public Text levelLengthText;

    public int pickUpAmount;
    public int levelLength;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        pickUpAmount = 55;
        levelLength = 300;
    }

    private void Update()
    {
        // Change visible values in game option menu
        if (MainMenu.instance.IsInGameOptions)
        {
            pickUpAmountText.text = pickUpAmountSlider.value + " %";
            levelLengthText.text = levelLengthSlider.value.ToString();
        } 
    }

    


    public void SetOptionsMenuElementsActive()
    {
        
    }

    public void SetMainMenuElementsActive()
    {
        
    }

    public void PlayClickSound()
    {
        SoundManager.instance.PlayMenuClickSoundFx();
    }

    public void OnValueChangePickUpAmount()
    {
        pickUpAmount = (int)pickUpAmountSlider.value;
        PlayerPrefs.SetInt("pickUpAmount", (int)pickUpAmountSlider.value);
        Debug.Log("Setpickupamount" + (int)pickUpAmountSlider.value);
    }

    public void OnValueChangeLevelLength()
    {
        levelLength = (int)levelLengthSlider.value;
        PlayerPrefs.SetInt("levelLength", (int)levelLengthSlider.value);
        Debug.Log("SetLevellength" + (int)levelLengthSlider.value);
    }
}
