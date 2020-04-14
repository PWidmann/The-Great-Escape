using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    static HoleManager instance = null;
    public List<GameObject> holes;
    public AttackScript attackScriptHookThrowerReference;
    public AttackScript attackScriptStoneThrowerReference;

    public static HoleManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
}
