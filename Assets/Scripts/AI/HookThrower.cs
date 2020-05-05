using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HookThrower : MonoBehaviour
{
    [SerializeField] GameObject hookPrefab;

    public static GameObject hook;
    AudioSource hookThrowerSfx;
    Transform raftObject;

    static bool hookInstantiated = false;

    bool isInstantiating = false; // used to control the sequence of update instructions.
    bool hasTargetLocked = false; // used to make the hook not follow the raft when hook thrown.
    Vector2 target;
    bool hasNoHookInHand = false; // used to play audio properly
    int randomHoleNumber;
    bool startAttack = false;

    bool isPullingHook = false;
    float hookThrowerAccuaracy = 1f;

    float nextThrowCooldown;


    Pathfinder pathfinder;
    AttackScript attackScript;
    float nextThrowCoolDown;
    bool thisIsActive = false;

    // Hook rope
    public LineRenderer lineRenderer;
    public Material lineRendererMaterial;

    public static bool HookInstantiated { get => hookInstantiated; set => hookInstantiated = value; }
    public bool ThisIsActive { get => thisIsActive; set => thisIsActive = value; }
    public bool IsPullingHook { get => isPullingHook; set => isPullingHook = value; }

    void Start()
    {
        hookThrowerSfx = GetComponent<AudioSource>();
        raftObject = GameObject.FindGameObjectWithTag("Raft").transform;
        pathfinder = GetComponent<Pathfinder>();
        attackScript = GetComponent<AttackScript>();
    }

    void Update()
    {
        if (!AIController.RaftHooked && RaftController.AllPlayersOnRaft && !AIController.instance.isDebugging &&
            !AIController.instance.isWaitingForAi && !PlayerController.instance.GameOver)
                pathfinder.Move();

        // Deactivate enemies when endscreen
        if (hookInstantiated && (PlayerInterface.instance.gameOver || PlayerInterface.instance.win))
        {
            hook.SetActive(false);
            return;
        }


        if (hookInstantiated && hook != null)
        {
            bool isAboveRaft = hook.transform.position.y > AIController.instance.raftTransform.transform.position.y;
            if (isAboveRaft && hook.transform.position.y <= target.y && !AIController.RaftHooked)
                DestroyHook();
            else if (!isAboveRaft && hook.transform.position.y >= target.y && !AIController.RaftHooked)
                DestroyHook();

        }

        // When hook hits the target it should be parented with the raft so it gives the illusion
        // that the hook thrower pulls the raft to the shore.
        if (AIController.RaftHooked)
            MakeHookAsChildGameObject();

        if (!hookInstantiated && thisIsActive && RaftController.AllPlayersOnRaft && !AIController.instance.isWaitingForAi
            && isPullingHook)
            InstantiateHook();

        HookRope();
    }

    void FixedUpdate()
    {
        if (hookInstantiated && !AIController.RaftHooked && hook != null)
            ThrowHook(hookThrowerAccuaracy, AIController.instance.throwSpeed);
    }

    public void GetHookInstantiationReady()
    {
        //isPullingHook = true;
        //if (!AIController.instance.isWaitingForAi)
        //{
        //    // First instanitation of hook object.
        //    if (!hookInstantiated && !isInstantiating && RaftController.AllPlayersOnRaft)
        //    {
        //        //AIController.IsPreperingHook = true;
        //        isPullingHook = true;
        //    }
        //}
    }

    void InstantiateHook()
    {
        if (Time.time > nextThrowCoolDown)
        {
            hook = Instantiate(hookPrefab, transform.position, Quaternion.identity);
            if (gameObject.transform.position.y > raftObject.transform.position.y)
                hook.transform.Rotate(new Vector3(0, 0, 180));

            hookInstantiated = true;
            isInstantiating = true;
            RaftController.HookMoving = true;
            nextThrowCoolDown = Time.time + AIController.instance.coolDownTimeInSeconds;
        }
    }

    void LockTarget()
    {
        if (!hasTargetLocked)
            target = HoleManager.Instance.holes[Random.Range(0, HoleManager.Instance.holes.Count)].transform.position;
    }

    public void ThrowHook(float hitAccuracy, float throwSpeed)
    {  
        LockTarget();

        if (!hasNoHookInHand)
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[5], hookThrowerSfx);

        hasTargetLocked = true;

        hook.transform.position = Vector3.MoveTowards(hook.transform.position,
            new Vector2(target.x * hitAccuracy, target.y), Time.deltaTime * throwSpeed);
        hasNoHookInHand = true;
    }

    void MakeHookAsChildGameObject()
    {
        if (hook != null)
            hook.transform.parent = raftObject.transform;
    }

    public void DestroyHook()
    {
        Destroy(hook);
        thisIsActive = false;
        AIController.RaftHooked = false;
        AIController.IsPreperingHook = false;
        hookInstantiated = false;
        RaftController.HookMoving = false;
        isInstantiating = false;
        hasTargetLocked = false;
        hasNoHookInHand = false;
        isPullingHook = false;
    }

    void HookRope()
    {
        if (AIController.RaftHooked && isPullingHook)
        {
            List<Vector3> pos = new List<Vector3>();

            pos.Add(new Vector3(transform.position.x, transform.position.y, -5));
            pos.Add(new Vector3(hook.transform.position.x, hook.transform.position.y, -5));

            lineRenderer.enabled = true;
            lineRenderer.sortingLayerName = "HookRope";
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            lineRenderer.SetPositions(pos.ToArray());
        }
        else
            lineRenderer.enabled = false;
    }
}
