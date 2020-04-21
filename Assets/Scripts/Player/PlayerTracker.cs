using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    static bool weaponMoving = false;
    static bool isColliding = false;

    [SerializeField] Transform raftTransform;

    [SerializeField] PlayerController playerController;

    public bool WeaponMoving { get => weaponMoving; set => weaponMoving = value; }
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
        if (collision.gameObject.tag.Equals("Spear"))
        {
            isColliding = true;
            playerController.DoDamage();
            AttackScript.instance.DisableWeapon(collision.gameObject);
            Debug.Log("Collsion!");
        }
    }
}
