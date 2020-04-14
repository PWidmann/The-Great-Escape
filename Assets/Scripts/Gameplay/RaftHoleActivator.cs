using UnityEngine;

public class RaftHoleActivator : MonoBehaviour
{

    SpriteRenderer holeSprite;

    // Start is called before the first frame update
    void Start()
    {
        holeSprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HoleManager.Instance.holes.Count != HoleManager.Instance.CurrentHoleListCount)
            return;
        else
        {
            GameObject targetHookThrowerGO = HoleManager.Instance.holes[
                HoleManager.Instance.attackScriptHookThrowerReference.RandomHoleNumber];
            GameObject targetStoneThrowerGO = HoleManager.Instance.holes[
                HoleManager.Instance.attackScriptStoneThrowerReference.RandomHoleNumber];

            if (!holeSprite.enabled && collision.gameObject.tag.Equals("Stone") &&
                (targetHookThrowerGO.name.Equals(name) || targetStoneThrowerGO.name.Equals(name)))
            {
                RaftController.instance.RaftAudio.clip = SoundManager.instance.soundFx[8];
                RaftController.instance.RaftAudio.Play();
                holeSprite.enabled = true;
                if (!HoleManager.Instance.attackScriptHookThrowerReference.WeaponDisabled &&
                        !HoleManager.Instance.attackScriptStoneThrowerReference.WeaponDisabled)
                    HoleManager.Instance.holes.Remove(gameObject);
                RaftController.instance.moveSpeed -= 0.5f;
            }
        }
    }
}
