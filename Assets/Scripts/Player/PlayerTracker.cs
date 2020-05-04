using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    static bool isColliding = false;

    [SerializeField] Transform raftTransform;
    [SerializeField] PlayerController playerController;
    [SerializeField] AudioSource playerAudio;

    public static bool IsColliding { get => isColliding; set => isColliding = value; }

    void Update()
    {
        GetPlayerPos();
    }

    public Vector2 GetPlayerPos()
    {
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float yLookDirection = playerController.Animator.GetFloat("moveY");
        float xLookDirection = playerController.Animator.GetFloat("moveX");

        bool isAbovePlayer = collision.gameObject.transform.position.y > gameObject.transform.position.y;
        bool isRightFromPlayer = collision.gameObject.transform.position.x > gameObject.transform.position.x;

        if (collision.gameObject.tag.Equals("Spear") && !playerController.Animator.GetBool("isBlocking"))
            DoDamage(collision.gameObject);

        else if (collision.gameObject.tag.Equals("Spear") && playerController.Animator.GetBool("isBlocking") &&
            (isAbovePlayer && yLookDirection < 0 || !isAbovePlayer && yLookDirection > 0))
            DoDamage(collision.gameObject);

        else if (collision.gameObject.tag.Equals("Spear") && playerController.Animator.GetBool("isBlocking") &&
            (isRightFromPlayer && xLookDirection < 0 || !isRightFromPlayer && xLookDirection > 0))
            DoDamage(collision.gameObject);

        else if (collision.gameObject.tag.Equals("Spear") && playerController.Animator.GetBool("isBlocking"))
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[6], playerAudio);
    }

    void DoDamage(GameObject gameObject)
    {
        SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[9], playerAudio);
        isColliding = true;
        playerController.DoDamage();
        AttackScript.instance.DisableWeapon(gameObject);
        Debug.Log("Collsion!");
    }
}
