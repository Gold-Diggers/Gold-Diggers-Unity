using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelTransitionScript : MonoBehaviour {

    private PlayerCollisionController player;
    private PlayerBaseController player_upgrades;
    private AsyncOperation loadOp;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerCollisionController>();
        string levelName = "Level " + GlobalPlayerScript.Instance.level;
        print("Loading " + levelName);
        loadOp = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        loadOp.allowSceneActivation = false;
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
            loadOp.allowSceneActivation = true;
        }
    }
}
