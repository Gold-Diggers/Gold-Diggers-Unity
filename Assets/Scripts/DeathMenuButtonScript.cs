using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class DeathMenuButtonScript : MonoBehaviour {

    private const int DEFAULT_SAVED_LEVEL = 1;
    private int savedLevel;

    public Image screenImage;
    public Sprite defaultScreen;
    public Sprite replayHoverScreen;
    public Sprite quitHoverScreen;

    public int SavedLevel
    {
        get
        {
            return savedLevel;
        }
        set
        {
            savedLevel = value;
        }
    }

	// Use this for initialization
	void Start () {
        savedLevel = DEFAULT_SAVED_LEVEL;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnReplayHover()
    {
        screenImage.sprite = replayHoverScreen;
    }

    public void OnQuitHover()
    {
        screenImage.sprite = quitHoverScreen;
    }

    public void OnHoverLeave()
    {
        screenImage.sprite = defaultScreen;
    }

    public void Replay()
    {
        string levelName = "Level " + SavedLevel;
        print("Loading " + levelName + " ...");
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void Quit()
    {
        print("Leaving to main menu...");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
