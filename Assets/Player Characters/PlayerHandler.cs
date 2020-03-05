using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance = null;
    public bool playSceneActive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public bool player1active = false;
    public bool player2active = false;
    public bool player3active = false;
    public bool player4active = false;

    public string player1controls;
    public string player2controls;
    public string player3controls;
    public string player4controls;

    // Used in PlayerController Class
    public bool player1assigned = false;
    public bool player2assigned = false;
    public bool player3assigned = false;
    public bool player4assigned = false;

    public int playerCount = 0;

    private void Start()
    {

    }

    private void Update()
    {
        ListenForPlayers();
    }

    private void ListenForPlayers()
    {
        if (Input.GetButtonDown("J1ButtonA") || Input.GetButtonDown("J1ButtonB"))
        {
            string inputController = "J1";
            if (!IsControllerAssigned(inputController))
            {
                AssignController(inputController);
            }
        }

        if (Input.GetButtonDown("J2ButtonA") || Input.GetButtonDown("J2ButtonB"))
        {
            string inputController = "J2";
            if (!IsControllerAssigned(inputController))
            {
                AssignController(inputController);
            }
        }

        if (Input.GetButtonDown("J3ButtonA") || Input.GetButtonDown("J3ButtonB"))
        {
            string inputController = "J3";
            if (!IsControllerAssigned(inputController))
            {
                AssignController(inputController);
            }
        }

        if (Input.GetButtonDown("J4ButtonA") || Input.GetButtonDown("J4ButtonB"))
        {
            string inputController = "J4";
            if (!IsControllerAssigned(inputController))
            {
                AssignController(inputController);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            string inputController = "Keyboard";
            if (!IsControllerAssigned(inputController))
            {
                AssignController(inputController);
            }
        }
    }

    public void AssignController(string inputController)
    {
        playerCount++;
        switch (playerCount)
        {
            case 1:
                player1controls = inputController;
                player1active = true;
                Debug.Log("Player 1 active, with Controller: " + inputController);
                break;
            case 2:
                player2controls = inputController;
                player2active = true;
                Debug.Log("Player 2 active, with Controller: " + inputController);
                break;
            case 3:
                player3controls = inputController;
                player3active = true;
                Debug.Log("Player 3 active, with Controller: " + inputController);
                break;
            case 4:
                player4active = true;
                player4controls = inputController;
                Debug.Log("Player 4 active, with Controller: " + inputController);
                break;
        }
    }

    public bool IsControllerAssigned(string inputController)
    {
        if (player1controls == inputController || player2controls == inputController || player3controls == inputController || player4controls == inputController)
        {
            return true;
        }
        else
        { 
            return false;
        }
    }
}

