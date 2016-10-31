using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerCollisionController : MonoBehaviour
{
    // dialogue use
    public Text dialogue;
    public Button yesButton;
    public Button noButton;
    private const string LEVEL_1_DIALOGUE_FRONT = "Hiya buddy! I’m tired of mining for diamonds but I found a key to this cool" +
        " lookin’ diamond cave. Want in? Trade you ";
    private const string LEVEL_1_DIALOGUE_BACK = " diamonds for it!";
    private const string LEVEL_2_DIALOGUE_FRONT = "Greetings, Helmeted One. Let me relieve you of your burdens - ";
    private const string LEVEL_2_DIALOGUE_BACK = " diamonds, and your soul. Feel a little lighter? Go forth to retrieve your soul, if you wish. Otherwise, you may leave with what you have left… without your soul.";

    // diamond penalty use
    private const int DIAMOND_RANGE_1 = 25;
    private const int DIAMOND_LOSE_RANGE_1 = 1;

    private const int DIAMOND_RANGE_2 = 34;
    private const int DIAMOND_LOSE_RANGE_2 = 5;

    private const int DIAMOND_RANGE_3 = 45;
    private const int DIAMOND_LOSE_RANGE_3 = 10;

    private const int DIAMOND_RANGE_4 = 65;
    private const int DIAMOND_LOSE_RANGE_4 = 15;

    private const int DIAMOND_RANGE_5 = 90;
    private const int DIAMOND_LOSE_RANGE_5 = 20;

    private const int DIAMOND_LOSE_MAX = 25;

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
    private const string DISPLAY_FIVE_DIAMOND = "+5 diamonds";
    private const string DISPLAY_TEN_DIAMOND = "+10 diamonds";
    private const string DISPLAY_SPECIAL_DIAMOND = "Special Diamond Collected";

    private const string FIVE_DIAMOND_NAME = "Diamond5(Clone)";
    private const string TEN_DIAMOND_NAME = "Diamond10(Clone)";
    private const string PLATFORM = "Platform";
    private const string MONSTER = "Monster";
    private const string TRAP = "Trap";
    private const string DIAMOND = "Diamond";
    private const string SPECIAL_DIAMOND = "SpecialDiamond";
    private const string TREASURE_CHEST = "TreasureChest";
    private const string SPECIAL_TREASURE_CHEST = "SpecialTreasureChest";
    private const string BG_BOUNDARY = "BackgroundBoundary";
    private const string VENDING_MACHINE = "VendingMachine";
    private const string END_LEVEL = "EndLevel";
    private const string END_LEVEL_TWO = "EndLevel2";
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
    public int level;

    public AudioSource injuredSound;

    private Animator anim;

    private bool isLevelTwoEndingPlayed;
    private int penalty;

    // treasure chest asset attributes
    public GameObject spawnedDiamond;
    public GameObject spawnedSpecialDiamond;
    public GameObject spawnedMonsterType1;
    public GameObject spawnedMonsterType1_level2;
    public GameObject spawnedMonsterType1_level3;
    public GameObject spawnedMonsterType2;
    public GameObject spawnedMonsterType2_level2;
    public GameObject spawnedMonsterType2_level3;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lives = GlobalPlayerScript.Instance.lives;
        isHurt = false;
        diamonds = GlobalPlayerScript.Instance.diamonds;
        specialDiamonds = GlobalPlayerScript.Instance.specialDiamonds;
        anim = GetComponent<Animator>();
        level = GlobalPlayerScript.Instance.level;
        isLevelTwoEndingPlayed = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public bool getHurt()
    {
        return isHurt;
    }

    public void enforceDiamondPenalty()
    {
        int penalty = getDiamondPenalty();
        diamonds -= penalty;
    }

    // Handles collisions with diamonds or end level
    void OnTriggerEnter2D(Collider2D other)
    {
        Assert.IsTrue(other.gameObject.CompareTag(DIAMOND) || other.gameObject.CompareTag(SPECIAL_DIAMOND) ||
            other.gameObject.CompareTag(END_LEVEL) || other.gameObject.CompareTag(VENDING_MACHINE));

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
                      Equals(collidedObject, TRAP) || Equals(collidedObject, BG_BOUNDARY) || Equals(collidedObject, VENDING_MACHINE) ||
                      Equals(collidedObject, END_LEVEL) || Equals(collidedObject, END_LEVEL_TWO), ERROR_UNEXPECTED_COLLISION_EVENT);

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

            case END_LEVEL_TWO:
                if (!isLevelTwoEndingPlayed) triggerEndLevelTwo();
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

    private void triggerEndLevelTwo()
    {
        isLevelTwoEndingPlayed = true;
        GetComponent<PlayerBaseController>().setEndLevel(); // prevent movement by calling end level at base controller.
        GameObject depthMeter = GameObject.Find("Main Camera/PlayerGUICanvas/Panel/DepthMeter");
        depthMeter.SetActive(false); // hide depth meter
        StartCoroutine(spawnDevil());
    }

    private IEnumerator spawnDevil()
    {
        yield return new WaitForSeconds(1f);
        GameObject endLvlBtnController = GameObject.Find("EndLevelButtonController");
        endLvlBtnController.GetComponent<LevelTwoEndScript>().fadeInDevil(); // fade in devil.
    }

    // Called by LevelTwoEndScript when devil finishes fading in.
    public void informDevilFadedIn()
    {
        penalty = getDiamondPenalty();
        enforceDiamondPenalty();
        dialogue.text = LEVEL_2_DIALOGUE_FRONT + penalty + LEVEL_2_DIALOGUE_BACK;
    }

    // Called by LevelTwoEndScript when devil steals diamond and soul.
    public void informDiamondAndSoulStolen() {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    private void triggerDiamondInteraction(Collider2D coll)
    {
        if (Equals(coll.gameObject.name, TEN_DIAMOND_NAME))
        {
            StartCoroutine(displayMovingUICollectDiamond(DISPLAY_TEN_DIAMOND));
            IncrementDiamondCount(10);
            Destroy(coll.gameObject);
        } else if (Equals(coll.gameObject.name, FIVE_DIAMOND_NAME))
        {
            StartCoroutine(displayMovingUICollectDiamond(DISPLAY_FIVE_DIAMOND));
            IncrementDiamondCount(5);
            Destroy(coll.gameObject);
        }
        else
        {
            StartCoroutine(displayMovingUICollectDiamond(DISPLAY_ONE_DIAMOND));
            IncrementDiamondCount(1);
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
                if (level == 1)
                {
                    spawnObject(spawnedMonsterType1, coll);
                }
                else if (level == 2)
                {
                    spawnObject(spawnedMonsterType1_level2, coll);
                }
                else if (level == 3)
                {
                    spawnObject(spawnedMonsterType1_level3, coll);
                }
                else if (SceneManager.GetActiveScene().name == "TutorialLevel")
                {
                    spawnObject(spawnedMonsterType1, coll);
                }
                
            }
            else if (randSpawn == MONSTER_TWO)
            {
                if (level == 1)
                {
                    spawnObject(spawnedMonsterType2, coll);
                }
                else if (level == 2)
                {
                    spawnObject(spawnedMonsterType2_level2, coll);
                }
                else if (level == 3)
                {
                    spawnObject(spawnedMonsterType2_level3, coll);
                }
                else if (SceneManager.GetActiveScene().name == "TutorialLevel")
                {
                    spawnObject(spawnedMonsterType2, coll);
                }
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

    public int getDiamondPenalty()
    {
        if (diamonds == 0) return 0;
        if (diamonds <= DIAMOND_RANGE_1)
        {
            return DIAMOND_LOSE_RANGE_1;
        }
        else if (diamonds <= DIAMOND_RANGE_2)
        {
            return DIAMOND_LOSE_RANGE_2;
        }
        else if (diamonds <= DIAMOND_RANGE_3)
        {
            return DIAMOND_LOSE_RANGE_3;
        }
        else if (diamonds <= DIAMOND_RANGE_4)
        {
            return DIAMOND_LOSE_RANGE_4;
        }
        else if (diamonds <= DIAMOND_RANGE_5)
        {
            return DIAMOND_LOSE_RANGE_5;
        }
        else
        {
            return DIAMOND_LOSE_MAX;
        }
    }

    private void IncrementDiamondCount(int numToAdd)
    {
        diamonds += numToAdd;
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

    public void enforceInjury()
    {
        injuredSound.Play();
        StartCoroutine(Blink(5, 0.1f, 0.1f));
        updateHurt(true);
        lives -= 1;
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

    private void checkIfPlayerDied()
    {
        Assert.IsTrue(lives >= 0 || level == 0, ERROR_INVALID_LIVES_VALUE);
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
