using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
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
