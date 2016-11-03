using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuButtonScript : MonoBehaviour {

    public Image screenImage;
    public Sprite defaultScreen;
    public Sprite resumeHoverScreen;
    public Sprite leaveHoverScreen;
    public Canvas pauseCanvas;

    private float speed = 2f;
    private GameObject player;

    // Use this for initialization
    void Start()
    {
        pauseCanvas.enabled = false;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (player.transform.position.y >= -85)
            {
                Time.timeScale = 0f;
                pauseCanvas.enabled = true;
            }
        }
    }

    public void OnResumeHover()
    {
        screenImage.sprite = resumeHoverScreen;
    }

    public void OnLeaveHover()
    {
        screenImage.sprite = leaveHoverScreen;
    }

    public void OnHoverLeave()
    {
        screenImage.sprite = defaultScreen;
    }

    public void Resume()
    {
        pauseCanvas.enabled = false;
        Time.timeScale = 1.0f;
    }

    public void LeaveGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
