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
            Invoke("DestroyHook", 0.5f); 
        } 
    }

    void DestroyHook()
    {
        AIController.instance.hookThrower.DestroyHook();
    }
}
