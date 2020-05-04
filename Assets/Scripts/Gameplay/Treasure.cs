using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, Iinteractable
{
    public void Interact(PlayerController playerController)
    {
        if (playerController.CheckInput(playerController, "ButtonA", KeyCode.E))
        {
            gameObject.SetActive(false);
            playerController.hasTreasure = true;
            PlayerController.hasTreasureTaken = true;
            playerController.MoveSpeed = 3;
            playerController.UIImageElement.treasureImage.gameObject.SetActive(true);
        }
    }
}
