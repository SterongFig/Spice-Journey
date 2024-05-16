using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SummonEnemySmall : MonoBehaviour
{
    [SerializeField]
    GameObject FinalEnemy;
    
    // Start is called before the first frame update
    void Start()
    {
        StopAllCoroutines();
        StartCoroutine(createEnemy());
    }

    private IEnumerator createEnemy()
    {
        while (true)
        {
            GameObject newGameObject = Instantiate(FinalEnemy.gameObject);
            newGameObject.transform.position = transform.position - new Vector3 (0,1.5f,0);
            //GameObject enemyObject = new GameObject("Enemy");
            //SpriteRenderer spriteRenderer = enemyObject.AddComponent<SpriteRenderer>();
            //spriteRenderer.sprite = enemyImg;
            ////enemy main
            //enemyObject.name = "Enemy";
            //enemyObject.tag = "Enemy";
            //enemyObject.transform.position = transform.position - new Vector3(0, 1, 0);
            //enemyObject.transform.localPosition = transform.position - new Vector3(0, 1, 0);
            //BoxCollider2D boxCollider2D = enemyObject.AddComponent<BoxCollider2D>();
            //boxCollider2D.size = new Vector2(0.8423498f, 0.735705f);
            //CircleCollider2D circleCollider2D = enemyObject.AddComponent<CircleCollider2D>();
            //circleCollider2D.offset = new Vector2(0, -0.45f);
            //circleCollider2D.radius = 0.1f;
            //circleCollider2D.isTrigger = true;
            //Rigidbody2D rigidbody2D = enemyObject.AddComponent<Rigidbody2D>();
            //rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            ////enemyObject.AddComponent<Enemy>();
            ////enemy head
            //GameObject enemyHeadObject = new GameObject("Head");
            //enemyHeadObject.transform.SetParent(enemyObject.transform);
            ////set parent
            //enemyHeadObject.transform.SetParent(enemyObject.transform);
            //enemyHeadObject.transform.localPosition = enemyObject.transform.position;
            //BoxCollider2D boxCollider2DHead = enemyHeadObject.AddComponent<BoxCollider2D>();
            //boxCollider2DHead.isTrigger = true;
            //boxCollider2DHead.offset = new Vector2(0.008050531f, 0.2961341f);
            //boxCollider2DHead.size = new Vector2(0.7457073f, 0.1244732f);
            //enemyHeadObject.AddComponent<EnemyHead>();
            yield return new WaitForSeconds(10);
        }
    }
}
