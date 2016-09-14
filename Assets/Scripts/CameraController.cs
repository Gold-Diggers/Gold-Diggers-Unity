using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 cameraOffset;
    private const float CAMERA_BOUNDS = 1.52f;
    
	// Use this for initialization
	void Start () {
        cameraOffset = transform.position - player.transform.position;
	}

    // Update is called once per frame
    void LateUpdate()
    {
        float yPos = player.transform.position.y + cameraOffset.y;
        float xPos = Mathf.Clamp(player.transform.position.x + cameraOffset.x, -CAMERA_BOUNDS , CAMERA_BOUNDS);
        float zPos = player.transform.position.z + cameraOffset.z;
        transform.position = new Vector3(xPos, yPos, zPos);

        //transform.position = player.transform.position + cameraOffset;
    } 
}
