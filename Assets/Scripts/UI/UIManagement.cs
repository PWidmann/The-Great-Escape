using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    // UIElements that have to be enabled/disabled and/or used with the Menuinteraction

    public static UIManagement instance = null;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundFxSlider;

    public Text masterVolSliderTextCount;
    public Text musicVolSliderTextCount;
    public Text sfxVolSliderTextCount;

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
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this);
    }
}
