using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlossController : MonoBehaviour
{
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

    [SerializeField] int movementSpeedXdir = 1;
    [SerializeField] int rotationSpeed = 1;
    [SerializeField] Transform hookThrowerTransform;

    // Start is called before the first frame update
    void Start()
    {
        CurrentPosition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if (!HookThrower.BoatHooked)
            SteerFloss();
        else
            PullRaftToShore();

    }

    void SteerFloss()
    {
        float xNew = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeedXdir;
        float zRotation = Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed;
        if (xNew != 0)
            CurrentPosition = transform.position;
        else if (!HookMoving)
        {
            OldPosition = CurrentPosition;
            return;
        }
        transform.Translate(xNew, 0, 0);
        transform.Rotate(0, 0, zRotation);
        Debug.Log("Current: " + CurrentPosition + "Old: " + OldPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Hook"))
        {
            HookThrower.BoatHooked = true;
            Debug.Log("Hooked!");
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
        if (collision.gameObject.tag.Equals("Wall"))
        {
            IscollidingWithWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        iscollidingWithWall = false;
    }
}
