using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelMenu : MonoBehaviour
{
    public Sprite lockSprite;

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level0" + levelId;
        SceneManager.LoadScene(levelName);
    }
}
