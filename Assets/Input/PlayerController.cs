using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void Start()
    {
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
    }

    void Move()
    {
        InputAxisHandling();

        myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.deltaTime + Interactibles.instance.change);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SteeringWheel")
        {
            Debug.Log(playerControls + " is near the steering wheel!");
        }

        if (other.gameObject.tag == "Raft")
        {
            isOnRaft = true;
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        

        if (other.gameObject.tag == "SteeringWheel")
        {
            if (playerControls == "Keyboard")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log(playerControls + " is steering the raft!");
                }
            }
            else
            {
                if (Input.GetButtonDown(playerControls + "ButtonA"))
                {
                    Debug.Log(playerControls + " is steering the raft!");
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "SteeringWheel")
        {
            Debug.Log("Left Steering Wheel");
        }
        if (other.gameObject.tag == "Raft")
        {
            isOnRaft = false;
        }
    }
}
