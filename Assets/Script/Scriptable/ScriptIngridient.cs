using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewIngridient", menuName ="Ingridient")]
public class ScriptIngridient : ScriptableObject
{
    public new string name; //raw_() ; () ; ()_potong
    public Sprite rawSprite;
    public Sprite ingSprite;
    public Sprite chopSprite;
    public Sprite cookSprite;
    public bool isChop;
    public bool isCook;
    public bool isBoiled;
    public bool isFried;
    public bool isSmoked;
    public Platetarget platetarget;
    public Cooktarget cooktarget;
    public ScriptIngridient secondaryTF;
}

public enum Platetarget { ingridient, chop, cooked, none }
public enum Cooktarget { none, ingridient, chop }