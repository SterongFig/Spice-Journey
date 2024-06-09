using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelMenu : MonoBehaviour
{
    public Sprite lockSprite;
    [SerializeField] AudioSource bgm;

    private void Start()
    {
        float timestamp = PlayerPrefs.GetFloat("main_bgm", 0.0f);
        bgm.time = timestamp;
        bgm.Play();
    }

    public void Back()
    {
        PlayerPrefs.SetFloat("main_bgm", bgm.time);
        //bgm.Stop();
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level0" + levelId;
        SceneManager.LoadScene(levelName);
    }
}
