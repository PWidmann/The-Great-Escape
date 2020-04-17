using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    //Fields for OverlapBox
    [SerializeField] PlayerInterface playerInterface;
    PlayerOverlapBox overLapBox;

    Rigidbody2D myRigidbody;
    int moveSpeed = 16;
    int playerNumber;
    string playerControls;
    public Vector3 change;
    public Vector2 changeToNormalize;
    public GameObject raft;

    public bool isSteeringRaft = false;
    public bool hasShield = false;
    public bool isOnRaft = false;

    public bool raftCanMove = true;
    public BoxCollider2D collider;

    public static PlayerController instance = null;
    bool raftIsPulled = false;
    bool hasExited;

    //Animation
    private Animator animator;
    private bool isMoving = false;
    bool canMove = true;
    //User Interface

    public float playerHealth = 100;

    public bool RaftIsPulled { get => raftIsPulled; set => raftIsPulled = value; }

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        if (PlayerHandler.instance.playSceneActive)
        {
            myRigidbody = GetComponent<Rigidbody2D>();

            playerControls = GetPlayerController();
            animator = GetComponent<Animator>();
            DestroyPlayerObjectIfNotActive();
        }

        if (instance == null)
            instance = this;
        overLapBox = gameObject.GetComponent<PlayerOverlapBox>();
    }

    void Update()
    {
        if (overLapBox.OverLappedCollider != null)
            OnOverLappingCollidersEnter2D();
        else if (overLapBox.OverLappedCollider == null && !hasExited)
            OnOverLappingCollidersExit2D();

        Move();
        RaftHandling();
        SwordAttack();
        ShieldUsage();
        DropShieldCheck();
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

        if (change != Vector3.zero && !isSteeringRaft)
        {
            isMoving = true;
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
        }

        if (isOnRaft)
        {
            if (isSteeringRaft)
            {
                animator.SetFloat("moveX", 0.1f);

                if ((change.x > 0 || change.y > 0) && !SoundManager.instance.soundFxSource.isPlaying)
                {
                    SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[4];
                    SoundManager.instance.soundFxSource.Play();
                }

                

                RaftController.instance.change = change;

                //Move the character with the raft
                myRigidbody.MovePosition(transform.position + RaftController.instance.change * RaftController.instance.moveSpeed * Time.deltaTime);
            }
            else
            {
                //Move the character with the raft
                if (!HookThrower.BoatHooked)
                    myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.deltaTime + RaftController.instance.change * RaftController.instance.moveSpeed * Time.deltaTime);
                else
                {
                    animator.SetBool("isMoving", false);
                    Invoke("GivePlayerControlsBack", 1f);
                }
            }
        }
        else
        {
            myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
        }
        
    }

    void GivePlayerControlsBack()
    {
        myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.deltaTime + RaftController.instance.change * RaftController.instance.moveSpeed * Time.deltaTime);
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

            if (canMove)
            {
                change.x = changeToNormalize.x;
                change.y = changeToNormalize.y;
            }
            
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
            
            if (canMove)
            {
                change.x = changeToNormalize.x;
                change.y = changeToNormalize.y;
            }
        }

        // Player unable to fall off the raft.
        if (isOnRaft || RaftIsPulled)
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
            if (distance < 1f && Input.GetKeyDown(KeyCode.E) && !RaftController.instance.raftIsInUse && !isSteeringRaft
                && !HookThrower.BoatHooked)
            {
                SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[3];
                SoundManager.instance.soundFxSource.Play();
                isSteeringRaft = true;
                RaftController.instance.raftIsInUse = true;
                RaftController.instance.raftUser = playerNumber.ToString();
                Debug.Log(RaftController.instance.raftUser + " is steering raft!");
            }
            else if (isSteeringRaft && Input.GetKeyDown(KeyCode.E))
            {
                SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[3];
                SoundManager.instance.soundFxSource.Play();
                isSteeringRaft = false;
                RaftController.instance.raftIsInUse = false;
                RaftController.instance.raftUser = null;
                Debug.Log("Player " + playerNumber + " stopped steering raft");
            }
        }
        else // Gamepad
        {
            if (distance < 1f && Input.GetButtonDown(playerControls + "ButtonA") && !RaftController.instance.raftIsInUse && 
                !isSteeringRaft && !HookThrower.BoatHooked)
            {
                SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[3];
                SoundManager.instance.soundFxSource.Play();
                isSteeringRaft = true;
                RaftController.instance.raftIsInUse = true;
                RaftController.instance.raftUser = playerNumber.ToString();
                Debug.Log(RaftController.instance.raftUser + " is steering raft!");
            }
            else if (isSteeringRaft && Input.GetButtonDown(playerControls + "ButtonA"))
            {
                SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[3];
                SoundManager.instance.soundFxSource.Play();
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

    void OnOverLappingCollidersEnter2D()
    {
        //Interactibles have to be on layer 15

        //Medkit
        if (overLapBox.OverLappedCollider.gameObject.tag.Equals("Medkit"))
        {
            hasExited = false;
            playerInterface.medKitInfoText.gameObject.SetActive(true);

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("J" + playerNumber + "ButtonA"))
                && playerInterface.leafCount < 2)
                playerInterface.medKitInfoText.text = "Not enough leafes.";
            else if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("J" + playerNumber + "ButtonA"))
                && playerHealth == 100 && playerInterface.leafCount >= 2)
                playerInterface.medKitInfoText.text = "You already have max health.";
            else if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("J" + playerNumber + "ButtonA"))
                && playerHealth < 100 && playerInterface.leafCount >= 2)
            {
                playerInterface.medKitInfoText.text = "You healed yourself.";
                playerHealth += 80;
                if (playerHealth > 100)
                    playerHealth = 100;
                playerInterface.leafCount -= 2;
                if (playerInterface.leafCount < 0)
                    playerInterface.leafCount = 0;

            }
        }

        // Shield
        if (overLapBox.OverLappedCollider.gameObject.tag.Equals("Shield"))
        { 
            if((Input.GetKeyDown(KeyCode.E)) || Input.GetButtonDown("J" + playerNumber + "ButtonA"))
            {
                if (!RaftController.instance.shieldIsInUse)
                {
                    RaftController.instance.shieldObject.SetActive(false);
                    hasShield = true;
                    RaftController.instance.shieldIsInUse = true;
                }
            }
        }
        else if (overLapBox.OverLappedCollider.gameObject.tag.Equals("Hole"))
        {
            hasExited = false;
            playerInterface.repairInfoText.gameObject.SetActive(true);
            playerInterface.MakeRepairInfoTextAbovePlayer(gameObject);

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("J" + playerNumber + "ButtonA"))
                && playerInterface.stickCount < 1)
                playerInterface.repairInfoText.text = "Not enough sticks.";
            else if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("J" + playerNumber + "ButtonA"))
                && playerInterface.stickCount >= 1)
            {
                playerInterface.medKitInfoText.text = "You repaired the hole.";
                HoleManager.Instance.holes.Add(overLapBox.OverLappedCollider.gameObject);
                RaftHoleActivator.DisableSpriteRenderer(overLapBox.OverLappedCollider.gameObject);
                playerInterface.stickCount--;
                Debug.Log("StickCount: " + playerInterface.stickCount);
                if (playerInterface.stickCount < 0)
                    playerInterface.stickCount = 0;
            }
        }
    }

    void SwordAttack()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("J" + playerNumber + "ButtonRB"))
        {
            DropShield();
            animator.SetTrigger("isAttacking");
        }
    }

    public void SetCanMoveTrue()
    {
        canMove = true;
    }

    void ShieldUsage()
    {
        if ((Input.GetKeyDown(KeyCode.Q)) || Input.GetButtonDown("J" + playerNumber + "ButtonY"))
        {
            if (hasShield)
            {
                animator.SetBool("isBlocking", true);
                canMove = false;
                change = Vector3.zero;
            }
        }

        if ((Input.GetKeyUp(KeyCode.Q)) || Input.GetButtonUp("J" + playerNumber + "ButtonY"))
        {
            animator.SetBool("isBlocking", false);
            canMove = true;
        }
    }

    void DropShieldCheck()
    {
        if (hasShield)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("J" + playerNumber + "ButtonB"))
            {
                hasShield = false;
                RaftController.instance.shieldIsInUse = false;
                RaftController.instance.shieldObject.transform.position = transform.position;
                RaftController.instance.shieldObject.SetActive(true);
            }
        }
    }

    void DropShield()
    {
        if (hasShield)
        {
            hasShield = false;
            RaftController.instance.shieldIsInUse = false;
            RaftController.instance.shieldObject.transform.position = transform.position;
            RaftController.instance.shieldObject.SetActive(true);
        }
    }

    void OnOverLappingCollidersExit2D()
    {
        if (overLapBox.PreviousOverlappedColliders != null && 
           (overLapBox.PreviousOverlappedColliders.gameObject.tag.Equals("Medkit") || 
            overLapBox.OverLappedCollider == null))
        {
            //overLapBox.PreviousOverlappedColliders = null;
            hasExited = true;
            ResetMedkitInfoText();
        }
        
        if (overLapBox.PreviousOverlappedColliders != null &&
           (overLapBox.PreviousOverlappedColliders.gameObject.tag.Equals("Hole") ||
            overLapBox.OverLappedCollider == null))
        {
            //overLapBox.PreviousOverlappedColliders = null;
            hasExited = true;
            ResetRepairInfoText();
        }
    }

    void ResetMedkitInfoText()
    {
        playerInterface.medKitInfoText.text = "Press E/Button A to heal";
        playerInterface.medKitInfoText.gameObject.SetActive(false);
    }

    void ResetRepairInfoText()
    {
        playerInterface.repairInfoText.text = "Press E/Button A to repair hole.";
        playerInterface.repairInfoText.gameObject.SetActive(false);
    }
}
