using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void Settings()
    {
        SceneManager.LoadScene("SettingMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
