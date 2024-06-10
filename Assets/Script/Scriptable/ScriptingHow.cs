using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHowData", menuName = "HowImg")]
public class ScriptingHow : ScriptableObject
{
    public int indexes;
    public Sprite img;
    public string title;
    public string description;
    public float openAt;
}
