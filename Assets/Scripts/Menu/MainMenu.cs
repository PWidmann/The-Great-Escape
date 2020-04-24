using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int mainSelectedButton = 1;
    int optionSelected = 1;

    public static MainMenu instance = null;

    [Header("Start Button")]
    [SerializeField] Button startButton;
    [SerializeField] Text startButtonText;

    [Header("Options Button")]
    [SerializeField] Button optionsButton;
    [SerializeField] Text optionsButtonText;

    [Header("Quit Button")]
    [SerializeField] Button quitButton;
    [SerializeField] Text quitButtonText;

    [Header("Options Menu")]
    [SerializeField] Image masterVolumeSliderImage;
    [SerializeField] Image musicVolumeSliderImage;
    [SerializeField] Image soundFxVolumeSliderImage;
    [SerializeField] Button saveButton;
    [SerializeField] Text saveButtonText;
    [SerializeField] Text masterVolumeText;
    [SerializeField] Text musicVolumeText;
    [SerializeField] Text soundFxVolumeText;

    [Header("Sound Values")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundFxVolumeSlider;

    bool pauseMenuActive = true;

    // Gamepad
    bool oneTimeStickMovement = false;
    static bool controllerMovementStopped = false;

    public int MainMenuSelectedButton { get => mainSelectedButton; set => mainSelectedButton = value; }
    public int OptionMenuSelectedButton { get => optionSelected; set => optionSelected = value; }
    public static bool ControllerMovementStopped { get => controllerMovementStopped; set => controllerMovementStopped = value; }

    bool isInOptions = false;

    private void Awake()
    {
        if (instance = null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
        ControllerInput();

        ChangeButtonAppearance(mainSelectedButton, optionSelected);
        ChangeSlider();

    }

    public void ButtonStart()
    {
        pauseMenuActive = false;
        SceneManager.LoadScene("Pre Game");
    }

    public void ButtonOptions()
    {
        isInOptions = true;
        optionSelected = 1;

        UIManagement.instance.SetOptionsMenuElementsActive(true);
        UIManagement.instance.SetMainMenuElementsActive(false);
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void ButtonSaveSettings()
    {
        PlayerPrefs.Save();

        UIManagement.instance.SetOptionsMenuElementsActive(false);
        UIManagement.instance.SetMainMenuElementsActive(true);

        isInOptions = false;
    }
    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && !controllerMovementStopped)
            MoveMenuPointerDown();

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && !controllerMovementStopped)
            MoveMenuPointerUp();

        // Confirm selected button
        if (Input.GetKeyDown(KeyCode.Return))
            ClickOnPointingButton();

    }

    void ControllerInput()
    {
        if (Input.GetAxisRaw("J1Vertical") < 0 && !oneTimeStickMovement && !controllerMovementStopped)
            MoveMenuPointerUp();

        if (Input.GetAxisRaw("J1Vertical") > 0 && !oneTimeStickMovement && !controllerMovementStopped)
            MoveMenuPointerDown();

        if (Input.GetButtonDown("J1ButtonA"))
            ClickOnPointingButton();

        // Reset joystick for only one step per stick movement
        if (Input.GetAxisRaw("J1Vertical") == 0)
            oneTimeStickMovement = false;
    }

    private void ClickOnPointingButton()
    {
        if (isInOptions)
        {
            if (optionSelected == 4)
            {
                ButtonSaveSettings();
            }
        }
        else
        {
            switch (MainMenuSelectedButton)
            {
                case 1:
                    SoundManager.instance.PlayMenuClickSoundFx();
                    ButtonStart();
                    break;
                case 2:
                    SoundManager.instance.PlayMenuClickSoundFx();
                    ButtonOptions();
                    break;
                case 3:
                    SoundManager.instance.PlayMenuClickSoundFx();
                    ButtonQuit();
                    break;
            }
        }
    }

    private void MoveMenuPointerDown()
    {
        SoundManager.instance.PlayMenuPointerSoundFx();

        if (isInOptions)
        {
            optionSelected -= 1;
            if (optionSelected == 0)
            {
                optionSelected = 4;
            }
        }
        else
        {
            mainSelectedButton -= 1;
            if (mainSelectedButton == 0)
            {
                mainSelectedButton = 3;
            }
        }
        oneTimeStickMovement = true;
    }

    private void MoveMenuPointerUp()
    {
        SoundManager.instance.PlayMenuPointerSoundFx();

        if (isInOptions)
        {
            optionSelected += 1;
            if (optionSelected == 5)
            {
                optionSelected = 1;
            }
        }
        else
        {
            MainMenuSelectedButton += 1;
            if (MainMenuSelectedButton == 4)
            {
                MainMenuSelectedButton = 1;
            }
        }

        oneTimeStickMovement = true;
    }

    void ChangeButtonAppearance(int _selectedButton, int _optionSelected)
    {
        if (isInOptions)
        {
            switch (_optionSelected)
            {
                case 1:
                    SliderColorTurnSelected(masterVolumeSliderImage, masterVolumeText);

                    SliderColorTurnUnselected(musicVolumeSliderImage, musicVolumeText);
                    SliderColorTurnUnselected(soundFxVolumeSliderImage, soundFxVolumeText);
                    ButtonColorTurnUnselected(saveButton, saveButtonText);
                    break;
                case 2:
                    SliderColorTurnSelected(musicVolumeSliderImage, musicVolumeText);

                    SliderColorTurnUnselected(masterVolumeSliderImage, masterVolumeText);
                    SliderColorTurnUnselected(soundFxVolumeSliderImage, soundFxVolumeText);
                    ButtonColorTurnUnselected(saveButton, saveButtonText);
                    break;
                case 3:
                    SliderColorTurnSelected(soundFxVolumeSliderImage, soundFxVolumeText);

                    SliderColorTurnUnselected(masterVolumeSliderImage, masterVolumeText);
                    SliderColorTurnUnselected(musicVolumeSliderImage, musicVolumeText);
                    ButtonColorTurnUnselected(saveButton, saveButtonText);
                    break;
                case 4:
                    ButtonColorTurnSelected(saveButton, saveButtonText);

                    SliderColorTurnUnselected(masterVolumeSliderImage, masterVolumeText);
                    SliderColorTurnUnselected(musicVolumeSliderImage, musicVolumeText);
                    SliderColorTurnUnselected(soundFxVolumeSliderImage, soundFxVolumeText);
                    break;
            }
        }
        else
        {
            switch (_selectedButton)
            {
                case 1:
                    ButtonColorTurnSelected(startButton, startButtonText);

                    ButtonColorTurnUnselected(optionsButton, optionsButtonText);
                    ButtonColorTurnUnselected(quitButton, quitButtonText);
                    break;
                case 2:
                    ButtonColorTurnSelected(optionsButton, optionsButtonText);

                    ButtonColorTurnUnselected(startButton, startButtonText);
                    ButtonColorTurnUnselected(quitButton, quitButtonText);
                    break;
                case 3:
                    ButtonColorTurnSelected(quitButton, quitButtonText);

                    ButtonColorTurnUnselected(startButton, startButtonText);
                    ButtonColorTurnUnselected(optionsButton, optionsButtonText);
                    break;
            }
        }
    }

    void ChangeSlider()
    {
        if (isInOptions && !controllerMovementStopped)
        {
            switch (optionSelected)
            {
                case 1:
                    masterVolumeSlider.value += Input.GetAxisRaw("J1Horizontal");
                    masterVolumeSlider.value += Input.GetAxisRaw("Horizontal");
                    break;
                case 2:
                    musicVolumeSlider.value += Input.GetAxisRaw("J1Horizontal");
                    musicVolumeSlider.value += Input.GetAxisRaw("Horizontal");
                    break;
                case 3:
                    soundFxVolumeSlider.value += Input.GetAxisRaw("J1Horizontal");
                    soundFxVolumeSlider.value += Input.GetAxisRaw("Horizontal");
                    break;
            }
        }
    }

    public void ButtonColorTurnSelected(Button btn, Text txt)
    {
        //Button Color
        ColorBlock colors = btn.colors;
        colors.normalColor = new Color32(114, 114, 114, 255);

        btn.colors = colors;

        txt.color = Color.white;
    }

    public void ButtonColorTurnUnselected(Button btn, Text txt)
    {
        //Button Color
        ColorBlock colors = btn.colors;
        colors.normalColor = Color.white;

        btn.colors = colors;

        txt.color = Color.black;
    }

    public void SliderColorTurnSelected(Image slider, Text txt)
    {
        //Slider Color
        slider.color = Color.white;

        txt.color = Color.white;
    }

    public void SliderColorTurnUnselected(Image slider, Text txt)
    {
        //Slider Color
        slider.color = new Color32(114, 114, 114, 255);

        txt.color = new Color32(114, 114, 114, 255);
    }

    // On pointer event function. Highlights the button over which the mouse hovers or points.
    public void SelectButtonDisplay(int selectedButton)
    {
        MainMenuSelectedButton = selectedButton;
    }

    public void SelectedUIElementInOptions(int optionSelect)
    {
        optionSelected = optionSelect;
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
    }
}

