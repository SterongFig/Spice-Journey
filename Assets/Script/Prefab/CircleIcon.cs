using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleIcon : MonoBehaviour
{
    public GameObject child;
    public Sprite setSprite;

    void Update()
    {
        child.GetComponent<SpriteRenderer>().sprite = setSprite;
    }
}
