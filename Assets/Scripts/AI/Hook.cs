using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour, Iinteractable
{
    public void Interact(PlayerController playerController)
    {
        if (playerController.CheckInput(playerController, "ButtonX", KeyCode.Space) && AIController.RaftHooked)
        {
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[12], PlayerInterface.instance.pickUpAudio);
            Invoke("DestroyHook", 0.5f); 
        } 
    }

    void DestroyHook()
    {
        foreach (HookThrower hookThrower in AIController.hookThrowers)
        {
            hookThrower.DestroyHook();
        }
    }
}
