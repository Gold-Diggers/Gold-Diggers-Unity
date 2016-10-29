using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialSceneScript : MonoBehaviour {

    public Image tutorialScreen;
    public GameObject monster;
    public Vector3 monsterPosition;
    public GameObject monsterPrefab;
    public GameObject treasureChest;
    public Vector3 treasureChestPosition;
    public GameObject treasureChestPrefab;
    private bool toOpenTutorialScreen;
    private bool hasSpawnMonster;
    private bool hasSpawnChest;

	// Use this for initialization
	void Start () {
        tutorialScreen.enabled = false;
        toOpenTutorialScreen = false;
        hasSpawnMonster = false;
        hasSpawnChest = false;
        monsterPosition = monster.transform.position;
        treasureChestPosition = treasureChest.transform.position;
        Destroy(monster);
        monster = (GameObject) Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        Destroy(treasureChest);
        treasureChest = (GameObject)Instantiate(treasureChestPrefab, treasureChestPosition, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
        registerKeyInput();
        respawnAllRelevantObjects();
	}

    void registerKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            toOpenTutorialScreen = !toOpenTutorialScreen;
            toggleTutorialScreen(toOpenTutorialScreen);

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    void respawnAllRelevantObjects()
    {
        if (monster == null && !hasSpawnMonster)
        {
            hasSpawnMonster = true;
            StartCoroutine(spawnMonster());
        }
        if (treasureChest == null && !hasSpawnChest)
        {
            hasSpawnChest = true;
            StartCoroutine(spawnChest());
        }
    }

    IEnumerator spawnMonster()
    {
        hasSpawnMonster = true;
        yield return new WaitForSecondsRealtime(3F);
        monster = (GameObject)Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        hasSpawnMonster = false;
    }

    IEnumerator spawnChest()
    {
        hasSpawnChest = true;
        yield return new WaitForSecondsRealtime(3F);
        treasureChest = (GameObject)Instantiate(treasureChestPrefab, treasureChestPosition, Quaternion.identity);
        hasSpawnChest = false;
    }

    void toggleTutorialScreen(bool toOpen)
    {
        tutorialScreen.enabled = toOpen;
    }
}
