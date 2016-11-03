using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGUIScript : MonoBehaviour {

    public PlayerCollisionController player;
    public PlayerBaseController player_upgrades;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite normalHeart;
    public Sprite armoredHeart;
    public Image jetpackFlag;
    public Image shovelFlag;

    public Text diamondCount;
    public Image soulDiamond;
    public Text soulDiamondCount;

    public Text depthRemaining;

    private Rigidbody2D playerBody;
    private Collider2D playerCollider;
    private Collider2D levelGroundCollider;
    private bool isLevelScene;
    private bool isTutorialScene;
    private float levelEndPos;

    // Use this for initialization
    void Start ()
    {
        Scene currScene = SceneManager.GetActiveScene();
        isTutorialScene = Equals(currScene.name, "TutorialLevel");
        if (!isTutorialScene)
        {
            player = FindObjectOfType<PlayerCollisionController>();
            player_upgrades = FindObjectOfType<PlayerBaseController>();
            soulDiamond.enabled = false;
            soulDiamondCount.enabled = false;
            
            isLevelScene = (!Equals(currScene.name, "Level 1.5") && !Equals(currScene.name, "Level 2.5"));
            if (isLevelScene)
            {
                jetpackFlag.enabled = player_upgrades.hasJetpackUpgrade;
                shovelFlag.enabled = player_upgrades.hasShovelUpgrade;
                getLevelGroundCollider();
                getPlayerBodyAndCollider();
                initializeDepthMeter();
            }
        }
    }

    private void initializeDepthMeter()
    {
        float startingPos = playerBody.position.y - playerCollider.bounds.extents.y;
        levelEndPos = levelGroundCollider.bounds.center.y + levelGroundCollider.bounds.extents.y;
        float levelDistance = startingPos * 10 - levelEndPos * 10;
        depthRemaining.text = (levelDistance.ToString() + "m\nleft");
    }

    private void getPlayerBodyAndCollider()
    {
        GameObject playerObject = GameObject.Find("Player");
        playerBody = playerObject.GetComponent<Rigidbody2D>();
        playerCollider = playerObject.GetComponent<Collider2D>();
    }

    private void getLevelGroundCollider()
    {
        GameObject background = GameObject.FindGameObjectsWithTag("BackgroundBoundary")[0];
        levelGroundCollider = background.GetComponents<Collider2D>()[2];
    }

    // Update is called once per frame
    void Update () {
        if (!isTutorialScene)
        {
            UpdateHeartDisplay(player.lives);
            UpdateDiamondCount(player.diamonds);
            UpdateSpecialDiamondDisplay(player.specialDiamonds);
            if (isLevelScene)
            {
                UpdateUpgradeFlags();
                UpdateDepthRemaining(playerBody.position.y);
            }
        }
	}

    void UpdateSpecialDiamondDisplay(int currSpecialDiamonds)
    {
        if (currSpecialDiamonds != 0)
        {
            soulDiamond.enabled = true;
            soulDiamondCount.enabled = true;
            soulDiamondCount.text = player.specialDiamonds.ToString();
        }
        else
        {
            soulDiamond.enabled = false;
            soulDiamondCount.enabled = false;
        }
    }

    void UpdateDepthRemaining(float depthValue)
    {
        depthRemaining.text = (Mathf.Floor(depthValue - playerCollider.bounds.extents.y) * 10 - Mathf.Floor(levelEndPos) * 10).ToString() + "m\nleft";
    }

    void UpdateDiamondCount(int currentDiamonds)
    {
        diamondCount.text = currentDiamonds.ToString();
    }

    void UpdateHeartDisplay(int currentLives)
    {
        handleRightHeartDisplay(currentLives);
        handleMiddleHeartDisplay(currentLives);
        handleLeftHeartDisplay(currentLives);
    }

    void handleRightHeartDisplay(int lives)
    {
        if (lives >= 6)
        {
            heart3.sprite = armoredHeart;
            heart3.enabled = true;
        }
        else if (lives >= 3)
        {
            heart3.sprite = normalHeart;
            heart3.enabled = true;
        }
        else if (lives < 3) heart3.enabled = false;
        else print("Error: Invalid state reached.");
    }

    void handleMiddleHeartDisplay(int lives)
    {
        if (lives >= 5)
        {
            heart2.sprite = armoredHeart;
            heart2.enabled = true;
        }
        else if (lives >= 2)
        {
            heart2.sprite = normalHeart;
            heart2.enabled = true;
        }
        else if (lives < 2) heart2.enabled = false;
        else print("Error: Invalid state reached.");
    }

    void handleLeftHeartDisplay(int lives)
    {
        if (lives >= 4)
        {
            heart1.sprite = armoredHeart;
            heart1.enabled = true;
        }
        else if (lives >= 1)
        {
            heart1.sprite = normalHeart;
            heart1.enabled = true;
        }
        else if (lives < 1) heart1.enabled = false;
        else print("Player has died and player should see death screen.");
    }

    void UpdateUpgradeFlags()
    {
        jetpackFlag.enabled = player_upgrades.hasJetpackUpgrade;
        shovelFlag.enabled = player_upgrades.hasShovelUpgrade;
    }
}
