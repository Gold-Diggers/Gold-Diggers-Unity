using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTwoEndScript : MonoBehaviour {
    private const string LEVEL_2_DIALOGUE_END = "Welcome to the depths of Hell.";

    public GameObject player;
    public GameObject exitDoor;
    public GameObject floor;
    public GameObject background;
    public Text dialogue;
    public Button yesButton;
    public Button noButton;

    private PlayerCollisionController playerAttr;
    private PlayerBaseController playerUpgrades;
    private float posExitDoor;

    private bool isMoveTowardsExit;

    // Use this for initialization
    void Start()
    {
        posExitDoor = exitDoor.transform.position.x;
        isMoveTowardsExit = false;
        playerAttr = FindObjectOfType<PlayerCollisionController>();
        playerUpgrades = FindObjectOfType<PlayerBaseController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveTowardsExit)
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
        disableButtons();
        savePlayerState(playerAttr);
        dialogue.text = LEVEL_2_DIALOGUE_END;
        Collider2D[] colliders = background.GetComponents<Collider2D>();
        colliders[2].enabled = false;
        floor.SetActive(false);
        StartCoroutine(introduceDelay());
    }

    private IEnumerator introduceDelay()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Level 2.5", LoadSceneMode.Single);
    }

    private void savePlayerState(PlayerCollisionController player)
    {
        GlobalPlayerScript.Instance.lives = player.lives;
        GlobalPlayerScript.Instance.diamonds = player.diamonds;
        GlobalPlayerScript.Instance.specialDiamonds = player.specialDiamonds;
        GlobalPlayerScript.Instance.level = 3;
        GlobalPlayerScript.Instance.hasJetpackUpgrade = playerUpgrades.hasJetpackUpgrade;
        GlobalPlayerScript.Instance.hasShovelUpgrade = playerUpgrades.hasShovelUpgrade;
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
                SceneManager.LoadScene("Ending 2", LoadSceneMode.Single);
            }
        }
    }

    private void disableButtons()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }
}
