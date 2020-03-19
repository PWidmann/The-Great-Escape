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

    /* 
    So the CURRENT raft position is at e.g. (0, 0)
    Now the player moves it left for 1 unit. It is then (-1, 0)
    Now the CURRENT position is (-1, 0) and the OLD position is (0, 0).
    Create fields that contains current position and old position. -> Done
    How does the OLD position gets saved? When I do OLD = CURRENT before the movement happens.
    Again CURRENT = (0, 0) -> CURRENT = OLD -> move 1 u left => CURRENT = (-1, 0), OLD = (0, 0).
    But everytime it stops moving -> CURRENT = OLD => (-1, 0) = (-1, 0).
    => Making return in Update to jump out of the function when I don't need it. -> Done
    But as soon as the player moves the raft it updates the position while it is not arrived.
    A boolean field e.g hookMoving needed that turns true when thrown. 
    */

    static Vector3 currentPosition;
    static Vector3 oldPosition;
    static bool hookMoving = false;
    static bool iscollidingWithWall = false; // Preventing the raft to collide multiple times when pulled to shore.

    public static Vector3 CurrentPosition { get => currentPosition; set => currentPosition = value; }
    public static Vector3 OldPosition { get => oldPosition; set => oldPosition = value; }
    public static bool HookMoving { get => hookMoving; set => hookMoving = value; }
    public static bool IscollidingWithWall { get => iscollidingWithWall; set => iscollidingWithWall = value; }

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
        CurrentPosition = transform.position;
        OldPosition = CurrentPosition;
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
        UpdateRaftPostion();
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
        if (collision.gameObject.tag.Equals("Hook"))
        {
            HookThrower.BoatHooked = true;
            Debug.Log("Hooked!");
        }
    }

    void UpdateRaftPostion()
    {
        if (transform.position != oldPosition)
            CurrentPosition = transform.position;
        OldPosition = CurrentPosition;
    }
}
