using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverlapBox : MonoBehaviour
{
    [SerializeField] Collider2D playerNonTriggerCollider;
    Vector2 boxSize;
    Vector2 point;
    [SerializeField] float overlapScale = 3f;
    float angle = 0.0f;
    Collider2D overLappedCollider;
    Collider2D currentOverlapCollider;
    Collider2D previousOverlappedColliders;
    LayerMask layerMask = 1 << 15;


    public Collider2D OverLappedCollider { get => overLappedCollider; }
    public Collider2D PreviousOverlappedColliders { get => previousOverlappedColliders;
        set => previousOverlappedColliders = value; }
    public Collider2D CurrentOverlapCollider { get => currentOverlapCollider; set => currentOverlapCollider = value; }

    // Start is called before the first frame update
    void Start()
    {
        boxSize = playerNonTriggerCollider.bounds.size;
        point = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MakeOverlapBox();
    }

    void MakeOverlapBox()
    {
        point = transform.position;
        overLappedCollider = Physics2D.OverlapBox(point, new Vector2(boxSize.x * 2, (boxSize.y * 2) + 0.5f), angle, layerMask);
        //if (overLappedCollider != null && currentOverlapCollider == overLappedCollider)
        //{
        //    currentOverlapCollider = overLappedCollider;
        //    Debug.Log("Overlapped with: " + overLappedCollider.name + " CurrentOverlap: " + currentOverlapCollider);
        //}
         if (currentOverlapCollider != overLappedCollider)
        {
            previousOverlappedColliders = currentOverlapCollider;
            Debug.Log("PreviousOverlap: " + previousOverlappedColliders);
            currentOverlapCollider = overLappedCollider;
        }
    }
}
