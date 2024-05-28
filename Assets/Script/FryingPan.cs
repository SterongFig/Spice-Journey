using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    //the Gameobject children of Frying Pan
    [SerializeField]
    GameObject bar;
    [SerializeField]
    GameObject fill;
    [SerializeField]
    GameObject fire;
    [SerializeField]
    GameObject pointReject;

    [SerializeField] ScoreManager scoreManager;

    [SerializeField]
    List<GameObject> bahanMasuk;

    float fillbar = 0;
    const float waktuMasak = 15;

    AudioSource source;
    AudioClip clip;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        //update fillbar
        if (fillbar > 0)
        {
            fill.GetComponent<RectTransform>().localScale = new Vector3((fillbar / waktuMasak) * 12, fill.GetComponent<RectTransform>().localScale.y, fill.GetComponent<RectTransform>().localScale.z);
        }
    }

    //coroutine object
    private IEnumerator coObject = null;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            return;
        }
        void Reject()
        {
            // flung/tolak(taruh di-samping) the ingridients
            var tmp = pointReject.transform.position;
            col.gameObject.transform.position = new Vector3(tmp.x, tmp.y, tmp.z);
        }
        void Accept()
        {
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
        // forbid any doesn't belong here
        if (!col.GetComponent<IngridientRead>().scriptIngridient.isBoiled)
        {
            Reject();
            return;
        }
        //if the object belong to cook here
        if (col.tag == "Ingridients" && col.GetComponent<IngridientRead>().scriptIngridient.cooktarget == Cooktarget.ingridient)
        {
            Accept();
            return;
        }
        if (col.tag == "Chopped" && col.GetComponent<IngridientRead>().scriptIngridient.cooktarget == Cooktarget.chop)
        {
            Accept();
            return;
        }
        Reject();
        return;
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
        // fill start from zero scale
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
        HasilKeluaran();
    }

    private void HasilKeluaran()
    {
        if (bahanMasuk.Count < 2)
        {
            GameObject clonedObject = Instantiate(bahanMasuk[0], transform.position, Quaternion.identity);
            clonedObject.tag = "Cooked";
            var sprite = clonedObject.GetComponent<SpriteRenderer>();
            sprite.sprite = clonedObject.GetComponent<IngridientRead>().scriptIngridient.cookSprite;
            sprite.enabled = true;
            clonedObject.GetComponent<BoxCollider2D>().enabled = true;
            clonedObject.GetComponent<CircleCollider2D>().enabled = true;
            clonedObject.transform.parent = null;
            clonedObject.name = clonedObject.name.Replace("(Clone)", "_masak");
            clonedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            clonedObject.GetComponent<Transform>().localScale = new Vector3(0.11f, 0.11f, 0.11f);
            clonedObject.transform.position = pointReject.transform.position;
        }
        else
        {
            List<Sprite> tmpSprite = new() { };
            List<string> tmpName = new() { };
            foreach (var item in bahanMasuk)
            {
                tmpSprite.Add(item.GetComponent<IngridientRead>().scriptIngridient.cookSprite);
                tmpName.Add(item.name);
            }
            Sprite sprite = scoreManager.CheckMenuCombination(tmpSprite);
            GameObject newGameObject = new GameObject("kombinasi_masak");
            newGameObject.tag = "Food";
            var spriteCloned = newGameObject.AddComponent<SpriteRenderer>();
            spriteCloned.sprite = sprite;
            spriteCloned.transform.localScale = Vector3.one * 0.21f;
            var boxCloned = newGameObject.AddComponent<BoxCollider2D>();
            boxCloned.size = new Vector2(7.5f, 1.2f);
            var circleCloned = newGameObject.AddComponent<CircleCollider2D>();
            circleCloned.radius = 0.75f;
            circleCloned.isTrigger = true;
            circleCloned.offset = new Vector2(0, -0.6f);
            var rigidCloned = newGameObject.AddComponent<Rigidbody2D>();
            rigidCloned.bodyType = RigidbodyType2D.Dynamic;
            rigidCloned.freezeRotation = true;
            var IngTrigF = newGameObject.AddComponent<IngridientsTiggerF>();
            IngTrigF.objInPlate = tmpSprite;
            IngTrigF.nameInPlate = tmpName;
            newGameObject.transform.position = pointReject.transform.position;
        }

        // Kosongkan daftar bahan makanan (opsi ini bisa diganti dengan mengeluarkan bahan)
        foreach (var item in bahanMasuk)
        {
            Destroy(item);
        }
        bahanMasuk.Clear();
    }
}
