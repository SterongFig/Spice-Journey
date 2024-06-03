using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPlatform : MonoBehaviour
{
    //GameObject Player;
    //private float? _tmpPlatformPositionTarget = null;

    //private void Start()
    //{
    //    // the parent GameObject
    //    Player = transform.parent.gameObject;
    //}

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.tag == "Platform" && Player.GetComponent<Rigidbody2D>().velocity.y > 0) //collide platform and upwards move
    //    {
    //        Player.GetComponent<BoxCollider2D>().enabled = false;
    //        _tmpPlatformPositionTarget = col.gameObject.transform.position.y;
    //        StopAllCoroutines();
    //        StartCoroutine(checkAbove());
    //    }
    //}

    //private IEnumerator checkAbove()
    //{
    //    while (true)
    //    {
    //        if (Player.transform.position.y > _tmpPlatformPositionTarget || Player.GetComponent<PlayerController>().onGround)
    //        {
    //            Player.GetComponent<BoxCollider2D>().enabled = true;
    //            break;
    //        }
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}

}
