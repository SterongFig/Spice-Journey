using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainMixer;

    void Start()
    {
        LoadVolumeSettings();
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeMasterVolume()
    {
        mainMixer.SetFloat("MasterVol", masterVol.value);
        PlayerPrefs.SetFloat("MasterVol", masterVol.value);
    }

    public void ChangeMusicVolume()
    {
        mainMixer.SetFloat("MusicVol", musicVol.value);
        PlayerPrefs.SetFloat("MusicVol", musicVol.value);
    }

    public void ChangeSfxVolume()
    {
        mainMixer.SetFloat("SfxVol", sfxVol.value);
        PlayerPrefs.SetFloat("SfxVol", sfxVol.value);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            float masterVolume = PlayerPrefs.GetFloat("MasterVol");
            mainMixer.SetFloat("MasterVol", masterVolume);
            masterVol.value = masterVolume;
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVol");
            mainMixer.SetFloat("MusicVol", musicVolume);
            musicVol.value = musicVolume;
        }

        if (PlayerPrefs.HasKey("SfxVol"))
        {
            float sfxVolume = PlayerPrefs.GetFloat("SfxVol");
            mainMixer.SetFloat("SfxVol", sfxVolume);
            sfxVol.value = sfxVolume;
        }
    }
}
