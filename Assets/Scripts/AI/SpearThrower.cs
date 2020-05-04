using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrower : MonoBehaviour
{
    AudioSource audioSource;
    AttackScript attackScript;

    void Start()
    {
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
