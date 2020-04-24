using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    // UIElements that have to be enabled/disabled and/or used with the Menuinteraction

    public static UIManagement instance = null;

    [Header("Slider Values")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundFxSlider;

    [Header("Slider Texts")]
    public Text masterVolSliderTextCount;
    public Text musicVolSliderTextCount;
    public Text sfxVolSliderTextCount;

    [Header("Buttons")]
    public Button startButton;
    public Button optionButton;
    public Button quitButton;
    public Button saveSettingsButton;

    public Toggle randomMusicPlayToggle;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(this);
    }

    public void SetOptionsMenuElementsActive(bool isActivated)
    {
        saveSettingsButton.gameObject.SetActive(isActivated);
        masterVolumeSlider.gameObject.SetActive(isActivated);
        soundFxSlider.gameObject.SetActive(isActivated);
        musicVolumeSlider.gameObject.SetActive(isActivated);
        randomMusicPlayToggle.gameObject.SetActive(isActivated);
        
    }

    public void SetMainMenuElementsActive(bool isActivated)
    {
        startButton.gameObject.SetActive(isActivated);
        instance.optionButton.gameObject.SetActive(isActivated);
        instance.quitButton.gameObject.SetActive(isActivated);
    }
}
