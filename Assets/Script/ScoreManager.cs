using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; 

    [SerializeField] Text scoreText;
    [SerializeField] Text multiText;

    float score = 0;
    const string init_multi = "combo 1x";
    float multi = 1;

    void Start()
    {
        score = 0;
        multi = 1;
    }

    void Update()
    {
        scoreText.text = score.ToString();
        multiText.text = "combo " + multi.ToString();
    }

    public void AddPoint(float scoreAdd, bool addMulti)
    {
        score += scoreAdd;
        if (addMulti)
        {
            AddMulti();
        }
    }

    public void MinusPoint(float scoreMinus)
    {
        score -= scoreMinus;
        ResetMulti();
    }

    public void AddMulti()
    {
        multi++;
        if (multi > 3)
        {
            multi = 3;
        }
    }

    public void ResetMulti()
    {
        multi = 1;
    }
}