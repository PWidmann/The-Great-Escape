using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    public static AIAnimation instance;

    public Animator animator;

    Vector3 previousPosition;
    Vector2 currentVelocity = Vector2.zero;

    float checkTimer = 0f;


    void Start()
    {
        if (instance == null)
            instance = this;

        previousPosition = transform.position;
    }

    void Update()
    {
        checkTimer += Time.deltaTime;

        if (checkTimer >= 0.2f)
        {
            checkTimer = 0f;

            if (previousPosition != null)
            {
                currentVelocity = (previousPosition - transform.position) / Time.deltaTime * -1;
            }

            previousPosition = transform.position;

            currentVelocity = currentVelocity.normalized;
        }


        if (currentVelocity == Vector2.zero)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);

        // Set animation state dependable on movement.
        animator.SetFloat("moveX", currentVelocity.x);
        animator.SetFloat("moveY", currentVelocity.y);

        // If the raft is over the enemy group
        if (RaftController.instance.transform.position.y > transform.position.y)
        {
            animator.SetFloat("moveY", 0.5f);
        }
    }

    public void TriggerAttackAnimation()
    {
        animator.SetTrigger("isAttacking");
    }
}
