using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIController : MonoBehaviour
{
    public UnityEvent AttackTrigger;
    public UnityEvent RunTrigger;
    public UnityEvent DebugTestingTrigger; // Turns off AI for other tests in our game.

    [HideInInspector] public bool isChecked;
    [SerializeField] bool isDebugging;

    public static AIController instance;

    [Range(0.0f, 1f)] public float hitAccuracy;
    [Range(1f, 1000f)] public float throwSpeed;


    // Update is called once per frame
    void Update()
    {
        if (HookThrower.BoatHooked)
            AttackTrigger.Invoke();
        else if (!HookThrower.BoatHooked && RaftController.AllPlayersOnRaft && !isDebugging)
            RunTrigger.Invoke();
        else if (isDebugging)
            DebugTestingTrigger.Invoke();
    }
}
