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
    public Sprite mainMenuHoverScreen;
    public Sprite quitGameHoverScreen;
    public Canvas loadingCanvas;
    private float originalRed;
    private float originalGreen;
    private float originalBlue;
    private string levelName;

    private AsyncOperation loadOp;

    private float speed = 2f;

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
        SavedLevel = GlobalPlayerScript.Instance.level;
        levelName = "Level " + SavedLevel;
        loadingCanvas.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void OnReplayHover()
    {
        screenImage.sprite = replayHoverScreen;
    }

    public void OnMainMenuHover()
    {
        screenImage.sprite = mainMenuHoverScreen;
    }

    public void OnQuitHover()
    {
        screenImage.sprite = quitGameHoverScreen;
    }

    public void OnHoverLeave()
    {
        screenImage.sprite = defaultScreen;
    }

    public void Replay()
    {
        print("Loading " + levelName + " ...");
        GlobalPlayerScript.Instance.lives = 3;
        loadingCanvas.enabled = true;
        loadOp = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        loadOp.allowSceneActivation = true;
    }

    public void NavigateToMainMenu()
    {
        print("Leaving to main menu...");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
