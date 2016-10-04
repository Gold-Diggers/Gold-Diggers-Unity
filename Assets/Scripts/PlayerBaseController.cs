using UnityEngine;
using UnityEngine.Assertions;
using System;

public class PlayerBaseController : MonoBehaviour {

    /* --------------------------------- START PLAYER DEFINITIONS --------------------------------- */
    /* ================= player mechanics calculations ================= */
    private const double Y_VELOCITY_THRESHOLD = -2.0;
    private const float HORIZONTAL_COLLISION_THRESHOLD_ENEMIES = 0.1f;
    private const float HORIZONTAL_COLLISION_THRESHOLD_PLATFORM = 0.3f;

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

    // Boolean constant to change for testing/production purposes
    private const bool IS_TESTING = true;

    /* ================= Use this for initialization ================= */
    void Start()
    {
        Assert.raiseExceptions = IS_TESTING;
        rb2d = GetComponent<Rigidbody2D>();
        yPos = transform.position.y;
        xPos = transform.position.x;
        isDigging = false;
        diggingCounter = 0;
        isRunning = false;
        anim = GetComponent<Animator>();
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
    	if (Math.Abs(transform.position.x - xPos) >= 0.00005) {
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
        if (Input.GetKey(KeyCode.W))
        {
            if (isCharacterOnPlatform()) // normal jump
            {
                rb2d.AddForce(new Vector2(0, 100) * jumpHeight);
            } else if (isCharacterFalling()) // hover
            {
                if (rb2d.velocity.y < Y_VELOCITY_THRESHOLD)
                {
                    rb2d.AddForce(new Vector2(0, 3.5f) * jumpHeight);
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
                //if (!isColliding(transform.position + movement))
                Vector3 currPos = transform.GetComponent<Collider2D>().bounds.center;
                if (!isColliding(currPos + movement))
                {
                    transform.position += movement;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
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
