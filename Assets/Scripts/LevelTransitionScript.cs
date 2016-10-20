using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelTransitionScript : MonoBehaviour {

    private AsyncOperation loadOp;

	// Use this for initialization
	void Start () {
        string levelName = "Level " + GlobalPlayerScript.Instance.level;
        print("Loading " + levelName);
        loadOp = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        loadOp.allowSceneActivation = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            loadOp.allowSceneActivation = true;
        }
    }
}
