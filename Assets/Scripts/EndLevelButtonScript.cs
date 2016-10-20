using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevelButtonScript : MonoBehaviour {
    public GameObject player;
    public GameObject steve;
    public GameObject exitDoor;
    public GameObject advanceDoor;
    public Button yesButton;
    public Button noButton;

    private float posAdvanceDoor;
    private float posExitDoor;

    private bool isMoveTowardsAdvance;
    private bool isMoveTowardsExit;

	// Use this for initialization
	void Start () {
        posAdvanceDoor = advanceDoor.transform.position.x;
        posExitDoor = exitDoor.transform.position.x;
        isMoveTowardsAdvance = false;
        isMoveTowardsExit = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isMoveTowardsAdvance)
        {
            movePlayerToAdvance();
        } else if (isMoveTowardsExit)
        {
            movePlayerToExit();
        }
    }

    public void clickExit()
    {
        disableButtons();
        isMoveTowardsExit = true;
        player.GetComponent<SpriteRenderer>().flipX = true;
        player.GetComponent<Animator>().SetBool("isRunning", true);
    }

    public void clickAdvance()
    {
        player.GetComponent<PlayerCollisionController>().enforceDiamondPenalty();
        disableButtons();
        steve.GetComponent<Animator>().Play("steveopen");
        StartCoroutine(openAdvanceDoor()); 
    }

    IEnumerator openAdvanceDoor()
    {
        yield return new WaitForSeconds(1f);
        advanceDoor.GetComponent<Animator>().Play("open");
        yield return new WaitForSeconds(0.8f);
        isMoveTowardsAdvance = true;
        player.GetComponent<SpriteRenderer>().flipX = false;
        player.GetComponent<Animator>().SetBool("isRunning", true);
    }

    private void movePlayerToAdvance()
    {
        if (player.transform.position.x < posAdvanceDoor)
        {
            player.transform.position = new Vector3(player.transform.position.x + 0.1f, player.transform.position.y);
        } else
        { // fade effect
            Color color = player.GetComponent<SpriteRenderer>().color;
            if (color.a > 0)
            {
                color.a -= 0.1f;
                player.GetComponent<SpriteRenderer>().color = color;
            } else
            { // after fading, go to transition
                SceneManager.LoadScene("Level 1.5", LoadSceneMode.Single);
            } 
        }
    }

    private void movePlayerToExit()
    {
        if (player.transform.position.x > posExitDoor)
        {
            player.transform.position = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y);
        }
        else
        { // fade effect
            Color color = player.GetComponent<SpriteRenderer>().color;
            if (color.a > 0)
            {
                color.a -= 0.1f;
                player.GetComponent<SpriteRenderer>().color = color;
            }
            else
            { // after fading, go to ending (main menu is a temp placeholder)
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
        }
    }

    private void disableButtons()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }
}
