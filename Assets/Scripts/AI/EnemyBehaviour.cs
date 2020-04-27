using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public bool IsInRange()
    {
        if (Vector2.Distance(gameObject.transform.position,
            AIController.instance.raftTransform.transform.position) > 35f)
            return false;
        else
            return true;
    }
}
