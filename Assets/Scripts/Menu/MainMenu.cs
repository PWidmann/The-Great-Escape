using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int selectedButton = 1;
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

    // Gamepad
    bool oneTimeStickMovement = false;
    static bool controllerMovementStopped = false;

    public int SelectedButton { get => selectedButton; set => selectedButton = value; }
    public static bool ControllerMovementStopped { get => controllerMovementStopped; set => controllerMovementStopped = value; }

    private void Awake()
    {
        if (instance = null)
            instance = this;
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

        ChangeButtonAppearance(SelectedButton);
    }

    /// <summary>
    /// Loads pre game scene
    /// </summary>
    public void ButtonStart()
    {
        SceneManager.LoadScene("Pre Game");
    }

    /// <summary>
    /// Opens options menu with sliders and buttons.
    /// </summary>
    public void ButtonOptions()
    {
        UIManagement.instance.startButton.gameObject.SetActive(false);
        UIManagement.instance.optionButton.gameObject.SetActive(false);
        UIManagement.instance.quitButton.gameObject.SetActive(false);

        UIManagement.instance.saveSettingsButton.gameObject.SetActive(true);
        UIManagement.instance.masterVolumeSlider.gameObject.SetActive(true);
        UIManagement.instance.soundFxSlider.gameObject.SetActive(true);
        UIManagement.instance.musicVolumeSlider.gameObject.SetActive(true);
    }

    /// <summary>
    /// Closes the game.
    /// </summary>
    public void ButtonQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Saves all changes made and goes back to the titlescreen.
    /// </summary>
    public void ButtonSaveSettings()
    {
        PlayerPrefs.Save();

        UIManagement.instance.saveSettingsButton.gameObject.SetActive(false);
        UIManagement.instance.masterVolumeSlider.gameObject.SetActive(false);
        UIManagement.instance.soundFxSlider.gameObject.SetActive(false);
        UIManagement.instance.musicVolumeSlider.gameObject.SetActive(false);

        UIManagement.instance.startButton.gameObject.SetActive(true);
        UIManagement.instance.optionButton.gameObject.SetActive(true);
        UIManagement.instance.quitButton.gameObject.SetActive(true);

    }
    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && !controllerMovementStopped)
        {
            SoundManager.instance.PlayMenuPointerSoundFx();
            SelectedButton -= 1;

            if (SelectedButton == 0)
            {
                SelectedButton = 3;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && !controllerMovementStopped)
        {
            SoundManager.instance.PlayMenuPointerSoundFx();
            SelectedButton += 1;

            if (SelectedButton == 4)
            {
                SelectedButton = 1;
            }
        }

        // Confirm selected button
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (SelectedButton)
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

    void ControllerInput()
    {
        // Up
        if (Input.GetAxisRaw("J1Vertical") < 0 && !oneTimeStickMovement && !controllerMovementStopped)
        {
            SelectedButton += 1;
            SoundManager.instance.PlayMenuPointerSoundFx();
            if (SelectedButton == 4)
            {
                SelectedButton = 1;
            }

            oneTimeStickMovement = true;
        }

        // Down
        if (Input.GetAxisRaw("J1Vertical") > 0 && !oneTimeStickMovement && !controllerMovementStopped)
        {
            SoundManager.instance.PlayMenuPointerSoundFx();
            SelectedButton -= 1;
            if (SelectedButton == 0)
            {
                SelectedButton = 3;
            }

            oneTimeStickMovement = true;
        }

        // A Button
        if (Input.GetButtonDown("J1ButtonA"))
        {
            switch (SelectedButton)
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

        // Reset joystick for only one step per stick movement
        if (Input.GetAxisRaw("J1Vertical") == 0)
        {
            oneTimeStickMovement = false;
        }
    }

    void ChangeButtonAppearance(int _selectedButton)
    {
        switch (_selectedButton)
        {
            case 1:
                ColorTurnSelected(startButton, startButtonText);

                ColorTurnUnselected(optionsButton, optionsButtonText);
                ColorTurnUnselected(quitButton, quitButtonText);
                break;
            case 2:
                ColorTurnSelected(optionsButton, optionsButtonText);

                ColorTurnUnselected(startButton, startButtonText);
                ColorTurnUnselected(quitButton, quitButtonText);
                break;
            case 3:
                ColorTurnSelected(quitButton, quitButtonText);

                ColorTurnUnselected(startButton, startButtonText);
                ColorTurnUnselected(optionsButton, optionsButtonText);
                break;
        }
    }

    public void ColorTurnSelected(Button btn, Text txt)
    {
        //Button Color
        ColorBlock colors = btn.colors;
        colors.normalColor = new Color32(114, 114, 114, 255);

        btn.colors = colors;

        txt.color = Color.white;
    }

    public void ColorTurnUnselected(Button btn, Text txt)
    {
        //Button Color
        ColorBlock colors = btn.colors;
        colors.normalColor = Color.white;

        btn.colors = colors;

        txt.color = Color.black;
    }

    /// <summary>
    /// On pointer event function. Highlights the button over which the mouse hovers or points.
    /// </summary>
    /// <param name="selectedButton">the button that's selected int</param>
    public void SelectButtonDisplay(int selectedButton)
    {
        SelectedButton = selectedButton;
    }
}

