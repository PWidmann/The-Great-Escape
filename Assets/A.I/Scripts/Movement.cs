using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform targetfloßObject;
    [SerializeField] int movementSpeed = 1;
    [SerializeField] Transform raftRotation;
    
    public void Move()
    {
        gameObject.SetActive(true);
        transform.position = Vector2.MoveTowards(
            transform.position, targetfloßObject.position, Time.deltaTime * movementSpeed);
    }
}
