using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngridientsTrigger : MonoBehaviour
{
    // for Boiling purposes
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Boiling" || col.tag == "Trash")
        {
            Destroy(gameObject);
        }
    }
}
