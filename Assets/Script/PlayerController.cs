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
    float speed = 13;
    float jumpPower = 40;
    //float jumpPower = 12;

    //check the position of event
    public bool onGround = false;
    bool onPlatform = false;
    bool isSomething = false;
    bool isChop = false;
    public ChopBoard chopScript;
    float ignoreInput = 0; // 0: main, 1: down, 2: nothing
    bool ignorePlatform = false;
    bool isPlate = false;

    //grab and put variables - ingridients
    GameObject tmpSomethingGrab = null;
    // Raw grab
    GameObject tmpRawGrab = null; //This is Raw Grab, for clone the object
    // obj in hand checker
    GameObject objectInHand = null;

    //player object parameter
    Rigidbody2D rb;
    Animator animator;
    // Audio list
    [SerializeField]
    // 0 : player_chop ; 1 : jump
    List<AudioClip> audioClipList;
    AudioSource source;

    // Player Settings
    [SerializeField]
    GameObject bar_count;
    [SerializeField]
    GameObject fill_count;
    [SerializeField]
    GameObject text_count_die;
    [SerializeField]
    GameObject playerInit;
    float die_counter = 5;
    //private const float init_position_x = 5.09f;
    //private const float init_position_y = 2.12f;
    private Vector3 default_vector = new Vector3(0.17f, 0.17f, 1);


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        //for ignore any don't need to collide
        GameObject[] ignore = ConcatenateArrays(
                GameObject.FindGameObjectsWithTag("Table"),
                GameObject.FindGameObjectsWithTag("Untagged"),
                GameObject.FindGameObjectsWithTag("Raw"),
                GameObject.FindGameObjectsWithTag("Board"),
                GameObject.FindGameObjectsWithTag("Boiling"),
                GameObject.FindGameObjectsWithTag("Frying"),
                GameObject.FindGameObjectsWithTag("Charcoal"),
                GameObject.FindGameObjectsWithTag("RawPlate")
            );
        foreach (var obj in ignore)
        {
            Physics2D.IgnoreCollision(obj.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());
        }
        text_count_die.GetComponent<TextMesh>().text = die_counter.ToString(); //Start with 5
    }

    void Update()
    {
        //check if the hand is grabbing plate? - conditions for plating purposes
        if(objectInHand != null)
        {
            if (objectInHand.name.Contains("plate"))
            {
                isPlate = true;
            }
            else
            {
                isPlate = false;
            }
        }
        else
        {
            isPlate = false;
        }

        //start Input
        if (ignoreInput == 0)
        {
            // this velocity is for jump
            float velo_value = rb.velocity.y;
            if (velo_value < 0)
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
            if (velo_value > 0)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }

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
                //ignore anything on feet
                //for ignore any don't need to collide
                //GameObject none = new GameObject("");
                //none.AddComponent<BoxCollider2D>();
                try
                {
                    List<GameObject> ignore = new() { tmpSomethingGrab != null ? tmpSomethingGrab : null, tmpRawGrab != null ? tmpRawGrab : null };
                    foreach (var obj in ignore)
                    {
                        Physics2D.IgnoreCollision(obj.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());
                    }
                }
                catch { }
                rb.velocity = new Vector2(0, 1) * jumpPower;
                source.clip = audioClipList[1];
                source.Play();
            }
            animator.SetFloat("isJump", rb.velocity.y);

            //down on PLatform
            if (Input.GetKeyDown(KeyCode.DownArrow) && onPlatform)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                onGround = false;
                // disable Ground and the code go to check below (else section)
                ignoreInput = 1;
                ignorePlatform = true;
                StartCoroutine(tmp()); // prevent vault
            }

            //grab raw_plate
            if (Input.GetKeyDown(KeyCode.Space) && isPlate && objectInHand == null)
            {
                if (isSomething)
                {
                    //garb plate
                    GrabObject(tmpSomethingGrab, true);
                    animator.SetBool("isCarry", true);
                    return;
                }
                //else
                //{
                //    //grab raw_plate
                //    GrabObject(tmpRawGrab2.CreatePlate(), false);
                //    animator.SetBool("isCarry", true);
                //    return;
                //}
            }
            //put plate

            try
            {
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
                    isPlate = false; //anything there is put include plate is now isPlate = false
                    animator.SetBool("isCarry", false);
                    //return;
                }
            }
            catch 
            {
                tmpSomethingGrab = null;
                tmpRawGrab = null;
                return;
            }
            

            // player do chopping - C key
            if (Input.GetKeyDown(KeyCode.C) && isChop && objectInHand == null && chopScript.checkChop())
            {
                ignoreInput = 2;
                // do player animation chopping
                animator.SetBool("isChop", true);
                source.clip = audioClipList[0];
                source.Play();
            }
        }
        if (ignoreInput == 1)
        {
            // turn on when hit Ground
            if (!onPlatform && onGround && ignorePlatform)
            //if (onGround && ignorePlatform)
            {
                GetComponent<BoxCollider2D>().enabled = true;
                ignoreInput = 0;
                ignorePlatform = false;
            }
        }
    }

    private IEnumerator tmp()
    {
        yield return new WaitForSeconds(0.4f);
        GetComponent<BoxCollider2D>().enabled = true;
        ignoreInput = 0;
        ignorePlatform = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //on Smart Platform to Allow down
        if (col.tag == "Floor" || col.tag == "Platform")
        {
            onGround = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
        if (col.tag == "Platform")
        {
            onPlatform = true;
        }
        if (col.tag == "Wall")
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        if (col.tag == "Plate")
        {
            isSomething = true;
            tmpSomethingGrab = col.gameObject;
            isPlate = true;
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

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Raw")
        {
            tmpRawGrab = col.gameObject;
        }
        if (col.tag == "Ingridients" || col.tag == "Food" || col.tag == "Chopped" || col.tag == "Cooked")
        {
            isSomething = true;
            tmpSomethingGrab = col.gameObject;
        }
        if (col.tag == "Board")
        {
            isChop = true;
            chopScript = col.GetComponent<ChopBoard>();
        }
        //platform allow jump
        if (col.tag == "Floor" || col.tag == "Platform")
        {
            onGround = true;
            animator.SetFloat("isJump", 0);
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
        if (col.tag == "Ingridients" || col.tag == "Food" || col.tag == "Chopped" || col.tag == "Cooked")
        {
            isSomething = false;
            tmpSomethingGrab = null;
        }
        if (col.tag == "Raw")
        {
            tmpRawGrab = null;
        }
        //if (col.tag == "RawPlate")
        //{
        //    tmpRawGrab2 = null;
        //    isPlate = true;
        //}
        if (col.tag == "Plate")
        {
            isSomething = false;
            tmpSomethingGrab = null;
            isPlate = true;
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
            objToGrab.transform.localPosition = new Vector2(0, 7); // Place it in the player's hand
            objToGrab.GetComponent<Rigidbody2D>().isKinematic = true; // Disable physics on the object
            objectInHand = objToGrab;
        }
        else
        {
            // Instantiate a copy of the raw object
            GameObject clonedObject = Instantiate(objToGrab, transform.position, Quaternion.identity);
            clonedObject.tag = "Ingridients";
            clonedObject.transform.parent = transform;
            clonedObject.transform.localPosition = new Vector2(0, 7);
            clonedObject.GetComponent<Rigidbody2D>().isKinematic = true;
            //add trigger collider
            CircleCollider2D circleCollider = clonedObject.AddComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
            circleCollider.offset = new Vector2(0, -3.2f);
            circleCollider.radius = 1;
            //change the name
            clonedObject.name = clonedObject.name.Replace("raw_", "").Replace("(Clone)", "");
            //change sprite
            clonedObject.GetComponent<SpriteRenderer>().sprite = clonedObject.GetComponent<IngridientRead>().scriptIngridient.ingSprite;
            //insert the IngridientsTrigger Script - boiling purposes
            if (clonedObject.GetComponent<IngridientRead>().scriptIngridient.isBoiled)
            {
                clonedObject.AddComponent<IngridientsTrigger>();
            }
            //insert the IngridientsTrigger2 Script - frying purposes
            if (clonedObject.GetComponent<IngridientRead>().scriptIngridient.isFried)
            {
                clonedObject.AddComponent<IngridientsTrigger2>();
            }
            //insert the IngridientsTrigger2 Script - coal purposes
            if (clonedObject.GetComponent<IngridientRead>().scriptIngridient.isSmoked)
            {
                clonedObject.AddComponent<IngridientsTrigger3>();
            }
            if (clonedObject.GetComponent<IngridientRead>().scriptIngridient.cooktarget == Cooktarget.none)
            {
                clonedObject.AddComponent<IngridientsTiggerF>();
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
        ignoreInput = 0;
        animator.SetBool("isChop", false);
        // use function grab object so player is make the change
        GrabChoppedNull(tmpSomethingGrab, true);
        tmpSomethingGrab = null;
    }

    private void GrabChoppedNull(GameObject objToGrab, bool ingridients)
    {
        // when condition is chop, change the sprite of object - but not to hand
        if (isChop && !objToGrab.name.Contains("_potong"))
        {
            objToGrab.tag = "Chopped";
            objToGrab.name += "_potong";
            objToGrab.GetComponent<SpriteRenderer>().sprite = objToGrab.GetComponent<IngridientRead>().scriptIngridient.chopSprite;
            //insert the IngridientsTrigger Script - boiling purposes
            if (objToGrab.GetComponent<IngridientRead>().scriptIngridient.isBoiled)
            {
                objToGrab.GetOrAddComponent<IngridientsTrigger>();
            }
            //insert the IngridientsTrigger2 Script - frying purposes
            else if (objToGrab.GetComponent<IngridientRead>().scriptIngridient.isFried)
            {
                objToGrab.GetOrAddComponent<IngridientsTrigger2>();
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
        ignoreInput = 2;
        die_counter = 5;
        transform.localScale = default_vector; // orientation right
        bar_count.GetComponent<SpriteRenderer>().enabled = true;
        fill_count.GetComponent<SpriteRenderer>().enabled = true;
        text_count_die.GetComponent<MeshRenderer>().enabled = true;
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.position = playerInit.transform.position;
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
        ignoreInput = 0;
        onGround = true;
        animator.SetFloat("dieNums", 1);
        animator.SetBool("isDie", false);
    }
}
