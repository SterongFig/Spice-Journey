using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class IngridientsTrigger2 : MonoBehaviour
{
    // for Frying purposes
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
            Platetarget target = GetComponent<IngridientRead>().scriptIngridient.platetarget;
            if (tag == "Ingridients" && target == Platetarget.ingridient)
            {
                Destroy(gameObject);
            }
            if (tag == "Chopped" && target == Platetarget.chop)
            {
                Destroy(gameObject);
            }
            if (tag == "Cooked" && target == Platetarget.cooked)
            {
                Destroy(gameObject);
            }
        }
        if (col.tag == "Frying")
        {
            if ((GetComponent<IngridientRead>().scriptIngridient.cooktarget == Cooktarget.ingridient && tag == "Ingridients") ||
                (GetComponent<IngridientRead>().scriptIngridient.cooktarget == Cooktarget.chop && tag == "Chopped"))
            {
                Destroy(gameObject);
            }
        }
    }
}
