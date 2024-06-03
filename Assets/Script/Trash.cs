using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ingridients" || col.tag == "Food" || col.tag == "Plate" || col.tag == "Chopped" || col.tag == "Cooked")
        {
            Destroy(col.gameObject);
            source.Play();
        }
    }
}
