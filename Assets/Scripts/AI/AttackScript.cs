using System.Collections;
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
    float nextThrowAfterCooldown = 10f;
    float randomNumberInRaftBoundsX;
    float randomNumberInRaftBoundsY;
    int randomPlayerNumber;

    PlayerHandler playerHandler;
    Vector2 target;
    bool hasTargetLocked = false;
    AudioSource audio;
    bool hasNoWeaponInHand = false;
    Collider2D stoneCollider;

    private void Start()
    {
        players = new List<GameObject>();
        playerHandler = new PlayerHandler();

        if (instance == null)
            instance = this;

        audio = GetComponent<AudioSource>();
        stoneCollider = GetComponent<Collider2D>();
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

                // Offset of throw in x dirction
                randomNumberInRaftBoundsX = Random.Range(-RaftController.instance.GetRaftColliderBoundSize().x,
                    RaftController.instance.GetRaftColliderBoundSize().x);

                // Offset in y direction
                randomNumberInRaftBoundsY = Random.Range(-RaftController.instance.GetRaftColliderBoundSize().y,
                    RaftController.instance.GetRaftColliderBoundSize().y);

                weaponInstantiated = true;
            }
        }
        else if (gameObject.GetComponent<SpearThrower>() is SpearThrower)
        {
            if (Time.time > nextThrowAfterCooldown)
            {
                nextThrowAfterCooldown = Time.time + coolDownTimeInSeconds;
                spear = Instantiate(spearPrefab, transform.position, Quaternion.identity);
                weaponInstantiated = true;

                // Picks random Player as target.
                randomPlayerNumber = Random.Range(0, players.Count);
            }
        }
    }

    void ThrowWeapon(GameObject weapon, Vector3 start, Vector3 target, float hitAccuracy, float throwSpeed)
    {
        if (!hasNoWeaponInHand)
        {
            audio.clip = SoundManager.instance.soundFx[5];
            audio.Play();
        }

        weapon.GetComponent<Rigidbody2D>().AddForce((target - start) * hitAccuracy * throwSpeed);
        hasTargetLocked = true;
        hasNoWeaponInHand = true;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerTracker>().WeaponMoving = true;
        }
        Debug.Log("Weapon: " + weapon);
        if (weapon.transform.position.y > target.y && !PlayerTracker.IsColliding)
            DestroyWeapon(weapon);
        else if (weapon.Equals(stone) && weapon.transform.position.y < target.y)
        {
            Physics2D.IgnoreCollision(stoneCollider, RaftController.instance.GetRaftCollider(), true);
            Debug.Log("IgnoringCollisions!");
        }
        else if (weapon.Equals(stone) && weapon.transform.position.y >= target.y)
        {
            Physics2D.IgnoreCollision(stoneCollider, RaftController.instance.GetRaftCollider(), true);
            DestroyWeapon(weapon);
        }
    }

    void Update()
    {
        if (!aiController.isChecked)
            CheckForActivePlayers();
    }

    private void FixedUpdate()
    {
        GameObject weapon = null;
        if (weaponInstantiated && !PlayerTracker.IsColliding)
        {
            if (gameObject.GetComponent<StoneThrower>() is StoneThrower ||
                gameObject.GetComponent<HookThrower>() is HookThrower)
                weapon = stone;
            else if (gameObject.GetComponent<SpearThrower>() is SpearThrower)
                weapon = spear;

            if (!hasTargetLocked)
            {
                if (weapon.Equals(stone))
                {
                    target = new Vector2(RaftController.instance.GetRaftPos().x + randomNumberInRaftBoundsX,
                        RaftController.instance.GetRaftPos().y + randomNumberInRaftBoundsY);
                    //hasTargetLocked = true;
                }
                else
                {
                    target = players[randomPlayerNumber].GetComponent<PlayerTracker>().GetPlayerPos();
                    //hasTargetLocked = true;
                }
            }
        }
        if (weapon != null)
            ThrowWeapon(weapon, weapon.transform.position, target,
                aiController.hitAccuracy, aiController.throwSpeed);
    }

    void CheckForActivePlayers()
    {
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

    public void DestroyWeapon(GameObject weapon)
    {
        Destroy(weapon);
        PlayerTracker.IsColliding = false;
        weaponInstantiated = false;
        hasTargetLocked = false;
        hasNoWeaponInHand = false;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerTracker>().WeaponMoving = false;
        }
    }
}
