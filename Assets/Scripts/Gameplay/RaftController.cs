using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RaftController : MonoBehaviour
{
    public static RaftController instance = null;

    public bool raftIsInUse = false;
    public string raftUser;
    public Transform rudder;
    public Sprite[] rudderSpriteArray = new Sprite[2];
    public SpriteRenderer rudderSprite;
    public Rigidbody2D rb;
    public Vector3 change = Vector3.zero;
    public int moveSpeed = 15;

    public GameObject landTileMap;

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

    static bool hookMoving = false;
    static bool iscollidingWithWall = false; // Preventing the raft to collide multiple times when pulled to shore.
    static bool allPlayersOnRaft = false;

    public static bool HookMoving { get => hookMoving; set => hookMoving = value; }
    public static bool IscollidingWithWall { get => iscollidingWithWall; set => iscollidingWithWall = value; }
    public static bool AllPlayersOnRaft { get => allPlayersOnRaft; set => allPlayersOnRaft = value; }

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

        raftAudio = GetComponent<AudioSource>();
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

            rb.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
        if (HookThrower.BoatHooked)
        {
            PlayerController.instance.isSteeringRaft = false;
            PullRaftToShore();
        } 

        SteeringInteraction();
        RudderMovement(change);
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
        {
            p4distance = Vector2.Distance(player4transform.position, RaftController.instance.rudder.transform.position);
        }
            

        if (!RaftController.instance.raftIsInUse)
        {
            if (p1distance < 1f || p2distance < 1f || p3distance < 1f || p4distance < 1f)
            {
                if (!SoundManager.instance.soundFxSource.isPlaying && Time.time > 5f)
                {
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
            PlayerController.instance.RaftIsPulled = true;
            PlayerController.instance.isOnRaft = false;
            rb.MovePosition(transform.position + Vector3.down *
                Time.deltaTime * 1);
        }
        else
        {
            PlayerController.instance.RaftIsPulled = false;
            PlayerController.instance.isOnRaft = true;
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

        if (collision.gameObject.tag.Equals("Hook"))
        {
            raftAudio.clip = SoundManager.instance.soundFx[8];
            raftAudio.Play();
            HookThrower.BoatHooked = true;
            Debug.Log("Hooked!");
        }
        else if (collision.gameObject.tag.Equals("Weapon"))
        {
            raftAudio.clip = SoundManager.instance.soundFx[8];
            raftAudio.Play();
        }
        else if (collision.gameObject.tag.Equals("Pickup"))
            Debug.Log("IgnoreColling pick up for now."); 
        else if (collision.gameObject.name.Equals(AttackScript.GetActivePlayers().name))
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
