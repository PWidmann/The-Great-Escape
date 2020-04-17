using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIController : MonoBehaviour
{
    public UnityEvent AttackTrigger;
    public UnityEvent RunTrigger;
    public UnityEvent DebugTestingTrigger; // Turns off AI for other tests in our game.
    public UnityEvent WaitForAiTrigger; // Event that makes the AI wait before they attack.
    public UnityEvent DestroyStoneTrigger; // Self-explainatory.

    [HideInInspector] public bool isChecked;
    [SerializeField] bool isDebugging;
    public bool isWaitingForAi = true;

    public float delayTimer; // delay for starting attack.
    public int movementSpeed = 7;

    public static AIController instance;

    [Range(0.0f, 1f)] public float hitAccuracy;
    [Range(1f, 1000f)] public float throwSpeed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (HookThrower.BoatHooked)
            AttackTrigger.Invoke();
        else if (!HookThrower.BoatHooked && RaftController.AllPlayersOnRaft && !isDebugging && !isWaitingForAi)
            RunTrigger.Invoke();
        else if (isDebugging)
            DebugTestingTrigger.Invoke();
        else if (isWaitingForAi && RaftController.AllPlayersOnRaft)
            WaitForAiTrigger.Invoke();
        if (RaftHoleActivator.IsHit && RaftHoleActivator.HitCounter >= 2)
            DestroyStoneTrigger.Invoke();
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
