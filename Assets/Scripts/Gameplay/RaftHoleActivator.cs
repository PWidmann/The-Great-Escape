using UnityEngine;

public class RaftHoleActivator : MonoBehaviour
{
    SpriteRenderer holeSprite;
    static bool spriteEnabled = false;
    static bool isHit = false;
    static int hitCounter = 0;

    public static bool SpriteEnabled { get => spriteEnabled; }
    public static bool IsHit { get => isHit; set => isHit = value; }
    public static int HitCounter { get => hitCounter; set => hitCounter = value; }

    // Start is called before the first frame update
    void Start()
    {
        holeSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // test
        if (holeSprite.enabled)
            spriteEnabled = true;
        else
            spriteEnabled = false;

        if (!spriteEnabled)
            gameObject.layer = 9;
        else
            gameObject.layer = 15;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HoleManager.Instance.holes.Count != HoleManager.Instance.CurrentHoleListCount)
            return;
        else
        {
            /*GameObject targetHookThrowerGO = HoleManager.Instance.holes[
                HoleManager.Instance.attackScriptHookThrowerReference.RandomHoleNumber];
            GameObject targetStoneThrowerGO = HoleManager.Instance.holes[
                HoleManager.Instance.attackScriptStoneThrowerReference.RandomHoleNumber];

            if (!holeSprite.enabled && collision.gameObject.tag.Equals("Stone") &&
                (targetHookThrowerGO.name.Equals(name) || targetStoneThrowerGO.name.Equals(name)))
            {
                RaftController.instance.RaftAudio.clip = SoundManager.instance.soundFx[8];
                RaftController.instance.RaftAudio.Play();
                holeSprite.enabled = true;
                spriteEnabled = true;
                if (!HoleManager.Instance.attackScriptHookThrowerReference.WeaponDisabled &&
                        !HoleManager.Instance.attackScriptStoneThrowerReference.WeaponDisabled)
                    HoleManager.Instance.holes.Remove(gameObject);
                RaftController.instance.moveSpeed -= 0.5f;
            }*/
            if (collision.gameObject.tag.Equals("Stone"))
            {
                hitCounter++;
                RaftController.instance.RaftAudio.clip = SoundManager.instance.soundFx[8];
                RaftController.instance.RaftAudio.Play();
                holeSprite.enabled = true;
                spriteEnabled = true;
                isHit = true;
                if (!HoleManager.Instance.attackScriptHookThrowerReference.WeaponDisabled &&
                        !HoleManager.Instance.attackScriptStoneThrowerReference.WeaponDisabled)
                    HoleManager.Instance.holes.Remove(gameObject);
                RaftController.instance.moveSpeed -= 0.5f;
                if (RaftController.instance.moveSpeed < 0)
                    RaftController.instance.moveSpeed = 1;
            }

        }
    }

    public static void DisableSpriteRenderer(GameObject hole, bool setActive = false)
    {
        hole.GetComponent<SpriteRenderer>().enabled = setActive;
        spriteEnabled = false;
    }
}
