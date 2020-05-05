using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int mainMenuSelectedButton = 1;
    int gameOptionSelectedButton = 1;
    int soundOptionSelectedButton = 1;
    int pauseMenuSelectedButton = 2;

    public static MainMenu instance = null;
    [Header("Main Menu")]
    [SerializeField] ButtonElement startButton;
    [SerializeField] ButtonElement gameOptionButton;
    [SerializeField] ButtonElement soundOptionButton;
    [SerializeField] ButtonElement quitButton;

    [Header("Sound Option Menu")]
    [SerializeField] ButtonElement saveSoundSettingsButton;
    [SerializeField] SliderElement masterVolSlider;
    [SerializeField] SliderElement musicVolSlider;
    [SerializeField] SliderElement soundFxVolSlider;

    [Header("Game Option Menu")]
    [SerializeField] ButtonElement saveOptionSettingsButton;
    [SerializeField] ButtonElement easyButton;
    [SerializeField] ButtonElement mediumButton;
    [SerializeField] ButtonElement hardButton;
    [SerializeField] SliderElement pickUpAmountSlider;
    [SerializeField] SliderElement levelLengthSlider;

    bool isPauseMenuActive = false;

    // Gamepad
    bool oneTimeStickMovement = false;
    static bool controllerMovementStopped = false;

    bool isInGameOptionsMenu = false;
    bool isInSoundMenu = false;
    bool isInPauseMenu = false;
    public string loadedSceneName;

    AiDifficulty aiDifficulty = new AiDifficulty();

    public int MainMenuSelectedButton { get => mainMenuSelectedButton; set => mainMenuSelectedButton = value; }
    public int OptionMenuSelectedButton { get => gameOptionSelectedButton; set => gameOptionSelectedButton = value; }
    public int SoundMenuSelectedButton { get => soundOptionSelectedButton; set => soundOptionSelectedButton = value; }
    public int PauseMenuSelectedButton { get => pauseMenuSelectedButton; set => pauseMenuSelectedButton = value; }
    public static bool ControllerMovementStopped { get => controllerMovementStopped; set => controllerMovementStopped = value; }
    public bool IsInGameOptions { get => isInGameOptionsMenu; set => isInGameOptionsMenu = value; }
    public bool IsInSoundOptions { get => isInSoundMenu; set => isInSoundMenu = value; }
    public bool IsInPauseMenu { get => isInPauseMenu; set => isInPauseMenu = value; }
    public bool IsPauseMenuActive { get => isPauseMenuActive; set => isPauseMenuActive = value; }
    public AiDifficulty AiDifficulty { get => aiDifficulty; set => aiDifficulty = value; }

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
        Scene m_Scene = SceneManager.GetActiveScene();
        loadedSceneName = m_Scene.name;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
        ControllerInput();

        ChangeButtonAppearance(mainMenuSelectedButton, gameOptionSelectedButton, soundOptionSelectedButton, pauseMenuSelectedButton);
        ChangeSliderValue();

    }

    public void ButtonStart()
    {
        isPauseMenuActive = false;
        
        SceneManager.LoadScene("Pre Game");
        //UIManagement.instance.gameOptionsMenu.SetActive(true);
    }
    public void ButtonGameOptions()
    {
        isInGameOptionsMenu = true;
        gameOptionSelectedButton = 1;

        if (PlayerPrefs.HasKey("pickUpAmount"))
            UIManagement.instance.pickUpAmountSlider.value = PlayerPrefs.GetInt("pickUpAmount");
        else
            UIManagement.instance.pickUpAmountSlider.value = 55;

        if (PlayerPrefs.HasKey("levelLength"))
            UIManagement.instance.levelLengthSlider.value = PlayerPrefs.GetInt("levelLength");
        else
            UIManagement.instance.levelLengthSlider.value = 1000;

        UIManagement.instance.mainMenu.SetActive(false);
        UIManagement.instance.gameOptionsMenu.SetActive(true);
    }

    public void ButtonSoundOptions()
    {
        isInSoundMenu = true;
        soundOptionSelectedButton = 1;

        UIManagement.instance.mainMenu.SetActive(false);
        UIManagement.instance.soundOptionsMenu.SetActive(true);
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void ButtonSaveSettings()
    {
        PlayerPrefs.Save();

        UIManagement.instance.soundOptionsMenu.SetActive(false);
        UIManagement.instance.gameOptionsMenu.SetActive(false);
        UIManagement.instance.mainMenu.SetActive(true);

        isInSoundMenu = false;
        isInGameOptionsMenu = false;
        mainMenuSelectedButton = 1;
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
        if (loadedSceneName.Equals("Main Menu"))
        {
            if (isInGameOptionsMenu)
            {
                switch (gameOptionSelectedButton)
                {
                    case 1:
                        SetDifficultyEasy();
                        break;
                    case 2:
                        SetDifficultyMedium();
                        break;
                    case 3:
                        SetDifficultyHard();
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        ButtonSaveSettings();
                        break;
                }

            }
            else if (isInSoundMenu)
            {
                switch (soundOptionSelectedButton)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        ButtonSaveSettings();
                        break;
                }
            }
            else if (!isInSoundMenu && !isInGameOptionsMenu)
            {
                switch (MainMenuSelectedButton)
                {
                    case 1:
                        SoundManager.instance.PlayMenuClickSoundFx();
                        ButtonStart();
                        break;
                    case 2:
                        SoundManager.instance.PlayMenuClickSoundFx();
                        ButtonGameOptions();
                        break;
                    case 3:
                        SoundManager.instance.PlayMenuClickSoundFx();
                        ButtonSoundOptions();
                        break;
                    case 4:
                        SoundManager.instance.PlayMenuClickSoundFx();
                        ButtonQuit();
                        break;
                }
            }
        }
        else
        {
            if (isInPauseMenu)
            {
                switch (pauseMenuSelectedButton)
                {
                    case 1:
                        PauseMenu.instance.ResumeGame();
                        break;
                    case 2:
                        PauseMenu.instance.BackToMainMenu();
                        break;
                }
            }
        }
    }

    void MoveMenuPointerDown()
    {
        if (loadedSceneName.Equals("Main Menu"))
        {
            SoundManager.instance.PlayMenuPointerSoundFx();
            if (isInGameOptionsMenu)
            {
                gameOptionSelectedButton -= 1;
                if (gameOptionSelectedButton == 0)
                    gameOptionSelectedButton = 6;
            }
            else if (isInSoundMenu)
            {
                soundOptionSelectedButton -= 1;
                if (soundOptionSelectedButton == 0)
                    soundOptionSelectedButton = 4;
            }
            else
            {
                mainMenuSelectedButton -= 1;
                if (mainMenuSelectedButton == 0)
                    mainMenuSelectedButton = 4;
            }
        }
        else
        {
            if (isInPauseMenu)
            {
                pauseMenuSelectedButton -= 1;
                if (pauseMenuSelectedButton == 0)
                    pauseMenuSelectedButton = 2;
            }
        }

        oneTimeStickMovement = true;
    }

    void MoveMenuPointerUp()
    {
        if (loadedSceneName.Equals("Main Menu"))
        {
            SoundManager.instance.PlayMenuPointerSoundFx();
            if (isInGameOptionsMenu)
            {
                gameOptionSelectedButton += 1;
                if (gameOptionSelectedButton == 7)
                    gameOptionSelectedButton = 1;
            }
            else if (isInSoundMenu)
            {
                soundOptionSelectedButton += 1;
                if (soundOptionSelectedButton == 5)
                    soundOptionSelectedButton = 1;
            }
            else
            {
                mainMenuSelectedButton += 1;
                if (mainMenuSelectedButton == 5)
                    mainMenuSelectedButton = 1;
            }
        }
        else
        {
            if (isInPauseMenu)
            {
                pauseMenuSelectedButton += 1;
                if (pauseMenuSelectedButton == 3)
                    pauseMenuSelectedButton = 1;
            }
        }
        oneTimeStickMovement = true;
    }

    void ChangeButtonAppearance(int _mainMenuSelected, int _gameOptionMenuSelected, int _soundOptionMenuSelected, int _pauseMenuSelected)
    {
        if (isInGameOptionsMenu && loadedSceneName == "Main Menu")
        {
            //Options Menu
            switch (_gameOptionMenuSelected)
            {
                case 1:
                    easyButton.IsSelectedButton = true;
                    mediumButton.IsSelectedButton = false;
                    hardButton.IsSelectedButton = false;
                    pickUpAmountSlider.IsSelectedSlider = false;
                    levelLengthSlider.IsSelectedSlider = false;
                    saveOptionSettingsButton.IsSelectedButton = false;
                    break;
                case 2:
                    easyButton.IsSelectedButton = false;
                    mediumButton.IsSelectedButton = true;
                    hardButton.IsSelectedButton = false;
                    pickUpAmountSlider.IsSelectedSlider = false;
                    levelLengthSlider.IsSelectedSlider = false;
                    saveOptionSettingsButton.IsSelectedButton = false;
                    break;
                case 3:
                    easyButton.IsSelectedButton = false;
                    mediumButton.IsSelectedButton = false;
                    hardButton.IsSelectedButton = true;
                    pickUpAmountSlider.IsSelectedSlider = false;
                    levelLengthSlider.IsSelectedSlider = false;
                    saveOptionSettingsButton.IsSelectedButton = false;
                    break;
                case 4:
                    easyButton.IsSelectedButton = false;
                    mediumButton.IsSelectedButton = false;
                    hardButton.IsSelectedButton = false;
                    pickUpAmountSlider.IsSelectedSlider = true;
                    levelLengthSlider.IsSelectedSlider = false;
                    saveOptionSettingsButton.IsSelectedButton = false;
                    break;
                case 5:
                    easyButton.IsSelectedButton = false;
                    mediumButton.IsSelectedButton = false;
                    hardButton.IsSelectedButton = false;
                    pickUpAmountSlider.IsSelectedSlider = false;
                    levelLengthSlider.IsSelectedSlider = true;
                    saveOptionSettingsButton.IsSelectedButton = false;
                    break;
                case 6:
                    easyButton.IsSelectedButton = false;
                    mediumButton.IsSelectedButton = false;
                    hardButton.IsSelectedButton = false;
                    pickUpAmountSlider.IsSelectedSlider = false;
                    levelLengthSlider.IsSelectedSlider = false;
                    saveOptionSettingsButton.IsSelectedButton = true;
                    break;
            }
        }
        else if (isInSoundMenu)
        {
            //Sound Options
            switch (_soundOptionMenuSelected)
            {
                case 1:
                    masterVolSlider.IsSelectedSlider = true;
                    musicVolSlider.IsSelectedSlider = false;
                    soundFxVolSlider.IsSelectedSlider = false;
                    saveSoundSettingsButton.IsSelectedButton = false;
                    break;
                case 2:
                    masterVolSlider.IsSelectedSlider = false;
                    musicVolSlider.IsSelectedSlider = true;
                    soundFxVolSlider.IsSelectedSlider = false;
                    saveSoundSettingsButton.IsSelectedButton = false;
                    break;
                case 3:
                    masterVolSlider.IsSelectedSlider = false;
                    musicVolSlider.IsSelectedSlider = false;
                    soundFxVolSlider.IsSelectedSlider = true;
                    saveSoundSettingsButton.IsSelectedButton = false;
                    break;
                case 4:
                    masterVolSlider.IsSelectedSlider = false;
                    musicVolSlider.IsSelectedSlider = false;
                    soundFxVolSlider.IsSelectedSlider = false;
                    saveSoundSettingsButton.IsSelectedButton = true;
                    break;
            }
        }
        else if (isInPauseMenu)
        {
            switch (_pauseMenuSelected)
            {
                case 1:
                    PauseMenu.instance.resumeButton.IsSelectedButton = true;
                    PauseMenu.instance.quitButton.IsSelectedButton = false;
                    break;
                case 2:
                    PauseMenu.instance.resumeButton.IsSelectedButton = false;
                    PauseMenu.instance.quitButton.IsSelectedButton = true;
                    break;
            }
        }
        else
        {
            
            if (loadedSceneName == "Main Menu")
            {
                //Main Menu
                switch (_mainMenuSelected)
                {
                    case 1:
                        if (startButton != null)
                        {
                            startButton.IsSelectedButton = true;
                            gameOptionButton.IsSelectedButton = false;
                            soundOptionButton.IsSelectedButton = false;
                            quitButton.IsSelectedButton = false;
                        }
                        break;
                    case 2:
                        startButton.IsSelectedButton = false;
                        gameOptionButton.IsSelectedButton = true;
                        soundOptionButton.IsSelectedButton = false;
                        quitButton.IsSelectedButton = false;
                        break;
                    case 3:
                        startButton.IsSelectedButton = false;
                        gameOptionButton.IsSelectedButton = false;
                        soundOptionButton.IsSelectedButton = true;
                        quitButton.IsSelectedButton = false;
                        break;
                    case 4:
                        startButton.IsSelectedButton = false;
                        gameOptionButton.IsSelectedButton = false;
                        soundOptionButton.IsSelectedButton = false;
                        quitButton.IsSelectedButton = true;
                        break;
                }
            }
        }
    }

    void ChangeSliderValue()
    {
        if (isInSoundMenu && !controllerMovementStopped)
        {
            switch (soundOptionSelectedButton)
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

        if (isInGameOptionsMenu && !controllerMovementStopped)
        {
            if (gameOptionSelectedButton == 4)
            { 
                UIManagement.instance.pickUpAmountSlider.value += Input.GetAxisRaw("J1Horizontal");
                UIManagement.instance.pickUpAmountSlider.value += Input.GetAxisRaw("Horizontal");
                PlayerPrefs.SetInt("pickUpAmount", (int)UIManagement.instance.pickUpAmountSlider.value);
            }

            if (gameOptionSelectedButton == 5)
            {
                UIManagement.instance.levelLengthSlider.value += 5 * Input.GetAxisRaw("J1Horizontal");
                UIManagement.instance.levelLengthSlider.value += 5 * Input.GetAxisRaw("Horizontal");
                PlayerPrefs.SetInt("levelLength", (int)UIManagement.instance.levelLengthSlider.value);
            }
        }
    }

    // On pointer event function. Highlights the button over which the mouse hovers or points.
    public void SelectButtonDisplay(int selectedButton)
    {
        if(!isInSoundMenu && !isInGameOptionsMenu)
            mainMenuSelectedButton = selectedButton;
        if(isInSoundMenu)
            SoundMenuSelectedButton = selectedButton;
        if(isInGameOptionsMenu)
            gameOptionSelectedButton = selectedButton;
        if (isInPauseMenu)
            pauseMenuSelectedButton = selectedButton;
    }

    public void SelectedUIElementInOptions(int optionSelect)
    {
        if(isInSoundMenu)
            soundOptionSelectedButton = optionSelect;
        if(isInGameOptionsMenu)
            gameOptionSelectedButton = optionSelect;
    }

    public void GetSceneName()
    {
        Scene m_Scene = SceneManager.GetActiveScene();
        loadedSceneName = m_Scene.name;
    }

    public void SetSceneName(string sceneName)
    {
        loadedSceneName = sceneName;
    }

    public void SetDifficultyEasy()
    {
        UIManagement.instance.difficultyText.text = "Difficulty: Easy";
        aiDifficulty = AiDifficulty.Easy;
        PlayerPrefs.SetInt("Difficulty", (int)aiDifficulty);
        Debug.Log("Difficulty: " + aiDifficulty);
    }
    public void SetDifficultyMedium()
    {
        UIManagement.instance.difficultyText.text = "Difficulty: Medium";
        aiDifficulty = AiDifficulty.Normal;
        PlayerPrefs.SetInt("Difficulty", (int)aiDifficulty);
        Debug.Log("Difficulty: " + aiDifficulty);
    }
    public void SetDifficultyHard()
    {
        UIManagement.instance.difficultyText.text = "Difficulty: Hard";
        aiDifficulty = AiDifficulty.Hard;
        PlayerPrefs.SetInt("Difficulty", (int)aiDifficulty);
        Debug.Log("Difficulty: " + aiDifficulty);
    }
}

