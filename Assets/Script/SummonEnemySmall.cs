using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SummonEnemySmall : MonoBehaviour
{
    [SerializeField]
    GameObject FinalEnemy;
    [SerializeField]
    float waiting;
    
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
            yield return new WaitForSeconds(waiting);
        }
    }
}
