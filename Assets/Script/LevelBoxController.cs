using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBoxController : MonoBehaviour
{
    [SerializeField] LevelMenu canvasLevel;
    [SerializeField] ScriptingLevel LevelData;
    [SerializeField] List<GameObject> starsList;
    [SerializeField] GameObject text;
    [SerializeField] TMP_Text highscoreObj;

    void Start()
    {
        if (LevelData == null)
        {
            highscoreObj.text = "";
            text.SetActive(false);
            for (int i = 0; i < 3; i++)
            {
                starsList[i].SetActive(false);
            }
            GetComponent<Image>().sprite = canvasLevel.lockSprite;
            return;
        }

        //highscore
        float highscore = PlayerPrefs.GetFloat(LevelData.levelScene);
        Debug.Log(highscore);
        if(highscore == 0)
        {
            highscoreObj.text = "";
        }
        else
        {
            highscoreObj.text = "highscore : " + highscore ?? "".ToString();
        }

        //star component
        List<float> level_aim = LevelData.star_init;
        for (int i = 0; i < 3; i++)
        {
            if (highscore >= level_aim[i])
            {
                starsList[i].SetActive(true);
            }
            else
            {
                starsList[i].SetActive(false);
            }
        }
    }
}
