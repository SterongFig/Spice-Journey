using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField]
    GameObject pointReject;
    public List<Sprite> objInPlate;
    public List<string> nameInPlate;
    int newSpriteCount = 0;
    int currentSpriteCount = 0;

    //this is for combination purposes
    public Sprite sprite_combination = null;

    [SerializeField] ScoreManager scoreManager;

    private void Update()
    {
        if (sprite_combination != null)
        {
            newSpriteCount = 0;
            currentSpriteCount = 0;
            DestroyAllChildren();
            GameObject newGameObject = new GameObject("combination");
            SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite_combination;
            spriteRenderer.sortingLayerName = "UI";
            spriteRenderer.sortingOrder = 2;
            newGameObject.AddComponent<Rigidbody2D>().isKinematic = true;
            newGameObject.transform.SetParent(transform);
            newGameObject.transform.localPosition = new Vector2(0, 1);
            newGameObject.transform.localScale = Vector3.one * 0.8f;
        }
        else
        {
            newSpriteCount = objInPlate.Count;
            if (objInPlate != null || newSpriteCount > currentSpriteCount)
            {
                // Iterate through the lists
                for (int i = currentSpriteCount; i < newSpriteCount; i++)
                {
                    GameObject newGameObject = new GameObject(nameInPlate[i]);
                    SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = objInPlate[i];
                    spriteRenderer.sortingLayerName = "UI";
                    spriteRenderer.sortingOrder = i;
                    newGameObject.AddComponent<Rigidbody2D>().isKinematic = true;
                    newGameObject.transform.SetParent(transform);
                    newGameObject.transform.localPosition = new Vector2(0, 2.44f + (i * 1));
                    newGameObject.transform.localScale = Vector3.one * 0.65f;
                }
                currentSpriteCount = newSpriteCount;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Ingridients")
        {
            if (col.gameObject.transform.parent == null) // the Ingridients is not on player hand
            { 
                // check if the ingridient tag is allow in this add to plate? if not then reject & return
                if (col.GetComponent<IngridientRead>().scriptIngridient.platetarget != Platetarget.ingridient)
                {
                    col.transform.position = pointReject.transform.position;
                    return;
                }
                if (name == "raw_plate")
                {
                    var newPlate = CreatePlate();
                    AddToList(col, Platetarget.ingridient , newPlate);
                    Destroy(col);
                }
                else
                {
                    AddToList(col, Platetarget.ingridient);
                    Destroy(col);
                }
            }
        }
        if (col.tag == "Chopped")
        {
            if (col.gameObject.transform.parent == null)
            {
                if (col.GetComponent<IngridientRead>().scriptIngridient.platetarget != Platetarget.chop)
                {
                    col.transform.position = pointReject.transform.position;
                    return;
                }
                if (name == "raw_plate")
                {
                    var newPlate = CreatePlate();
                    AddToList(col, Platetarget.chop, newPlate);
                    Destroy(col);
                }
                else
                {
                    AddToList(col, Platetarget.chop);
                    Destroy(col);
                }
            }
        }
        if (col.tag == "Cooked")
        {
            if (col.gameObject.transform.parent == null)
            {
                if (col.GetComponent<IngridientRead>().scriptIngridient.platetarget != Platetarget.cooked)
                {
                    col.transform.position = pointReject.transform.position;
                    return;
                }
                if (name == "raw_plate")
                {
                    var newPlate = CreatePlate();
                    AddToList(col, Platetarget.cooked, newPlate);
                    Destroy(col);
                }
                else
                {
                    AddToList(col, Platetarget.cooked);
                    Destroy(col);
                }
            }
        }
        // food is a bit diffrent
        if (col.tag == "Food")
        {
            if (col.gameObject.transform.parent == null)
            {
                if (name == "raw_plate")
                {
                    var newPlate = CreatePlate();
                    newPlate.GetComponent<PlateController>().objInPlate = col.GetComponent<IngridientsTiggerF>().objInPlate;
                    newPlate.GetComponent<PlateController>().nameInPlate = col.GetComponent<IngridientsTiggerF>().nameInPlate;
                    newPlate.GetComponent<PlateController>().sprite_combination = col.GetComponent<SpriteRenderer>().sprite;
                }
                // if on other plate it will disapear
                Destroy(col);
                
            }
        }
    }

    public void AddToList(Collider2D col, Platetarget ingridients, GameObject newPlate = null)
    {
        if (newPlate != null)
        {
            if (ingridients == Platetarget.ingridient || ingridients == Platetarget.chop || ingridients == Platetarget.cooked)
            {
                newPlate.GetComponent<PlateController>().objInPlate.Add(col.gameObject.GetComponent<SpriteRenderer>().sprite);
                newPlate.GetComponent<PlateController>().nameInPlate.Add(col.gameObject.name);
            }
        }
        else
        {
            // this
            if (ingridients == Platetarget.ingridient || ingridients == Platetarget.chop || ingridients == Platetarget.cooked)
            {
                objInPlate.Add(col.gameObject.GetComponent<SpriteRenderer>().sprite);
                nameInPlate.Add(col.gameObject.name);
            }
        }
        sprite_combination = scoreManager.CheckMenuCombination(objInPlate);
    }
    
    public GameObject CreatePlate()
    {
        //objInPlate.Add(col.gameObject);
        GameObject clonedObject = Instantiate(gameObject, transform.position, Quaternion.identity);
        clonedObject.tag = "Plate";
        clonedObject.transform.localScale = transform.localScale;
        clonedObject.transform.localPosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Rigidbody2D rigid2d = clonedObject.AddComponent<Rigidbody2D>();
        rigid2d.freezeRotation = true;
        //change the name
        clonedObject.name = clonedObject.name.Replace("raw_", "").Replace("(Clone)", "");
        return clonedObject;
    }

    // Call this method to destroy all child objects
    private  void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

//dont uncomment
//public enum Platetarget { ingridient, chop, cooked }