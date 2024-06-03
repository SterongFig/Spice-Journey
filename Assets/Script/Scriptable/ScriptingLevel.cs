using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level")]
public class ScriptingLevel : ScriptableObject
{
    public string levelName;
    public string levelDescription;
    public string levelScene;
    public int indexes;
    public List<float> star_init;
    public float time;
    public List<ScriptingMenu> menuServe;
}
