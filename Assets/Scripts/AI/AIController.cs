using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    // Usually the AI got fired by events. Everything was controlled here but due to instantiating them
    // at runtime, it didn't work anymore. So the events had to go unfortunetly and everything has to be
    // in seperate scripts.

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
    public static List<StoneThrower> stoneThrowers = new List<StoneThrower>();
    public static List<SpearThrower> spearThrowers = new List<SpearThrower>();
    static bool isMakingAction = false;
    static bool isPreperingHook = false;
    static bool raftHooked = false;
    float distanceToRaft;

    public static bool IsMakingAction { get => isMakingAction; set => isMakingAction = value; }
    public static bool IsPreperingHook { get => isPreperingHook; set => isPreperingHook = value; }
    public static bool RaftHooked { get => raftHooked; set => raftHooked = value; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        GetThrowerObjectScriptComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMakingAction && !isDebugging && !isWaitingForAi)
        {
            hookThrowers[Random.Range(0, hookThrowers.Count)].MakeAction();
            stoneThrowers[Random.Range(0, stoneThrowers.Count)].MakeAction();
            spearThrowers[Random.Range(0, spearThrowers.Count)].MakeAction();
        }
        else if (!isMakingAction && !isDebugging && isWaitingForAi)
            StartCoroutine(Attack(delayTimer));

        if (!isPreperingHook)
            hookThrowers[Random.Range(0, hookThrowers.Count)].GetHookInstantiationReady();

        if (!PlayerInterface.instance.gameOver || !PlayerInterface.instance.win)
            distanceToRaft = GetDistanceBetweenAIandRaft();
    }

    void GetThrowerObjectScriptComponents()
    {
        foreach (GameObject throwerObject in EnemySpawner.spawnedEnemies)
        {
            hookThrowers.Add(throwerObject.GetComponent<HookThrower>());
            stoneThrowers.Add(throwerObject.GetComponentInChildren<StoneThrower>());
            spearThrowers.Add(throwerObject.GetComponentInChildren<SpearThrower>());
        }
    }

    public void StartAttackWithDelay()
    {
        StartCoroutine(Attack(delayTimer));
    }

    IEnumerator Attack(float delayTimeInSeconds)
    {
        yield return new WaitForSecondsRealtime(delayTimeInSeconds);
        if (distanceToRaft >= 40)
            movementSpeed = 20;
        Debug.Log("movementSpeed: " + movementSpeed + "" + "Distance: " + distanceToRaft);
        StartCoroutine(AdjustAiSpeed());
        isWaitingForAi = false;
    }

    IEnumerator AdjustAiSpeed()
    {
        yield return new WaitForSecondsRealtime(delayTimer - 3);
        movementSpeed = 7;
    }

    float GetDistanceBetweenAIandRaft()
    {
        foreach (GameObject hookThrower in EnemySpawner.spawnedEnemies)
        {
            if (hookThrower != null)
                return Vector2.Distance(hookThrower.transform.position, raftTransform.transform.position);
        }
        return 0f;
    }
}
