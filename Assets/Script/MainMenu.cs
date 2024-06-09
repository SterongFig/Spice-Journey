using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    [SerializeField] AudioSource bgm;

    private void Start()
    {
        float timestamp = PlayerPrefs.GetFloat("main_bgm", 0.0f);
        bgm.time = timestamp;
        bgm.Play();
    }

    public void StartGame()
    {
        PlayerPrefs.SetFloat("main_bgm", bgm.time);
        //bgm.Stop();
        SceneManager.LoadScene("LevelMenu");
    }

    public void Settings()
    {
        PlayerPrefs.SetFloat("main_bgm", bgm.time);
        //bgm.Stop();
        SceneManager.LoadScene("SettingMenu");
    }

    public void QuitGame()
    {
        PlayerPrefs.SetFloat("main_bgm", 0);
        //bgm.Stop();
        Application.Quit();
    }
}
