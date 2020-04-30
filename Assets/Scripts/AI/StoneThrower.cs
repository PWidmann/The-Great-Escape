using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneThrower : MonoBehaviour
{
    public static AudioSource audioSource;
    AttackScript attackScript;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        attackScript = GetComponent<AttackScript>();
    }

    public void MakeAction()
    {
        if (RaftController.AllPlayersOnRaft && !AIController.instance.isDebugging &&
            !AIController.instance.isWaitingForAi && !PlayerController.instance.GameOver)
        {
            attackScript.PrepareAttack();
            AIController.IsMakingAction = true;
        }
    }
}
