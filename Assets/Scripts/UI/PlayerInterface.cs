using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    public static PlayerInterface instance = null;

    [Header("Tutorial")]
    public bool startWithTutorial = true;
    public GameObject tutorialPanel;
    public bool tutorialActive = false;

    [Header("Player Interface")]
    // Inventory References
    public Text leafCountText;
    public Text stickCountText;
    public Text medKitInfoText;
    public Text repairInfoText;

    public GameObject player1health;
    public GameObject player2health;
    public GameObject player3health;
    public GameObject player4health;

    // Player Inventory
    public int leafCount = 0;
    public int stickCount = 0;

    [SerializeField] Camera uiCamForTextFollowPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        player1health.SetActive(false);
        player2health.SetActive(false);
        player3health.SetActive(false);
        player4health.SetActive(false);
        repairInfoText.gameObject.SetActive(false);
        medKitInfoText.text = "Press E/Button A to heal";
        medKitInfoText.gameObject.SetActive(false);

        ShowPlayerHealth();

        if (startWithTutorial)
        {
            tutorialActive = true;
        }
    }

    // Update is called once per frame
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

    public void MakeRepairInfoTextAbovePlayer(GameObject player, bool isCalled = false)
    {
        if (!isCalled)
        {
            Vector3 screenPos = uiCamForTextFollowPlayer.WorldToScreenPoint(player.transform.position);
            repairInfoText.rectTransform.position = new Vector2(screenPos.x, screenPos.y + 1);
            isCalled = true;
        }
    }

    void Tutorial()
    {
        if (tutorialActive)
            tutorialPanel.SetActive(true);
        else
            tutorialPanel.SetActive(false);

        if (tutorialActive && Input.GetKeyDown(KeyCode.E) || tutorialActive && Input.GetButtonDown("J1ButtonA") || tutorialActive && Input.GetButtonDown("J2ButtonA") || tutorialActive && Input.GetButtonDown("J3ButtonA") || tutorialActive && Input.GetButtonDown("J4ButtonA"))
        {
            tutorialActive = false;
        }
    }
}
