using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopBoard : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField]
    GameObject tmpSomethingGrab;
    int? tmpSomethingGrab2 = null;
    [SerializeField]
    List<Sprite> ingSprites;
    [SerializeField]
    List<Sprite> chopSprites;

    //the Gameobject children of ChopBoard
    [SerializeField]
    GameObject bar;
    [SerializeField]
    GameObject fill;
    [SerializeField]
    GameObject knife;


    //bahan di board hanya 1
    [SerializeField]
    bool bahanMasuk = false;

    //forbidden to insert
    List<string> forbid = new() { "bumbu", "lontong_potong", "sapi_potong", "ayam_potong", "mie", "nasi", "beras" };

    //Memorize all the Ingridients Combination
    List<String> boil_ingridients = new() { "beras", "sapi_potong", "ayam_potong" };
    List<String> fry_ingridients = new() { "nasi", "mie", "cabai_potong", "bumbu_aceh" };

    float fillbar = 0;
    const float waktuPotong = 3.5f;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //update fillbar
        if (fillbar > 0)
        {
            fill.GetComponent<RectTransform>().localScale = new Vector3((fillbar / waktuPotong) * 2.19f, 0.13f, 1);
        }
    }

    //coroutine object
    private IEnumerator coObject = null;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ingridients" && !forbid.Contains(col.name))
        {
            bahanMasuk = true;
            tmpSomethingGrab = col.gameObject;
            //detect sprite and find index combination
            var detect = col.gameObject.GetComponent<SpriteRenderer>().sprite;
            int indexes = ingSprites.IndexOf(detect);
            tmpSomethingGrab = col.gameObject;
            tmpSomethingGrab2 = indexes;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Ingridients")
        {
            bahanMasuk = false;
            tmpSomethingGrab = null;
            tmpSomethingGrab2 = null;
        }
    }

    // use by user to tell the board is the ing. ok to chop? - return y/n
    public bool checkChop()
    {
        if (!bahanMasuk)
        {
            return false;
        }
        // yes, then do animaton
        coObject = chopTrigger();
        StartCoroutine(coObject);
        return true;
    }


    private IEnumerator chopTrigger()
    {
        // Tampilkan efek potong
        bar.GetComponent<SpriteRenderer>().enabled = true;
        fill.GetComponent<SpriteRenderer>().enabled = true;
        // fill start from zero scale
        fill.GetComponent<RectTransform>().localScale = new Vector3(0, 0.13f, 1);
        knife.GetComponent<SpriteRenderer>().enabled = false;

        // Tunggu selama waktu potong
        // pakai true dan break/berhenti jika barfill sudah penuh
        while (true)
        {
            fillbar = fillbar + 0.1f;
            yield return new WaitForSeconds(0.1f);
            if (fillbar >= waktuPotong)
            {
                break;
            }
        }

        // Matikan efek memasak / Reset Visual
        bar.GetComponent<SpriteRenderer>().enabled = false;
        fill.GetComponent<SpriteRenderer>().enabled = false;
        knife.GetComponent<SpriteRenderer>().enabled = true;
        fillbar = 0;
        coObject = null;

        // bahan masuk false
        bahanMasuk = false;
        //GrabChoppedNull(tmpSomethingGrab, true);
        playerController.ChoppingDone();
    }

    //private void GrabChoppedNull(GameObject objToGrab, bool ingridients)
    //{
    //    // when condition is chop, change the sprite of object - but not to hand
    //    if (!objToGrab.name.Contains("_potong"))
    //    {
    //        objToGrab.tag = "Chopped";
    //        objToGrab.name += "_potong";
    //        objToGrab.GetComponent<SpriteRenderer>().sprite = chopSprites[tmpSomethingGrab2.Value];
    //        //insert the IngridientsTrigger Script - boiling purposes
    //        if (boil_ingridients.Contains(objToGrab.name))
    //        {
    //            objToGrab.AddComponent<IngridientsTrigger>();
    //        }
    //        //insert the IngridientsTrigger2 Script - frying purposes
    //        else if (fry_ingridients.Contains(objToGrab.name))
    //        {
    //            objToGrab.AddComponent<IngridientsTrigger2>();
    //        }
    //        //else don't add trigger to cook - example lontong is not cookable
    //        objToGrab.transform.parent = transform;
    //        objToGrab.transform.localPosition = new Vector2(0, 2);
    //    }
    //}
}
