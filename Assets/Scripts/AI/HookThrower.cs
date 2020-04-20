using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookThrower : MonoBehaviour
{
    // The hook has to be instantiated as a GameObject -> Done


    // Hook thrower should hit the raft with the hook.
    // The enemy team attacks when the event Attacktrigger is fired.
    // It should get triggered when the hook thrower manages to hook the raft.
    // -> BoatHooked has to be true;
    // => Trigger detection required! -> Done
    // Right now the hook object follows the raft. It only should be moving torwards it.
    // => Saving the latest position of the raft - Done
    // If raft is hooked, it should stop and the player can't move it anymore until it is unhooked.
    // -> Done

    // The raft when hooked has to be pulled to the shore in the movement script. -> Done
    // The hook object has to be pulled down too when the the raft gets pulled.
    // It just has to follow the raft when it is hooked but on the same location where it hit the raft.
    // -> Make it as child GameObject when it gets hooked. -> Done

    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform raftObject;

    [SerializeField] AIController aiController;
    GameObject hook;
    AudioSource hookThrowerSfx;

    static bool boatHooked = false;
    static bool hookInstantiated = false;

    bool isInstantiating = false; // used to control the sequence of update instructions.
    bool hasTargetLocked = false; // used to make the hook not follow the raft when hook thrown.
    Vector2 target;
    bool hasNoHookInHand = false;
    int randomHoleNumber;
    bool startAttack = false;

    public static bool BoatHooked
    {
        get { return boatHooked; }
        set { boatHooked = value; }
    }

    public static bool HookInstantiated
    {
        get { return hookInstantiated; }
        set { hookInstantiated = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hookThrowerSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!AIController.instance.isWaitingForAi)
        {
            // Prevent out of range exception by making sure that the indexes are always updated.
            if (HoleManager.Instance.CurrentHoleListCount != HoleManager.Instance.holes.Count)
            {
                randomHoleNumber = Random.Range(0, HoleManager.Instance.holes.Count);
                HoleManager.Instance.CurrentHoleListCount = HoleManager.Instance.holes.Count;
            }

            // First instanitation of hook object.
            if (!hookInstantiated && !isInstantiating && RaftController.AllPlayersOnRaft)
            {
                Invoke("InstantiateHook", Random.Range(AIController.instance.minHookThrowDelayTimer, 
                    AIController.instance.maxHookThrowDelayTimer));

                // Determines randomHoleNumber.
                randomHoleNumber = Random.Range(0, HoleManager.Instance.holes.Count);

                isInstantiating = true;
            }
            else if (hookInstantiated && !boatHooked)
                ThrowHook(aiController.hitAccuracy, aiController.throwSpeed);

            // When hook is instantiated and the hook doesn't hit the target then it gets destroyed.
            if (hookInstantiated)
            {
                if (hook.transform.position.Equals(target) && !boatHooked)
                {
                    DestroyHook();
                    hookInstantiated = false;
                    isInstantiating = false;
                    RaftController.HookMoving = false;
                    hasTargetLocked = false;
                }
            }
        }

        // When hook hits the target it should be parented with the raft so it gives the illusion
        // that the hook thrower pulls the raft to the shore.
        if (boatHooked)
        {
           
                MakeHookAsChildGameObject();
        }
    }

    void InstantiateHook()
    {
        hook = Instantiate(hookPrefab, transform.position, Quaternion.identity);
        hookInstantiated = true;
        RaftController.HookMoving = true;
    }

    public void ThrowHook(float hitAccuracy, float throwSpeed)
    {
        if (!hasTargetLocked)
        {
            target = HoleManager.Instance.holes[randomHoleNumber].transform.position;
        }

        if (!hasNoHookInHand)
        {
            hookThrowerSfx.clip = SoundManager.instance.soundFx[5];
            hookThrowerSfx.Play();
        }
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
        PlayerInterface.instance.destroyHookInfoText.gameObject.SetActive(false);
        Destroy(hook);
        boatHooked = false;
        hookInstantiated = false;
        RaftController.HookMoving = false;
        isInstantiating = false;
        hasTargetLocked = false;
        hasNoHookInHand = false;
    }
}
