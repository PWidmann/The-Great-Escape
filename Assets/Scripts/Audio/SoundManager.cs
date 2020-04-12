using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


/// <summary>
/// Handles the music and soundfx in the game. 
/// Author: Dennis Bannasch 
/// Date: 10.4.2020
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource backGroundMusicSource;
    public AudioSource soundFxSource;
    [SerializeField] AudioMixer audioMixer;

    public List<AudioClip> backGroundMusic;
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
    }

    void Start()
    {
        // Play background music
        backGroundMusicSource.clip = backGroundMusic[0];
        backGroundMusicSource.Play();

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

    /// <summary>
    /// On value change on slider. Changes volume of all sounds.
    /// </summary>
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

    /// <summary>
    /// On value change on slider. Changes volume of background music.
    /// </summary>
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

    /// <summary>
    /// On slider value change. Changes volume of soundfx.
    /// </summary>
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

    /// <summary>
    /// On slider value change. When volume of soundfx gets changed it plays a sound effect so the player can adjust the
    /// volume properly.
    /// </summary>
    public void PlayExampleSoundFx()
    {
        soundFxSource.clip = soundFx[0];
        soundFxSource.Play();
    }

    public void PlayMenuPointerSoundFx()
    {
        soundFxSource.clip = soundFx[1];
        soundFxSource.Play();
    }

    public void PlayMenuClickSoundFx()
    {
        soundFxSource.clip = soundFx[2];
        soundFxSource.Play();
    }
}
