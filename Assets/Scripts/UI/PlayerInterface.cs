using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    public static PlayerInterface instance = null;

    // Tutorial
    [Header("Tutorial")]
    public bool startWithTutorial = true;
    public GameObject tutorialPanel;
    public bool tutorialActive = false;

    // Inventory References
    [Header("Gameplay")]
    public Text leafCountText;
    public Text stickCountText;
    public Text medKitInfoText;
    public Text repairInfoText;
    public Text takeShieldText;
    public Text activationShieldText;

    public GameObject player1health;
    public GameObject player2health;
    public GameObject player3health;
    public GameObject player4health;

    // Player Inventory
    public int leafCount = 0;
    public int stickCount = 0;

    [SerializeField] Camera uiCamForTextFollowObjects;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (startWithTutorial)
            tutorialActive = true;

        player1health.SetActive(false);
        player2health.SetActive(false);
        player3health.SetActive(false);
        player4health.SetActive(false);
        repairInfoText.gameObject.SetActive(false);
        medKitInfoText.text = "Press E/Button A to heal";
        medKitInfoText.gameObject.SetActive(false);

        ShowPlayerHealth();
    }

    void Update()
    {
        leafCountText.text = "Leafs: " + leafCount;
        stickCountText.text = "Sticks: " + stickCount;

        Tutorial();
    }

    void ShowPlayerHealth()
    {
        if(PlayerHandler.instance.player1active)
            player1health.SetActive(true);
        if (PlayerHandler.instance.player2active)
            player2health.SetActive(true);
        if (PlayerHandler.instance.player3active)
            player3health.SetActive(true);
        if (PlayerHandler.instance.player4active)
            player4health.SetActive(true);
    }

    public void ShowTextAbovePlayer(GameObject player, Text interactableText, bool isCalled = false)
    {
        if (!isCalled)
        {
            Vector3 screenPos = uiCamForTextFollowObjects.WorldToScreenPoint(player.transform.position);
            interactableText.rectTransform.position = new Vector2(screenPos.x, screenPos.y + 1);
            isCalled = true;
        }
    }

    public void ShowTextNextToObject(GameObject gameObject, Text interactableText, bool isCalled = false)
    {
        if (!isCalled)
        {
            Vector3 screenPos = uiCamForTextFollowObjects.WorldToScreenPoint(gameObject.transform.position);
            interactableText.rectTransform.position = new Vector2(screenPos.x, screenPos.y + 1);
            isCalled = true;
        }
    }

    public void ResetMedkitInfoText()
    {
        medKitInfoText.text = "Press E/Button A to heal";
        medKitInfoText.gameObject.SetActive(false);
    }

    public void ResetRepairInfoText()
    {
        repairInfoText.text = "Press E/Button A to repair hole.";
        repairInfoText.gameObject.SetActive(false);
    }

    void Tutorial()
    {
        if (tutorialActive)
            tutorialPanel.SetActive(true);
        else
            tutorialPanel.SetActive(false);

        if (tutorialActive)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("J1ButtonA") || Input.GetButtonDown("J2ButtonA") || Input.GetButtonDown("J3ButtonA") || Input.GetButtonDown("J4ButtonA"))
                tutorialActive = false;
        }
    }
}
