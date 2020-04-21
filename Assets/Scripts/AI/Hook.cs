using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour, Iinteractable
{
    GameObject hookThrowerObject;
    HookThrower hookThrower;

    void Awake()
    {
        hookThrowerObject = GameObject.FindGameObjectWithTag("Hookthrower");
        Debug.Log("hookThrowerObject " + hookThrowerObject);
        hookThrower = hookThrowerObject.GetComponent<HookThrower>();
        Debug.Log("hookThrower " + hookThrower);
    }

    public void Interact(PlayerController playerController)
    {
        Debug.Log("hookThrower " + hookThrower);
        if (playerController.CheckInput(playerController, "ButtonX", KeyCode.Space) && HookThrower.BoatHooked)
        {
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[12], PlayerInterface.instance.pickUpAudio);
            Invoke("DestroyHook", 0.5f); 
        } 
    }

    void DestroyHook()
    {
        AIController.instance.hookThrower.DestroyHook();
    }
}
