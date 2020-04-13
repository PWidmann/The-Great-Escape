using System.Collections;
using System.Collections.Generic;
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
        if (!holeSprite.enabled && RaftController.instance.IsHitByStone)
        {
            holeSprite.enabled = true;
        }
    }
}
