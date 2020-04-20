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

            
        

        SteeringInteraction();
        RudderMovement(change);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + change * moveSpeed * Time.fixedDeltaTime);
        
        if (HookThrower.BoatHooked)
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
                    !HookThrower.BoatHooked)
                {
                    nextRudderInteractSoundfx = Time.time + soundFxReplayTimer;
                    SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[11];
                    SoundManager.instance.soundFxSource.Play();
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
        if (!IscollidingWithWall)
        {
            rb.MovePosition(transform.position + Vector3.down *
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
        if (collision.gameObject.tag.Equals("LandTile"))
        {
            IscollidingWithWall = true;
            Debug.Log("Hitting wall!");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        iscollidingWithWall = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerCounter = 0;
        if (!aIController.isChecked)
            return;

        if (collision.gameObject.tag.Equals("Hook"))
        {
            RaftAudio.clip = SoundManager.instance.soundFx[8];
            RaftAudio.Play();
            HookThrower.BoatHooked = true;
            Debug.Log("Hooked!");
        }
        else if (collision.gameObject.tag.Equals("Stone"))
        {


        }
        else if (collision.gameObject.tag.Equals("Pickup"))
            Debug.Log("IgnoreColliding pick up for now."); 
        if (aIController.isChecked && collision.gameObject.name.Equals(AttackScript.GetActivePlayers().name))
        {
            playerCounter++;
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
