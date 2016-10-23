using UnityEngine;
using System.Collections;

public class GlobalPlayerScript : MonoBehaviour {

    public static GlobalPlayerScript Instance;
    public int lives;
    public int diamonds;
    public int specialDiamonds;
    public int level = 1;

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
