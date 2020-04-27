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
        hookThrower = hookThrowerObject.GetComponent<HookThrower>();
    }

    public void Interact(PlayerController playerController)
    {
        if (playerController.CheckInput(playerController, "ButtonX", KeyCode.Space) && HookThrower.BoatHooked)
        {
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[12], PlayerInterface.instance.pickUpAudio);
            Invoke("DestroyHook", 0.5f); 
        } 
    }

    void DestroyHook()
    {
        EnemySpawner.PreviousEnemy.GetComponent<HookThrower>().DestroyHook();
    }
}
