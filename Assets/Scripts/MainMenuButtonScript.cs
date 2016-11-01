using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenuButtonScript : MonoBehaviour {

    public Image screenImage;
    public Sprite defaultScreen;
    public Sprite startHoverScreen;
    public Sprite tutorialHoverScreen;
    public Sprite quitGameHoverScreen;
    public Canvas tutorialCanvas;
    public Canvas loadingCanvas;
    public Text prompt;
    private float originalRed;
    private float originalGreen;
    private float originalBlue;
    private AsyncOperation loadOp;

    private float speed = 2f;

    // Use this for initialization
    void Start () {
        tutorialCanvas.enabled = false;
        loadingCanvas.enabled = false;
        originalRed = prompt.color.r;
        originalGreen = prompt.color.g;
        originalBlue = prompt.color.b;
    }
	
	// Update is called once per frame
	void Update ()
    {
        openTutorialOnEscapeKey();
        triggerTextFadeEffect();
    }

    private void openTutorialOnEscapeKey()
    {
        if (Input.GetKeyDown("escape"))
        {
            tutorialCanvas.enabled = false;
        }
    }

    private void triggerTextFadeEffect()
    {
        prompt.color = new Color(originalRed, originalGreen, originalBlue, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f);
    }

    public void OnStartHover()
    {
        screenImage.sprite = startHoverScreen;
    }

    public void OnTutorialHover()
    {
        screenImage.sprite = tutorialHoverScreen;
    }

    public void OnQuitHover()
    {
        screenImage.sprite = quitGameHoverScreen;
    }

    public void OnHoverLeave()
    {
        screenImage.sprite = defaultScreen;
    }

    public void Play()
    {
        loadingCanvas.enabled = true;
        print("Loading Level 1...");
        GlobalPlayerScript.Instance.reinitialiseValues();
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
        //loadOp.allowSceneActivation = true;
    }

    public void OpenTutorial()
    {
        //print("Opening tutorial screen...");
        tutorialCanvas.enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenTutorialScene()
    {
        SceneManager.LoadScene("TutorialLevel", LoadSceneMode.Single);
    }
}
