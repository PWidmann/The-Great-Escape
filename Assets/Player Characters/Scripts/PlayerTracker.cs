using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    /*
     * Same algorithm needed as in "FlossController.cs" for the CURRENT and OLD position. 
     */

    Vector3 currentPosition;
    Vector3 oldPosition;
    static bool weaponMoving = false;
    static bool isColliding = false;

    [SerializeField] Transform raftTransform;

    public Vector3 CurrentPosition { get => currentPosition; set => currentPosition = value; }
    public Vector3 OldPosition { get => oldPosition; set => oldPosition = value; }
    public bool WeaponMoving { get => weaponMoving; set => weaponMoving = value; }
    public static bool IsColliding { get => isColliding; set => isColliding = value; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentPosition = transform.position;
        OldPosition = CurrentPosition;
    }

    void Update()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        if (transform.position != OldPosition)
            CurrentPosition = transform.position;
        OldPosition = CurrentPosition;
        Debug.Log("[Player pos] Current: " + CurrentPosition + "Old: " + OldPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Weapon"))
        {
            isColliding = true;
            collision.gameObject.SetActive(false);
            Debug.Log("Collsion!");
        }
    }
}
