using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelThreeEndScript : MonoBehaviour {
    private bool isFading;
    private GameObject player;

    private const int DIAMOND_THRESHOLD = 175;

    void Start()
    {
        isFading = false;
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

        if (specialDiamond == 3)
        { // Golden Ending
            SceneManager.LoadScene("Ending 3", LoadSceneMode.Single);
        } else if (diamond >= DIAMOND_THRESHOLD)
        { // Diamond Collector
            SceneManager.LoadScene("Ending 4", LoadSceneMode.Single);
        } else
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); // dummy
        }
    }
}
