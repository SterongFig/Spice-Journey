using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelMenu : MonoBehaviour
{
    public Sprite lockSprite;
    [SerializeField] AudioSource bgm;
    [SerializeField] GameObject textHTP;

    private void Start()
    {
        float timestamp = PlayerPrefs.GetFloat("main_bgm", 0.0f);
        bgm.time = timestamp;
        bgm.Play();
        if (PlayerPrefs.GetInt("LevelOpen", 0) == 0)
        {
            textHTP.SetActive(true);
        }
        else
        {
            textHTP.SetActive(false);
        }
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

    public void OpenHow()
    {
        PlayerPrefs.SetFloat("main_bgm", bgm.time);
        SceneManager.LoadScene("HowTo");
    }
    public void OpenTutor()
    {
        PlayerPrefs.SetFloat("main_bgm", bgm.time);
        SceneManager.LoadScene("Level00");
    }
}
