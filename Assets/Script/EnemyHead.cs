using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerFoot")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
