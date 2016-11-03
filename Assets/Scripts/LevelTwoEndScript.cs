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
    public GameObject devil;
    public GameObject diamondCluster;
    public GameObject soul;
    private GameObject diamondObj;
    private GameObject soulObj;
    public Text dialogue;
    public Button yesButton;
    public Button noButton;

    private PlayerCollisionController playerAttr;
    private PlayerBaseController playerUpgrades;
    private float posExitDoor;

    private bool isMoveTowardsExit;
    private bool isPayingSoul;
    private bool isPayingDiamond;
    private bool isSoulPaid;
    private bool isDiamondPaid;

    private const float DIAMOND_ANIMATION_MOVE_X = 0.05f;
    private const float DIAMOND_ANIMATION_MOVE_Y = 0.01f;
    private const float DIAMOND_SOUL_DISTANCE = 0.75f;

    // Use this for initialization
    void Start()
    {
        posExitDoor = exitDoor.transform.position.x;
        isMoveTowardsExit = false;
        playerAttr = FindObjectOfType<PlayerCollisionController>();
        playerUpgrades = FindObjectOfType<PlayerBaseController>();
        isPayingSoul = false;
        isPayingDiamond = false;
        isSoulPaid = false;
        isDiamondPaid = false;
}

    // Update is called once per frame
    void Update()
    {
        payDiamond();
        paySoul();
        advanceScene();
        if (isMoveTowardsExit)
        {
            movePlayerToExit();
        }
    }

    public void fadeInDevil()
    {
        StartCoroutine(fadeDevil());
    }

    private IEnumerator fadeDevil()
    {
        Color color = devil.GetComponent<SpriteRenderer>().color;
        color.a += 0.025f;
        if (color.a >= 1f)
        { // finish fading in.
            int penalty = player.GetComponent<PlayerCollisionController>().getDiamondPenalty();
            player.GetComponent<PlayerCollisionController>().informDevilFadedIn();
            StartCoroutine(devilStealAnim(penalty));
        } else
        {
            devil.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(fadeDevil());
        }
    }

    private IEnumerator devilStealAnim(int penalty)
    {
        devil.GetComponent<Animator>().Play("devilsteal");
        yield return new WaitForSeconds(0.7f);
        Vector3 playerPos = player.transform.position;
        playerPos.x += DIAMOND_SOUL_DISTANCE;
        if (penalty != 0)
        {
            diamondObj = (GameObject)Instantiate(diamondCluster, playerPos, Quaternion.identity);
            isPayingDiamond = true;
        }
        else
        {
            isDiamondPaid = true;
        }
        playerPos.x -= DIAMOND_SOUL_DISTANCE * 2;
        soulObj = (GameObject)Instantiate(soul, playerPos, Quaternion.identity);
        isPayingSoul = true;
    }

    private void payDiamond()
    {
        if (isPayingDiamond)
        {
            Vector3 devilPos = devil.transform.position;
            Vector3 diamondPos = diamondObj.transform.position;
            if (Mathf.Abs(devilPos.x - diamondPos.x) <= DIAMOND_ANIMATION_MOVE_X)
            {
                isPayingDiamond = false;
                StartCoroutine(fadeDiamond());
            }
            else if (devilPos.x > diamondPos.x)
            {
                diamondPos.x += DIAMOND_ANIMATION_MOVE_X;
                diamondPos.y += DIAMOND_ANIMATION_MOVE_Y;
                diamondObj.transform.position = diamondPos;
            }
            else
            { // devil < diamond pos
                diamondPos.x -= DIAMOND_ANIMATION_MOVE_X;
                diamondPos.y += DIAMOND_ANIMATION_MOVE_Y;
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
        }
        else
        {
            Destroy(diamondObj);
            isDiamondPaid = true;
        }
    }

    private void paySoul()
    {
        if (isPayingSoul)
        {
            Vector3 devilPos = devil.transform.position;
            Vector3 soulPos = soulObj.transform.position;
            if (Mathf.Abs(devilPos.x - soulPos.x) <= DIAMOND_ANIMATION_MOVE_X)
            {
                isPayingSoul = false;
                StartCoroutine(fadeSoul());
            }
            else if (devilPos.x > soulPos.x)
            {
                soulPos.x += DIAMOND_ANIMATION_MOVE_X;
                soulPos.y += DIAMOND_ANIMATION_MOVE_Y;
                soulObj.transform.position = soulPos;
            }
            else
            { // devil < diamond pos
                soulPos.x -= DIAMOND_ANIMATION_MOVE_X;
                soulPos.y += DIAMOND_ANIMATION_MOVE_Y;
                soulObj.transform.position = soulPos;
            }
        }
    }

    IEnumerator fadeSoul()
    {
        if (soulObj.GetComponent<SpriteRenderer>().color.a > 0)
        {
            Color color = soulObj.GetComponent<SpriteRenderer>().color;
            color.a -= 0.1f;
            soulObj.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(fadeSoul());
        }
        else
        {
            Destroy(soulObj);
            isSoulPaid = true;
        }
    }

    private void advanceScene()
    {
        if (isDiamondPaid && isSoulPaid)
        {
            devil.GetComponent<Animator>().Play("devil");
            player.GetComponent<PlayerCollisionController>().informDiamondAndSoulStolen();
            isDiamondPaid = false;
            isSoulPaid = false;
        }
    }

    public void clickExit()
    {
		if (SceneManager.GetActiveScene ().name == "Level 2")
		{
			print ("Completed ending 2.");
			GlobalPlayerScript.Instance.hasEndings [1] = true;
		}
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
            { // after fading, go to ending
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
