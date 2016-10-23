﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGUIScript : MonoBehaviour {

    public PlayerCollisionController player;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite normalHeart;
    public Sprite armoredHeart;

    public Text diamondCount;
    public Image soulDiamond;

    public Text depthRemaining;

    private Rigidbody2D playerBody;
    private Collider2D playerCollider;
    private Collider2D levelGroundCollider;
    private float levelEndPos;

    // Use this for initialization
    void Start ()
    {
        player = FindObjectOfType<PlayerCollisionController>();
        soulDiamond.enabled = false;
        getLevelGroundCollider();
        getPlayerBodyAndCollider();
        initializeDepthMeter();
    }

    private void initializeDepthMeter()
    {
        float startingPos = playerBody.position.y - playerCollider.bounds.extents.y;
        levelEndPos = levelGroundCollider.bounds.center.y + levelGroundCollider.bounds.extents.y;
        float levelDistance = startingPos * 10 - levelEndPos * 10;
        depthRemaining.text = (levelDistance.ToString() + "m\nleft");
    }

    private void getPlayerBodyAndCollider()
    {
        GameObject playerObject = GameObject.Find("Player");
        playerBody = playerObject.GetComponent<Rigidbody2D>();
        playerCollider = playerObject.GetComponent<Collider2D>();
    }

    private void getLevelGroundCollider()
    {
        GameObject background = GameObject.FindGameObjectsWithTag("BackgroundBoundary")[0];
        levelGroundCollider = background.GetComponents<Collider2D>()[2];
    }

    // Update is called once per frame
    void Update () {
        print(playerBody.position);
        UpdateHeartDisplay(player.lives);
        UpdateDiamondCount(player.diamonds);
        UpdateSpecialDiamondDisplay(player.specialDiamonds);
        UpdateDepthRemaining(playerBody.position.y);
	}

    void UpdateSpecialDiamondDisplay(int currSpecialDiamonds)
    {
        if (currSpecialDiamonds == GlobalPlayerScript.Instance.level) soulDiamond.enabled = true;
    }

    void UpdateDepthRemaining(float depthValue)
    {
        depthRemaining.text = (Mathf.Floor((depthValue - playerCollider.bounds.extents.y) * 10 - levelEndPos * 10).ToString() + "m\nleft");
    }

    void UpdateDiamondCount(int currentDiamonds)
    {
        diamondCount.text = currentDiamonds.ToString();
    }

    void UpdateHeartDisplay(int currentLives)
    {
        handleRightHeartDisplay(currentLives);
        handleMiddleHeartDisplay(currentLives);
        handleLeftHeartDisplay(currentLives);
    }

    void handleRightHeartDisplay(int lives)
    {
        if (lives >= 6) heart3.sprite = armoredHeart;
        else if (lives >= 3) heart3.sprite = normalHeart;
        else if (lives > 0) heart3.enabled = false;
        else print("Error: Invalid state reached.");
    }

    void handleMiddleHeartDisplay(int lives)
    {
        if (lives >= 5) heart2.sprite = armoredHeart;
        else if (lives >= 2) heart2.sprite = normalHeart;
        else if (lives > 0) heart2.enabled = false;
        else print("Error: Invalid state reached.");
    }

    void handleLeftHeartDisplay(int lives)
    {
        if (lives >= 4) heart1.sprite = armoredHeart;
        else if (lives >= 1) heart1.sprite = normalHeart;
        else if (lives == 0) heart1.enabled = false;
        else print("Player has died and player should see death screen.");
    }
}