using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayerSorting : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    private float player1yPos;
    private float player2yPos;
    private float player3yPos;
    private float player4yPos;

    private int playerNumber;

    void Update()
    {
        UpdatePositions();

        SortPlayersInLayer();
    }

    void SortPlayersInLayer()
    {
        if(player1)    
            player1.GetComponent<SpriteRenderer>().sortingOrder = -(int)(player1.transform.position.y * 100);
        if (player2)
            player2.GetComponent<SpriteRenderer>().sortingOrder = -(int)(player2.transform.position.y * 100);
        if (player3)
            player3.GetComponent<SpriteRenderer>().sortingOrder = -(int)(player3.transform.position.y * 100);
        if (player4)
            player4.GetComponent<SpriteRenderer>().sortingOrder = -(int)(player4.transform.position.y * 100);
    }

    void UpdatePositions()
    {
        if (PlayerHandler.instance.player1active)
        { 
            if(player1)
            player1yPos = player1.transform.position.y;
        }
        if (PlayerHandler.instance.player2active)
        {
            if (player2)
                player2yPos = player2.transform.position.y;
        }
        if (PlayerHandler.instance.player3active)
        {
            if (player3)
                player3yPos = player3.transform.position.y;
        }
        if (PlayerHandler.instance.player4active)
        {
            if (player4)
                player4yPos = player4.transform.position.y;
        }
    }
}
