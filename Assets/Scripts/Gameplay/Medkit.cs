﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour, Iinteractable
{
    [SerializeField] PlayerInterface playerInterface;
    [SerializeField] AudioSource medkitAudio;

    public void Interact(PlayerController playerController)
    {
        // Check for amount of leafes
        if (playerController.CheckInput(playerController, "ButtonA", KeyCode.E) && playerInterface.leafCount < 2)
            playerInterface.medKitInfoText.text = "Not enough leafes.";

        // Check for Playerhealth 
        else if (playerController.CheckInput(playerController, "ButtonA", KeyCode.E) 
            && playerController.playerHealth == 100)
            playerInterface.medKitInfoText.text = "You already have max health.";

        else if (playerController.CheckInput(playerController, "ButtonA", KeyCode.E) 
            && playerController.playerHealth < 100)
            Heal(playerController);
    }

    void Heal(PlayerController playerController)
    {
        SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[10], medkitAudio);
        playerInterface.medKitInfoText.text = "You healed yourself.";
        playerController.playerHealth += 60f;
        playerController.IsHealing = true;

        if (playerController.playerHealth > 100)
            playerController.playerHealth = 100;

        playerInterface.leafCount -= 2;
        if (playerInterface.leafCount < 0)
            playerInterface.leafCount = 0;
    }
}
