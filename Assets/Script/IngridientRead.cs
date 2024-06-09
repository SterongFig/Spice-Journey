using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngridientRead : MonoBehaviour
{
    public ScriptIngridient scriptIngridient;
    public void Replace(ScriptIngridient newScript)
    {
        this.scriptIngridient = newScript;
}
}
