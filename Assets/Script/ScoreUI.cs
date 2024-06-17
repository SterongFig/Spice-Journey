using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text sceneNameText;
    [SerializeField] TMP_Text plusScoreText;
    [SerializeField] TMP_Text minusScoreText;
    [SerializeField] TMP_Text ordersDeliveredText;
    [SerializeField] TMP_Text totalScoreText;
    [SerializeField] GameObject highScoreObj;
    [SerializeField] GameObject[] starObjects; // Array of star GameObjects
    [SerializeField] TMP_Text[] starNum; // Array of star Text

    //audio
    [SerializeField] AudioSource star_bing;
    [SerializeField] AudioSource audiance;
    [SerializeField] AudioSource drum;
    [SerializeField] AudioSource waw;

    private string sceneName;
    private float plusScore;
    private float minusScore;
    private float ordersDelivered;
    private float totalScore;
    private bool highScore;
    private int index;
    private int opened;
    private List<float> star_aim_score;

    [SerializeField] List<ScriptingLevel> levelList;

    void Start()
    {
        // get data level
        index = PlayerPrefs.GetInt("tmpDataLevel");
        opened = PlayerPrefs.GetInt("LevelOpen"); // level already opened
        if(opened == index)
        { 
            PlayerPrefs.SetInt("LevelOpen", index + 1); 
        }
        sceneName = levelList[index].levelName;
        // data using float
        plusScore = PlayerPrefs.GetFloat("tmpPlusPoint");
        minusScore = PlayerPrefs.GetFloat("tmpMinusPoint");
        ordersDelivered = PlayerPrefs.GetFloat("tmpDelivered");
        // plus (+) + minusscore (-) positif ketemu negatif
        totalScore = plusScore + minusScore;
        //highscore bool from PlayerPrefsInt
        highScore = PlayerPrefs.GetInt("tmpBoolHighScore") == 1 ? true : false;
        star_aim_score = levelList[index].star_init;
        for (int i = 0; i < starNum.Length; i++)
        {
            starNum[i].text = levelList[index].star_init[i].ToString();
        }
        drum.Play();
        UpdateUI();
    }

    private void UpdateUI()
    {
        sceneNameText.text = sceneName;
        plusScoreText.text = $"Score\t\t{plusScore}";
        minusScoreText.text = $"Orders Late\t{minusScore}";
        ordersDeliveredText.text = $"Delivered\t\t{ordersDelivered}";
        totalScoreText.text = $"Total\t\t{totalScore}";
        highScoreObj.SetActive(highScore);

        // Determine how many stars to display based on total score
        StopAllCoroutines();
        StartCoroutine(CalculateDisplayedStars(totalScore));
    }

    private IEnumerator CalculateDisplayedStars(float score)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        bool all_in = true;
        for (int i = 0; i < starObjects.Length; i++)
        {
            if (score < star_aim_score[i])
            {
                starObjects[i].GetComponent<Image>().color = new Color(0.09f,0.09f,0.09f);
                all_in = false;
                yield return new WaitForSeconds(0.5f);
            }
            else //achive able
            {
                starObjects[i].GetComponent<Image>().color = new Color(1f,1f,1f);
                star_bing.Play();
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
        if (all_in)
        {
            audiance.Play();
        }
        if (score < star_aim_score[0])
        {
            waw.Play();
        }
    }

    // Button method to restart the game
    public void RestartGame()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(levelList[index].levelName);
    }

    // Button method to go to the LevelMenu scene
    public void GoToLevelMenu()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("LevelMenu");
    }
}
