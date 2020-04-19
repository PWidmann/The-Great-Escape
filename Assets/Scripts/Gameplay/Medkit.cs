using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour, Iinteractable
{
    [SerializeField] PlayerInterface playerInterface;

    public void Interact(PlayerController playerController)
    {
        // Check for amount of leafes
        if ((Input.GetKeyDown(KeyCode.E) || playerController.CheckInput(playerController, "ButtonA"))
            && playerInterface.leafCount < 2)
        {
            playerInterface.medKitInfoText.text = "Not enough leafes.";
        }

        // Check for Playerhealth 
        else if ((Input.GetKeyDown(KeyCode.E) || playerController.CheckInput(playerController, "ButtonA")) 
            && playerController.playerHealth == 100)
        {
            playerInterface.medKitInfoText.text = "You already have max health.";
        }
        else if ((Input.GetKeyDown(KeyCode.E) || playerController.CheckInput(playerController, "ButtonA")) 
            && playerController.playerHealth < 100)
        {
            Heal(playerController);
        }
    }

    void Heal(PlayerController playerController)
    {
        playerInterface.medKitInfoText.text = "You healed yourself.";
        playerController.playerHealth += 80;

        if (playerController.playerHealth > 100)
            playerController.playerHealth = 100;

        playerInterface.leafCount -= 2;
        if (playerInterface.leafCount < 0)
            playerInterface.leafCount = 0;

    }
}
