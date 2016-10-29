using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelThreeEndScript : MonoBehaviour {
    private bool isFading;

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
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); // dummy
        }
    }
}
