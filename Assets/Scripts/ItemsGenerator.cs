﻿using UnityEngine;
using System.Collections;

public class ItemsGenerator : MonoBehaviour
{
    public GameObject background;
    public GameObject diamond;
    // Handle the bounds of the map / background
    private Vector3 topLeft;
    private Vector3 btmRight;

    // constants
    private const float DIST_NO_SPAWN_FROM_BTM = 20f;
    // Regular Vertical
    private const int NUM_REGULAR_VERTICAL_SET = 3; // number of sets
    private const int NUM_IN_REGULAR_VERTICAL = 3; // 1 set consist of how many diamonds
    private const float SPACING_REGULAR_VERTICAL = 1.75f;
    // Regular Horizontal
    private const int NUM_REGULAR_HORIZONTAL_SET = 3; // number of sets
    private const int NUM_IN_REGULAR_HORIZONTAL = 3; // 1 set consist of how many diamonds
    private const float SPACING_REGULAR_HORIZONTAL = 1.75f;
    private const float SPACING_OFF_GROUND_REGULAR_HORIZONTAL = 1.25f;
    // Regular diagonal right
    private const int NUM_REGULAR_DIAGONAL_R_SET = 3; // number of sets
    private const int NUM_IN_REGULAR_DIAGONAL_R = 3; // 1 set consist of how many diamonds
    private const float SPACING_REGULAR_DIAGONAL_R = 1.25f;
    // Regular diagonal left
    private const int NUM_REGULAR_DIAGONAL_L_SET = 3; // number of sets
    private const int NUM_IN_REGULAR_DIAGONAL_L = 3; // 1 set consist of how many diamonds
    private const float SPACING_REGULAR_DIAGONAL_L = 1.25f;
    // Lone diamond
    private const int NUM_LONE_DIAMOND_SET = 2;
    private const float SPACING_OFF_GROUND_LONE_DIAMOND = 1.25f;

    void Start()
    {
        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
        topLeft = transform.position;
        Vector3 sizeBg = sr.bounds.size;
        btmRight = new Vector3(topLeft.x + sizeBg.x, topLeft.y - sizeBg.y + DIST_NO_SPAWN_FROM_BTM, topLeft.z);

        generateDiamonds();
    }

    private void generateDiamonds()
    {
        generateVerticalRegularSet();
        generateHorizontalRegularSet();
        generateDiagonalRightRegularSet();
        generateDiagonalLeftRegularSet();
        generateLoneDiamondSet();
    }

    private void generateLoneDiamondSet()
    {
        for (int i = 0; i < NUM_LONE_DIAMOND_SET; i++)
        {
            generateLoneDiamond();
        }
    }

    private void generateDiagonalRightRegularSet()
    {
        for (int i = 0; i < NUM_REGULAR_DIAGONAL_R_SET; i++)
        {
            generateDiagonalRightRegular();
        }
    }

    private void generateDiagonalLeftRegularSet()
    {
        for (int i = 0; i < NUM_REGULAR_DIAGONAL_L_SET; i++)
        {
            generateDiagonalLeftRegular();
        }
    }

    private void generateVerticalRegularSet()
    {
        for (int i = 0; i < NUM_REGULAR_VERTICAL_SET; i++)
        {
            generateVerticalRegular();
        }
    }

    private void generateHorizontalRegularSet()
    {
        for (int i = 0; i < NUM_REGULAR_HORIZONTAL_SET; i++)
        {
            generateHorizontalRegular();
        }
    }

    private void generateDiagonalLeftRegular()
    {
        float randX = Random.Range(topLeft.x, btmRight.x);
        float randY = Random.Range(btmRight.y, topLeft.y);
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_REGULAR_DIAGONAL_L; i++)
        { // Check if spawning diamonds will collide
            Vector2 ptA = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptB = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 262143);
            if (col.Length > 0) // If collide with something else, spawn again.
            {
                generateDiagonalLeftRegular();
                return;
            }
            tempX -= SPACING_REGULAR_DIAGONAL_L;
            tempY -= SPACING_REGULAR_DIAGONAL_L;
        }
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_REGULAR_DIAGONAL_L; i++)
        {
            Instantiate(diamond, new Vector3(randX, randY, 0), Quaternion.identity);
            randX -= SPACING_REGULAR_DIAGONAL_L;
            randY -= SPACING_REGULAR_DIAGONAL_L;
        }
    }

    private void generateDiagonalRightRegular()
    {
        float randX = Random.Range(topLeft.x, btmRight.x);
        float randY = Random.Range(btmRight.y, topLeft.y);
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_REGULAR_DIAGONAL_R; i++)
        { // Check if spawning diamonds will collide
            Vector2 ptA = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptB = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 262143);
            if (col.Length > 0) // If collide with something else, spawn again.
            {
                generateDiagonalRightRegular();
                return;
            }
            tempX += SPACING_REGULAR_DIAGONAL_R;
            tempY -= SPACING_REGULAR_DIAGONAL_R;
        }
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_REGULAR_DIAGONAL_R; i++)
        {
            Instantiate(diamond, new Vector3(randX, randY, 0), Quaternion.identity);
            randX += SPACING_REGULAR_DIAGONAL_R;
            randY -= SPACING_REGULAR_DIAGONAL_R;
        }
    }

    private void generateHorizontalRegular()
    {
        Vector2 ptA = new Vector2(topLeft.x, topLeft.y);
        Vector2 ptB = new Vector2(btmRight.x, btmRight.y);
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1<<8); // platform detection

        int choiceOfPlatform = Random.Range(0, col.Length - 1);
        Collider2D chosenPlatform = col[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_REGULAR_HORIZONTAL;
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_REGULAR_HORIZONTAL; i++)
        { // Check if spawning diamonds will collide
            Vector2 ptC = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptD = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

            if (coll.Length > 0) // If collide with something else, spawn again.
            {
                generateHorizontalRegular();
                return;
            }
            tempX += SPACING_REGULAR_HORIZONTAL;
        }
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_REGULAR_HORIZONTAL; i++)
        {
            Instantiate(diamond, new Vector3(randX, randY, 0), Quaternion.identity);
            randX += SPACING_REGULAR_HORIZONTAL;
        }
    }

    private void generateVerticalRegular()
    {
        float randX = Random.Range(topLeft.x, btmRight.x);
        float randY = Random.Range(btmRight.y, topLeft.y);
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_REGULAR_VERTICAL; i++ )
        { // Check if spawning diamonds will collide
            Vector2 ptA = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptB = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 262143);
            if (col.Length > 0) // If collide with something else, spawn again.
            {
                generateVerticalRegular();
                return;
            }
            tempY += SPACING_REGULAR_VERTICAL;
        }
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_REGULAR_VERTICAL; i++)
        {
            Instantiate(diamond, new Vector3(randX, randY, 0), Quaternion.identity);
            randY += SPACING_REGULAR_VERTICAL;
        }
    }

    private void generateLoneDiamond()
    {
        Vector2 ptA = new Vector2(topLeft.x, topLeft.y);
        Vector2 ptB = new Vector2(btmRight.x, btmRight.y);
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 8); // platform detection

        int choiceOfPlatform = Random.Range(0, col.Length - 1);
        Collider2D chosenPlatform = col[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_LONE_DIAMOND;

        // Check if spawning diamond will collide
        Vector2 ptC = new Vector2((float)(randX - 0.5), (float)(randY + 0.5));
        Vector2 ptD = new Vector2((float)(randX + 0.5), (float)(randY - 0.5));
        Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

        if (coll.Length > 0) // If collide with something else, spawn again.
        {
            generateLoneDiamond();
            return;
        }
        
        // Spawn lone diamond
        Instantiate(diamond, new Vector3(randX, randY, 0), Quaternion.identity);
    }
}
