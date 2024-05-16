using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class IngridientsTrigger2 : MonoBehaviour
{
    // for Frying purposes
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Frying" || col.tag == "Trash")
        {
            Destroy(gameObject);
        }
    }
}
