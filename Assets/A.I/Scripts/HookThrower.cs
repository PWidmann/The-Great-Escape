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
    
    /* Plans for later:
     * The throw should be physiscs based. Right now because of missing assets I don't want to mess with
     * the physics. 
     * I need animations and the character sprites and time to program the throw properly.
     * The current thow algorithm is only a base for the alpha version of this game.
     */

    /* Information for team:
     * I post a demo scene on Github where you can test the scripts and you can see how they work
     * in combination with the prefabs and configuration of GameObjects. Ask me if you have questions.s
     */

    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform raftObject;

    [SerializeField] Text InfoHookText;

    [SerializeField] AIController aiController;
    GameObject hook;

    static bool boatHooked = false;
    static bool hookInstantiated = false;
    bool isInstantiating = false; // used to control the sequence of update instructions.

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
        Debug.Log("Type: " + gameObject.GetComponent<HookThrower>());
    }

    // Update is called once per frame
    void Update()
    {
        // First instanitation of hook object.
        if (!hookInstantiated && !isInstantiating) 
        {
            Invoke("InstantiateHook", Random.Range(5f, 10f));
            isInstantiating = true;
        }
        else if (hookInstantiated && !boatHooked)
            ThrowHook(aiController.hitAccuracy, aiController.throwSpeed);

        // When hook is instantiated and the hook doesn't hit the target then it gets destroyed.
        if (hookInstantiated)
        {
            if (hook.transform.position == FlossController.OldPosition && !boatHooked)
            {
                DestroyHook(hook);
                hookInstantiated = false;
                isInstantiating = false;
                FlossController.HookMoving = false;
            }
        }

        // When hook hits the target it should be parented with the raft so it gives the illusion
        // that the hook thrower pulls the raft to the shore.
        if (boatHooked)
        {
            InfoHookText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
                DestroyHook(hook);
            else
                MakeHookAsChildGameObject();
        }
    }

    void InstantiateHook()
    {
        hook = Instantiate(hookPrefab, transform.position, Quaternion.identity);
        hookInstantiated = true;
        FlossController.HookMoving = true;
    }

    public void ThrowHook(float hitAccuracy, float throwSpeed)
    {
        hook.transform.position = Vector2.MoveTowards(hook.transform.position,
            FlossController.OldPosition * hitAccuracy, Time.deltaTime * throwSpeed);

    }

    void MakeHookAsChildGameObject()
    {
        hook.transform.parent = raftObject.transform;
    }

    void DestroyHook(GameObject hookPrefab)
    {
        InfoHookText.gameObject.SetActive(false);
        Destroy(hookPrefab);
        boatHooked = false;
        hookInstantiated = false;
        FlossController.HookMoving = false;
        isInstantiating = false;
    }
}
