using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    int moveSpeed = 16;
    int playerNumber;
    string playerControls;
    public Vector3 change;
    public Vector2 changeToNormalize;
    public GameObject raft;

    public bool isSteeringRaft = false;
    public bool isOnRaft = false;

    public bool raftCanMove = true;
    public BoxCollider2D collider;

    

    //User Interface

    public float playerHealth = 100;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        if (PlayerHandler.instance.playSceneActive)
        {
            myRigidbody = GetComponent<Rigidbody2D>();

            playerControls = GetPlayerController();
            DestroyPlayerObjectIfNotActive();
        }
    }

    void Update()
    {
        Move();
        RaftHandling();

        UpdatePlayerHealth();
    }

    void UpdatePlayerHealth()
    {
        if (PlayerHandler.instance.playSceneActive)
        {
            switch (playerNumber)
            {
                case 1:
                    PlayerInterface.instance.player1health.GetComponent<Text>().text = "Player 1 HP: " + playerHealth.ToString();
                    break;
                case 2:
                    PlayerInterface.instance.player2health.GetComponent<Text>().text = "Player 2 HP: " + playerHealth.ToString();
                    break;
                case 3:
                    PlayerInterface.instance.player3health.GetComponent<Text>().text = "Player 3 HP: " + playerHealth.ToString();
                    break;
                case 4:
                    PlayerInterface.instance.player4health.GetComponent<Text>().text = "Player 4 HP: " + playerHealth.ToString();
                    break;
            }
        }
            
    }

    void Move()
    {
        InputAxisHandling();

        if (isOnRaft)
        {
            if (isSteeringRaft)
            {
                RaftController.instance.change = change;

                //Move the character with the raft
                myRigidbody.MovePosition(transform.position + RaftController.instance.change * RaftController.instance.moveSpeed * Time.deltaTime);
            }
            else
            {
                //Move the character with the raft
                myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.deltaTime + RaftController.instance.change * RaftController.instance.moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
        }
        
    }

    string GetPlayerController()
    {
        if (PlayerHandler.instance.player1assigned)
        {
            if (PlayerHandler.instance.player2assigned)
            {
                if (PlayerHandler.instance.player3assigned)
                {
                    PlayerHandler.instance.player4assigned = true;
                    playerNumber = 4;
                    
                    return PlayerHandler.instance.player4controls;
                }
                else
                {
                    PlayerHandler.instance.player3assigned = true;
                    playerNumber = 3;
                    
                    return PlayerHandler.instance.player3controls;
                }
            }
            else
            {
                PlayerHandler.instance.player2assigned = true;
                playerNumber = 2;
                
                return PlayerHandler.instance.player2controls;

            }
        }
        else
        {
            PlayerHandler.instance.player1assigned = true;
            playerNumber = 1;
            
            return PlayerHandler.instance.player1controls;
            
        }

        
    }

    void InputAxisHandling()
    {
        if (playerControls == "Keyboard")
        {
            // Keyboard X Axis
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                changeToNormalize.x = Input.GetAxisRaw("Horizontal");

            }
            else
            {
                changeToNormalize.x = 0;

            }

            // Keyboard Y Axis
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                changeToNormalize.y = Input.GetAxisRaw("Vertical");

            }
            else
            {
                changeToNormalize.y = 0;

            }

            changeToNormalize.Normalize();
            change.x = changeToNormalize.x;
            change.y = changeToNormalize.y;
        }
        else
        {
            // Gamepad X Axis
            if (Input.GetAxisRaw(playerControls + "Horizontal") != 0)
            {
                changeToNormalize.x = Input.GetAxisRaw(playerControls + "Horizontal");
            }
            else
            {
                changeToNormalize.x = 0;
            }

            // Gamepad Y Axis
            if (Input.GetAxisRaw(playerControls + "Vertical") != 0)
            {
                changeToNormalize.y = Input.GetAxisRaw(playerControls + "Vertical");
            }
            else
            {
                changeToNormalize.y = 0;
            }

            changeToNormalize.Normalize();
            change.x = changeToNormalize.x;
            change.y = changeToNormalize.y;
        }

        // Player unable to fall off the raft.
        if (isOnRaft)
        {
            if (transform.position.x >= raft.transform.position.x + collider.bounds.size.x / 
                raft.transform.localScale.x)
                transform.position = new Vector2(transform.position.x - 0.1f, transform.position.y);
            else if (transform.position.x <= raft.transform.position.x - collider.bounds.size.x /
                raft.transform.localScale.x)
                transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
            else if (transform.position.y >= raft.transform.position.y + collider.bounds.size.y /
                raft.transform.localScale.y)
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.1f);
            else if (transform.position.y <= raft.transform.position.y - collider.bounds.size.y /
                raft.transform.localScale.y)
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
        }
    }

    void DestroyPlayerObjectIfNotActive()
    {
        switch (playerNumber)
        {
            case 1:
                if (!PlayerHandler.instance.player1active)
                {
                    if(gameObject)
                        Destroy(gameObject);
                }
                break;
            case 2:
                if (!PlayerHandler.instance.player2active)
                {
                    if (gameObject)
                        Destroy(gameObject);
                }
                break;
            case 3:
                if (!PlayerHandler.instance.player3active)
                {
                    if (gameObject)
                        Destroy(gameObject);
                }
                break;
            case 4:
                if (!PlayerHandler.instance.player4active)
                {
                    if (gameObject)
                        Destroy(gameObject);
                }
                break;
        }
    }

    void RaftHandling()
    {
        // Enter steering mode
        float distance = Vector2.Distance(transform.position, RaftController.instance.rudder.transform.position);

        if (playerControls == "Keyboard")
        {
            if (distance < 1f && Input.GetKeyDown(KeyCode.E) && !RaftController.instance.raftIsInUse && !isSteeringRaft)
            {
                isSteeringRaft = true;
                RaftController.instance.raftIsInUse = true;
                RaftController.instance.raftUser = playerNumber.ToString();
                Debug.Log(RaftController.instance.raftUser + " is steering raft!");
            }
            else if (isSteeringRaft && Input.GetKeyDown(KeyCode.E))
            {
                isSteeringRaft = false;
                RaftController.instance.raftIsInUse = false;
                RaftController.instance.raftUser = null;
                Debug.Log("Player " + playerNumber + " stopped steering raft");
            }
        }
        else // Gamepad
        {
            if (distance < 1f && Input.GetButtonDown(playerControls + "ButtonA") && !RaftController.instance.raftIsInUse && !isSteeringRaft)
            {
                isSteeringRaft = true;
                RaftController.instance.raftIsInUse = true;
                RaftController.instance.raftUser = playerNumber.ToString();
                Debug.Log(RaftController.instance.raftUser + " is steering raft!");
            }
            else if (isSteeringRaft && Input.GetButtonDown(playerControls + "ButtonA"))
            {
                isSteeringRaft = false;
                RaftController.instance.raftIsInUse = false;
                RaftController.instance.raftUser = null;
                Debug.Log("Player " + playerNumber + " stopped steering raft");
            }
        }
        
    }

    //For movement handling on raft
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Raft")
        {
            isOnRaft = true;
        }
    }

    //For movement handling on raft
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Raft")
        {
            isOnRaft = false;
        }
    }
}
