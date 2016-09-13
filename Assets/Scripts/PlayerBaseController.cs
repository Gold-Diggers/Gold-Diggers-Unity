using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerBaseController : MonoBehaviour {
    public float speed;
    public float jumpHeight;
    private const double Y_VELOCITY_THRESHOLD = -2.00000000;
    private Rigidbody2D rb2d;
    private float yPos;
    // For handling period when player is digging
    private bool isDigging;
    private int diggingCounter;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        yPos = transform.position.y;
        isDigging = false;
        diggingCounter = 0;
    }

    void updateYPos()
    {
        yPos = transform.position.y;
    }

    void FixedUpdate()
    {
        handleJump();
        handleDig();
        handleHorizontalMovement();
        handleDigCooldown();
        updateYPos(); // must be last
    }

    void handleJump()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (isCharacterOnPlatform())
            { // normal jump
                rb2d.AddForce(new Vector2(0, 100) * jumpHeight);
            } else if (isCharacterFalling())
            { // hover
                //print(rb2d.velocity.y);
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
                float currX = transform.position.x;
                float currY = transform.position.y;
                Vector2 ptA = new Vector2((float)(currX - 0.45), (float)(currY - 0.95));
                Vector2 ptB = new Vector2((float)(currX + 0.45), currY - 1);
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
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
        } 
    }
}
