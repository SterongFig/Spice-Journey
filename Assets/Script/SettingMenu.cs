using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;



public class SettingsMenu : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public AudioMixer audioMixer;
	public void SetVolume (float volume)
	{
		audioMixer.SetFloat("volume", volume);
	}
}
