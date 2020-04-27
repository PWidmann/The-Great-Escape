﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookThrower : MonoBehaviour
{
    [SerializeField] GameObject hookPrefab;

    GameObject hook;
    AudioSource hookThrowerSfx;
    Transform raftObject;

    static bool boatHooked = false;
    static bool hookInstantiated = false;

    bool isInstantiating = false; // used to control the sequence of update instructions.
    bool hasTargetLocked = false; // used to make the hook not follow the raft when hook thrown.
    Vector2 target;
    bool hasNoHookInHand = false; // used to play audio properly
    int randomHoleNumber;
    bool startAttack = false;

    Pathfinder pathfinder;
    AttackScript attackScript;
    EnemyBehaviour enemyBehaviour;

    public static bool BoatHooked { get => boatHooked; set => boatHooked = value; }
    public static bool HookInstantiated { get => hookInstantiated; set => hookInstantiated = value; }

    void Start()
    {
        hookThrowerSfx = GetComponent<AudioSource>();
        raftObject = GameObject.FindGameObjectWithTag("Raft").transform;
        pathfinder = GetComponent<Pathfinder>();
        attackScript = GetComponent<AttackScript>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void Update()
    {
        if (enemyBehaviour.IsInRange())
        {
            if (!boatHooked && RaftController.AllPlayersOnRaft && !AIController.instance.isDebugging &&
                !AIController.instance.isWaitingForAi && !PlayerController.instance.GameOver)
            {
                pathfinder.Move();
                //attackScript.PrepareAttack();
            }
        }

        //// Deactivate enemies when endscreen
        if (hookInstantiated && (PlayerInterface.instance.gameOver || PlayerInterface.instance.win))
        {
            hook.SetActive(false);
            Destroy(gameObject);
            return;
        }

        //if (!AIController.instance.isWaitingForAi)
        //{
        //    // First instanitation of hook object.
        //    if (!hookInstantiated && !isInstantiating && RaftController.AllPlayersOnRaft)
        //        PrepereHookInstantiation();
        //    else if (hookInstantiated && !boatHooked)
        //        ThrowHook(AIController.instance.hitAccuracy, AIController.instance.throwSpeed);

        //    // When hook is instantiated and the hook doesn't hit the target then it gets destroyed.
        //    if (hookInstantiated)
        //    {
        //        if (hook.transform.position.Equals(target) && !boatHooked)
        //            DestroyHook();
        //    }
        //}

        // When hook hits the target it should be parented with the raft so it gives the illusion
        // that the hook thrower pulls the raft to the shore.
        if (boatHooked)
            MakeHookAsChildGameObject();
    }

    void PrepereHookInstantiation()
    {
        if (!RaftController.HookMoving)
            Invoke("InstantiateHook", Random.Range(AIController.instance.minHookThrowDelayTimer,
                AIController.instance.maxHookThrowDelayTimer));

        randomHoleNumber = Random.Range(0, HoleManager.Instance.holes.Count);

        isInstantiating = true;
    }

    void InstantiateHook()
    {
        hook = Instantiate(hookPrefab, transform.position, Quaternion.identity);
        hookInstantiated = true;
        RaftController.HookMoving = true;
    }

    void LockTarget()
    {
        if (!hasTargetLocked)
        {
            target = HoleManager.Instance.holes[randomHoleNumber].transform.position;
        }
    }

    public void ThrowHook(float hitAccuracy, float throwSpeed)
    {
        LockTarget();

        if (!hasNoHookInHand)
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[5], hookThrowerSfx);

        hasTargetLocked = true;
        hook.transform.position = Vector2.MoveTowards(hook.transform.position,
            target * hitAccuracy, Time.deltaTime * throwSpeed);
        hasNoHookInHand = true;
    }

    void MakeHookAsChildGameObject()
    {
        hook.transform.parent = raftObject.transform;
    }

    public void DestroyHook()
    {
        Destroy(hook);
        boatHooked = false;
        hookInstantiated = false;
        RaftController.HookMoving = false;
        isInstantiating = false;
        hasTargetLocked = false;
        hasNoHookInHand = false;
    }
}
