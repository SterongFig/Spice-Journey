using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] GameObject textObj;
    [SerializeField] GameObject player;
    [SerializeField] DeleveryManagerUI deleveryManagerUI;
    [SerializeField] ScriptingMenu oneIngtest;
    List<string> list_tutorials =
        new() {
            "use arrows (left,right) to move", // 0
            "use arrows (up) to jump",
            "use arrows (down) to get down from platform",
            "see this delivery menu??",
            "Try delivery as FAST !!", // 4
            "TRY : lontong",
            "lontong = lontong(potong)",
            "GRAB lontong here, use SPACEBAR",
            "PUT here, use SPACEBAR",
            "CHOP it, use C to chop", // 9
            "Nice, wait a bit",
            "Place to this plate",
            "GRAB it again",
            "SERVE it HERE",
            "If same as need it will + 10 points", // 14
            "this is enemy, if you touch you die",
            "and revieve after 5 seconds",
            "you can kill it by stomp it !!",
            "watch out the hole here !!",
            "this is timer in level", // 19
            "there is a combo in level",
            "try delivered in order from right to left",
            "you can get x1.5 up to x2.5 combo",
            "the left bottom here is PAUSE",
            "TRY Experiments with the recepie", // 24
            "If done, you can quit now"
        };
    List<float> size =
        new() { 
            72, // 0
            72,
            60,
            72,
            72, // 4
            72,
            72,
            54,
            35,
            48, // 9
            50,
            48,
            48,
            48,
            72, // 14
            40,
            40,
            52,
            52,
            52, // 19
            60,
            60,
            60,
            52,
            60, // 24
        };
    List<Vector3> vector3s =
        new() {
            new Vector3(480,830,0), // 0
            new Vector3(480,830,0),
            new Vector3(480,830,0),
            new Vector3(100,830,0),
            new Vector3(100,830,0), // 4
            new Vector3(480,830,0),
            new Vector3(480,830,0),
            new Vector3(340,630,0),
            new Vector3(480,200,0),
            new Vector3(480,250,0), // 9
            new Vector3(480,260,0),
            new Vector3(650,630,0),
            new Vector3(650,630,0),
            new Vector3(1350,630,0),
            new Vector3(480,830,0), // 14
            new Vector3(1390,250,0),
            new Vector3(1390,250,0),
            new Vector3(1390,250,0),
            new Vector3(1350,180,0),
            new Vector3(1390,830,0), //19
            new Vector3(480,830,0),
            new Vector3(480,830,0),
            new Vector3(480,830,0),
            new Vector3(1390,150,0),
            new Vector3(480,830,0), // 24
        };
    [SerializeField]
    int index = 0;
    bool inside = false;

    // Start is called before the first frame update
    void Start()
    {
        //deleveryManagerUI.AddRecepie(oneIngtest);
        textObj.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();

        if (inside) return;

        if (index == 0 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            index++;
        }
        if(index == 1 && (Input.GetKeyDown(KeyCode.UpArrow)))
        {
            index++;
        }
        if(index == 2 && (Input.GetKeyDown(KeyCode.DownArrow)) && player.GetComponent<Transform>().position.y > 5)
        {
            index++;
        }
        if (index == 3)
        {
            inside = true;
            StartCoroutine(timeout(1.5f, 3));
        }
        if (index == 4)
        {
            inside = true;
            StartCoroutine(timeout(1.5f, 4));
        }
        if (index == 5)
        {
            inside = true;
            StartCoroutine(timeout(1.5f, 5));
        }
        if (index == 6)
        {
            inside = true;
            StartCoroutine(timeout(3f, 6));
        }
        if (index == 7 && (Input.GetKeyDown(KeyCode.Space)) && player.GetComponent<Transform>().position.y > 5)
        {
            index++;
        }
        if (index == 8 && (Input.GetKeyDown(KeyCode.Space)) && player.GetComponent<Transform>().position.y < 5)
        {
            index++;
        }
        if (index == 9 && (Input.GetKeyDown(KeyCode.C)) && player.GetComponent<Transform>().position.y < 5)
        {
            index++;
        }
        if (index == 10)
        {
            inside = true;
            StartCoroutine(timeout(3f, 10));
        }
        if (index == 11 && (Input.GetKeyDown(KeyCode.Space)) && player.GetComponent<Transform>().position.y > 5)
        {
            index++;
        }
        if (index == 12)
        {
            inside = true;
            StartCoroutine(timeout(1.5f, 12));
        }
        if (index == 13 && (Input.GetKeyDown(KeyCode.Space)) && player.GetComponent<Transform>().position.y > 5)
        {
            index++;
        }
        if (index == 14)
        {
            inside = true;
            StartCoroutine(timeout(3.5f, 14));
        }
        if (index == 15)
        {
            inside = true;
            StartCoroutine(timeout(3f, 15));
        }
        if (index == 16)
        {
            inside = true;
            StartCoroutine(timeout(3f, 16));
        }
        if (index == 17)
        {
            inside = true;
            StartCoroutine(timeout(3f, 17));
        }
        if (index == 18)
        {
            inside = true;
            StartCoroutine(timeout(3f, 18));
        }
        if (index == 19)
        {
            inside = true;
            StartCoroutine(timeout(4f, 19));
        }
        if (index == 20)
        {
            inside = true;
            StartCoroutine(timeout(4f, 20));
        }
        if (index == 21)
        {
            inside = true;
            StartCoroutine(timeout(4f, 21));
        }
        if (index == 22)
        {
            inside = true;
            StartCoroutine(timeout(4f, 22));
        }
        if (index == 23)
        {
            inside = true;
            StartCoroutine(timeout(4f, 23));
            int levelOpen = PlayerPrefs.GetInt("LevelOpen", 0);
            // this is to add the num to 1 - just one time
            if (levelOpen == 0)
            {
                PlayerPrefs.SetInt("LevelOpen", 1);
            }
        }
        if (index == 24)
        {
            inside = true;
            StartCoroutine(timeout(10f, 24));
        }
    }
    private void UpdateText()
    {
        textObj.GetComponent<TextMeshProUGUI>().text = list_tutorials[index];
        textObj.GetComponent<TextMeshProUGUI>().fontSize = size[index];
        textObj.GetComponent<RectTransform>().position = vector3s[index];
    }
    private IEnumerator timeout(float time, int indexing)
    {
        if(indexing == index)
        {
            while (true)
            {
                yield return new WaitForSeconds(time);
                break;
            }
            index = index + 1;
            inside = false;
            yield break;
        }
    }
}
