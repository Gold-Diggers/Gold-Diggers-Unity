using UnityEngine;
using System.Collections;

public class GlobalPlayerScript : MonoBehaviour {

    public static GlobalPlayerScript Instance;
    public int lives = 3;
    public int diamonds = 0;
    public int specialDiamonds = 0;
    public int level = 1;
    public bool hasJetpackUpgrade = false;
    public bool hasShovelUpgrade = false;

	void Awake () {
	    if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
	}
}
