using UnityEngine;
using System.Collections;

public class DevilAnimatorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Animate();
	}

    void Animate()
    {
        Vector3 movement = new Vector3(transform.position.x, transform.position.y + 0.005f * Mathf.Sin(Time.time * 2), 0);
        transform.position = movement;
    }
}
