using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, Iinteractable
{
    public void Interact(PlayerController playerController)
    {
        if (playerController.CheckInput(playerController, "ButtonA", KeyCode.E))
        {
            if (!RaftController.instance.shieldIsInUse)
            {
                playerController.hasShield = true;
                RaftController.instance.shieldIsInUse = true;
                RaftController.instance.shieldObject.SetActive(false);
            }
        }
    }
}
