using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (gameObject.name)
            {
                case "Leaf(Clone)":
                    PlayerInterface.instance.leafCount += 1;
                    Debug.Log("Leaf picked up");
                    break;
                case "Stick(Clone)":
                    PlayerInterface.instance.stickCount += 1;
                    Debug.Log("Stick picked up");
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
