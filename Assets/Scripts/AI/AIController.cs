using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIController : MonoBehaviour
{
    // Usually the AI got fired by events. Everything was controlled here but due to instantiating them
    // at runtime, it didn't work anymore. So the events had to go unfortunetly and everything has to be
    // in seperate scripts.

    public UnityEvent AttackTrigger;
    //UnityEvent RunTrigger = new UnityEvent();
    public UnityEvent DebugTestingTrigger; // Turns off AI for other tests in our game.
    public UnityEvent WaitForAiTrigger; // Event that makes the AI wait before they attack.
    public UnityEvent DestroyStoneTrigger; 
    public UnityEvent LoseTrigger;

    [HideInInspector] public bool isChecked;
    public bool isDebugging;
    public bool isWaitingForAi = true;

    public float delayTimer; // delay for starting attack.
    public int movementSpeed = 7;

    public static AIController instance;

    [Header("General")]
    [Range(0.0f, 1f)] public float hitAccuracy;
    [Range(1f, 1000f)] public float throwSpeed;
    public float distanceBetweenAI = 8f;

    [Header("HookThrower")]
    public float minHookThrowDelayTimer = 3f;
    public float maxHookThrowDelayTimer = 6f;

    [Header("Spear-/Stonethrower")]
    public float coolDownTimeInSeconds = 5f;
    
    public AiDifficulty aiDifficulty;

    public GameObject raftTransform; // Needed for Pathfinder reference.
    [SerializeField] GameObject hookThrower;
    public static List<HookThrower> hookThrowers = new List<HookThrower>();
    static bool isMakingAction = false;
    static bool isPreperingHook = false;

    public static bool IsMakingAction { get => isMakingAction; set => isMakingAction = value; }
    public static bool IsPreperingHook { get => isPreperingHook; set => isPreperingHook = value; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        //RunTrigger.AddListener(hookThrower.GetComponent<Pathfinder>().Move);
        //AttackTrigger.AddListener(AttackScript.instance.PrepareAttack);
        //RunTrigger.AddListener(AttackScript.instance.PrepareAttack);

        //DebugTestingTrigger.AddListener(AttackScript.instance.KeepAIDisabled);
        //WaitForAiTrigger.AddListener(StartAttackWithDelay);
        //DestroyStoneTrigger.AddListener(AttackScript.instance.DestroyStone);
        GetThrowerObjectScriptComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMakingAction)
        {
            hookThrowers[Random.Range(0, hookThrowers.Count)].MakeAction();
        }

        if (!isPreperingHook)
        {
            hookThrowers[Random.Range(0, hookThrowers.Count)].GetHookInstantiationReady();
        }

        if (HookThrower.BoatHooked && !PlayerController.instance.GameOver)
            AttackTrigger.Invoke();
        //else if (!HookThrower.BoatHooked && RaftController.AllPlayersOnRaft && !isDebugging && !isWaitingForAi && 
        //    !PlayerController.instance.GameOver)
        //    RunTrigger.Invoke();
        else if (isDebugging)
            DebugTestingTrigger.Invoke();
        else if (isWaitingForAi && RaftController.AllPlayersOnRaft && !PlayerController.instance.GameOver)
            WaitForAiTrigger.Invoke();

        //if (RaftHoleActivator.IsHit && RaftHoleActivator.HitCounter >= 2)
        //    DestroyStoneTrigger.Invoke();

        if (PlayerController.instance.GameOver)
            LoseTrigger.Invoke();
    }

    void GetThrowerObjectScriptComponents()
    {
        foreach (GameObject hookThrower in EnemySpawner.spawnedEnemies)
        {
            hookThrowers.Add(hookThrower.GetComponent<HookThrower>());
        }
    }

    public void StartAttackWithDelay()
    {
        StartCoroutine(Attack(delayTimer));
    }

    IEnumerator Attack(float delayTimeInSeconds)
    {
        yield return new WaitForSecondsRealtime(delayTimeInSeconds);
        movementSpeed = 20;
        StartCoroutine(AdjustAiSpeed());
        isWaitingForAi = false;
    }

    IEnumerator AdjustAiSpeed()
    {
        yield return new WaitForSecondsRealtime(delayTimer - 3);
        movementSpeed = 7;
    }
}
