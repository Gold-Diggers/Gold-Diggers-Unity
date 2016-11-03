using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionScript : MonoBehaviour {

    public Canvas loadingCanvas;
    public Image loadingScreen;
    private PlayerCollisionController player;
    private PlayerBaseController player_upgrades;
    private AsyncOperation loadOp;

	// Use this for initialization
	void Start () {
        loadingScreen.enabled = false;
        player = FindObjectOfType<PlayerCollisionController>();
        player_upgrades = FindObjectOfType<PlayerBaseController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void savePlayerState()
    {
        GlobalPlayerScript.Instance.lives = player.lives;
        GlobalPlayerScript.Instance.diamonds = player.diamonds;
        GlobalPlayerScript.Instance.specialDiamonds = player.specialDiamonds;
        GlobalPlayerScript.Instance.level = player.level;
        GlobalPlayerScript.Instance.hasJetpackUpgrade = player_upgrades.hasJetpackUpgrade;
        GlobalPlayerScript.Instance.hasShovelUpgrade = player_upgrades.hasShovelUpgrade;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            savePlayerState();
            loadingScreen.enabled = true;
            string levelName = "Level " + GlobalPlayerScript.Instance.level;
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }
    }
}
