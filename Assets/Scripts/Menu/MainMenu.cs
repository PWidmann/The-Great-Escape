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
    [SerializeField] ButtonElement startButton;
    [SerializeField] ButtonElement optionButton;
    [SerializeField] ButtonElement quitButton;
    [SerializeField] ButtonElement saveSettingsButton;

    [SerializeField] SliderElement masterVolSlider;
    [SerializeField] SliderElement musicVolSlider;
    [SerializeField] SliderElement soundFxVolSlider;


    bool isPauseMenuActive = false;

    // Gamepad
    bool oneTimeStickMovement = false;
    static bool controllerMovementStopped = false;

    bool isInOptions = false;
    string loadedSceneName;

    public int MainMenuSelectedButton { get => mainSelectedButton; set => mainSelectedButton = value; }
    public int OptionMenuSelectedButton { get => optionSelected; set => optionSelected = value; }
    public static bool ControllerMovementStopped { get => controllerMovementStopped; set => controllerMovementStopped = value; }
    public bool IsInOptions { get => isInOptions; set => isInOptions = value; }
    public bool IsPauseMenuActive { get => isPauseMenuActive; set => isPauseMenuActive = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        loadedSceneName = SceneManager.GetActiveScene().name;
        if (loadedSceneName.Equals("Main Menu"))
            isPauseMenuActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPauseMenuActive)
        {
            KeyboardInput();
            ControllerInput();

            ChangeButtonAppearance(mainSelectedButton, optionSelected);
            ChangeSliderValue();
        }

    }

    public void ButtonStart()
    {
        isPauseMenuActive = false;
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
                ButtonSaveSettings();
        }
        else
        {
            switch (MainMenuSelectedButton)
            {
                case 1:
                    SoundManager.instance.PlayMenuClickSoundFx();
                    DecideWhichEventMethodToCall();
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

    void DecideWhichEventMethodToCall()
    {
        if (loadedSceneName.Equals("Main Menu"))
            ButtonStart();
        else
            ResumeGame();
    }

    void MoveMenuPointerDown()
    {
        SoundManager.instance.PlayMenuPointerSoundFx();

        if (isInOptions)
        {
            optionSelected -= 1;
            if (optionSelected == 0)
                optionSelected = 4;
        }
        else
        {
            mainSelectedButton -= 1;
            if (mainSelectedButton == 0)
                mainSelectedButton = 3;
        }
        oneTimeStickMovement = true;
    }

    void MoveMenuPointerUp()
    {
        SoundManager.instance.PlayMenuPointerSoundFx();

        if (isInOptions)
        {
            optionSelected += 1;
            if (optionSelected == 5)
                optionSelected = 1;
        }
        else
        {
            mainSelectedButton += 1;
            if (mainSelectedButton == 4)
                mainSelectedButton = 1;
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
                    masterVolSlider.IsSelectedSlider = true;
                    musicVolSlider.IsSelectedSlider = false;
                    soundFxVolSlider.IsSelectedSlider = false;
                    saveSettingsButton.IsSelectedButton = false;
                    break;
                case 2:
                    musicVolSlider.IsSelectedSlider = true;
                    masterVolSlider.IsSelectedSlider = false;
                    soundFxVolSlider.IsSelectedSlider = false;
                    saveSettingsButton.IsSelectedButton = false;
                    break;
                case 3:
                    soundFxVolSlider.IsSelectedSlider = true;
                    musicVolSlider.IsSelectedSlider = false; 
                    masterVolSlider.IsSelectedSlider = false;
                    saveSettingsButton.IsSelectedButton = false;
                    break;
                case 4:
                    saveSettingsButton.IsSelectedButton = true;
                    soundFxVolSlider.IsSelectedSlider = false;
                    musicVolSlider.IsSelectedSlider = false;
                    masterVolSlider.IsSelectedSlider = false;
                    break;
            }
        }
        else
        {
            switch (_selectedButton)
            {
                case 1:
                    quitButton.IsSelectedButton = false;
                    startButton.IsSelectedButton = true;
                    optionButton.IsSelectedButton = false;
                    break;
                case 2:
                    startButton.IsSelectedButton = false;
                    optionButton.IsSelectedButton = true;
                    quitButton.IsSelectedButton = false;
                    break;
                case 3:
                    optionButton.IsSelectedButton = false;
                    quitButton.IsSelectedButton = true;
                    startButton.IsSelectedButton = false;
                    break;
            }
        }
    }

    void ChangeSliderValue()
    {
        if (isInOptions && !controllerMovementStopped)
        {
            switch (optionSelected)
            {
                case 1:
                    UIManagement.instance.masterVolumeSlider.value += Input.GetAxisRaw("J1Horizontal");
                    UIManagement.instance.masterVolumeSlider.value += Input.GetAxisRaw("Horizontal");
                    break;
                case 2:
                    UIManagement.instance.musicVolumeSlider.value += Input.GetAxisRaw("J1Horizontal");
                    UIManagement.instance.musicVolumeSlider.value += Input.GetAxisRaw("Horizontal");
                    break;
                case 3:
                    UIManagement.instance.soundFxSlider.value += Input.GetAxisRaw("J1Horizontal");
                    UIManagement.instance.soundFxSlider.value += Input.GetAxisRaw("Horizontal");
                    break;
            }
        }
    }

    // On pointer event function. Highlights the button over which the mouse hovers or points.
    public void SelectButtonDisplay(int selectedButton)
    {
        mainSelectedButton = selectedButton;
    }

    public void SelectedUIElementInOptions(int optionSelect)
    {
        optionSelected = optionSelect;
    }

    public void ResumeGame()
    {
        UIManagement.instance.pauseMenuPanel.SetActive(false);
        isPauseMenuActive = false;
        Time.timeScale = 1;
    }

    public void OpenPauseMenu(PlayerController playerController = null)
    {
        isPauseMenuActive = true;
        Time.timeScale = 0;
        UIManagement.instance.pauseMenuPanel.gameObject.SetActive(true);
    }
}

