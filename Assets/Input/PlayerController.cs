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
    public Vector2 changeNormalized;

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

        myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
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
            if (Input.GetAxis("Horizontal") != 0f)
            {
                if (Input.GetAxis("Horizontal") > 0.01f) // Change to GetAxisRaw
                {
                    changeNormalized.x = 1;
                }
                if (Input.GetAxis("Horizontal") < -0.01f)
                {
                    changeNormalized.x = -1;
                }
                

            }
            else
            {
                changeNormalized.x = 0;
            }

            // Keyboard Y Axis
            if (Input.GetAxis("Vertical") != 0f)
            {
                if (Input.GetAxis("Vertical") > 0.01f)
                {
                    changeNormalized.y = 1;
                }
                if (Input.GetAxis("Vertical") < -0.01f)
                {
                    changeNormalized.y = -1;
                }
            }
            else
            {
                changeNormalized.y = 0;
            }

            changeNormalized.Normalize();
            change.x = changeNormalized.x;
            change.y = changeNormalized.y;
        }
        else
        {
            // Gamepad X Axis
            if (Input.GetAxis(playerControls + "Horizontal") != 0f)
            {
                if (Input.GetAxis(playerControls + "Horizontal") > 0.01f)
                {
                    changeNormalized.x = 1;
                }
                if (Input.GetAxis(playerControls + "Horizontal") < -0.01f)
                {
                    changeNormalized.x = -1;
                }


            }
            else
            {
                changeNormalized.x = 0;
            }

            // Gamepad Y Axis
            if (Input.GetAxis(playerControls + "Vertical") != 0f)
            {
                if (Input.GetAxis(playerControls + "Vertical") > 0.01f)
                {
                    changeNormalized.y = 1;
                }
                if (Input.GetAxis(playerControls + "Vertical") < -0.01f)
                {
                    changeNormalized.y = -1;
                }
            }
            else
            {
                changeNormalized.y = 0;
            }

            changeNormalized.Normalize();
            change.x = changeNormalized.x;
            change.y = changeNormalized.y;
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
}
