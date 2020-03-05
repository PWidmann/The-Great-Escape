using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactibles : MonoBehaviour
{
    public static Interactibles instance = null;

    public bool raftIsInUse = false;
    GameObject steeringWheel;
    public Rigidbody2D rb;
    public Vector3 change = Vector3.zero;

    public Rigidbody2D player1rb;
    public Rigidbody2D player2rb;
    public Rigidbody2D player3rb;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

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
        steeringWheel = GameObject.Find("SteeringTrigger");

    }
    
    void Update()
    {

        if (Input.GetKey(KeyCode.P))
        {
            change.x = 1;
            change *= Time.deltaTime;
        }
        else
        {
            change = Vector3.zero;
        }

        rb.MovePosition(new Vector3(transform.position.x, transform.position.y) + change);
    }
}
