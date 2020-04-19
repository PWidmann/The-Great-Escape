using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, Iinteractable
{
    public void Interact(PlayerController playerController)
    {
        if ((Input.GetKeyDown(KeyCode.E)) || playerController.CheckInput(playerController, "ButtonA"))
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
