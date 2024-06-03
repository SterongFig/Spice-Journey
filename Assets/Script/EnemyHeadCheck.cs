using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" || col.tag == "EnemyHead")
        {
            //player.transform.position = new Vector3(10, 10, 10);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0f);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300f); // bounce effect
            player.GetComponent<PlayerController>().onGround = true;
        }
        if(col.tag == "Floor" || col.tag == "Platform")
        {
            player.GetComponent<PlayerController>().onGround = true;
        }
    }
}
