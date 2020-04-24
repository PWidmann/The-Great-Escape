using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOptionsManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
            UIManagement.instance.masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        if (PlayerPrefs.HasKey("musicVolume"))
            UIManagement.instance.musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        if (PlayerPrefs.HasKey("sfxVolume"))
            UIManagement.instance.soundFxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        UIManagement.instance.masterVolSliderTextCount.text =
            SoundManager.instance.GetDecibelPercentage("MasterVolume").ToString() + "%";

        UIManagement.instance.musicVolSliderTextCount.text =
            SoundManager.instance.GetDecibelPercentage("MusicVolume").ToString() + "%";

        UIManagement.instance.sfxVolSliderTextCount.text =
            SoundManager.instance.GetDecibelPercentage("SFXVolume").ToString() + "%";

    }

    public void ChangeMasterVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", UIManagement.instance.masterVolumeSlider.value);

        // Get db-Level of Audiomixer
        SoundManager.instance.AudioMixer.SetFloat(
            "MasterVolume", Mathf.Round(UIManagement.instance.masterVolumeSlider.value));

        UIManagement.instance.masterVolSliderTextCount.text =
            SoundManager.instance.GetDecibelPercentage("MasterVolume").ToString() + "%";
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", UIManagement.instance.musicVolumeSlider.value);
        SoundManager.instance.AudioMixer.SetFloat(
            "MusicVolume", Mathf.Round(UIManagement.instance.musicVolumeSlider.value));

        UIManagement.instance.musicVolSliderTextCount.text = 
            SoundManager.instance.GetDecibelPercentage("MusicVolume").ToString() + "%";
    }

    public void ChangeSoundFxVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", UIManagement.instance.musicVolumeSlider.value);
        SoundManager.instance.AudioMixer.SetFloat("SFXVolume", Mathf.Round(UIManagement.instance.soundFxSlider.value));

        UIManagement.instance.sfxVolSliderTextCount.text = 
            SoundManager.instance.GetDecibelPercentage("SFXVolume").ToString() + "%";
    }


    public void SaveRandomPlayToggle()
    {
        PlayerPrefs.SetInt("toggleRM", UIManagement.instance.randomMusicPlayToggle.isOn ? 1 : 0);
    }
}
