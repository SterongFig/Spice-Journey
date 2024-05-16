using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Compilation;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float hAxis;
    float speed = 9;
    float jumpPower = 11;

    //check the position of event
    public bool onGround = false;
    [SerializeField]
    bool onPlatform = false;
    bool isSomething = false;
    bool isChop = false;
    ChopBoard chopScript;
    bool ignoreInput = false;
    bool ignorePlatform = false;
    [SerializeField]
    bool isPlate = false;
    [SerializeField]
    bool isServing = false;

    //grab and put variables - ingridients
    [SerializeField]
    GameObject tmpSomethingGrab = null;
    int? tmpSomethingGrab2 = null;
    // Raw grab
    GameObject tmpRawGrab = null; //This is Raw Grab, for clone the object
    int? tmpRawGrab2 = null; //This For the Sprite
    [SerializeField]
    GameObject objectInHand = null;

    //player object parameter
    Rigidbody2D rb;
    Animator animator;
    // Audio list
    [SerializeField]
    // 0 : player_chop
    List<AudioClip> audioClipList;
    AudioSource source;
    AudioClip clip;

    //PlayerMemorizeRaw-Ingridients Combination
    [SerializeField]
    // 0 : raw_lontong ; 1 : raw_sapi
    List<Sprite> rawSprite;
    [SerializeField]
    // 0 : lontong ; 1 : sapi
    List<Sprite> ingSprites;
    [SerializeField]
    // 0 : lontong_potong ; 1 : sapi_potong
    List<Sprite> chopSprites;

    //Player Memorize all the Ingridients Combination
    List<String> boil_ingridients = new() { "beras", "sapi_potong", "ayam_potong" };
    List<String> fry_ingridients = new() { "nasi", "mie", "cabai_potong", "bumbu_aceh" };

    // Player Settings
    [SerializeField]
    GameObject bar_count;
    [SerializeField]
    GameObject fill_count;
    [SerializeField]
    GameObject text_count_die;
    [SerializeField]
    GameObject die_trigger;
    float die_counter = 5;
    private const float init_position_x = 5.09f;
    private const float init_position_y = 2.12f;
    private Vector3 default_vector = new Vector3(0.17f, 0.17f, 1);


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        //for ignore any don't need to collide
        GameObject[] ignore = ConcatenateArrays(
                GameObject.FindGameObjectsWithTag("Table"),
                GameObject.FindGameObjectsWithTag("Raw"),
                GameObject.FindGameObjectsWithTag("Board")
            );
        foreach (var obj in ignore)
        {
            Physics2D.IgnoreCollision(obj.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());
        }
        text_count_die.GetComponent<TextMesh>().text = die_counter.ToString(); //Start with 5
    }

    void Update()
    {
        if (!ignoreInput)
        {
            // moving left right
            hAxis = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(hAxis, 0);
            transform.Translate(direction * Time.deltaTime * speed);
            animator.SetFloat("isMoving", Math.Abs(hAxis));
            if (hAxis < 0)
            {
                var tmp = default_vector.x * -1;
                transform.localScale = new Vector3(tmp, default_vector.y, default_vector.z);
            }
            if (hAxis > 0)
            {
                transform.localScale = default_vector;
            }
            //transform.localScale = new Vector3(transform.localScale.x * hAxis, transform.localScale.y, transform.localScale.z);

            //jump
            if (Input.GetKeyDown(KeyCode.UpArrow) && onGround)
            {
                rb.velocity = new Vector2(0, 1) * jumpPower;                
            }
            animator.SetFloat("isJump", rb.velocity.y);

            //down on PLatform
            if (Input.GetKeyDown(KeyCode.DownArrow) && onPlatform)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                onGround = false;
                // disable Ground and the code go to check below (else section)
                ignoreInput = true;
                ignorePlatform = true;
            }

            //function to grab and put object
            if (Input.GetKeyDown(KeyCode.Space) && objectInHand == null)
            {
                if (isSomething)
                {
                    GrabObject(tmpSomethingGrab, true);
                    animator.SetBool("isCarry", true);
                    return;
                }
                else
                {
                    GrabObject(tmpRawGrab, false);
                    animator.SetBool("isCarry", true);
                    return;
                }
            }
            //put object
            if (Input.GetKeyDown(KeyCode.Space) && objectInHand != null)
            {
                PlaceObjectBack(objectInHand);
                animator.SetBool("isCarry", false);
                return;
            }

            // player do chopping - C key
            if (Input.GetKeyDown(KeyCode.C) && isChop && objectInHand == null && chopScript.checkChop())
            {
                ignoreInput = true;
                // do player animation chopping
                animator.SetBool("isChop", true);
                source.clip = audioClipList[0];
                source.Play();
            }
        }
        if (ignoreInput)
        {
            // turn on when hit Ground
            if (!onPlatform && onGround && ignorePlatform)
            {
                GetComponent<BoxCollider2D>().enabled = true;
                ignoreInput = false;
                ignorePlatform = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //platform allow jump
        if (col.tag == "Floor" || col.tag == "Platform")
        {
            onGround = true;
            animator.SetFloat("isJump", 0);
        }
        //on Smart Platform to Allow down
        if (col.tag == "Platform")
        {
            onPlatform = true;
        }
        if (col.tag == "Ingridients" || col.tag == "Food")
        {
            isSomething = true;
            //detect sprite and find index combination
            var detect = col.gameObject.GetComponent<SpriteRenderer>().sprite;
            int indexes = ingSprites.IndexOf(detect);
            tmpSomethingGrab = col.gameObject;
            tmpSomethingGrab2 = indexes;
        }
        if (col.tag == "Raw")
        {
            var detect = col.gameObject.GetComponent<SpriteRenderer>().sprite;
            int indexes = rawSprite.IndexOf(detect);
            tmpRawGrab = col.gameObject;
            tmpRawGrab2 = indexes;
        }
        if (col.tag == "Board")
        {
            isChop = true;
            chopScript = col.GetComponent<ChopBoard>();
        }
        if (col.tag == "Chopped")
        {
            isSomething = true;
            tmpSomethingGrab = col.gameObject;
        }
        if (col.tag == "Die" || col.tag == "Enemy")
        {
            // prevent double coroutines
            StopAllCoroutines();
            if (col.tag == "Die")
            {
                animator.SetFloat("dieNums", 0);
            }
            animator.SetBool("isDie", true);
            StartCoroutine(PlayerDie());
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Floor" || col.tag == "Platform")
        {
            onGround = false;
        }
        if (col.tag == "Platform")
        {
            onPlatform = false;
        }
        if (col.tag == "Ingridients" || col.tag == "Food" || col.tag == "Chopped")
        {
            isSomething = false;
            tmpSomethingGrab = null;
            tmpSomethingGrab2 = null;
        }
        if (col.tag == "Raw")
        {
            tmpRawGrab = null;
            tmpRawGrab2 = null;
        }
        if (col.tag == "Board")
        {
            isChop = false;
            chopScript = null;
        }
    }

    private void GrabObject(GameObject objToGrab, bool ingridients)
    {
        if (ingridients)
        {
            // Set the object as a child of the player (to move with the player)
            objToGrab.transform.parent = transform;
            objToGrab.transform.localPosition = new Vector2(0, 6); // Place it in the player's hand
            objToGrab.GetComponent<Rigidbody2D>().isKinematic = true; // Disable physics on the object
            objectInHand = objToGrab;
        }
        else
        {
            // Instantiate a copy of the raw object
            GameObject clonedObject = Instantiate(objToGrab, transform.position, Quaternion.identity);
            clonedObject.tag = "Ingridients";
            clonedObject.transform.parent = transform;
            clonedObject.transform.localPosition = new Vector2(0, 6);
            clonedObject.GetComponent<Rigidbody2D>().isKinematic = true;
            //add trigger collider
            CircleCollider2D circleCollider = clonedObject.AddComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
            circleCollider.offset = new Vector2(0, -3.2f);
            circleCollider.radius = 1;
            //change the name
            clonedObject.name = clonedObject.name.Replace("raw_", "").Replace("(Clone)", "");
            //change sprite
            clonedObject.GetComponent<SpriteRenderer>().sprite = ingSprites[tmpRawGrab2.Value];
            //insert the IngridientsTrigger Script - boiling purposes
            if (boil_ingridients.Contains(clonedObject.name))
            {
                clonedObject.AddComponent<IngridientsTrigger>();
            }
            //insert the IngridientsTrigger2 Script - frying purposes
            else if (fry_ingridients.Contains(clonedObject.name))
            {
                clonedObject.AddComponent<IngridientsTrigger2>();
            }
            //else don't add trigger to cook - example lontong is not cookable
            // Set the copy as the object in hand
            objectInHand = clonedObject;
        }
    }

    private void PlaceObjectBack(GameObject objToPlaceBack)
    {
        // Release the object from being a child of the player
        objToPlaceBack.transform.parent = null;
        objToPlaceBack.GetComponent<Rigidbody2D>().isKinematic = false; // Enable physics on the object
        objectInHand = null; // Reset the object in hand to null
    }

    // use by board to tell chop is done - change the object to chopped
    public void ChoppingDone()
    {
        ignoreInput = false;
        animator.SetBool("isChop", false);
        // use function grab object so player is make the change
        GrabChoppedNull(tmpSomethingGrab, true);
        tmpSomethingGrab = null;
        tmpSomethingGrab2 = null;
    }

    private void GrabChoppedNull(GameObject objToGrab, bool ingridients)
    {
        // when condition is chop, change the sprite of object - but not to hand
        if (isChop && !objToGrab.name.Contains("_potong"))
        {
            objToGrab.tag = "Chopped";
            objToGrab.name += "_potong";
            objToGrab.GetComponent<SpriteRenderer>().sprite = chopSprites[tmpSomethingGrab2.Value];
            //insert the IngridientsTrigger Script - boiling purposes
            if (boil_ingridients.Contains(objToGrab.name))
            {
                objToGrab.AddComponent<IngridientsTrigger>();
            }
            //insert the IngridientsTrigger2 Script - frying purposes
            else if (fry_ingridients.Contains(objToGrab.name))
            {
                objToGrab.AddComponent<IngridientsTrigger2>();
            }
            //else don't add trigger to cook - example lontong is not cookable
            objToGrab.transform.parent = transform;
            objToGrab.transform.localPosition = new Vector2(0, 8);
        }
    }

    //arrays concat shorthand
    private GameObject[] ConcatenateArrays(params GameObject[][] arrays)
    {
        return arrays.SelectMany(array => array).ToArray();
    }

    private IEnumerator PlayerDie()
    {
        ignoreInput = true;
        die_counter = 5;
        transform.localScale = default_vector; // orientation right
        bar_count.GetComponent<SpriteRenderer>().enabled = true;
        fill_count.GetComponent<SpriteRenderer>().enabled = true;
        text_count_die.GetComponent<MeshRenderer>().enabled = true;
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.position = new Vector2(init_position_x, init_position_y);
        die_trigger.transform.position = new Vector3(init_position_x - 1.2f, gameObject.transform.position.y - 3, 0);
        while (true)
        {
            text_count_die.GetComponent<TextMesh>().text = die_counter.ToString();
            if (die_counter <= 0)
            {
                break;
            }
            die_counter = die_counter - 1;
            yield return new WaitForSeconds(1);
        }
        bar_count.GetComponent<SpriteRenderer>().enabled = false;
        fill_count.GetComponent<SpriteRenderer>().enabled = false;
        text_count_die.GetComponent<MeshRenderer>().enabled = false;
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
        die_counter = 5;
        ignoreInput = false;
        onGround = true;
        animator.SetFloat("dieNums", 1);
        animator.SetBool("isDie", false);
    }
}
