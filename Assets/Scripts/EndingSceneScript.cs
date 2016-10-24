﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingSceneScript : MonoBehaviour {
    private GameObject[] layers;
    private int frameCounter;
    private int currentFrame;
    private bool isFading;
    private bool isFadingIn;

    private const int FRAME_STAY_DELAY = 100;
    private const int FRAMES_BEFORE_TRANSIT = 100;

	// Use this for initialization
    
	void Start () {
        frameCounter = 0;
        isFading = false;
        isFadingIn = false;
        currentFrame = transform.childCount - 1;

        layers = new GameObject[transform.childCount];
        for (int i=0; i<layers.Length; i++)
        {
            layers[i] = transform.GetChild(i).gameObject;
        }
    }

    void handleFadeIn()
    {
        if (!isFadingIn)
        { // First time running the fade in.
            isFadingIn = true;
            Color color2 = layers[currentFrame].GetComponent<SpriteRenderer>().color;
            color2.a = 0f; // change to transparent to start fading in
            layers[currentFrame].GetComponent<SpriteRenderer>().color = color2;
            layers[currentFrame].SetActive(true);
        } else
        {
            Color color = layers[currentFrame].GetComponent<SpriteRenderer>().color;
            if (color.a <= 1f)
            { // fading in
                color.a += (float)(1.0 / FRAMES_BEFORE_TRANSIT);
                layers[currentFrame].GetComponent<SpriteRenderer>().color = color;
            } else
            {   // done fade in.
                isFadingIn = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Show the frame for specified time.
        if (!isFading)
        {
            // Check if need fade in
            if (layers[currentFrame].activeSelf == false || isFadingIn)
            {
                handleFadeIn();
                return;
            }
            
            frameCounter++;
            if (frameCounter >= FRAME_STAY_DELAY)
            {
                frameCounter = 0;
                isFading = true;
            }

            return;
        }
        
        // Fading effect
        Color color = layers[currentFrame].GetComponent<SpriteRenderer>().color;
        if (color.a <= 0)
        {
            // Change frame
            if (currentFrame > 0)
            {
                currentFrame--;
                isFading = false;
            } else
            {
                // Finish showing all frames
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
            
        }
        else
        {
            color.a -= (float)(1.0 / FRAMES_BEFORE_TRANSIT);
            layers[currentFrame].GetComponent<SpriteRenderer>().color = color;
        }
    }
}