using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour, Iinteractable
{
    [SerializeField] PlayerInterface playerInterface;

    static HoleManager instance = null;
    public List<GameObject> holes;

    public AttackScript attackScriptHookThrowerReference;
    public AttackScript attackScriptStoneThrowerReference;
    int currentHoleListCount;

    public static HoleManager Instance { get => instance; }
    public int CurrentHoleListCount { get => currentHoleListCount; set => currentHoleListCount = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Start()
    {
        currentHoleListCount = holes.Count;
    }

    public void Interact(PlayerController playerController)
    {
        // Check sticks amount
        if ((Input.GetKeyDown(KeyCode.E) || playerController.CheckInput(playerController, "ButtonA"))
                && playerInterface.stickCount < 1)
        {
            playerInterface.repairInfoText.text = "Not enough sticks.";
        }
        else if (Input.GetKeyDown(KeyCode.E) || playerController.CheckInput(playerController, "ButtonA"))
        {
            RepairHole(playerController);
        }
    }

    void RepairHole(PlayerController playerController)
    {
        playerInterface.medKitInfoText.text = "You repaired the hole.";
        holes.Add(playerController.OverLapBox.OverLappedCollider.gameObject);
        RaftHoleActivator.DisableSpriteRenderer(playerController.OverLapBox.OverLappedCollider.gameObject);

        playerInterface.stickCount--;
        if (playerInterface.stickCount < 0)
            playerInterface.stickCount = 0;
    }
}
