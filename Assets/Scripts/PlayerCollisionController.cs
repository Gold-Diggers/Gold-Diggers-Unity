﻿using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCollisionController : MonoBehaviour {
    // player constants for handling all types of collisions
    private const int NUM_LIVES_START = 300;
    private const int INVINCIBILITY_FRAME = 100;
    private const float PLAYER_X_OFFSET = 0.10f;
    private const int NUM_DIAMONDS_START = 0;
    private const int NUM_SPECIAL_DIAMONDS_START = 0;
    private const int NUM_SPECIAL_DIAMONDS_MAX = 3;

    // treasure chest constants
    private const int SPAWN_DIAMOND = 0;
    private const int SPAWN_MONSTER = 1;

    // strings
    private const string PLATFORM = "Platform";
    private const string MONSTER = "Monster";
    private const string TRAP = "Trap";
    private const string DIAMOND = "Diamond";
    private const string SPECIAL_DIAMOND = "SpecialDiamond";
    private const string TREASURE_CHEST = "TreasureChest";
    private const string SPECIAL_TREASURE_CHEST = "SpecialTreasureChest";
    private const string BG_BOUNDARY = "BackgroundBoundary";
    private const string ERROR_INVALID_LIVES_VALUE = "ERROR: 'lives' attribute cannot be < 0.";
    private const string ERROR_INVALID_INVINCIBILITY_VALUE = "ERROR: 'invincibility' attribute cannot be < 0.";
    private const string ERROR_INVALID_SPECIAL_DIAMOND_VALUE = "ERROR: 'specialDiamonds' attribute cannot be > 3.";
    private const string ERROR_INVALID_RANDOM_VALUE = "ERROR: Random integer for treasure chest is not between [0, 1].";
    private const string ERROR_UNEXPECTED_COLLISION_EVENT = "ERROR: Unexpected collision event occurred.\n\n"
                                                            + "Collider is not 'Platform', 'BackgroundBoundary', "
                                                            + "'Diamond', 'TreasureChest', 'SpecialTreasureChest', "
                                                            + "'Monster' or 'Trap'.";

    // player attributes
    private bool isHurt;
    private int invincibility;
    private Rigidbody2D rb2d;

    public int lives;
    public int diamonds;
    public int specialDiamonds;

    // treasure chest asset attributes
    public GameObject spawnedDiamond;
    public GameObject spawnedSpecialDiamond;
    public GameObject spawnedMonster;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        lives = NUM_LIVES_START;
        isHurt = false;
        invincibility = 0;
        diamonds = NUM_DIAMONDS_START;
        specialDiamonds = NUM_SPECIAL_DIAMONDS_START;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        updateInvincibility();
    }

    // Handles collisions with diamonds
    void OnTriggerEnter2D(Collider2D other)
    {
        Assert.IsTrue(other.gameObject.CompareTag(DIAMOND) || other.gameObject.CompareTag(SPECIAL_DIAMOND));
        if (other.gameObject.CompareTag(DIAMOND))
        {
            triggerDiamondInteraction(other);
        }
        else if (other.gameObject.CompareTag(SPECIAL_DIAMOND))
        {
            triggerSpecialDiamondInteraction(other);
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
                      Equals(collidedObject, TRAP) || Equals(collidedObject, BG_BOUNDARY), ERROR_UNEXPECTED_COLLISION_EVENT);

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
                triggerTrapInteraction();
                break;

            default:
                break;
        }
    }

    void restartLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void updateInvincibility()
    {
        Assert.IsTrue(invincibility >= 0, ERROR_INVALID_INVINCIBILITY_VALUE);
        if (IsInvincible()) DecrementInvincibility();
        else if (IsVulnerable()) isHurt = false;
    }

    private void triggerDiamondInteraction(Collider2D coll)
    {
        print("Player has collected a diamond.");
        IncrementDiamondCountByOne();
        Destroy(coll.gameObject);
    }

    private void triggerSpecialDiamondInteraction(Collider2D coll)
    {
        print("Player has collected a special diamond.");
        IncrementSpecialDiamondCountByOne();
        Assert.IsTrue(specialDiamonds <= NUM_SPECIAL_DIAMONDS_MAX);
        Destroy(coll.gameObject);
    }

    // a coroutine function must always return an IEnumerator
    // to run the coroutine function, use StartCoroutine(<coroutine_function>(args))
    IEnumerator triggerTreasureChestInteraction(Collision2D coll)
    {
        coll.gameObject.SetActive(false);
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
            spawnObject(spawnedMonster, coll);
        }
        Destroy(coll.gameObject);
    }

    IEnumerator triggerSpecialTreasureChestInteraction(Collision2D coll)
    {
        coll.gameObject.SetActive(false);
        // needed to make the script "pause" for a specified amount of time
        yield return new WaitForSeconds(1F);
        spawnObject(spawnedSpecialDiamond, coll);
        Destroy(coll.gameObject);
    }

    private void triggerMonsterInteraction(Collision2D coll)
    {
        // Uncomment this print statement for fine-tuning the collision mechanism of player with monster.
        //print(isMonsterHitFromTop(coll));
        if (IsDigButtonPressed()) // if the player held the 'dig' button when coliding with monster
        {
            if (isMonsterHitFromTop(coll))
            {
                print("Player has killed the monster.");
                Destroy(coll.gameObject);
            }
            else
            {
                if (isHurt) return; // if player is already hurt, he/she is granted invincibility frames
                print("Player has touched a monster.");
                enforceInjury();
            }
        }
        else
        {
            if (isHurt) return; // if player is already hurt, he/she is granted invincibility frames
            print("Player has touched a monster.");
            enforceInjury();
        }
    }

    private void triggerTrapInteraction()
    {
        if (isHurt) return; // if player is already hurt, he/she is granted invincibility frames
        print("Player has touched a trap.");
        enforceInjury();
    }

    /* ======================================  PRIMITIVE METHODS ====================================== */
    private bool IsVulnerable()
    {
        return invincibility == 0;
    }

    private bool IsInvincible()
    {
        return invincibility > 0;
    }

    private int DecrementInvincibility()
    {
        return invincibility--;
    }

    private void IncrementDiamondCountByOne()
    {
        diamonds++;
    }

    private void spawnObject(GameObject obj, Collision2D coll)
    {
        Instantiate(obj, new Vector3(coll.gameObject.transform.position.x, coll.gameObject.transform.position.y, 0), Quaternion.identity);
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

    private void enforceInjury()
    {
        isHurt = true;
        lives -= 1;
        checkIfPlayerDied();
    }

    private void checkIfPlayerDied()
    {
        Assert.IsTrue(lives >= 0, ERROR_INVALID_LIVES_VALUE);
        if (IsDead())
        {
            print("Player has [" + lives + "] remaining and has died.");
            restartLevel();
        }
        else if (IsAlive())
        {
            print("Player has lost 1 life with [" + lives + "] remaining.");
            invincibility = INVINCIBILITY_FRAME;
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
