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

    // Need for player List adding


    public static List<GameObject> players = new List<GameObject>();
    public static AttackScript instance; // need for method call

    GameObject stone;
    GameObject spear;

    bool weaponInstantiated;

    float nextThrowAfterCooldown;
    int randomPlayerNumber;
    int randomHoleNumber;

    PlayerHandler playerHandler;
    Vector2 target;
    bool hasTargetLocked = false;
    AudioSource audio;
    bool hasNoWeaponInHand = false;
    Collider2D stoneCollider;
    bool weaponDisabled = false;
    static int playerCounter = 0;

    public Vector2 Target { get => target; set => target = value; }
    public int RandomHoleNumber { get => randomHoleNumber; set => randomHoleNumber = value; }
    public bool WeaponDisabled { get => weaponDisabled; set => weaponDisabled = value; }
    public int RandomPlayerNumber { get => randomPlayerNumber; set => randomPlayerNumber = value; }

    private void Start()
    {
        playerHandler = new PlayerHandler();

        if (instance == null)
            instance = this;

        audio = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        if (gameObject.GetComponent<StoneThrower>() is StoneThrower
            || gameObject.GetComponent<HookThrower>() is HookThrower)
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
                nextThrowAfterCooldown = Time.time + aiController.coolDownTimeInSeconds;

                // Picks the a random Hole-GameObject as target.
                RandomHoleNumber = Random.Range(0, HoleManager.Instance.holes.Count);
            }
        }
        else if (gameObject.GetComponent<SpearThrower>() is SpearThrower)
        {
            if (Time.time > nextThrowAfterCooldown)
            {
                if (!weaponInstantiated)
                {
                    spear = Instantiate(spearPrefab, transform.position, Quaternion.identity);
                    weaponInstantiated = true;
                }
                else
                {
                    EnableWeapon(spear);
                    spear.transform.position = transform.position;
                }
                nextThrowAfterCooldown = Time.time + aiController.coolDownTimeInSeconds;

                // Picks random Player as target.
                randomPlayerNumber = Random.Range(0, players.Count);
                Debug.Log("RandNumber SPEAR: " + randomPlayerNumber);
            }
        }
    }

    void ThrowWeapon(GameObject weapon, Vector3 start, Vector3 target, float hitAccuracy, float throwSpeed)
    {
        if (!hasNoWeaponInHand && gameObject.activeSelf)
        {
            audio.clip = SoundManager.instance.soundFx[5];
            audio.Play();
        }
        hasTargetLocked = true;
        Vector2 direction = (target - start).normalized;
        if (weapon.tag.Equals("Stone") && Vector2.Distance(weapon.transform.position, target) < 1f)
            weapon.layer = 10;
        else
            weapon.layer = 14;
        direction.Normalize();
        weapon.GetComponent<Rigidbody2D>().AddForce(direction * hitAccuracy * throwSpeed);

        hasNoWeaponInHand = true;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerTracker>().WeaponMoving = true;
        }
        Debug.Log("Weapon: " + weapon);
        if (weapon.transform.position.y > target.y && !PlayerTracker.IsColliding)
            DisableWeapon(weapon);
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
                if (weapon.tag.Equals("Stone"))
                {
                    Target = HoleManager.Instance.holes[RandomHoleNumber].transform.position;
                    //hasTargetLocked = true;
                    if (gameObject.name.Equals("Steinwerfer"))
                        Debug.Log("RandNumbSame as Before? " + randomHoleNumber);
                }
                else
                {
                    Target = players[randomPlayerNumber].GetComponent<PlayerTracker>().GetPlayerPos();
                    //hasTargetLocked = true;
                }
            }
        }
        if (weapon != null)
            ThrowWeapon(weapon, weapon.transform.position, Target,
                aiController.hitAccuracy, aiController.throwSpeed);
    }

    public void DisableWeapon(GameObject weapon)
    {
        weapon.SetActive(false);
        if (weapon.tag.Equals("Stone"))
        {
            RaftHoleActivator.IsHit = false;
            RaftHoleActivator.HitCounter = 0;
        }
        weaponDisabled = true;
        PlayerTracker.IsColliding = false;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerTracker>().WeaponMoving = false;
        }
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
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerTracker>().WeaponMoving = false;
        }
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
