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
    public Text loadingText;
    private float originalRed;
    private float originalGreen;
    private float originalBlue;

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
        loadingText.enabled = false;
        originalRed = loadingText.color.r;
        originalGreen = loadingText.color.g;
        originalBlue = loadingText.color.b;
    }
	
	// Update is called once per frame
	void Update () {
        triggerTextFadeEffect();
	}

    private void triggerTextFadeEffect()
    {
        loadingText.color = new Color(originalRed, originalGreen, originalBlue, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f);
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
        loadingText.enabled = true;
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        loadOp.allowSceneActivation = true;
    }

    public void Quit()
    {
        print("Leaving to main menu...");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
