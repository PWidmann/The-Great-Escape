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
                    SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[0], PlayerInterface.instance.pickUpAudio);
                    if(PlayerInterface.instance.leafCount < 998)
                        PlayerInterface.instance.leafCount += 1;
                    break;
                case "Stick(Clone)":
                    SoundManager.instance.PlaySoundFx(SoundManager.instance.soundFx[0], PlayerInterface.instance.pickUpAudio);
                    if (PlayerInterface.instance.stickCount < 998)
                        PlayerInterface.instance.stickCount += 1;
                    break;
            }
            
            Destroy(gameObject);
            
            

        }
    }
}
