using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "EnemyHead")
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0f);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100f); // bounce effect
        }
        if(col.tag == "Floor")
        {
            player.GetComponent<PlayerController>().onGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Floor")
        {
            player.GetComponent<PlayerController>().onGround = false;
        }
    }
}
