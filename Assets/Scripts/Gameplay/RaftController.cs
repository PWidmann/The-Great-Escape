using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RaftController : MonoBehaviour
{
    public static RaftController instance = null;

    public bool raftIsInUse = false;
    public bool shieldIsInUse = false;
    public string raftUser;
    public Transform rudder;
    public Sprite[] rudderSpriteArray = new Sprite[2];
    public SpriteRenderer rudderSprite;
    public Rigidbody2D rb;
    public Vector3 change = Vector3.zero;
    public float moveSpeed = 6f;

    public GameObject landTileMap;

    public GameObject shieldObject;

    public bool isColliding = false;

    public Vector2 trackVelocity;
    public Vector2 lastPos;

    public GameObject rudderInteractText;
    public Transform player1transform;
    public Transform player2transform;
    public Transform player3transform;
    public Transform player4transform;

    public Transform hookThrowerTransform;

    AudioSource raftAudio;
    Collider2D raftCollider;

    float soundFxReplayTimer = 1.5f;
    float nextRudderInteractSoundfx = 0f;

    List<string> playerWithRaftCollisions = new List<string>();
    int playerCounter = 0;

    string previousGOName;

    public static bool isPlayerLeavingRaft = false; // For AI stop moving when one player is at end of river.

    [SerializeField] AIController aIController;

    static bool hookMoving = false;
    static bool iscollidingWithWall = false; // Preventing the raft to collide multiple times when pulled to shore.
    static bool allPlayersOnRaft = false;
    bool isHitByStone = false;

    public static bool HookMoving { get => hookMoving; set => hookMoving = value; }
    public static bool IscollidingWithWall { get => iscollidingWithWall; set => iscollidingWithWall = value; }
    public static bool AllPlayersOnRaft { get => allPlayersOnRaft; set => allPlayersOnRaft = value; }
    public bool IsHitByStone { get => isHitByStone; set => isHitByStone = value; }
    public AudioSource RaftAudio { get => raftAudio; set => raftAudio = value; }
    public Collider2D RaftCollider { get => raftCollider; set => raftCollider = value; }
    public int PlayerCounter { get => playerCounter; set => playerCounter = value; }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rudderInteractText.SetActive(false);
        lastPos = rb.position;

        RaftAudio = GetComponent<AudioSource>();
        raftCollider = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        if (!raftIsInUse)
        {
            change = Vector2.zero;
        }

        trackVelocity = (rb.position - lastPos) * 50;
        lastPos = rb.position;

        if (!PlayerInterface.instance.gameOver || !PlayerInterface.instance.win)
            SteeringInteraction();
        else
            return;
        RudderMovement(change);

        Debug.Log("RaftSpeed: " + moveSpeed);
    }

    private void FixedUpdate()
    {
        //Prevent Raft from moving backwards
        if (change.x < 0)
            change.x = 0;

        rb.MovePosition(transform.position + change * moveSpeed * Time.fixedDeltaTime);
        
        if (AIController.RaftHooked)
        {
            PlayerController.instance.isSteeringRaft = false;
            raftIsInUse = false;
            raftUser = null;
            PullRaftToShore();
        }

    }

    void SteeringInteraction()
    {
        // Show interaction text when player is near rudder

        float p1distance = 10f;
        float p2distance = 10f;
        float p3distance = 10f;
        float p4distance = 10f;

        if (player1transform)
            p1distance = Vector2.Distance(player1transform.position, RaftController.instance.rudder.transform.position);
        if (player2transform)
            p2distance = Vector2.Distance(player2transform.position, RaftController.instance.rudder.transform.position);
        if (player3transform)
            p3distance = Vector2.Distance(player3transform.position, RaftController.instance.rudder.transform.position);
        if (player4transform)
            p4distance = Vector2.Distance(player4transform.position, RaftController.instance.rudder.transform.position);


        if (!RaftController.instance.raftIsInUse)
        {
            if (p1distance < 1f || p2distance < 1f || p3distance < 1f || p4distance < 1f)
            {
                if (!SoundManager.instance.soundFxSource.isPlaying && Time.time > nextRudderInteractSoundfx &&
                    !AIController.RaftHooked)
                {
                    nextRudderInteractSoundfx = Time.time + soundFxReplayTimer;
                    SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[11]);
                }
                rudderInteractText.SetActive(true);
            }
            else
            {
                rudderInteractText.SetActive(false);
            }
        }
        else
        {
            rudderInteractText.SetActive(false);
        }
    }

    void PullRaftToShore()
    {
        if (!IscollidingWithWall && HookThrower.hook != null)
        {
            bool isAboveRaft = HookThrower.hook.transform.position.y > transform.position.y;
            rb.MovePosition(transform.position + 1 * (isAboveRaft ? Vector3.up : Vector3.down) *
                Time.fixedDeltaTime * 1);
        }
    }

    void RudderMovement(Vector3 change)
    {
        // Change rudder sprite depending on movement
        if (change.y == 0f)
        {
            rudderSprite.sprite = rudderSpriteArray[0];
        }

        if (change.y > 0f)
        {
            rudderSprite.sprite = rudderSpriteArray[2];
        }

        if (change.y < 0f)
        {
            rudderSprite.sprite = rudderSpriteArray[1];
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("LandTile") && !AIController.RaftHooked)
        {
            // LandTile hit sound
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[16], raftAudio);
            IscollidingWithWall = true;
            //Debug.Log("Hitting wall!");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        iscollidingWithWall = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Hook"))
        {
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[8], raftAudio);
            AIController.RaftHooked = true;
            Debug.Log("Hooked!");
        }
        if (collision.gameObject.tag.Equals("Player") && playerCounter <= AttackScript.players.Count && !allPlayersOnRaft)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.HasEnteredRaft = true;
            if (player.HasEnteredRaft && !player.IsColliding)
            {
                playerCounter++;
                player.IsColliding = true;
            }
            Debug.Log("RaftController playerCounter " + playerCounter);
            if (playerCounter == AttackScript.players.Count)
                allPlayersOnRaft = true;
        }
    }

    public Vector2 GetRaftPos()
    {
        return transform.position;
    }

    public Vector2 GetRaftColliderBoundSize()
    {
        return raftCollider.bounds.size;
    }

    public Collider2D GetRaftCollider()
    {
        return raftCollider;
    }
}
