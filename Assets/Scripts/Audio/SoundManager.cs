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

    float minVolume = -80.0f; // The lowest possible volume in the audio mixer in decibel.

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

        // Get percentage of slider values
        float volumeMasterPercentage = 
            Mathf.Round((UIManagement.instance.masterVolumeSlider.value - minVolume) / minVolume * 100 * -1);
        float volumeMusicPercentage = 
            Mathf.Round((UIManagement.instance.musicVolumeSlider.value - minVolume) / minVolume * 100 * -1);
        float volumeSfxPercentage = 
            Mathf.Round((UIManagement.instance.soundFxSlider.value - minVolume) / minVolume * 100 * -1);

        // Show the percentage in game.
        UIManagement.instance.masterVolSliderTextCount.text = volumeMasterPercentage.ToString() + "%";
        UIManagement.instance.musicVolSliderTextCount.text = volumeMusicPercentage.ToString() + "%";
        UIManagement.instance.sfxVolSliderTextCount.text = volumeSfxPercentage.ToString() + "%";
    }

    public void ChangeMasterVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", UIManagement.instance.masterVolumeSlider.value); // Sets key for every vol change
        audioMixer.SetFloat("MasterVolume", Mathf.Round(UIManagement.instance.masterVolumeSlider.value)); // Get db-Level of Audiomixer

        // Audiomixer db-values in percent:
        float masterVol = 0;
        float volumePercentage = 0;
        bool result = audioMixer.GetFloat("MasterVolume", out masterVol);
        if (result)
            volumePercentage = Mathf.Round((masterVol - minVolume) / minVolume * 100 * -1);
        else
            volumePercentage = 0;

        UIManagement.instance.masterVolSliderTextCount.text = volumePercentage.ToString() + "%";
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", UIManagement.instance.musicVolumeSlider.value);
        audioMixer.SetFloat("MusicVolume", Mathf.Round(UIManagement.instance.musicVolumeSlider.value));
        // Audiomixer db-values in percent:
        float musicVol = 0;
        float volumePercentage = 0;
        bool result = audioMixer.GetFloat("MusicVolume", out musicVol);
        if (result)
            volumePercentage = Mathf.Round((musicVol - minVolume) / minVolume * 100 * -1);
        else
            volumePercentage = 0;

        UIManagement.instance.musicVolSliderTextCount.text = volumePercentage.ToString() + "%";
    }

    public void ChangeSoundFxVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", UIManagement.instance.musicVolumeSlider.value);
        audioMixer.SetFloat("SFXVolume", Mathf.Round(UIManagement.instance.soundFxSlider.value));
        // Audiomixer db-values in percent:
        float sfxVol = 0;
        float volumePercentage = 0;
        bool result = audioMixer.GetFloat("SFXVolume", out sfxVol);
        if (result)
            volumePercentage = Mathf.Round((sfxVol - minVolume) / minVolume * 100 * -1);
        else
            volumePercentage = 0;

        UIManagement.instance.sfxVolSliderTextCount.text = volumePercentage.ToString() + "%";
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
        soundFxSource.clip = audioClip;
        soundFxSource.Play();
    }
}
