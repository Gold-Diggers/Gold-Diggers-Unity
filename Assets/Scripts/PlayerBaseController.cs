using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;

public class PlayerBaseController : MonoBehaviour {

    /* --------------------------------- START PLAYER DEFINITIONS --------------------------------- */
    /* ================= player mechanics calculations ================= */
    private const double Y_VELOCITY_THRESHOLD = -2.0;
    private const float HORIZONTAL_COLLISION_THRESHOLD_ENEMIES = 0.1f;
    private const float HORIZONTAL_COLLISION_THRESHOLD_PLATFORM = 0.3f;
    private const float JUMP_FORCE = 100f;
    private const float HOVER_FORCE = 3.5f;
    private const float CHAR_ON_PLATFORM_Y_DIFF_THRESHOLD = 0.001f;

    /* ================= Player physics attributes ================= */
    public float speed;
    public float jumpHeight;
    private Rigidbody2D rb2d;
    private float yPos;
    private float xPos;

    /* ================= Player control mechanic attributes ================= */
    // For handling period when player is digging
    private bool isDigging;
    private int diggingCounter;
    private const float DIG_X_OFFSET = 0.27f;
    private const float DIG_Y_OFFSET_TOP = 0.95f;
    private const float DIG_Y_OFFSET_BTM = 1f;

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
        isDigging = false;
        diggingCounter = 0;
        isRunning = false;
        anim = GetComponent<Animator>();
        toFlip = false;
        playerSpriteRend = GetComponent<SpriteRenderer>();
    }

    /* --------------------------------- START PLAYER CONTROL FUNCTIONS --------------------------------- */
    void updatePos()
    {
        yPos = transform.position.y;
        xPos = transform.position.x;
    }

    void FixedUpdate()
    {
        handleHorizontalMovement();
        handleJump();
        handleDig();
        handleDigCooldown();
        handleAnimation();
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
    }

    void handleJump()
    {
        if (Input.GetKey(KeyCode.W) && !isRepelled)
        {
            if (isCharacterOnPlatform()) // normal jump
            {
                rb2d.AddForce(new Vector2(0, JUMP_FORCE) * jumpHeight);
            } else if (isCharacterFalling()) // hover
            {
                if (rb2d.velocity.y < Y_VELOCITY_THRESHOLD)
                {
                    rb2d.AddForce(new Vector2(0, HOVER_FORCE) * jumpHeight);
                }
            }
        }
    }

    private void handleDigCooldown()
    {
        if (isDigging)
        {
            diggingCounter--;
            if (diggingCounter == 0)
            {
                isDigging = false;
            }
        }
    }

    private void updateDigging()
    {
        if (!isDigging)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("fly dig"))
            {
                // Play dig animation if not jump attacking.
                GetComponent<Animator>().Play("dig");
            }
        }
        updateDigParams();
    }

    private void updateDigParams()
    {
        isDigging = true;
        diggingCounter = 5;
    }

    // Dig might dig 2 blocks instead of 1 in order to allow player to fall through.
    void handleDig()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (isCharacterOnPlatform())
            {
                updateDigging();
                float currX = transform.GetComponent<Collider2D>().bounds.center.x;
                float currY = transform.GetComponent<Collider2D>().bounds.center.y;
                Vector2 ptA = new Vector2((float)(currX - DIG_X_OFFSET), (float)(currY - DIG_Y_OFFSET_TOP));
                Vector2 ptB = new Vector2((float)(currX + DIG_X_OFFSET), currY - DIG_Y_OFFSET_BTM);
                Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1<<8);
                
                foreach (Collider2D current in col)
                {
                    if (current.transform.tag == "Platform")
                    {
                        Destroy(current.gameObject);
                    }
                }
            } else
            { // character is in flight
                if (!isDigging)
                {
                    if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("dig"))
                    {
                        // Play jump attack animation if not digging normally.
                        GetComponent<Animator>().Play("fly dig");
                    }
                }
            }
        }
    }

    bool isCharacterOnPlatform()
    {
        //if (transform.position.y == yPos)
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

        if (!isDigging)
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
