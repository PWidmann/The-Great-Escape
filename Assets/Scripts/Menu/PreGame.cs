using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PreGame : MonoBehaviour
{
    [Header("Player Panels")]
    public GameObject player1panel;
    public GameObject player2panel;
    public GameObject player3panel;
    public GameObject player4panel;

    [Header("Player Control Images")]
    public Image player1controlImage;
    public Image player2controlImage;
    public Image player3controlImage;
    public Image player4controlImage;

    [Header("Control Images")]
    public Sprite controllerImage;
    public Sprite keyboardImage;

    [Header("Confirm Player Text")]
    public GameObject confirmText;

    bool confirmPlayers = false;
    int sfxPlayCount = 0;

    private void Start()
    {
        player1panel.SetActive(false);
        player2panel.SetActive(false);
        player3panel.SetActive(false);
        player4panel.SetActive(false);


    }
    // Update is called once per frame
    void Update()
    {
        PlayerPanels();
        ConfirmPlayers();
    }

    void PlayerPanels()
    {
        // Player 1 Panel
        if (PlayerHandler.instance.player1active)
        {
            SetControlImage(1, PlayerHandler.instance.player1controls);
            player1panel.SetActive(true);
        }
        else
        {
            player1panel.SetActive(false);
        }

        // Player 2 Panel
        if (PlayerHandler.instance.player2active)
        {
            SetControlImage(2, PlayerHandler.instance.player2controls);
            player2panel.SetActive(true);
        }
        else
        {
            player2panel.SetActive(false);
        }

        // Player 3 Panel
        if (PlayerHandler.instance.player3active)
        {
            SetControlImage(3, PlayerHandler.instance.player3controls);
            player3panel.SetActive(true);
        }
        else
        {
            player3panel.SetActive(false);
        }

        // Player 4 Panel
        if (PlayerHandler.instance.player4active)
        {
            SetControlImage(4, PlayerHandler.instance.player4controls);
            player4panel.SetActive(true);
        }
        else
        {
            player4panel.SetActive(false);
        }
    }

    void ConfirmPlayers()
    {
        if (confirmPlayers)
        {
            // Play sound fx only once.
            if (!SoundManager.instance.soundFxSource.isPlaying && sfxPlayCount == 0)
            {
                SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[3];
                SoundManager.instance.soundFxSource.Play();
                sfxPlayCount++;
            }

            confirmText.SetActive(true);
        }
        else
        {
            confirmText.SetActive(false);
        }
        
        // Keyboard confirmation
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Start game
            if (PlayerHandler.instance.playerCount > 0 && confirmPlayers == true)
            {
                SoundManager.instance.PlayMenuPointerSoundFx();
                ButtonStart();
            }

            // Set confirmation active
            if (PlayerHandler.instance.playerCount > 0 && confirmPlayers == false)
            {
                // Check if Panel for J1 is active
                if (PlayerHandler.instance.player1controls == "Keyboard" || PlayerHandler.instance.player2controls == "Keyboard" || PlayerHandler.instance.player3controls == "Keyboard" || PlayerHandler.instance.player4controls == "Keyboard")
                {
                    confirmPlayers = true;
                }
            }
        }

        // Controller confirmation
        if (Input.GetButtonDown("J1ButtonA"))
        {
            //Start game
            if (PlayerHandler.instance.playerCount > 0 && confirmPlayers == true)
            {
                SoundManager.instance.PlayMenuClickSoundFx();
                ButtonStart();
            }

            // Set confirmation active
            if (PlayerHandler.instance.playerCount > 0 && confirmPlayers == false)
            {
                // Check if Panel for J1 is active
                if (PlayerHandler.instance.player1controls == "J1" || PlayerHandler.instance.player2controls == "J1" || PlayerHandler.instance.player3controls == "J1" || PlayerHandler.instance.player4controls == "J1")
                {
                    confirmPlayers = true;
                } 
            }
        }

        // Cancel confirmation
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("J1ButtonB"))
        {
            if (confirmPlayers)
            {
                confirmPlayers = false;
            }
        }
    }

    void SetControlImage(int playernumber, string controls)
    {
        switch (playernumber)
        {
            case 1:
                player1controlImage.sprite = AssignControlImage(controls);
                break;
            case 2:
                player2controlImage.sprite = AssignControlImage(controls);
                break;
            case 3:
                player3controlImage.sprite = AssignControlImage(controls);
                break;
            case 4:
                player4controlImage.sprite = AssignControlImage(controls);
                break;

        }
    }

    Sprite AssignControlImage(string controls)
    {
        if (controls == "Keyboard")
        {
            return keyboardImage;
        }
        else
        {
            return controllerImage;
        }
    }

    public void ButtonStart()
    {
        SceneManager.LoadScene("The Great Escape");
        PlayerHandler.instance.playSceneActive = true;
    }
}
