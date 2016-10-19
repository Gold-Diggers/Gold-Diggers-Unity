using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerCollisionController : MonoBehaviour
{
    public Canvas heartsCanvas;
    public Text diamondText;
    public Image specialDiamondImage;

    // dialogue use
    public Text dialogue;
    public Button yesButton;
    public Button noButton;
    private const string LEVEL_1_DIALOGUE_FRONT = "Hiya buddy! I’m tired of mining for diamonds but I found a key to this cool" +
        " lookin’ diamond cave. Want in? Trade you ";
    private const string LEVEL_1_DIALOGUE_BACK = " diamonds for it!";

    // diamond penalty use
    private const int DIAMOND_RANGE_1 = 5;
    private const int DIAMOND_LOSE_RANGE_1 = 1; // value

    private const int DIAMOND_RANGE_2 = 25;
    private const float DIAMOND_LOSE_RANGE_2 = 0.5f; // percent 

    private const int DIAMOND_RANGE_3 = 45;
    private const float DIAMOND_LOSE_RANGE_3 = 0.55f; // percent 

    private const int DIAMOND_RANGE_4 = 65;
    private const float DIAMOND_LOSE_RANGE_4 = 0.6f; // percent 

    private const int DIAMOND_RANGE_5 = 85;
    private const float DIAMOND_LOSE_RANGE_5 = 0.65f; // percent 

    private const float DIAMOND_LOSE_MAX = 0.70f; // percent

    // Canvas with text used to display + ? diamond
    public Canvas diamondDisplay;

    // player constants for handling all types of collisions
    private const int NUM_LIVES_START = 3;
    private const float PLAYER_X_OFFSET = 0.025f;
    private const int NUM_DIAMONDS_START = 0;
    private const int NUM_SPECIAL_DIAMONDS_START = 0;
    private const int NUM_SPECIAL_DIAMONDS_MAX = 3;
    private const int REPEL_PLAYER_FORCE = 200;

    // treasure chest constants
    private const int SPAWN_DIAMOND = 0;
    private const int SPAWN_MONSTER = 1;

    // monster type constants
    private const int MONSTER_ONE = 0;
    private const int MONSTER_TWO = 1;

    // display collect diamond UI constants
    private const float DELAY_MOVE_TEXT = 0.05F;
    private const float Y_OFFSET_MOVE_TEXT = 0.07F;
    private const int TIMES_TO_MOVE_TEXT = 10;

    // strings
    private const string DISPLAY_ONE_DIAMOND = "+1 diamond";
    private const string DISPLAY_TEN_DIAMOND = "+10 diamond";
    private const string DISPLAY_SPECIAL_DIAMOND = "Special Diamond Collected";

    private const string TEN_DIAMOND_NAME = "Diamond10(Clone)";
    private const string PLATFORM = "Platform";
    private const string MONSTER = "Monster";
    private const string TRAP = "Trap";
    private const string DIAMOND = "Diamond";
    private const string SPECIAL_DIAMOND = "SpecialDiamond";
    private const string TREASURE_CHEST = "TreasureChest";
    private const string SPECIAL_TREASURE_CHEST = "SpecialTreasureChest";
    private const string BG_BOUNDARY = "BackgroundBoundary";
    private const string END_LEVEL = "EndLevel";
    private const string ERROR_INVALID_LIVES_IMAGE = "ERROR: hearts index is out of range.";
    private const string ERROR_INVALID_LIVES_VALUE = "ERROR: 'lives' attribute cannot be < 0.";
    private const string ERROR_INVALID_SPECIAL_DIAMOND_VALUE = "ERROR: 'specialDiamonds' attribute cannot be > 3.";
    private const string ERROR_INVALID_RANDOM_VALUE = "ERROR: Random integer for treasure chest is not between [0, 1].";
    private const string ERROR_UNEXPECTED_COLLISION_EVENT = "ERROR: Unexpected collision event occurred.\n\n"
                                                            + "Collider is not 'Platform', 'BackgroundBoundary', "
                                                            + "'Diamond', 'TreasureChest', 'SpecialTreasureChest', "
                                                            + "'Monster' or 'Trap'.";

    // player attributes
    private bool isHurt;
    private Rigidbody2D rb2d;

    public int lives;
    public int diamonds;
    public int specialDiamonds;

    private Animator anim;

    // treasure chest asset attributes
    public GameObject spawnedDiamond;
    public GameObject spawnedSpecialDiamond;
    public GameObject spawnedMonsterType1;
    public GameObject spawnedMonsterType2;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lives = NUM_LIVES_START;
        isHurt = false;
        diamonds = NUM_DIAMONDS_START;
        specialDiamonds = NUM_SPECIAL_DIAMONDS_START;
        diamondText.text = diamonds.ToString();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    // Handles collisions with diamonds or end level
    void OnTriggerEnter2D(Collider2D other)
    {
        Assert.IsTrue(other.gameObject.CompareTag(DIAMOND) || other.gameObject.CompareTag(SPECIAL_DIAMOND) ||
            other.gameObject.CompareTag(END_LEVEL));
        if (other.gameObject.CompareTag(DIAMOND))
        {
            triggerDiamondInteraction(other);
        }
        else if (other.gameObject.CompareTag(SPECIAL_DIAMOND))
        {
            triggerSpecialDiamondInteraction(other);
        } else if (other.gameObject.CompareTag(END_LEVEL))
        {
            triggerEndLevel();
        }
    }

    // Dummy implementations
    // Use OnCollisionStay2D to check every frame for collision handling.
    void OnCollisionStay2D(Collision2D coll)
    {
        string collidedObject = coll.gameObject.tag;
        // if (!Equals(collidedObject, PLATFORM)) print(collidedObject);
        Assert.IsTrue(Equals(collidedObject, PLATFORM) || Equals(collidedObject, DIAMOND) || Equals(collidedObject, SPECIAL_DIAMOND) ||
                      Equals(collidedObject, TREASURE_CHEST) || Equals(collidedObject, SPECIAL_TREASURE_CHEST) || Equals(collidedObject, MONSTER) ||
                      Equals(collidedObject, TRAP) || Equals(collidedObject, BG_BOUNDARY) || Equals(collidedObject, END_LEVEL), ERROR_UNEXPECTED_COLLISION_EVENT);

        switch (collidedObject)
        {
            case PLATFORM:
                // this case is already handled in the control functions
                break;

            case BG_BOUNDARY:
                // no action needed here
                break;

            case DIAMOND:
                // Action handled by OnTriggerEnter2D()
                break;

            case SPECIAL_DIAMOND:
                // Action handled by OnTriggerEnter2D()
                break;

            case TREASURE_CHEST:
                StartCoroutine(triggerTreasureChestInteraction(coll)); // using a coroutine allows us to perform features such as time delays
                break;

            case SPECIAL_TREASURE_CHEST:
                StartCoroutine(triggerSpecialTreasureChestInteraction(coll));
                break;

            case MONSTER:
                triggerMonsterInteraction(coll);
                break;

            case TRAP:
                triggerTrapInteraction(coll);
                break;

            case END_LEVEL:
                // Action handled by OnTriggerEnter2D()
                break;

            default:
                break;
        }
    }

    void showDeathScreen()
    {
        SceneManager.LoadScene("DeathMenu", LoadSceneMode.Single);
    }

    private void triggerEndLevel()
    {
        GetComponent<PlayerBaseController>().setEndLevel(); // prevent movement by calling end level at base controller.
        dialogue.text = LEVEL_1_DIALOGUE_FRONT + getDiamondPenalty() + LEVEL_1_DIALOGUE_BACK;
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    private void triggerDiamondInteraction(Collider2D coll)
    {
        if (Equals(coll.gameObject.name, TEN_DIAMOND_NAME))
        {
            StartCoroutine(displayMovingUICollectDiamond(DISPLAY_TEN_DIAMOND));
            //print("Player has collected 10 diamonds.");
            IncrementDiamondCountByTen();
            Destroy(coll.gameObject);
        }
        else
        {
            StartCoroutine(displayMovingUICollectDiamond(DISPLAY_ONE_DIAMOND));
            //print("Player has collected a diamond.");
            IncrementDiamondCountByOne();
            Destroy(coll.gameObject);
        }
    }

    IEnumerator displayMovingUICollectDiamond(string text)
    {
        Canvas diamondDisplayCanvas = (Canvas)Instantiate(diamondDisplay, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        diamondDisplayCanvas.transform.SetParent(gameObject.transform);

        Text diamondDisplayText = diamondDisplayCanvas.GetComponentInChildren<Text>();
        diamondDisplayText.text = text;
        diamondDisplayText.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        for (int i = 0; i < TIMES_TO_MOVE_TEXT; i++)
        {
            yield return new WaitForSeconds(DELAY_MOVE_TEXT);
            diamondDisplayText.transform.position = diamondDisplayText.transform.position + new Vector3(0, Y_OFFSET_MOVE_TEXT, 0);
        }
        Destroy(diamondDisplayCanvas.gameObject);
    }

    private void triggerSpecialDiamondInteraction(Collider2D coll)
    {
        StartCoroutine(displayMovingUICollectDiamond(DISPLAY_SPECIAL_DIAMOND));
        // print("Player has collected a special diamond.");
        IncrementSpecialDiamondCountByOne();
        Assert.IsTrue(specialDiamonds <= NUM_SPECIAL_DIAMONDS_MAX);
        Destroy(coll.gameObject);
    }

    // a coroutine function must always return an IEnumerator
    // to run the coroutine function, use StartCoroutine(<coroutine_function>(args))
    IEnumerator triggerTreasureChestInteraction(Collision2D coll)
    {
        //coll.gameObject.SetActive(false);
        Destroy(coll.gameObject.GetComponent<BoxCollider2D>());
        coll.gameObject.GetComponent<Animator>().Play("Chest Open");
        // needed to make the script "pause" for a specified amount of time
        yield return new WaitForSeconds(1F);
        int option = UnityEngine.Random.Range(SPAWN_DIAMOND, SPAWN_MONSTER + 1); // interval is [min, max), so we add 1 to 'max'
        Assert.IsTrue(option == SPAWN_DIAMOND || option == SPAWN_MONSTER);
        if (ToSpawnDiamond(option))
        {
            spawnObject(spawnedDiamond, coll);
        }
        else if (ToSpawnMonster(option))
        {
            int randSpawn = Random.Range(0, 2);
            if (randSpawn == MONSTER_ONE)
            {
                spawnObject(spawnedMonsterType1, coll);
            }
            else if (randSpawn == MONSTER_TWO)
            {
                spawnObject(spawnedMonsterType2, coll);
            }
        }
        Destroy(coll.gameObject);
    }

    IEnumerator triggerSpecialTreasureChestInteraction(Collision2D coll)
    {
        //coll.gameObject.SetActive(false);
        Destroy(coll.gameObject.GetComponent<BoxCollider2D>());
        coll.gameObject.GetComponent<Animator>().Play("Chest Open");
        // needed to make the script "pause" for a specified amount of time
        yield return new WaitForSeconds(1F);
        spawnObject(spawnedSpecialDiamond, coll);
        Destroy(coll.gameObject);
    }

    private void triggerMonsterInteraction(Collision2D coll)
    {
        if (isHurt) return; // if player is already hurt, he/she is granted invincibility frames
        print("Player has touched a monster.");
        enforceInjury();
        repelPlayer(coll);
        anim.SetBool("isSusDig", false);
    }

    IEnumerator monsterDead(Collision2D coll)
    {
        GameObject toDie = coll.gameObject;
        Destroy(coll.gameObject.GetComponent<BoxCollider2D>());
        coll.gameObject.GetComponent<Animator>().Play("monster dead");
        // needed to make the script "pause" for a specified amount of time
        yield return new WaitForSeconds(0.7F);
        Destroy(toDie);
    }

    private void triggerTrapInteraction(Collision2D coll)
    {
        if (isHurt) return; // if player is already hurt, he/she is granted invincibility frames
        print("Player has touched a trap.");
        enforceInjury();
        repelPlayer(coll);
    }

    private void repelPlayer(Collision2D coll)
    {
        //return;
        rb2d.AddForce(new Vector2((transform.position.x - coll.gameObject.transform.position.x) * REPEL_PLAYER_FORCE,
            (transform.position.y - coll.gameObject.transform.position.y) * (REPEL_PLAYER_FORCE / 5)));
    }

    /* ======================================  PRIMITIVE METHODS ====================================== */

    private int getDiamondPenalty()
    {
        if (diamonds == 0) return 0;
        if (diamonds <= DIAMOND_RANGE_1)
        {
            return DIAMOND_LOSE_RANGE_1;
        }
        else if (diamonds <= DIAMOND_RANGE_2)
        {
            return (int)(DIAMOND_LOSE_RANGE_2 * diamonds);
        }
        else if (diamonds <= DIAMOND_RANGE_3)
        {
            return (int)(DIAMOND_LOSE_RANGE_3 * diamonds);
        }
        else if (diamonds <= DIAMOND_RANGE_4)
        {
            return (int)(DIAMOND_LOSE_RANGE_4 * diamonds);
        }
        else if (diamonds <= DIAMOND_RANGE_5)
        {
            return (int)(DIAMOND_LOSE_RANGE_5 * diamonds);
        }
        else
        {
            return (int)(DIAMOND_LOSE_MAX * diamonds);
        }
    }

    private void IncrementDiamondCountByOne()
    {
        diamonds++;
        updateDiamond();
    }

    private void IncrementDiamondCountByTen()
    {
        diamonds += 10;
        updateDiamond();
    }

    private void spawnObject(GameObject obj, Collision2D coll)
    {
        Instantiate(obj, new Vector3(coll.gameObject.transform.position.x, coll.gameObject.transform.position.y - 0.2f, 0), Quaternion.identity);
    }

    private static bool ToSpawnMonster(int option)
    {
        return option == SPAWN_MONSTER;
    }

    private static bool ToSpawnDiamond(int option)
    {
        return option == SPAWN_DIAMOND;
    }

    private int IncrementSpecialDiamondCountByOne()
    {
        specialDiamondImage.enabled = true;
        return specialDiamonds++;
    }

    private bool isMonsterHitFromTop(Collision2D coll)
    {
        /*
        print("PlayerX: " + rb2d.position.x + " | MonsterX: " + coll.collider.bounds.min.x + ","
                 + coll.collider.bounds.max.x);

        Use this printline code to do checking of desired acceptable position to constitute a hit from top
        */
        bool isHitFromTop = GetIsHitFromTop(coll);
        if (isHitFromTop) return true;
        return false;
    }

    private bool GetIsHitFromTop(Collision2D coll)
    {
        return (rb2d.position.x + PLAYER_X_OFFSET) > coll.collider.bounds.min.x &&
               (rb2d.position.x - PLAYER_X_OFFSET) < coll.collider.bounds.max.x;
    }

    private static bool IsDigButtonPressed()
    {
        return Input.GetKey(KeyCode.S);
    }

    private void enforceInjury()
    {
        StartCoroutine(Blink(5, 0.1f, 0.1f));
        updateHurt(true);
        lives -= 1;
        updateLives();
        checkIfPlayerDied();
    }

    IEnumerator Blink(int nTimes, float timeOn, float timeOff)
    {
        while (nTimes > 0)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(timeOn);
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(timeOff);
            nTimes--;
        }
        GetComponent<SpriteRenderer>().enabled = true;
        updateHurt(false);
    }

    private void updateHurt(bool isPlayerHurt)
    {
        isHurt = isPlayerHurt;
        if (isHurt)
        {
            GetComponent<PlayerBaseController>().updateRepelled();
        }
    }

    private void updateLives()
    {
        Image[] hearts = heartsCanvas.GetComponentsInChildren<Image>();
        Assert.IsTrue(hearts.Length > lives, ERROR_INVALID_LIVES_IMAGE);
        hearts[lives].enabled = false;
    }

    private void updateDiamond()
    {
        diamondText.text = diamonds.ToString();
    }

    private void checkIfPlayerDied()
    {
        Assert.IsTrue(lives >= 0, ERROR_INVALID_LIVES_VALUE);
        if (IsDead())
        {
            print("Player has [" + lives + "] remaining and has died.");
            showDeathScreen();
        }
        else if (IsAlive())
        {
            print("Player has lost 1 life with [" + lives + "] remaining.");
        }
    }

    private bool IsDead()
    {
        return lives == 0;
    }

    private bool IsAlive()
    {
        return lives > 0;
    }
}
