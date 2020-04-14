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
    float nextThrowAfterCooldown;
    int randomPlayerNumber;
    int randomHoleNumber;

    PlayerHandler playerHandler;
    Vector2 target;
    bool hasTargetLocked = false;
    AudioSource audio;
    bool hasNoWeaponInHand = false;
    Collider2D stoneCollider;
    bool instantiatedStoneHasColliderComponent = false;
    bool weaponDisabled = false;

    public Vector2 Target { get => target; set => target = value; }
    public int RandomHoleNumber { get => randomHoleNumber; set => randomHoleNumber = value; }
    public bool WeaponDisabled { get => weaponDisabled; set => weaponDisabled = value; }

    private void Start()
    {
        players = new List<GameObject>();
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
                nextThrowAfterCooldown = Time.time + coolDownTimeInSeconds;

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
                nextThrowAfterCooldown = Time.time + coolDownTimeInSeconds;

                // Picks random Player as target.
                randomPlayerNumber = Random.Range(0, players.Count);
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
        Vector2 direction = target - start;
        direction.Normalize();
        weapon.GetComponent<Rigidbody2D>().AddForce(direction * hitAccuracy * throwSpeed);
        hasNoWeaponInHand = true;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerTracker>().WeaponMoving = true;
        }
        Debug.Log("Weapon: " + weapon);
        if (weapon.tag.Equals("Spear") && weapon.transform.position.y > target.y && !PlayerTracker.IsColliding)
            DisableWeapon(weapon);
        else if (weapon.tag.Equals("Stone") && weapon.transform.position.y > target.y)
            DisableWeapon(weapon);
    }

    void Update()
    {
        if (!aiController.isChecked)
            CheckForActivePlayers();

        // Prevent out of range exception by making sure that the indexes are always updated.
        if (HoleManager.Instance.CurrentHoleListCount != HoleManager.Instance.holes.Count)
        {
            randomHoleNumber = Random.Range(0, HoleManager.Instance.holes.Count);
            HoleManager.Instance.CurrentHoleListCount = HoleManager.Instance.holes.Count;
        }
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

    public void DisableWeapon(GameObject weapon)
    {
        weapon.SetActive(false);
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
}
