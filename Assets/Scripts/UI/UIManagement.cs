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
        
    }

    private void Update()
    {
        if(MainMenu.instance.IsInGameOptions)
            pickUpAmountText.text = pickUpAmountSlider.value + " %";
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
}
