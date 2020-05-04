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
        float lookDirection = playerController.Animator.GetFloat("moveY");
        bool isAbovePlayer = collision.gameObject.transform.position.y > gameObject.transform.position.y;

        if (collision.gameObject.tag.Equals("Spear") && !playerController.Animator.GetBool("isBlocking"))
            DoDamage(collision.gameObject);

        else if (collision.gameObject.tag.Equals("Spear") && playerController.Animator.GetBool("isBlocking") &&
            (isAbovePlayer && lookDirection < 0 || !isAbovePlayer && lookDirection > 0))
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
