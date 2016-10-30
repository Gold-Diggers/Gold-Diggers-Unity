using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelThreeEndScript : MonoBehaviour {
    private bool isFading;
    private GameObject player;
    private float startTime;
    private float timeTaken;

    private const int DIAMOND_THRESHOLD = 175;
    private const float TIME_THRESHOLD = 50f;

    void Start()
    {
        isFading = false;
        startTime = Time.time;
    }

    void Update()
    {
        if (isFading)
        {
            startFading();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFading && other.gameObject.name == "Player")
        {
            timeTaken = Time.time - startTime;
            isFading = true;
            player = other.gameObject;
        }
    }

    void startFading()
    {
        GameObject fadeObj = GameObject.Find("FadeEffect");
        Color color = fadeObj.GetComponent<SpriteRenderer>().color;
        color.a += 0.025f;
        fadeObj.GetComponent<SpriteRenderer>().color = color;
        if (color.a >= 1f)
        { // if fading is done
            // Check conditions for different endings.
            loadEndingScene();
        }
    }

    void loadEndingScene()
    {
        int specialDiamond = player.GetComponent<PlayerCollisionController>().specialDiamonds;
        int diamond = player.GetComponent<PlayerCollisionController>().diamonds;
        print(timeTaken);

        if (specialDiamond == 3)
        { // Golden Ending
            SceneManager.LoadScene("Ending 3", LoadSceneMode.Single);
        } else if (diamond >= DIAMOND_THRESHOLD)
        { // Diamond Collector
            SceneManager.LoadScene("Ending 4", LoadSceneMode.Single);
        } else if (timeTaken <= TIME_THRESHOLD)
        { // Hero Ending
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); // dummy
        } else
        { // Steve Ending
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); // dummy
        }
    }
}
