using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Singleton
    public static PlayerController instance = null;

    //Fields for OverlapBox
    [SerializeField] PlayerInterface playerInterface;
    [SerializeField] Medkit medkit;
    [SerializeField] HoleManager hole;
    [SerializeField] Shield shield;
    [SerializeField] Hook hook;
    PlayerOverlapBox overLapBox;

    //Misc
    [SerializeField] AttackScript attackScript;
    bool playerListIsFilled;

    //Player movement and collision
    Rigidbody2D myRigidbody;
    int moveSpeed = 4;
    int playerNumber;
    string playerControls;
    public Vector3 change;
    public Vector2 changeToNormalize;
    public BoxCollider2D collider;
    bool hasExited;

    //Raft
    public GameObject raft;
    public bool isOnRaft = false;

    bool hasEnteredRaft = false;
    bool isColliding = false;

    public bool raftCanMove = true;
    bool raftIsPulled = false;

    //Interactibles
    public bool isSteeringRaft = false;
    public bool hasShield = false;

    //Player states
    bool gameOver = false;
    bool isDead;

    //Animation
    private Animator animator;
    private bool isMoving = false;
    bool canMove = true;
    
    //User Interface
    public float playerHealth = 100;
    public Image HealthBarImage;

    //Properties
    public bool RaftIsPulled { get => raftIsPulled; set => raftIsPulled = value; }
    public int PlayerNumber { get => playerNumber; }
    public string PlayerControls { get => playerControls; set => playerControls = value; }
    public PlayerOverlapBox OverLapBox { get => overLapBox; set => overLapBox = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool HasEnteredRaft { get => hasEnteredRaft; set => hasEnteredRaft = value; }
    public bool IsColliding { get => isColliding; set => isColliding = value; }

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        if (PlayerHandler.instance.playSceneActive)
        {
            myRigidbody = GetComponent<Rigidbody2D>();

            PlayerControls = GetPlayerController();
            Animator = GetComponent<Animator>();
            DestroyPlayerObjectIfNotActive();
        }

        if (instance == null)
            instance = this;
        OverLapBox = gameObject.GetComponent<PlayerOverlapBox>();
    }

    void Update()
    {
        if (OverLapBox.OverLappedCollider != null)
            OnOverLappingCollidersEnter2D();
        else if (OverLapBox.OverLappedCollider == null && !hasExited)
            OnOverLappingCollidersExit2D();

        FillPlayerList();
        RaftHandling();
        SwordAttack();
        ShieldUsage();
        DropShieldCheck();
        UpdatePlayerHealth();

        if (!MainMenu.instance.IsPauseMenuActive &&  CheckInput(this, "StartButton", KeyCode.Escape))
            MainMenu.instance.OpenPauseMenu(this);
        else if (!MainMenu.instance.IsInOptions && CheckInput(this, "StartButton", KeyCode.Escape) &&
            MainMenu.instance.IsPauseMenuActive)
            MainMenu.instance.ResumeGame();
    }

    private void FixedUpdate()
    {
        if(!PlayerInterface.instance.tutorialActive && !PlayerInterface.instance.win && !PlayerInterface.instance.gameOver)
            Move();
    }

    void UpdatePlayerHealth()
    {
        float targetHealth = (playerHealth / 100);

        if (HealthBarImage)
        {
            if (HealthBarImage.fillAmount < targetHealth)
            {
                HealthBarImage.fillAmount += Time.deltaTime/2;
                targetHealth += Time.deltaTime;
            }
        }
            
    }

    void Move()
    {
        InputAxisHandling();

        if (change != Vector3.zero && !isSteeringRaft)
        {
            isMoving = true;
            Animator.SetFloat("moveX", change.x);
            Animator.SetFloat("moveY", change.y);
            Animator.SetBool("isMoving", true);
        }
        else
        {
            isMoving = false;
            Animator.SetBool("isMoving", false);
        }

        if (isOnRaft)
        {
            if (isSteeringRaft)
            {
                Animator.SetFloat("moveX", 0.1f);

                if ((change.x > 0 || change.y > 0) && !SoundManager.instance.soundFxSource.isPlaying)
                {
                    SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[4]);
                }

                

                RaftController.instance.change = change;

                //Move the character with the raft
                myRigidbody.MovePosition(transform.position + RaftController.instance.change * RaftController.instance.moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //Move the character with the raft
                if (!HookThrower.BoatHooked)
                    myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.fixedDeltaTime + RaftController.instance.change * RaftController.instance.moveSpeed * Time.fixedDeltaTime);
                else
                {
                    Animator.SetBool("isMoving", false);
                    Invoke("GivePlayerControlsBack", 1f);
                }
            }
        }
        else
        {
            myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.fixedDeltaTime);
        }
        
    }

    void GivePlayerControlsBack()
    {
        myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.fixedDeltaTime + RaftController.instance.change * RaftController.instance.moveSpeed * Time.deltaTime);
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
        if (PlayerControls == "Keyboard")
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
            if (Input.GetAxisRaw(PlayerControls + "Horizontal") != 0)
            {
                changeToNormalize.x = Input.GetAxisRaw(PlayerControls + "Horizontal");
            }
            else
            {
                changeToNormalize.x = 0;
            }

            // Gamepad Y Axis
            if (Input.GetAxisRaw(PlayerControls + "Vertical") != 0)
            {
                changeToNormalize.y = Input.GetAxisRaw(PlayerControls + "Vertical");
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
        switch (PlayerNumber)
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

        if (distance < 1f && CheckInput(this, "ButtonA", KeyCode.E)
            && !RaftController.instance.raftIsInUse && !isSteeringRaft && !HookThrower.BoatHooked)
        {
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[3]);
            isSteeringRaft = true;
            RaftController.instance.raftIsInUse = true;
            RaftController.instance.raftUser = PlayerNumber.ToString();
            Debug.Log(RaftController.instance.raftUser + " is steering raft!");
        }
        else if (isSteeringRaft && CheckInput(this, "ButtonA", KeyCode.E))
        {
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[3]);
            isSteeringRaft = false;
            RaftController.instance.raftIsInUse = false;
            RaftController.instance.raftUser = null;
            Debug.Log("Player " + PlayerNumber + " stopped steering raft");
        }

        if (isSteeringRaft && distance >= 1f)
        {

            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[3]);
            isSteeringRaft = false;
            RaftController.instance.raftIsInUse = false;
            RaftController.instance.raftUser = null;
            Debug.Log("Player " + PlayerNumber + " stopped steering raft");
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
            medkit.Interact(this);
        }

        // Holes
        else if (overLapBox.OverLappedCollider.gameObject.tag.Equals("Hole"))
        {
            hasExited = false;
            playerInterface.repairInfoText.gameObject.SetActive(true);
            playerInterface.ShowTextAbovePlayer(gameObject, playerInterface.repairInfoText);
            hole.Interact(this);
        }

        // Shield
        if (overLapBox.OverLappedCollider.gameObject.tag.Equals("Shield"))
        {
            shield.Interact(this);
        }

        // Spear
        if (overLapBox.OverLappedCollider.gameObject.tag.Equals("Hook"))
        {
            hasExited = false;
            playerInterface.destroyHookInfoText.gameObject.SetActive(true);
            playerInterface.ShowTextAbovePlayer(gameObject, playerInterface.destroyHookInfoText);
            hook.Interact(this);

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
            playerInterface.ResetInfoTexts(playerInterface.medKitInfoText, "Interact to heal");
        }

        if (overLapBox.PreviousOverlappedColliders != null &&
           (overLapBox.PreviousOverlappedColliders.gameObject.tag.Equals("Hole") ||
            overLapBox.OverLappedCollider == null))
        {
            //overLapBox.PreviousOverlappedColliders = null;
            hasExited = true;
            playerInterface.ResetInfoTexts(playerInterface.repairInfoText, "Interact to repair");
        }

        if (overLapBox.PreviousOverlappedColliders != null &&
           (overLapBox.PreviousOverlappedColliders.gameObject.tag.Equals("Hook") ||
            overLapBox.OverLappedCollider == null))
        {
            hasExited = true;
            playerInterface.ResetInfoTexts(playerInterface.destroyHookInfoText, "Attack!");
        }
    }


    void SwordAttack()
    {
        if (CheckInput(this, "ButtonX", KeyCode.Space))
        {
            DropShield();
            Animator.SetTrigger("isAttacking");
        }
    }

    public void SetCanMoveTrue()
    {
        canMove = true;
    }

    void ShieldUsage()
    {
        if (CheckShieldInput("ButtonY", KeyCode.Q))
        {
            if (hasShield)
            {
                Animator.SetBool("isBlocking", true);
                canMove = false;
                change = Vector3.zero;
            }
        }

        if (CheckShieldInput("ButtonY", KeyCode.Q))
        {
            Animator.SetBool("isBlocking", false);
            canMove = true;
        }
    }

    void DropShieldCheck()
    {
        if (hasShield)
        {
            if (CheckInput(this, "ButtonB", KeyCode.F))
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

    public bool CheckInput(PlayerController playerController, string button, KeyCode key = 0)
    {
        if (!playerControls.Equals("Keyboard"))
            return Input.GetButtonDown(playerControls + button);
        else
            return Input.GetKeyDown(key);


    }

    bool CheckShieldInput(string button, KeyCode key = 0)
    {
        if (!playerControls.Equals("Keyboard") && canMove)
            return Input.GetButtonDown(playerControls + button);
        else if (!playerControls.Equals("Keyboard") && !canMove)
            return Input.GetButtonUp(playerControls + button);
        else if (playerControls.Equals("Keyboard") && canMove)
            return Input.GetKeyDown(key);
        else
            return Input.GetKeyUp(key);
    }

    public void DoDamage()
    {
        playerHealth -= 10;

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            FillPlayerList();

            gameObject.SetActive(false);

            // Needed to check which GameObject is still active.
            for (int i = 0; i < AttackScript.players.Count; i++)
            {
                if (!AttackScript.players[i].activeSelf)
                {
                    AttackScript.players.Remove(AttackScript.players[i]);
                    AttackScript.instance.RandomPlayerNumber = Random.Range(0, AttackScript.players.Count);
                }
            }
            if (AttackScript.players.Count == 0)
            {
                gameOver = true;
                ShowEndScreen();
            }
        }
    }

    void FillPlayerList()
    {
        if (PlayerHandler.instance.playSceneActive)
        {
            switch (PlayerNumber)
            {
                case 1:
                    if (!playerListIsFilled)
                    {
                        AttackScript.players.Add(gameObject);
                        playerListIsFilled = true;
                    }
                    break;
                case 2:
                    if (!playerListIsFilled)
                    {
                        AttackScript.players.Add(gameObject);
                        playerListIsFilled = true;
                    }
                    break;
                case 3:
                    if (!playerListIsFilled)
                    {
                        AttackScript.players.Add(gameObject);
                        playerListIsFilled = true;
                    }
                    break;
                case 4:
                    if (!playerListIsFilled)
                    {
                        AttackScript.players.Add(gameObject);
                        playerListIsFilled = true;
                    }
                    break;
            }
        }
    }

    public void ShowEndScreen()
    {
        // TODO
        // Endscreen einfügen
        // Mach das besser im PlayerInterface. Ist nur im PlayerController, damit du die Methode siehst.
        // Endscreen Funktion wird im separaten Endscreen script behandelt
        // Kann als event getriggered werden.
        SoundManager.instance.backGroundMusicSource.Stop();
        SoundManager.instance.soundFxSource.Stop();

        // Lose Sound
        SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[15]);
        Debug.Log("GameOver!");
        PlayerInterface.instance.gameOver = true;
    }

    public void ShowWinScreen()
    {
        SoundManager.instance.backGroundMusicSource.Stop();
        SoundManager.instance.soundFxSource.Stop();

        // TODO 
        // Siegesbedingung einfügen
        // Siegsound abspielen lassen -> // SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[14]);
        // Siegscreen einfügen
    }
}
