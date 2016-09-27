using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerBaseController : MonoBehaviour {

    /* --------------------------------- START PLAYER DEFINITIONS --------------------------------- */
    /* ================= Important constant definitions ================= */
    // player mechanics calculations
    private const double Y_VELOCITY_THRESHOLD = -2.0;
    private const float HORIZONTAL_COLLISION_THRESHOLD = 0.3f;
    private const float PLAYER_X_OFFSET = 0.10f;
    private const float MONSTER_COLLISION_TOLERANCE = 0.5f;

    // player game attributes default values
    private const int NUM_LIVES_START = 300;
    private const int NUM_DIAMONDS_START = 0;
    private const int NUM_SPECIAL_DIAMONDS_START = 0;
    private const int NUM_SPECIAL_DIAMONDS_MAX = 3;
    private const int INVINCIBILITY_FRAME = 100;

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

    private bool isRunning;
    private Animator anim;

    /* ================= Player in-game rule mechanic attributes ================= */
    public int lives;
    public int diamonds;
    public int specialDiamonds;
    private bool isHurt;
    private int invincibility;

    /* --------------------------------- END PLAYER DEFINITIONS --------------------------------- */

    /* --------------------------------- START ERROR MESSAGES --------------------------------- */
    private const string ERROR_INVALID_LIVES_VALUE = "ERROR: 'lives' attribute cannot be < 0.";
    private const string ERROR_INVALID_INVINCIBILITY_VALUE = "ERROR: 'invincibility' attribute cannot be < 0.";
    private const string ERROR_INVALID_SPECIAL_DIAMOND_VALUE = "ERROR: 'specialDiamonds' attribute cannot be > 3.";
    private const string ERROR_UNEXPECTED_COLLISION_EVENT = "ERROR: Unexpected collision event occurred.\n\n"
                                                            + "Collider is not 'Platform', 'BackgroundBoundary', "
                                                            + "'Diamond', 'TreasureChest', 'SpecialTreasureChest', "
                                                            + "'Monster' or 'Trap'.";
    /* --------------------------------- END ERROR MESSAGES --------------------------------- */

    /* ================= Use this for initialization ================= */
    void Start()
    {
        lives = NUM_LIVES_START;
        diamonds = NUM_DIAMONDS_START;
        specialDiamonds = NUM_SPECIAL_DIAMONDS_START;
        rb2d = GetComponent<Rigidbody2D>();
        yPos = transform.position.y;
        xPos = transform.position.x;
        isDigging = false;
        isHurt = false;
        diggingCounter = 0;
        invincibility = 0;
        isRunning = false;
        anim = GetComponent<Animator>();
    }

    /* ================= Use this for restarting the level ================= */
    void restartLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    /* --------------------------------- START PLAYER CONTROL FUNCTIONS --------------------------------- */
    void updatePos()
    {
        yPos = transform.position.y;
        xPos = transform.position.x;
    }

    void FixedUpdate()
    {
        updateInvincibility();
        handleHorizontalMovement();
        handleJump();
        handleDig();
        handleDigCooldown();
        handleAnimation();
        updatePos(); // must be last
    }

    void handleAnimation() {
    	if (Math.Abs(transform.position.x - xPos) >= 0.00005) {
    		if (isRunning) {

			} else {
				print ("run");
				isRunning = true;
				anim.SetBool("isRunning", isRunning);
				//GetComponent<Animator>().StartPlayback();
			}
    	} else {
    		if (isRunning) {
    			isRunning = false;
    			print("idle");
    			//anim.Play("idle");
    			anim.SetBool("isRunning", isRunning);
    			//GetComponent<Animator>().StartPlayback();
			} else {

			}
    	}
    }

    void handleJump()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (isCharacterOnPlatform()) // normal jump
            {
                rb2d.AddForce(new Vector2(0, 100) * jumpHeight);
            } else if (isCharacterFalling()) // hover
            {
                if (rb2d.velocity.y < Y_VELOCITY_THRESHOLD)
                {
                    rb2d.AddForce(new Vector2(0, 5) * jumpHeight);
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
                Vector2 ptA = new Vector2((float)(currX - 0.27), (float)(currY - 0.95));
                Vector2 ptB = new Vector2((float)(currX + 0.27), currY - 1);
                Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1<<8);
                /*if (col.Length == 1)
                {
                    print("1");
                    Destroy(col[0].gameObject);
                } else if (col.Length == 2)
                {
                    print("2");
                    float distA = Math.Abs(col[0].transform.position.x - currX);
                    float distB = Math.Abs(col[1].transform.position.x - currX);
                    if (distA < distB)
                    {
                        transform.position = transform.position - new Vector3(distA, 0);
                        Destroy(col[0].gameObject);
                    } else
                    {
                        transform.position = transform.position + new Vector3(distB, 0);
                        Destroy(col[1].gameObject);
                    }
                } else
                {
                    // should not reach here.
                    print("im here");
                }*/
                foreach (Collider2D current in col)
                {
                    if (current.transform.tag == "Platform")
                    {
                        Destroy(current.gameObject);
                    }
                }
            }
        }
    }

    bool isCharacterOnPlatform()
    {
        //if (transform.position.y == yPos)
        if (Math.Abs(transform.position.y - yPos) < 0.001)
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
            
            if (Input.GetKey(KeyCode.D)) // Restricts movement if moving left or right will collide into wall/platform blocks.
            {
                Vector3 movement = Vector3.right * speed * Time.deltaTime;
                if (!isColliding(transform.position + movement))
                {
                    transform.position += movement;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Vector3 movement = Vector3.left * speed * Time.deltaTime;
                if (!isColliding(transform.position + movement))
                {
                    transform.position += movement;
                }
            }
        } 
    }

    bool isColliding(Vector3 point)
    {
        float currX = point.x;
        float currY = point.y;
        float threshold = HORIZONTAL_COLLISION_THRESHOLD;
        Vector2 ptA = new Vector2((float) (currX - threshold), (float)(currY + threshold));
        Vector2 ptB = new Vector2((float)(currX + threshold), (float)(currY - threshold * 2));
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, ((1 << 8) | (1 << 10) | (1 << 11)));
        if (col.Length == 0)
        {
            return false;
        } else
        {
            return true;
        }
    }
    /* --------------------------------- END PLAYER CONTROL FUNCTIONS --------------------------------- */

    /* --------------------------------- START PLAYER IN-GAME RULE FUNCTIONS --------------------------------- */

    void updateInvincibility()
    {
        // player is under invincibility frame
        if (invincibility > 0) invincibility--;
        // player's invincibility frame expires
        else if (invincibility == 0) isHurt = false;
        // invalid invincibility count
        else throw new System.ApplicationException(ERROR_INVALID_INVINCIBILITY_VALUE);
    }

    // Handles collisions with diamonds
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Diamond"))
        {
            triggerDiamondInteraction(other);
        }
    }

    // Dummy implementations
    // Use OnCollisionStay2D to check every frame for collision handling.
    void OnCollisionStay2D(Collision2D coll)
    {
        string collidedObject = coll.gameObject.tag;

        switch (collidedObject)
        {
            case "Platform":
                // this case is already handled in the control functions
                break;

            case "BackgroundBoundary":
                // no action needed here
                break;

            case "Diamond":
                // Action handled by OnTriggerEnter2D()
                break;

            case "TreasureChest":
                print("Player has opened a treasure chest.");
                // TODO: spawn diamond/monster from treasure chest
                // Call the treasure chest spawn function here,
                // which should destroy itself after spawning diamond/monster.
                break;

            case "SpecialTreasureChest":
                triggerSpecialTreasureChestInteraction(coll);
                break;

            case "Monster":
                triggerMonsterInteraction(coll);
                break;

            case "Trap":
                triggerTrapInteraction();
                break;

            default :
                throw new System.ApplicationException(ERROR_UNEXPECTED_COLLISION_EVENT);
        }
    }

    private void triggerDiamondInteraction(Collider2D coll)
    {
        print("Player has collected a diamond.");
        diamonds++;
        Destroy(coll.gameObject);
    }

    private void triggerSpecialTreasureChestInteraction(Collision2D coll)
    {
        print("Player has opened a treasure chest containing special diamond.");
        specialDiamonds++;
        print("Player has collected the special diamond.");
        Destroy(coll.gameObject);
        if (specialDiamonds > NUM_SPECIAL_DIAMONDS_MAX)
        {
            throw new System.ApplicationException(ERROR_INVALID_SPECIAL_DIAMOND_VALUE);
        }
    }

    private bool isMonsterHitFromTop(Collision2D coll)
    {
        /* print("PlayerX: " + rb2d.position.x + " | MonsterX: " + coll.collider.bounds.min.x + ","
                 + coll.collider.bounds.max.x); */
        bool isHitFromTop = (rb2d.position.x + PLAYER_X_OFFSET) > coll.collider.bounds.min.x &&
                            (rb2d.position.x - PLAYER_X_OFFSET) < coll.collider.bounds.max.x;

        if (isHitFromTop) return true;
        return false;
    }

    private void triggerMonsterInteraction(Collision2D coll)
    {
        // Uncomment this print statement for fine-tuning the collision mechanism of player with monster.
        //print(isMonsterHitFromTop(coll));
        if (Input.GetKey(KeyCode.S)) // if the player held the 'dig' button when coliding with monster
        {
            if (isMonsterHitFromTop(coll))
            {
                //print("Player has killed the monster.");
                Destroy(coll.gameObject);
            }
            else
            {
                if (isHurt) return; // if player is already hurt, he/she is granted invincibility frames
                //print("Player has touched a monster.");
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

    private void enforceInjury()
    {
        isHurt = true;
        lives -= 1;
        checkIfPlayerDied();
    }

    private void checkIfPlayerDied()
    {
        if (lives == 0)
        {
            print("Player has " + lives + " remaining and has died.");
            restartLevel();
        }
        else if (lives > 0)
        {
            print("Player has lost 1 life with " + lives + " remaining.");
            invincibility = INVINCIBILITY_FRAME;
        }
        else throw new System.ApplicationException(ERROR_INVALID_LIVES_VALUE);
    }
    /* --------------------------------- END PLAYER IN-GAME RULE FUNCTIONS --------------------------------- */
}
