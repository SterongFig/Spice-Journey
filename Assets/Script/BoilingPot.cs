using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BoilingPot : MonoBehaviour
{
    //the Gameobject children of Boiling pot
    [SerializeField]
    GameObject bar;
    [SerializeField]
    GameObject fill;
    [SerializeField]
    GameObject fire;
    [SerializeField]
    GameObject pointReject;

    [SerializeField]
    List<GameObject> bahanMasuk;

    float fillbar = 0;
    const float waktuMasak = 10;

    //forbidden to insert
    List<string> forbid = new() { "lontong", "lontong_potong", "sapi", "ayam", "bumbu_spesial", "bumbu_lontong", "mie", "nasi" };
    // yang boleh:
    // sapi_potong, ayam potong

    void Update()
    {
        //update fillbar
        if (fillbar > 0)
        {
            fill.GetComponent<RectTransform>().localScale = new Vector3((fillbar/waktuMasak) * 3.45f, fill.GetComponent<RectTransform>().localScale.y, fill.GetComponent<RectTransform>().localScale.z);
        }
    }

    //coroutine object
    private IEnumerator coObject = null;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ingridients" || col.tag == "Chopped")
        {
            // forbid any doesn't belong here
            if (forbid.Contains(col.name))
            {
                // flung/tolak(taruh di-samping) the ingridients
                var tmp = pointReject.transform.position;
                col.gameObject.transform.position = new Vector3(tmp.x, tmp.y, tmp.z);
                return;
            }
            //save the instance to List
            bahanMasuk.Add(instanceObj(col.gameObject));
            if (coObject == null)
            {
                coObject = cookTimer();
                StartCoroutine(coObject);
            }
            else
            {
                fillbar = fillbar / 2;
            }
        }
    }

    private GameObject instanceObj(GameObject objToCreate)
    {
        // Instantiate a copy of the raw object
        GameObject clonedObject = Instantiate(objToCreate, transform.position, Quaternion.identity);
        clonedObject.tag = "Ingridients";
        clonedObject.transform.parent = transform;
        //disable all move and hide the assets
        clonedObject.GetComponent<Rigidbody2D>().isKinematic = true;
        clonedObject.GetComponent<BoxCollider2D>().enabled = false;
        clonedObject.GetComponent<SpriteRenderer>().enabled = false;
        //change the name
        clonedObject.name = clonedObject.name.Replace("(Clone)", "");
        return clonedObject;
    }

    private IEnumerator cookTimer()
    {
        // Tampilkan efek memasak
        bar.GetComponent<SpriteRenderer>().enabled = true;
        fill.GetComponent<SpriteRenderer>().enabled = true;
        // fill start from zero scale - y dan z tetap
        fill.GetComponent<RectTransform>().localScale = new Vector3(0, fill.GetComponent<RectTransform>().localScale.y, fill.GetComponent<RectTransform>().localScale.z);
        fire.GetComponent<SpriteRenderer>().enabled = true;

        // Tunggu selama waktu memasak
        // pakai true dan break/berhenti jika barfill sudah penuh
        while (true)
        {
            fillbar = fillbar + 0.1f;
            yield return new WaitForSeconds(0.1f);
            if (fillbar >= waktuMasak)
            {
                break;
            }
        }

        // Matikan efek memasak / Reset Visual
        bar.GetComponent<SpriteRenderer>().enabled = false;
        fill.GetComponent<SpriteRenderer>().enabled = false;
        fire.GetComponent<SpriteRenderer>().enabled = false;
        fillbar = 0;
        coObject = null;

        // logic keluaran bahan makanan yang jadi

        // Kosongkan daftar bahan makanan (opsi ini bisa diganti dengan mengeluarkan bahan)
        bahanMasuk.Clear();
    }

}
