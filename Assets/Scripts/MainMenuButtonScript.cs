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

    // Use this for initialization
    void Start () {
        tutorialCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("escape"))
        {
            tutorialCanvas.enabled = false;
        }
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
        print("Opening tutorial screen...");
        tutorialCanvas.enabled = true;
    }
}
