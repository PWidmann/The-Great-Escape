using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance = null;

    public GameObject pauseMenu;

    public ButtonElement resumeButton;
    public ButtonElement quitButton;

    public bool isInPauseMenu = false;

    void Start()
    {
        if (instance == null)
            instance = this;

        MainMenu.instance.GetSceneName();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInPauseMenu)
            pauseMenu.SetActive(true);
        else
            pauseMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        isInPauseMenu = false;
        MainMenu.instance.IsInPauseMenu = false;
        Time.timeScale = 1;
    }

    public void OpenPauseMenu()
    {
        MainMenu.instance.PauseMenuSelectedButton = 1;
        isInPauseMenu = true;
        MainMenu.instance.IsInPauseMenu = true;
        Time.timeScale = 0;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
        MainMenu.instance.IsInPauseMenu = false;
        MainMenu.instance.loadedSceneName = "Main Menu";
    }
}
