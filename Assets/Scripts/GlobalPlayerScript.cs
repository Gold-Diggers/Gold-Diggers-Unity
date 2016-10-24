using UnityEngine;
using System.Collections;

public class GlobalPlayerScript : MonoBehaviour {

    public static GlobalPlayerScript Instance;
    public int lives;
    public int diamonds;
    public int specialDiamonds;
    public int level;
    public bool hasJetpackUpgrade;
    public bool hasShovelUpgrade;

    public void reinitialiseValues()
    {
        lives = 3;
        diamonds = 0;
        specialDiamonds = 0;
        level = 1;
        hasJetpackUpgrade = false;
        hasShovelUpgrade = false;
    }

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
