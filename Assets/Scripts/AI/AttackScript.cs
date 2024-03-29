﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackScript : MonoBehaviour
{
    [SerializeField] GameObject spearPrefab;
    [SerializeField] GameObject stonePrefab;
    Animator animator;
   
    public static List<GameObject> players = new List<GameObject>();
    public static AttackScript instance; 

    GameObject stone;
    GameObject spear;

    bool weaponInstantiated;

    float nextThrowAfterCooldown;
    int randomPlayerNumber;
    int randomHoleNumber;

    Vector2 target;
    bool hasTargetLocked = false;
    AudioSource audio;
    bool hasNoWeaponInHand = false;
    Collider2D stoneCollider;
    bool weaponDisabled = false;
    bool isStoneThrower = false;

    public Vector2 Target { get => target; set => target = value; }
    public int RandomHoleNumber { get => randomHoleNumber; set => randomHoleNumber = value; }
    public bool WeaponDisabled { get => weaponDisabled; set => weaponDisabled = value; }
    public int RandomPlayerNumber { get => randomPlayerNumber; set => randomPlayerNumber = value; }

    void Start()
    {
        if (instance == null)
            instance = this;

        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // For instantiation of Stone and Spear objects.
    public void PrepareAttack()
    {
        isStoneThrower = IsAttackerStoneThrower();
        if (isStoneThrower)
        {
            if (Time.time > nextThrowAfterCooldown)
            {
                if (!weaponInstantiated)
                {
                    stone = Instantiate(stonePrefab, transform.position, Quaternion.identity);
                    weaponInstantiated = true;
                }
                else
                {
                    EnableWeapon(stone);
                    stone.transform.position = transform.position;
                }
                nextThrowAfterCooldown = Time.time + AIController.instance.coolDownTimeInSeconds;

                RandomHoleNumber = Random.Range(0, HoleManager.Instance.holes.Count);
            }
        }
        else if (!isStoneThrower && this != null)
        {
            if (Time.time > nextThrowAfterCooldown)
            {
                if (!weaponInstantiated)
                {
                    spear = Instantiate(spearPrefab, transform.position, Quaternion.identity);

                    if (spear.transform.position.y > AIController.instance.raftTransform.transform.position.y)
                    {
                        spear.transform.Rotate(new Vector3(0, 0, 180));
                        spear.transform.localScale = new Vector3(-1, 1, 1);
                    }

                    weaponInstantiated = true;
                }
                else
                {
                    EnableWeapon(spear);
                    spear.transform.position = transform.position;
                }
                nextThrowAfterCooldown = Time.time + AIController.instance.coolDownTimeInSeconds;

                randomPlayerNumber = Random.Range(0, players.Count);
            }
        }
    }

    bool IsAttackerStoneThrower()
    {
        if (this != null)
        {
            if (gameObject.GetComponent<StoneThrower>() is StoneThrower ||
                    gameObject.GetComponent<HookThrower>() is HookThrower)
                return true;
        }
        return false;
    }

    void FixedUpdate()
    {
        GameObject weapon = GetTypeOfWeapon();
        LockTarget(weapon);
        if (weapon != null && (!PlayerInterface.instance.gameOver || !PlayerInterface.instance.win))
            ThrowWeapon(weapon, weapon.transform.position, Target,
                AIController.instance.hitAccuracy, AIController.instance.throwSpeed);
    }

    GameObject GetTypeOfWeapon()
    {
        if (weaponInstantiated && !PlayerTracker.IsColliding)
        {
            if (isStoneThrower)
                return stone;
            else 
                return spear;
        }
        return null;
    }

    void LockTarget(GameObject weapon)
    {
        if (weaponInstantiated && !PlayerTracker.IsColliding)
        {
            if (!hasTargetLocked)
            {
                if (weapon.tag.Equals("Stone"))
                    target = HoleManager.Instance.holes[RandomHoleNumber].transform.position;
                else
                    target = players[randomPlayerNumber].GetComponent<PlayerTracker>().GetPlayerPos();
            }
        }
    }

    void ThrowWeapon(GameObject weapon, Vector3 start, Vector3 target, float hitAccuracy, float throwSpeed)
    {
        if (!hasNoWeaponInHand && gameObject.activeSelf)
        {
            animator.SetTrigger("isAttacking");
            SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[5], audio);
        }

        IgnoreStoneCollidingWithRaftTemporarly(weapon);

        hasTargetLocked = true;
        Vector2 direction = (target - start).normalized;

        if (isStoneThrower)
            weapon.GetComponent<Rigidbody2D>().AddForce(direction * hitAccuracy * throwSpeed);
        else
            weapon.GetComponent<Rigidbody2D>().AddForce(direction * hitAccuracy * AIController.instance.spearThrowSpeed);
        hasNoWeaponInHand = true;
        CompareWeaponPositionsToRaft(weapon);
    }

    // Needed for the Stone to not trigger the holes at the buttom/top of the raft so it can hit the raft in the middle.
    void IgnoreStoneCollidingWithRaftTemporarly(GameObject weapon)
    {
        if (weapon.tag.Equals("Stone") && Vector2.Distance(weapon.transform.position, target) < 1f)
            weapon.layer = 10;
        else
            weapon.layer = 14;
    }

    public void DisableWeapon(GameObject weapon)
    {
        weapon.SetActive(false);
        if (weapon.tag.Equals("Stone"))
        {
            AIController.IsMakingAction = false;
            RaftHoleActivator.IsHit = false;
            RaftHoleActivator.HitCounter = 0;
        }
        weaponDisabled = true;
        PlayerTracker.IsColliding = false;
    }

    void CompareWeaponPositionsToRaft(GameObject weapon)
    {
        if (gameObject.transform.position.y < target.y && weapon.transform.position.y > target.y &&
            !PlayerTracker.IsColliding)
            DisableWeapon(weapon);

        else if (gameObject.transform.position.y > target.y && weapon.transform.position.y < target.y &&
            !PlayerTracker.IsColliding)
            DisableWeapon(weapon);

        else if (PlayerInterface.instance.gameOver || PlayerInterface.instance.win)
            DisableWeapon(weapon);
    }

    public void EnableWeapon(GameObject weapon)
    {
        weapon.SetActive(true);
        hasNoWeaponInHand = false;
        hasTargetLocked = false;
        weaponDisabled = false;
    }

    public void KeepAIDisabled()
    {
        gameObject.SetActive(false);
        weaponDisabled = true;
        PlayerTracker.IsColliding = false;
    }

    // EventTrigger
    public void DestroyStone()
    {
        stone.SetActive(false);
        RaftHoleActivator.IsHit = false;
        RaftHoleActivator.HitCounter = 0;
        Debug.Log("Destroyed");
    }
}
