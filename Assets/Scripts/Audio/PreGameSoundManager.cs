using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameSoundManager : MonoBehaviour
{
    AudioSource audioSource;

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
