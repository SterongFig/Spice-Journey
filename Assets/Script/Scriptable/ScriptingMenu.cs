using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMenu", menuName = "Menu")]
public class ScriptingMenu : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite artSprite;
    public Sprite billMenuSprite;
    public List<string> combination;
    public List<Sprite> targetSprite;
    public Cook from;
    public float addScore;
    public float waitingTime;
}

public enum Cook { none, boil, fry, smoke }