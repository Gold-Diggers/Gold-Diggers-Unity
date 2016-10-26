using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerBaseController : MonoBehaviour {

    /* --------------------------------- START PLAYER DEFINITIONS --------------------------------- */
    /* ================= player mechanics calculations ================= */
    private const double Y_VELOCITY_THRESHOLD = -2.0;
    private const float HORIZONTAL_COLLISION_THRESHOLD_ENEMIES = 0.1f;
    private const float HORIZONTAL_COLLISION_THRESHOLD_PLATFORM = 0.3f;
    private const float JUMP_FORCE = 67.5f;
    private const float HOVER_FORCE = 1.75f;
    private const float HOVER_JETPACK_FORCE = 3.5f;
    private const float CHAR_ON_PLATFORM_Y_DIFF_THRESHOLD = 0.001f;

    /* ================= Player physics attributes ================= */
    public float speed;
    public float jumpHeight;
    private Rigidbody2D rb2d;
    private float yPos;
    private float xPos;

    public bool isLevelEnd = false;

    /* ================= Player upgrades ============================*/
    public bool hasJetpackUpgrade;
    public bool hasShovelUpgrade;

    /* ================= Player control mechanic attributes ================= */
    // For handling period when player is digging
    private int diggingCounter;
    private int diggingCooldown;
    private const int COOLDOWN_AFTER_SUSTAINED_DIG = 100; // in frames
    private const int MAX_TIME_SUSTAINED_DIG = 50; // in frames
    private const float DIG_X_OFFSET = 0.27f;
    private const float DIG_Y_OFFSET_TOP = 0.95f;
    private const float DIG_Y_OFFSET_BTM = 0.75f;

    // For side attack
    private const float SIDEATTACK_X_OFFSET_LEFT = 0f;
    private const float SIDEATTACK_X_OFFSET_RIGHT = 1f; // 0.75
    private const float SIDEATTACK_Y_OFFSET_TOP = 0.25f;
    private const float SIDEATTACK_Y_OFFSET_BTM = 0.25f;
    private const float AMT_PLAYER_TRANSLATE_WHEN_SATT = 0.2f;
    private bool isSideAttack;

    // For jumping
    private int jumpCooldown;
    private const int COOLDOWN_JUMP = 10; // in frames
    private const float JUMP_ATK_X_OFFSET = 0.3f;
    private const float JUMP_ATK_Y_OFFSET_TOP = 0.1f;
    private const float JUMP_ATK_Y_OFFSET_BTM = 0.75f;

    /* ================= Player animations ================= */
    private bool isRunning;
    private Animator anim;
    private bool toFlip;
    private SpriteRenderer playerSpriteRend;
    private const float X_DIFF_ANIMATE_THRESHOLD = 0.00005f;

    private bool isRepelled;
    private const float AMT_COLLIDER_TRANSLATE_WHEN_FLIPPING = 2.7f;

    // Boolean constant to change for testing/production purposes
    private const bool IS_TESTING = true;

    public void setEndLevel()
    {
        isLevelEnd = true;
        anim.Play("idle");
        anim.SetBool("isRunning", false);
    }

    // Used by collision controller to update whether player is hurt.
    public void updateRepelled()
    {
        StartCoroutine("disableMove");
    }

    // a coroutine function must always return an IEnumerator
    // to run the coroutine function, use StartCoroutine(<coroutine_function>(args))
    IEnumerator disableMove()
    {
        isRepelled = true;
        // needed to make the script "pause" for a specified amount of time
        yield return new WaitForSeconds(0.5F);
        isRepelled = false;
    }

    /* ================= Use this for initialization ================= */
    void Start()
    {
        isRepelled = false;
        Assert.raiseExceptions = IS_TESTING;
        rb2d = GetComponent<Rigidbody2D>();
        yPos = transform.position.y;
        xPos = transform.position.x;
        diggingCounter = 0;
        diggingCooldown = 0;
        jumpCooldown = 0;
        isRunning = false;
        anim = GetComponent<Animator>();
        toFlip = false;
        playerSpriteRend = GetComponent<SpriteRenderer>();
        isSideAttack = false;

        hasJetpackUpgrade = GlobalPlayerScript.Instance.hasJetpackUpgrade;
        hasShovelUpgrade = GlobalPlayerScript.Instance.hasShovelUpgrade;
}

    /* --------------------------------- START PLAYER CONTROL FUNCTIONS --------------------------------- */
    void updatePos()
    {
        yPos = transform.position.y;
        xPos = transform.position.x;
    }

    void FixedUpdate()
    {
        if (!isLevelEnd)
        {
            handleHorizontalMovement();
            handleJump();
            handleDig();
            handleSideAttack();
            handleAnimation();
        }
        updatePos(); // must be last
    }

    void handleAnimation() {
    	if (Math.Abs(transform.position.x - xPos) >= X_DIFF_ANIMATE_THRESHOLD) {
    		if (isRunning) {

			} else {
				//print ("run");
				isRunning = true;
				anim.SetBool("isRunning", isRunning);
				//GetComponent<Animator>().StartPlayback();
			}
    	} else {
    		if (isRunning) {
    			isRunning = false;
    			//print("idle");
    			//anim.Play("idle");
    			anim.SetBool("isRunning", isRunning);
    			//GetComponent<Animator>().StartPlayback();
			} else {

			}
    	}

        // Handle falling
        if (!isDiggingAnim() && !isHoverAnim())
        {
            if (isCharacterFalling())
            {
                anim.SetBool("isFalling", true);
                anim.Play("Falling");
            }
            else
            {
                anim.SetBool("isFalling", false);
            }
        }
       
    }

    void handleSideAttack()
    {
        // only allow side attack if shovel upgrade is purchased
        if (hasShovelUpgrade)
        {
            if (Input.GetKey(KeyCode.O) && isCharacterOnPlatform())
            {
                if (!isSideAttackAnim())
                {
                    anim.Play("sideattack");
                   // Vector3 currPos = transform.position;
                    //currPos.x += AMT_PLAYER_TRANSLATE_WHEN_SATT;
                    //transform.position = currPos;
                    isSideAttack = true;
                    return;
                }
            }

            if (isSideAttackHappening())
            {
                sideAttack();
            } else if (isSideAttack && !isSideAttackAnim())
            {
                isSideAttack = false;
               // Vector3 currPos = transform.position;
               // currPos.x -= AMT_PLAYER_TRANSLATE_WHEN_SATT;
               // transform.position = currPos;
            }
        }
    }

    void sideAttack()
    {
        float currX = transform.GetComponent<Collider2D>().bounds.center.x;
        float currY = transform.GetComponent<Collider2D>().bounds.center.y;
        Vector2 ptA = new Vector2((float)(currX + SIDEATTACK_X_OFFSET_LEFT), (float)(currY + SIDEATTACK_Y_OFFSET_TOP));

        float xOfPtB;
        if (GetComponent<SpriteRenderer>().flipX == true)
        { // If facing left side.
            xOfPtB = currX - SIDEATTACK_X_OFFSET_RIGHT;
        } else
        { // If facing right side.
            xOfPtB = currX + SIDEATTACK_X_OFFSET_RIGHT;
        }
        Vector2 ptB = new Vector2(xOfPtB, currY - SIDEATTACK_Y_OFFSET_BTM);
        /*Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 8);

        foreach (Collider2D current in col)
        {
            if (current.transform.tag == "Platform")
            {
                // handleFallingObjects(current.gameObject);
                Destroy(current.gameObject);
            }
        }*/

        // Destroy monsters
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 10); // monsters layer

        if (col.Length != 0)
        {
            foreach (Collider2D current in col)
            {
                if (current.transform.tag == "Monster")
                {
                    current.gameObject.GetComponent<MonsterBehaviour>().kill();
                }
            }
        }
    }

    void handleFallingObjects(GameObject platformDestroyed)
    {
        float currX = platformDestroyed.transform.GetComponent<Collider2D>().bounds.center.x;
        float currY = platformDestroyed.transform.GetComponent<Collider2D>().bounds.center.y;

        Vector2 ptA = new Vector2(currX, currY + 0.7f);
        Vector2 ptB = new Vector2(currX, currY + 0.2f);
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 10 | 1 << 11); // monsters and traps

        foreach (Collider2D current in col)
        {
            if (current.transform.tag == "Trap")
            {
                makeObjectFall(current.gameObject);
            } else if (current.transform.tag == "Monster")
            {
                current.gameObject.GetComponent<MonsterBehaviour>().kill();
            }
        }

    }

    void makeObjectFall(GameObject fallingObj)
    {
        StartCoroutine(fall(fallingObj));
    }

    IEnumerator fall(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        
        Vector2 ptA = new Vector2(pos.x, pos.y);
        Vector2 ptB = new Vector2(pos.x, pos.y - 0.655f);
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 8);

        if (col.Length != 0)
        {
            yield return new WaitForSeconds(0.1f);
        } else
        {
            pos.y -= 0.1f;
            obj.transform.position = pos;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(fall(obj));
        }

    }

    void handleJump()
    {
        if (jumpCooldown > 0)
        {
            jumpCooldown--;
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.O)) && !isRepelled)
        {
            if ((Input.GetKey(KeyCode.W) && isCharacterOnPlatform())) // normal jump
            {
                if (jumpCooldown <= 0 && rb2d.velocity.y < 0.02)
                {                
                    jumpCooldown = COOLDOWN_JUMP;
                    StartCoroutine(jumpAnimate());
                    rb2d.AddForce(new Vector2(0, JUMP_FORCE) * jumpHeight);
                }   
            } else if ((Input.GetKey(KeyCode.O) && isCharacterFalling())) // hover
            {
                if (!isHoverAnim())
                {    
                    anim.SetBool("isHover", true);
                    if (hasJetpackUpgrade)
                    {
                        anim.Play("jetpack");
                    } else
                    {
                        anim.Play("hover");
                    }
                    
                }
                
                if (rb2d.velocity.y < Y_VELOCITY_THRESHOLD)
                {
                    if (hasJetpackUpgrade)
                    {
                        rb2d.AddForce(new Vector2(0, HOVER_JETPACK_FORCE) * jumpHeight);
                    } else
                    {
                        rb2d.AddForce(new Vector2(0, HOVER_FORCE) * jumpHeight);
                    }
                    
                }
            } else
            {
                anim.SetBool("isHover", false);
            }
        } else
        {
            anim.SetBool("isHover", false);
        }
    }

    private bool isSideAttackHappening()
    {
        AnimatorStateInfo currState = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        return (currState.IsName("sideattack"));
    }

    private bool isSideAttackAnim()
    {
        AnimatorStateInfo currState = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        return (currState.IsName("sideattack") || currState.IsName("sideattackfinish"));
    }

    private bool isHoverAnim()
    {
        AnimatorStateInfo currState = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        return (currState.IsName("hover") || currState.IsName("jetpack"));
    }

    IEnumerator jumpAnimate()
    {
        anim.SetBool("isJumping", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("isJumping", false);
    }

    private void digPlatform()
    {
        float currX = transform.GetComponent<Collider2D>().bounds.center.x;
        float currY = transform.GetComponent<Collider2D>().bounds.center.y;
        Vector2 ptA = new Vector2((float)(currX - DIG_X_OFFSET), (float)(currY - DIG_Y_OFFSET_TOP));
        Vector2 ptB = new Vector2((float)(currX + DIG_X_OFFSET), currY - DIG_Y_OFFSET_BTM);
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 8);

        foreach (Collider2D current in col)
        {
            if (current.transform.tag == "Platform")
            {
                Destroy(current.gameObject);
            }
        }    
    }

    // Dig might dig 2 blocks instead of 1 in order to allow player to fall through.
    void handleDig()
    {
        // Check if dig is in cooldown.
        if (diggingCooldown > 0)
        {
            diggingCooldown--;
            return;
        }

        if (isHoverAnim())
        {
            return;
        }

        // player digs.
        if (Input.GetKey(KeyCode.S))
        {
            if (isDiggingAnim())
            { // sustained digging.
                diggingCounter--;
                if (diggingCounter < 0)
                { // sustained digging for too long will trigger a cooldown.
                    diggingCooldown = COOLDOWN_AFTER_SUSTAINED_DIG;
                    anim.SetBool("isSusDig", false);
                } else
                {
                    digPlatform();
                    jumpAttack();
                }
                return;
            } else
            { // start digging
                diggingCounter = MAX_TIME_SUSTAINED_DIG;
                anim.SetBool("isSusDig", true);
                if (isCharacterOnPlatform() && !isDiggingAnim())
                { // character on platform digging.
                    GetComponent<Animator>().Play("dig");
                    digPlatform();
                    jumpAttack();
                }
                else
                { // character is in flight
                    if (!isDiggingAnim())
                    {
                        GetComponent<Animator>().Play("fly dig");
                        jumpAttack();
                    }
                }
            }
        } else
        { // player stop pressing dig
            anim.SetBool("isSusDig", false);
            if (isDiggingAnim()) // if dig animation still running even if player stop pressing dig
            {
                digPlatform();
                jumpAttack();
            }
        }
    }

    private void jumpAttack()
    {
        float currX = transform.GetComponent<Collider2D>().bounds.center.x;
        float currY = transform.GetComponent<Collider2D>().bounds.center.y;
        Vector2 ptA = new Vector2((float)(currX - JUMP_ATK_X_OFFSET), (float)(currY - JUMP_ATK_Y_OFFSET_TOP));
        Vector2 ptB = new Vector2((float)(currX + JUMP_ATK_X_OFFSET), currY - JUMP_ATK_Y_OFFSET_BTM);
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 10); // monsters layer

        if (col.Length != 0)
        {
            rb2d.velocity = Vector3.zero; // reset forces before trigger jump
            rb2d.AddForce(new Vector2(0, JUMP_FORCE) * jumpHeight); // jump when hit monster
            anim.SetBool("isSusDig", false); // cancel dig
            foreach (Collider2D current in col)
            {
                if (current.transform.tag == "Monster")
                {
                    current.gameObject.GetComponent<MonsterBehaviour>().kill();
                }
            }
        }
    }

    bool isCharacterOnPlatform()
    {
        /*float currX = transform.GetComponent<Collider2D>().bounds.center.x;
        float currY = transform.GetComponent<Collider2D>().bounds.center.y;
        Vector2 ptA = new Vector2((float)(currX - DIG_X_OFFSET), (float)(currY - 0.7f));
        Vector2 ptB = new Vector2((float)(currX + DIG_X_OFFSET), currY - 0.7f);
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 8);

        return col.Length > 0;*/

        if (Math.Abs(transform.position.y - yPos) < CHAR_ON_PLATFORM_Y_DIFF_THRESHOLD)
        {
            return true;
        } else
        {
            return false;
        }
    }

    bool isCharacterFalling()
    {
        if (transform.position.y < yPos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void handleHorizontalMovement()
    {
        // The commented out code is buggy - might get stuck
        /*float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2d.AddForce(movement * speed);*/

        //if (!isDigging)
        if (true)
        {
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) // when pressing left and right keys together
            {
                return;
            }

            // Restricts movement if moving left or right will collide into wall/platform blocks, or when repelling.
            if (Input.GetKey(KeyCode.D) && !isRepelled) 
            {
                flipPlayer(false);
                Vector3 movement = Vector3.right * speed * Time.deltaTime;
                //if (!isColliding(transform.position + movement))
                Vector3 currPos = transform.GetComponent<Collider2D>().bounds.center;
                if (!isColliding(currPos + movement))
                {
                    transform.position += movement;
                }
            }
            else if (Input.GetKey(KeyCode.A) && !isRepelled)
            {
                flipPlayer(true);
                Vector3 movement = Vector3.left * speed * Time.deltaTime;
                //if (!isColliding(transform.position + movement))
                Vector3 currPos = transform.GetComponent<Collider2D>().bounds.center;
                if (!isColliding(currPos + movement))
                {
                    transform.position += movement;
                }
            }
        } 
    }

    void flipPlayer(bool expected)
    {
        if (toFlip != expected)
        {
            toFlip = !toFlip;
            playerSpriteRend.flipX = toFlip;
            if (!playerSpriteRend.flipX)
            {
                Vector3 collPos = GetComponent<BoxCollider2D>().offset;
                GetComponent<BoxCollider2D>().offset = new Vector3(collPos.x - AMT_COLLIDER_TRANSLATE_WHEN_FLIPPING, collPos.y, collPos.z);
            }
            else
            {
                Vector3 collPos = GetComponent<BoxCollider2D>().offset;
                GetComponent<BoxCollider2D>().offset = new Vector3(collPos.x + AMT_COLLIDER_TRANSLATE_WHEN_FLIPPING, collPos.y, collPos.z);
            }
        }
    }

    private bool isDiggingAnim()
    {
        AnimatorStateInfo currState = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        return (currState.IsName("dig") || currState.IsName("fly dig") || currState.IsName("sustained dig"));
    }

    bool isColliding(Vector3 point)
    {
        float currX = point.x;
        float currY = point.y;
        float thresholdPlatform = HORIZONTAL_COLLISION_THRESHOLD_PLATFORM;
        Vector2 ptA = new Vector2((float) (currX - thresholdPlatform), (float)(currY + thresholdPlatform));
        Vector2 ptB = new Vector2((float)(currX + thresholdPlatform), (float)(currY - thresholdPlatform * 2));
        Collider2D[] colPlatform = Physics2D.OverlapAreaAll(ptA, ptB, (1 << 8));

        // monsters and traps
        float thresholdEnemies = HORIZONTAL_COLLISION_THRESHOLD_ENEMIES;
        Vector2 ptC = new Vector2((float)(currX - thresholdEnemies), (float)(currY + thresholdEnemies));
        Vector2 ptD = new Vector2((float)(currX + thresholdEnemies), (float)(currY - thresholdEnemies * 2));
        Collider2D[] colEnemies = Physics2D.OverlapAreaAll(ptC, ptD, ((1 << 10) | (1 << 11)));
        if (colPlatform.Length + colEnemies.Length == 0)
        {
            return false;
        } else
        {
            return true;
        }
    }
    /* --------------------------------- END PLAYER CONTROL FUNCTIONS --------------------------------- */
}
