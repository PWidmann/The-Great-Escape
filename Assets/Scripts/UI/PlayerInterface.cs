using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInterface : MonoBehaviour
{
    public static PlayerInterface instance = null;

    [Header("Debug Options")]
    public bool startWithTutorial = true;
    public bool skipGameToFinish = true;

    // Tutorial
    [Header("Tutorial")]
    public GameObject tutorialPanel;
    public bool tutorialActive = false;

    // Inventory References
    [Header("Gameplay")]
    public Text leafCountText;
    public Text stickCountText;
    public Text medKitInfoText;
    public Text repairInfoText;
    public Text destroyHookInfoText;

    public GameObject player1health;
    public GameObject player2health;
    public GameObject player3health;
    public GameObject player4health;

    // Player Inventory
    public int leafCount = 0;
    public int stickCount = 0;

    [Header("Endscreen")]
    // End Screen
    public bool gameOver = false;
    public bool win = false;
    public GameObject endScreenPanel;
    public GameObject player1Object;
    public GameObject player2Object;
    public GameObject player3Object;
    public GameObject player4Object;
    public GameObject RaftPosition;
    public BoxCollider2D raftPlayerCollider;
    public GameObject endFlag;
    float p1distance = 10f;
    float p2distance = 10f;
    float p3distance = 10f;
    float p4distance = 10f;
    float raftDistanceToEnd = 100f;

    [SerializeField] Camera uiCamForTextFollowObjects;

    [SerializeField] GameObject treasureObject;

    public AudioSource pickUpAudio;

    public Text treasureWarningText;

    public float RaftDistanceToEnd { get => raftDistanceToEnd; set => raftDistanceToEnd = value; }

    void Start()
    {
        if (instance == null)
            instance = this;

        if (startWithTutorial)
            tutorialActive = true;

        player1health.SetActive(false);
        player2health.SetActive(false);
        player3health.SetActive(false);
        player4health.SetActive(false);
        repairInfoText.gameObject.SetActive(false);
        medKitInfoText.text = "Interact to heal";
        medKitInfoText.gameObject.SetActive(false);
        endScreenPanel.SetActive(false);

        ShowPlayerHealth();
        SkipGameToFinish();
    }

    void Update()
    {
        leafCountText.text = "Leafs: " + leafCount;
        stickCountText.text = "Sticks: " + stickCount;

        Tutorial();
        GameOver();

        if (gameOver)
            treasureWarningText.gameObject.SetActive(false);
    }

    void ShowPlayerHealth()
    {
        // Show only player health of active players
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

    public void ResetInfoTexts(Text uiText, string message)
    {
        uiText.text = message;
        uiText.gameObject.SetActive(false);
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

    void GameOver()
    {
        CheckPlayersForEndCheckPoint();
        CheckRaftPositionAndSetCollision();

        if (gameOver)
            endScreenPanel.SetActive(true);
    }

    void CheckPlayersForEndCheckPoint()
    {
        if(player1Object)
            p1distance = Vector2.Distance(player1Object.transform.position, endFlag.transform.position);
        if (player2Object)
            p1distance = Vector2.Distance(player1Object.transform.position, endFlag.transform.position);
        if (player3Object)
            p1distance = Vector2.Distance(player1Object.transform.position, endFlag.transform.position);
        if (player4Object)
            p1distance = Vector2.Distance(player1Object.transform.position, endFlag.transform.position);

        if ((p1distance < 1f || p2distance < 1f || p3distance < 1f || p4distance < 1f) &&
            PlayerController.hasTreasureTaken)
        {
            win = true;
            endScreenPanel.SetActive(true);
        }
        else if ((p1distance < 1f || p2distance < 1f || p3distance < 1f || p4distance < 1f) &&
            !PlayerController.hasTreasureTaken)
            treasureWarningText.gameObject.SetActive(true);
        else 
            treasureWarningText.gameObject.SetActive(false);
    }

    void CheckRaftPositionAndSetCollision()
    { 
        raftDistanceToEnd = Vector2.Distance(RaftPosition.transform.position, endFlag.transform.position);

        if (raftDistanceToEnd <= 21f && PlayerController.hasTreasureTaken)
            raftPlayerCollider.enabled = false;

        else if (raftDistanceToEnd <= 21f && !PlayerController.hasTreasureTaken)
            treasureObject.layer = 15;
    }

    void SkipGameToFinish()
    {
        if (skipGameToFinish)
        {
            Vector3 nearEndRaftPosition = new Vector3(TileMapGenerator.instance.mapWidth - 30, (TileMapGenerator.instance.mapHeight / 2) + 2);
            Vector3 nearEndPlayerPosition = new Vector3(TileMapGenerator.instance.mapWidth - 30, (TileMapGenerator.instance.mapHeight / 2) + 1);

            //Raft
            TileMapGenerator.instance.raft.transform.position = nearEndRaftPosition;

            //Players
            TileMapGenerator.instance.player1.transform.position = nearEndPlayerPosition;
            TileMapGenerator.instance.player2.transform.position = nearEndPlayerPosition;
            TileMapGenerator.instance.player3.transform.position = nearEndPlayerPosition;
            TileMapGenerator.instance.player4.transform.position = nearEndPlayerPosition;
        }
    }
}
