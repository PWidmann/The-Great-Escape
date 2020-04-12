using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int selectedButton = 1;

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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
        ControllerInput();

        ChangeButtonAppearance(selectedButton);
    }

    public void ButtonStart()
    {
        SceneManager.LoadScene("Pre Game");
    }

    public void ButtonOptions()
    {

    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButton -= 1;

            if (selectedButton == 0)
            {
                selectedButton = 3;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedButton += 1;

            if (selectedButton == 4)
            {
                selectedButton = 1;
            }
        }

        // Confirm selected button
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (selectedButton)
            {
                case 1:
                    ButtonStart();
                    break;
                case 2:
                    break;
                case 3:
                    ButtonQuit();
                    break;
            }
        }
    }

    void ControllerInput()
    {
        // Up
        if (Input.GetAxisRaw("J1Vertical") < 0 && !oneTimeStickMovement)
        {
            selectedButton += 1;
            if (selectedButton == 4)
            {
                selectedButton = 1;
            }

            oneTimeStickMovement = true;
        }

        // Down
        if (Input.GetAxisRaw("J1Vertical") > 0 && !oneTimeStickMovement)
        {
            selectedButton -= 1;
            if (selectedButton == 0)
            {
                selectedButton = 3;
            }

            oneTimeStickMovement = true;
        }

        // A Button
        if (Input.GetButtonDown("J1ButtonA"))
        {
            switch (selectedButton)
            {
                case 1:
                    ButtonStart();
                    break;
                case 2:
                    break;
                case 3:
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

    void ColorTurnSelected(Button btn, Text txt)
    {
        //Button Color
        ColorBlock colors = btn.colors;
        colors.normalColor = new Color32(114, 114, 114, 255);

        btn.colors = colors;

        txt.color = Color.white;
    }

    void ColorTurnUnselected(Button btn, Text txt)
    {
        //Button Color
        ColorBlock colors = btn.colors;
        colors.normalColor = Color.white;

        btn.colors = colors;

        txt.color = Color.black;
    }
}

