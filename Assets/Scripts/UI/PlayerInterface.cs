using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    public static PlayerInterface instance = null;


    // Inventory References
    public Text leafCountText;
    public Text stickCountText;
    public Text medKitInfoText;

    public GameObject player1health;
    public GameObject player2health;
    public GameObject player3health;
    public GameObject player4health;

    // Player Inventory
    public int leafCount = 0;
    public int stickCount = 0;

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
        medKitInfoText.text = "Press E/Button A to heal";
        medKitInfoText.gameObject.SetActive(false);

        ShowPlayerHealth();
    }

    // Update is called once per frame
    void Update()
    {
        leafCountText.text = "Leafs: " + leafCount;
        stickCountText.text = "Sticks: " + stickCount;
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
}
