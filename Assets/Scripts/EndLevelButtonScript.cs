using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevelButtonScript : MonoBehaviour {
    public GameObject player;
    public GameObject steve;
    public GameObject exitDoor;
    public GameObject advanceDoor;
    public GameObject diamondCluster;
    public Button yesButton;
    public Button noButton;

    private PlayerCollisionController playerAttr;
    private PlayerBaseController playerBaseController;
    private float posAdvanceDoor;
    private float posExitDoor;
    private GameObject diamondObj;

    private bool isMoveTowardsAdvance;
    private bool isMoveTowardsExit;
    private bool isDiamondPaid;
    private bool isPayingDiamond;

	// Use this for initialization
	void Start () {
        posAdvanceDoor = advanceDoor.transform.position.x;
        posExitDoor = exitDoor.transform.position.x;
        isMoveTowardsAdvance = false;
        isMoveTowardsExit = false;
        playerAttr = FindObjectOfType<PlayerCollisionController>();
        playerBaseController = FindObjectOfType<PlayerBaseController>();
        isDiamondPaid = false;
        isPayingDiamond = false;
    }
	
	// Update is called once per frame
	void Update () {
        payDiamond();
        advanceScene();
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
        Vector3 playerPos = player.transform.position;
        disableButtons();
        if (player.GetComponent<PlayerCollisionController>().getDiamondPenalty() == 0)
        { // considered already paid if nothing to pay
            isDiamondPaid = true;
        } else
        { // pay diamond
            player.GetComponent<PlayerCollisionController>().enforceDiamondPenalty();
            diamondObj = (GameObject)Instantiate(diamondCluster, playerPos, Quaternion.identity);
            isPayingDiamond = true;
        }
    }

    private void payDiamond()
    {
        if (isPayingDiamond)
        {
            Vector3 stevePos = steve.transform.position;
            Vector3 diamondPos = diamondObj.transform.position;
            if (Mathf.Abs(stevePos.x - diamondPos.x) <= 0.05f)
            {
                isPayingDiamond = false;
                StartCoroutine(fadeDiamond());
            } else if (stevePos.x > diamondPos.x)
            {
                diamondPos.x += 0.05f;
                diamondPos.y += 0.01f;
                diamondObj.transform.position = diamondPos;
            } else
            { // steve < diamond pos
                diamondPos.x -= 0.05f;
                diamondPos.y += 0.01f;
                diamondObj.transform.position = diamondPos;
            }
        }
    }

    IEnumerator fadeDiamond()
    {
        if (diamondObj.GetComponent<SpriteRenderer>().color.a > 0)
        {
            Color color = diamondObj.GetComponent<SpriteRenderer>().color;
            color.a -= 0.1f;
            diamondObj.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(fadeDiamond());
        } else
        {
            Destroy(diamondObj);
            isDiamondPaid = true;
        }
    }

    private void advanceScene()
    {
        if (isDiamondPaid)
        {
            steve.GetComponent<Animator>().Play("steveopen");
            StartCoroutine(openAdvanceDoor());
        }
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
                savePlayerState(playerAttr, playerBaseController);
                SceneManager.LoadScene("Level 1.5", LoadSceneMode.Single);
            } 
        }
    }

    private static void savePlayerState(PlayerCollisionController player, PlayerBaseController playerBaseController)
    {
        GlobalPlayerScript.Instance.lives = player.lives;
        GlobalPlayerScript.Instance.diamonds = player.diamonds;
        GlobalPlayerScript.Instance.specialDiamonds = player.specialDiamonds;
        GlobalPlayerScript.Instance.hasJetpackUpgrade = playerBaseController.hasJetpackUpgrade;
        GlobalPlayerScript.Instance.hasShovelUpgrade = playerBaseController.hasShovelUpgrade;
        GlobalPlayerScript.Instance.level = 2;
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
            { // after fading, go to ending
                SceneManager.LoadScene("Ending 1", LoadSceneMode.Single);
            }
        }
    }

    private void disableButtons()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }
}
