using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unfortunately you can't load gameObjects from another scene and drag the button - click methods on the button click-events.
/// So for every scene and individual soundmanager is needed.
/// </summary>
public class PreGameSoundManager : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMenuPointerSoundFx()
    {
        SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[1];
        SoundManager.instance.soundFxSource.Play();
    }

    public void PlayMenuClickSoundFx()
    {
        SoundManager.instance.soundFxSource.clip = SoundManager.instance.soundFx[2];
        SoundManager.instance.soundFxSource.Play();
    }
}
