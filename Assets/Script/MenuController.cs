using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject bar;
    public GameObject fill;
    public ScoreManager scoreManager;
    const float waktutunggu = 30; //waktu tunggu disesuaikan
    float fillBar = 0;

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(FillBarFill());
    }

    // Update is called once per frame
    void Update()
    {
        fill.GetComponent<RectTransform>().sizeDelta = new Vector2((fillBar/waktutunggu) * 190, 20);
        if (fillBar >= waktutunggu)
        {
            fillBar = 0;
            scoreManager.MinusPoint(30);
            //play buzzer sound
            StopAllCoroutines();
            StartCoroutine(FillBarFill());
        }
    }

    private IEnumerator FillBarFill()
    {
        while (true)
        {
            fillBar = fillBar + 0.1f;
            yield return new WaitForSeconds(0.1f);
            if (fillBar >= waktutunggu)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
