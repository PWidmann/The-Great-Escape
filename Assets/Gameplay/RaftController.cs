using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RaftController : MonoBehaviour
{
    public static RaftController instance = null;

    public bool raftIsInUse = false;
    public string raftUser;
    public Transform rudder;
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
    }
    
    void Update()
    {
        if (!raftIsInUse)
        {
            change = Vector2.zero;
        }


        trackVelocity = (rb.position - lastPos) * 50;
        lastPos = rb.position;

        if (!HookThrower.BoatHooked)
            rb.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
        else
            PullRaftToShore();

        SteeringInteraction();
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
            transform.position = Vector2.MoveTowards(transform.position, hookThrowerTransform.position,
                Time.deltaTime * 1);
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
            HookThrower.BoatHooked = true;
            Debug.Log("Hooked!");
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
}
