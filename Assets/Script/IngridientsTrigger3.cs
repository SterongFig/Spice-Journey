using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngridientsTrigger3 : MonoBehaviour
{
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
    }
}
