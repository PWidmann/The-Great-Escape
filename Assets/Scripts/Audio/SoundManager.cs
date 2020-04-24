using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource backGroundMusicSource;
    public AudioSource soundFxSource;
    [SerializeField] AudioMixer audioMixer;

    public List<AudioClip> soundFx;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this);

        // Solution for Unity not having PlayerPrefs.GetBool(), booooo
        if (PlayerPrefs.HasKey("toggleRM"))
        {
            int savedToggle = PlayerPrefs.GetInt("toggleRM");

            switch (savedToggle)
            {
                case 0:
                    UIManagement.instance.randomMusicPlayToggle.isOn = false;
                    break;
                case 1:
                    UIManagement.instance.randomMusicPlayToggle.isOn = true;
                    break;
            }
        }
    }

    void Start()
    {
        // Get saved volume settings
        if (PlayerPrefs.HasKey("masterVolume"))
            UIManagement.instance.masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        if (PlayerPrefs.HasKey("musicVolume"))
            UIManagement.instance.musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        if (PlayerPrefs.HasKey("sfxVolume"))
            UIManagement.instance.soundFxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        UIManagement.instance.masterVolSliderTextCount.text = GetDecibelPercentage("MasterVolume").ToString() + "%";
        UIManagement.instance.musicVolSliderTextCount.text = GetDecibelPercentage("MusicVolume").ToString() + "%";
        UIManagement.instance.sfxVolSliderTextCount.text = GetDecibelPercentage("SFXVolume").ToString() + "%";
    }

    public void ChangeMasterVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", UIManagement.instance.masterVolumeSlider.value);

        // Get db-Level of Audiomixer
        audioMixer.SetFloat("MasterVolume", Mathf.Round(UIManagement.instance.masterVolumeSlider.value));

        UIManagement.instance.masterVolSliderTextCount.text = GetDecibelPercentage("MasterVolume").ToString() + "%";
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", UIManagement.instance.musicVolumeSlider.value);
        audioMixer.SetFloat("MusicVolume", Mathf.Round(UIManagement.instance.musicVolumeSlider.value));

        UIManagement.instance.musicVolSliderTextCount.text = GetDecibelPercentage("MusicVolume").ToString() + "%";
    }

    public void ChangeSoundFxVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", UIManagement.instance.musicVolumeSlider.value);
        audioMixer.SetFloat("SFXVolume", Mathf.Round(UIManagement.instance.soundFxSlider.value));

        UIManagement.instance.sfxVolSliderTextCount.text = GetDecibelPercentage("SFXVolume").ToString() + "%";
    }

    public void SaveRandomPlayToggle()
    {
        PlayerPrefs.SetInt("toggleRM", UIManagement.instance.randomMusicPlayToggle.isOn ? 1 : 0);
    }

    // Playing an example soundfx to make the user setup the sound effect's volume properly
    // Event function don't remove!
    public void PlayExampleSoundFx()
    {
        soundFxSource.clip = soundFx[0];
        soundFxSource.Play();
    }

    //Event function don't remove!
    public void PlayMenuPointerSoundFx()
    {
        soundFxSource.clip = soundFx[1];
        soundFxSource.Play();
    }

    // Event function don't remove!
    public void PlayMenuClickSoundFx()
    {
        soundFxSource.clip = soundFx[2];
        soundFxSource.Play();
    }

    // Methods when you need to play a sound effect. Sometimes there overlap so you can use a different audiosource s
    // aswell
    public void PlaySoundFx(AudioClip audioClip)
    {
        soundFxSource.clip = audioClip;
        soundFxSource.Play();
    }

    public void PlaySoundFx(AudioClip audioClip, AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    float GetDecibelPercentage(string key)
    {
        float minVolume = -80.0f; // The lowest possible volume in the audio mixer in decibel.
        float volumePercentage = 0;
        if (audioMixer.GetFloat(key, out float audioVolume))
            return volumePercentage = Mathf.Round((audioVolume - minVolume) / minVolume * 100 * -1);

        return volumePercentage = 0;
    }
}
