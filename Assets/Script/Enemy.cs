using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float time_die = 0;
    [SerializeField]
    bool onGround = false;
    float hAxis = -1;
    const float speed = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        StopAllCoroutines();
        StartCoroutine(CountdownDie());
    }

    // Update is called once per frame
    void Update()
    {
        if (onGround)
        {
            //do animation walk
            Vector2 direction = new Vector2(hAxis, 0);
            transform.Translate(direction * Time.deltaTime * speed);
        }
        // else do animation idle
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Floor")
        {
            StopAllCoroutines();
            onGround = true;
        }
        if(col.tag == "Player" || col.tag == "Trash" || col.tag == "Table")
        {
            return;
        }
        if(name != "Enemy") // it bump to wall or other, just for clone
        {
            hAxis = (hAxis == -1) ? 1 : -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Floor")
        {
            StopAllCoroutines();
            try
            {
                StartCoroutine(CountdownDie());
            }
            catch
            {
                // if there any error just Ignore it
                Debug.Log("the enemy already died - but coroutine started");
                return;
            }
            onGround = false;
        }
    }

    private IEnumerator CountdownDie()
    {
        while (true)
        {
            time_die++;
            if(time_die > 4 && name != "Enemy")
            {
                // means that this gameobject / enemy out of bounderies
                Destroy(gameObject);
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
