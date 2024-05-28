using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngridientsTiggerF : MonoBehaviour
{
    // this is for food only - food is already check and must go to plate only nothing else,
    // except rice is not using this script

    public List<Sprite> objInPlate;
    public List<string> nameInPlate;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Trash")
        {
            Destroy(gameObject);
        }
        if (col.tag == "Plate" || col.tag == "RawPlate")
        {
            if (transform.parent != null) // this still on player hand
            {
                return;
            }
            Destroy(gameObject);
        }
    }
}
