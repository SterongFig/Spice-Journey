using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HowToController : MonoBehaviour
{
    [SerializeField] GameObject imageObj;
    [SerializeField] GameObject textObj;
    [SerializeField] GameObject prevButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] AudioSource bgm;
    [SerializeField] List<ScriptingHow> howTo;
    [SerializeField] Sprite defaultNull;
    int indexing = 0;
    float levelOpen = 0;

    private void Start()
    {
        float timestamp = PlayerPrefs.GetFloat("main_bgm", 0.0f);
        indexing = PlayerPrefs.GetInt("HowToIndex", 0);
        imageObj.GetComponent<Image>().sprite = howTo[indexing].img;
        levelOpen = PlayerPrefs.GetInt("LevelOpen", 0);
        bgm.time = timestamp;
        bgm.Play();
        // this is to add the num to 1 - just one time
        if(levelOpen == 0)
        {
            PlayerPrefs.SetInt("LevelOpen", 1);
            levelOpen++;
        }
    }

    private void Update()
    {
        if (indexing == 0) { prevButton.gameObject.SetActive(false); } else { prevButton.gameObject.SetActive(true); }
        if (indexing == howTo.Count -1) { nextButton.gameObject.SetActive(false); } else { nextButton.gameObject.SetActive(true); }

    }

    public void Back()
    {
        PlayerPrefs.SetFloat("main_bgm", bgm.time);
        //bgm.Stop();
        SceneManager.LoadScene("LevelMenu");
    }

    public void NextPage()
    {
        indexing++;
        if(indexing == howTo.Count) { indexing--; }
        PlayerPrefs.SetInt("HowToIndex", indexing);
        CheckLeveling();
    }

    public void PreviousPage()
    {
        indexing--;
        if (indexing < 0) { indexing++; }
        PlayerPrefs.SetInt("HowToIndex", indexing);
        CheckLeveling();
    }

    private void CheckLeveling()
    {
        if (levelOpen >= howTo[indexing].openAt)
        {
            imageObj.GetComponent<Image>().sprite = howTo[indexing].img;
            textObj.GetComponent<TMP_Text>().enabled = false;
        }
        else 
        {
            imageObj.GetComponent<Image>().sprite = defaultNull;
            textObj.GetComponent<TMP_Text>().enabled = true;
            textObj.GetComponent<TMP_Text>().text = "open at level " + howTo[indexing].openAt.ToString();
        }
    }
}
