using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingStation : MonoBehaviour
{
    [SerializeField]
    GameObject warn;
    [SerializeField]
    Canvas canvas; //for score managing

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Plate")
        {
            List<Sprite> param1 = col.GetComponent<PlateController>().objInPlate;
            List<string> param2 = col.GetComponent<PlateController>().nameInPlate;
            if(param1.Count > 0)
            {
                canvas.GetComponent<ScoreManager>().CheckServe(param1, param2);
                Destroy(col.gameObject);
                return;
            }
            EnableWarn("plate empty");

        }
        if(col.tag == "Player" || col.tag == "Enemy" || col.tag == "Floor" || col.tag == "Platform")
        {
            return;
        }
        EnableWarn("serve using plate");
    }

    private void EnableWarn(string message)
    {
        warn.GetComponent<TextMesh>().text = message;
        warn.SetActive(true);
        new WaitForSeconds(3);
        warn.SetActive(false);
    }
}
