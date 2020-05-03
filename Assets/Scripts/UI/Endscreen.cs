using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endscreen : MonoBehaviour
{
    bool isEndScreenActive = false;
    bool isGameOver = false;
    bool isWin = false;

    public float confirmDisplayTimer = 5f;

    

    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject confirmationText;

    void Start()
    {
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        confirmationText.SetActive(false);
    }

    void Update()
    {
        if (isEndScreenActive)
        {
            confirmDisplayTimer -= Time.deltaTime;

            if (isWin)
                winScreen.SetActive(true);
            if (isGameOver)
                gameOverScreen.SetActive(true);

            ConfirmCheck();
        }

        EndCheck();
    }

    void EndCheck()
    {
        if (PlayerInterface.instance.win)
        {
            isWin = true;
            isEndScreenActive = true;
        }

        if (PlayerInterface.instance.gameOver)
        {
            isGameOver = true;
            isEndScreenActive = true;
        }
    }

    void ConfirmCheck()
    {
        if (confirmDisplayTimer <= 0)
        {
            confirmationText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("J1ButtonA") || Input.GetButtonDown("J2ButtonA") || Input.GetButtonDown("J3ButtonA") || Input.GetButtonDown("J4ButtonA"))
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
        
    }
}
