using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] //Menu that are serving UP
    List<ScriptingMenu> m_Menu; //ini hanya menu yang akan di serve

    [SerializeField]
    GameObject ServingOrder;
    DeleveryManagerUI delivery;

    [SerializeField] Text scoreText;
    [SerializeField] Text multiText;

    float score = 0;
    float multi = 1;
    float highscore = 0;

    //audio
    [SerializeField]
    // 0: ding, 1: waw, 2: time
    List<AudioClip> audioClipList;
    AudioSource source;
    AudioClip clip;

    void Start()
    {
        score = 0;
        multi = 1;
        delivery = ServingOrder.GetComponent<DeleveryManagerUI>();
        delivery.AddRecepie(GetRandomMenu());
        delivery.AddRecepie(GetRandomMenu());
        StopAllCoroutines();
        StartCoroutine(menuClocking());
    }

    void Update()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString();
        multiText.text = "combo " + multi.ToString();
        if (delivery.m_Menu.Count == 0)
        {
            delivery.AddRecepie(GetRandomMenu());
        }
    }

    public void CheckServe(List<Sprite> param1, List<string> param2)
    {
        bool checkSame = false;
        int i = 0;
        bool ordered = true;
        foreach (var item in delivery.m_Menu)
        {
            //test if there is Same
            if(AreSpriteListsEqual(item.targetSprite,param1))
            {
                checkSame = true;
                AddPoint(item.addScore, ordered);
                delivery.DestroyMenu(i);
                break;
            }
            ordered = false;
            i++;
        }
        if (!checkSame)
        {
            ResetMulti();
        }
    }

    public void AddPoint(float scoreAdd, bool addMulti)
    {
        if (!addMulti) { ResetMulti(); } //reset first if unordered
        score += (scoreAdd * multi);
        if (addMulti)
        {
            AddMulti();
        }

        if (highscore < score)
            PlayerPrefs.GetInt("highscore", 0);
    }

    public void MinusPoint(float scoreMinus)
    {
        score -= scoreMinus;
        ResetMulti();
    }

    public void AddMulti()
    {
        multi += 0.5f;
        if (multi > 2.5)
        {
            multi = 2.5f;
        }
    }

    public void ResetMulti()
    {
        multi = 1;
    }

    private bool AreSpriteListsEqual(List<Sprite> list1, List<Sprite> list2)
    {
        // Check if the lists are the same length
        if (list1.Count != list2.Count)
        {
            return false;
        }

        // Create dictionaries to count the occurrences of each sprite
        Dictionary<Sprite, int> spriteCount1 = CountSprites(list1);
        Dictionary<Sprite, int> spriteCount2 = CountSprites(list2);

        // Compare the counts of each sprite in both lists
        foreach (KeyValuePair<Sprite, int> kvp in spriteCount1)
        {
            if (!spriteCount2.ContainsKey(kvp.Key) || spriteCount2[kvp.Key] != kvp.Value)
            {
                return false;
            }
        }

        return true;
    }

    Dictionary<Sprite, int> CountSprites(List<Sprite> list)
    {
        Dictionary<Sprite, int> spriteCount = new Dictionary<Sprite, int>();

        foreach (Sprite sprite in list)
        {
            if (spriteCount.ContainsKey(sprite))
            {
                spriteCount[sprite]++;
            }
            else
            {
                spriteCount[sprite] = 1;
            }
        }

        return spriteCount;
    }

    private IEnumerator menuClocking()
    {
        while (true)
        {
            float sec = 15;
            yield return new WaitForSeconds(sec);
            if (!delivery.AddRecepie(GetRandomMenu())) // the add is fail - more frequent adding until true
            {
                sec = 5;
            }
            else
            {
                sec = 15;
            }
        }
    }

    private ScriptingMenu GetRandomMenu()
    {
        int randomIndex = Random.Range(0, m_Menu.Count);
        return m_Menu[randomIndex];
    }

    public Sprite? CheckMenuCombination(List<Sprite> list_ingridients)
    {
        if (list_ingridients.Count < 2)
        {
            return null;
        }
        foreach (var item in m_Menu)
        {
            //test if there is Same
            if (AreSpriteListsEqual(item.targetSprite, list_ingridients))
            {
                return item.artSprite;
            }
        }
        return null;
    }
}