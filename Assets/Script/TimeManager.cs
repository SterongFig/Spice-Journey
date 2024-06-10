using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] ScriptingLevel dataLevel;
    
    [SerializeField] Text timerText;
    float remainingTime;
    [SerializeField] Text countdownText; // Reference to the UI Text element for countdown
    int countdownTime = 3; // Countdown time in seconds
    [SerializeField] GameObject holdObject; // Reference to the "Hold" GameObject

    //audio
    [SerializeField]
    List<AudioClip> audioClipList;
    [SerializeField] AudioSource source;
    float isPlay = 30;
    [SerializeField] AudioSource bgm;

    private void Start()
    {
        remainingTime = dataLevel.time;
        // Start the countdown at the beginning
        countdownText.text = "ready";
        holdObject.gameObject.SetActive(true);
        //activate
        StopAllCoroutines();
        source.clip = audioClipList[0];
        source.loop = false;
        source.Play();
        StartCoroutine(Countdown());
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Only update the main timer if countdown has finished
        if (countdownTime <= 0)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else if (remainingTime < 0)
            {
                remainingTime = 0;
                source.clip = audioClipList[2];
                source.loop = false;
                source.Play();
                StopAllCoroutines();
                StartCoroutine(FinishGame());
                // finish after 5sec hold
            }
            // first isplay == false
            if (remainingTime <= 30 && isPlay == 30)
            {
                source.clip = audioClipList[1];
                source.loop = false;
                source.Play();
                bgm.pitch = 1.1f;
                isPlay = 10;

            }
            if (remainingTime <= 10 && isPlay == 10)
            {
                //bgm
                bgm.pitch = 1.2f;
                //audio time
                isPlay = 0;
                source.clip = audioClipList[3];
                source.loop = true;
                source.Play();
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private IEnumerator Countdown()
    {
        // Pause the game
        Time.timeScale = 0;

        while (countdownTime > 0)
        {
            countdownText.text = (countdownTime == 3) ? "ready" : (countdownTime == 2) ? "set" : "go!";
            yield return new WaitForSecondsRealtime(1f); // Use WaitForSecondsRealtime to ignore time scale
            countdownTime--;
        }
        source.clip = audioClipList[1];
        source.loop = false;
        source.Play();

        // Hide the "Ready" GameObject after countdown
        holdObject.SetActive(false);

        // Resume the game
        Time.timeScale = 1;
    }

    private IEnumerator FinishGame()
    {
        Time.timeScale = 0;
        holdObject.SetActive(true);
        countdownText.text = "finish";
        saveScore();
        yield return new WaitForSecondsRealtime(3f);
        //Sementara pakai load level menu - namun harus ke UI score
        LoadScore();
    }

    private void saveScore()
    {
        float score = scoreManager.currentScore();
        string savelocal = dataLevel.levelScene;
        if (PlayerPrefs.GetFloat(savelocal) < score)
        {
            PlayerPrefs.SetFloat(savelocal,score);
            PlayerPrefs.SetInt("tmpBoolHighScore", 1);
        }
    }

    void LoadScore()
    {
        PlayerPrefs.SetInt("tmpDataLevel", dataLevel.indexes);
        SceneManager.LoadScene("ScoreUI");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelMenu");
    }
}
