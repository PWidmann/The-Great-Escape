using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    public Animator animator;
    
    // Die änderungen hier wirken sich auf alle Gegner Charakter aus


    void Start()
    {
        
    }

    void Update()
    {
        // Float Werte -1 bis 1 für x und y, ein Vektor 2 mit der Momentanen Richtung des Charakters
        // Nach rechts = new Vector2(1, 0);
        // Nach links = new Vector2(-1, 0);
        // Nach unten = new Vector2(0, -1);
        // Nach oben = new Vector2(0, -1);

        // Hier Funktion machen für aktuelle Bewegung der 3er Gruppe in den "change" Vektor schreiben
        Vector2 change = new Vector2(1, 0);


        if (change == Vector2.zero)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);


        // Um die Attack Animation auszulösen, Animation wechselt danach automatisch wieder in Idle oder Walk:
        // animator.SetTrigger("isAttacking");

        // Sprites werden im Animator ausgewählt nach Bewegung des Charakters.
        animator.SetFloat("moveX", change.x);
        animator.SetFloat("moveY", change.y);
    }
}
