﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackScript : MonoBehaviour
{

    /* Attack algorithm:
    * It is based of events. The GameObjects of the Hookthrower, Spearthrower and StoneThrower
    * are subscribers to the the events Attacktrigger and Runtrigger.
    * Now when the event Attacktrigger fires, the Attack method gets called by all subscribers.
    * The attack method has to tell the subscribers what they have to do.
    * Since all GameObjects are subscribed, I can check for each instances. -> Done
    * When the instance is a StoneThrower OR a HookThrower subscriber, they should throw a stone
    * Otherwise if the instance is a SpearThrower subscriber, he should throw a spear.
    */

    [SerializeField] GameObject spearPrefab;
    [SerializeField] GameObject stonePrefab;
    [SerializeField] AIController aiController;

    public static List<GameObject> players;
    public static AttackScript instance; // need for method call

    GameObject stone;
    GameObject spear;

    bool weaponInstantiated;

    float coolDownTimeInSeconds = 5f;
    float nextThrowAfterCooldown;
    int randomNumber;

    PlayerHandler playerHandler;

    private void Start()
    {
        players = new List<GameObject>();
        playerHandler = new PlayerHandler();
        

    }

    public void Attack()
    {
        if (gameObject.GetComponent<StoneThrower>() is StoneThrower
            || gameObject.GetComponent<HookThrower>() is HookThrower)
        {
            if (Time.time > nextThrowAfterCooldown)
            {
                nextThrowAfterCooldown = Time.time + coolDownTimeInSeconds;
                stone = Instantiate(stonePrefab, transform.position, Quaternion.identity);
                Debug.Log("We threw a stone!");
                Debug.Log("Length " + players.Count);
                Debug.Log("Stone " + stone);
                /*
                 * Stone should be thrown at:
                 * Player1 from both Stone- and Hookthrower or
                 * Player2 "    "    "      "   "           or
                 * Player1 from Hookthrower or Stonethrower or
                 * Player2 "    "           "  "    
                 * => Make random number. -> Done
                 */
                randomNumber = Random.Range(0, 2);
                Debug.Log("Random number: " + randomNumber);
                weaponInstantiated = true;
                StartCoroutine(WeaponObjectsToDestroyTimer(stone, 3f));
            }
        }
        else if (gameObject.GetComponent<SpearThrower>() is SpearThrower)
        {
            if (Time.time > nextThrowAfterCooldown)
            {
                nextThrowAfterCooldown = Time.time + coolDownTimeInSeconds;
                spear = Instantiate(spearPrefab, transform.position, Quaternion.identity);
                Debug.Log("I threw a spear!");
                weaponInstantiated = true;
                randomNumber = Random.Range(0, 2);
                StartCoroutine(WeaponObjectsToDestroyTimer(spear, 3f));
            }
        }
    }

    IEnumerator WeaponObjectsToDestroyTimer(GameObject weapons, float waitInSeconds)
    {
        yield return new WaitForSeconds(waitInSeconds);
        Destroy(weapons);
        PlayerTracker.IsColliding = false;
        weaponInstantiated = false;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerTracker>().WeaponMoving = false;
        }
    }

    void ThrowWeapon(Transform weapon, Vector3 start, Vector3 target, float hitAccuracy, float throwSpeed)
    {
        weapon.position = Vector2.MoveTowards(start, target * hitAccuracy, 
            Time.deltaTime * throwSpeed);
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerTracker>().WeaponMoving = true;
        }
        Debug.Log("Weapon: " + weapon);
    }

    void Update()
    {
        if (!aiController.isChecked)
            CheckForActivePlayers();

        Debug.Log("Player List Count: " + players.Count);
        Debug.Log("which player active" + playerHandler.player3active);
        if (HookThrower.BoatHooked && weaponInstantiated && RaftController.IscollidingWithWall)
        {
            GameObject weapon = null;
            if (gameObject.GetComponent<StoneThrower>() is StoneThrower || 
                gameObject.GetComponent<HookThrower>() is HookThrower)
                weapon = stone;
            else if (gameObject.GetComponent<SpearThrower>() is SpearThrower)
                weapon = spear;

            ThrowWeapon(weapon.transform, weapon.transform.position,
                players[randomNumber].GetComponent<PlayerTracker>().OldPosition,
                aiController.hitAccuracy, aiController.throwSpeed);
            Debug.Log("Throw path: " +
                players[Random.Range(0, players.Count)].GetComponent<PlayerTracker>().OldPosition);
        }
    }

    void CheckForActivePlayers()
    {
        // Not adding gameObject to list for some reason. This is the whole problem.
        if (SceneManager.GetActiveScene().name.Equals("The Great Escape"))
        {
            if (PlayerHandler.instance.player1active) 
                players.Add(GameObject.FindGameObjectWithTag("Player"));
            else if (PlayerHandler.instance.player2active)
                players.Add(GameObject.FindGameObjectWithTag("Player"));
            else if (PlayerHandler.instance.player3active)
                players.Add(GameObject.FindGameObjectWithTag("Player"));
            else if (PlayerHandler.instance.player4active)
                players.Add(GameObject.FindGameObjectWithTag("Player"));
            aiController.isChecked = true;
            Debug.Log("Players count: " + players.Count);
        }
    }

    public static GameObject GetActivePlayers()
    {
        foreach (GameObject player in players)
        {
            return player;
        }
        return null;
    }
}
