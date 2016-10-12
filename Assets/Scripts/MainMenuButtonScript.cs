using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenuButtonScript : MonoBehaviour {

    public Image screenImage;
    public Sprite defaultScreen;
    public Sprite startHoverScreen;
    public Sprite tutorialHoverScreen;
    public Canvas tutorialCanvas;
    public Text prompt;
    private float originalRed;
    private float originalGreen;
    private float originalBlue;

    private float speed = 2f;

    // Use this for initialization
    void Start () {
        tutorialCanvas.enabled = false;
        originalRed = prompt.color.r;
        originalGreen = prompt.color.g;
        originalBlue = prompt.color.b;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("escape"))
        {
            tutorialCanvas.enabled = false;
        }
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

    public void OnHoverLeave()
    {
        screenImage.sprite = defaultScreen;
    }

    public void Play()
    {
        print("Loading Level 1...");
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }

    public void OpenTutorial()
    {
        //print("Opening tutorial screen...");
        prompt.color = new Color(originalRed, originalGreen, originalBlue, 1.0f);
        tutorialCanvas.enabled = true;
    }
}
