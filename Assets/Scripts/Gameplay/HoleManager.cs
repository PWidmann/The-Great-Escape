using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour, Iinteractable
{
    [SerializeField] PlayerInterface playerInterface;
    [SerializeField] AudioSource holeRepairAudio;

    static HoleManager instance = null;
    public List<GameObject> holes;

    public static HoleManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void Interact(PlayerController playerController)
    {
        // Check sticks amount
        if ((Input.GetKeyDown(KeyCode.E) || playerController.CheckInput(playerController, "ButtonA"))
                && playerInterface.stickCount < 1)
            playerInterface.repairInfoText.text = "Not enough sticks.";

        else if (Input.GetKeyDown(KeyCode.E) || playerController.CheckInput(playerController, "ButtonA"))
            RepairHole(playerController);
    }

    void RepairHole(PlayerController playerController)
    {
        SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[7]);
        playerInterface.medKitInfoText.text = "You repaired the hole.";
        RaftController.instance.moveSpeed += 0.13f;
        RaftHoleActivator.DisableSpriteRenderer(playerController.OverLapBox.OverLappedCollider.gameObject);

        playerInterface.stickCount--;
        if (playerInterface.stickCount < 0)
            playerInterface.stickCount = 0;
    }
}
