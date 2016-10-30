using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    /*public GameObject player;
    private Vector3 cameraOffset;
    private const float CAMERA_BOUNDS = 1.82f; // 1.52
    
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
    }*/
    public GameObject player;
    // private Vector3 cameraOffset;

    private float left = -0.2F; 
    private float right = 0.2F;
    private float top = 0.05F;
    private float bottom = -0.25F; // -0.15

    private const float CAM_Y_CANNOT_FALL_BEYOND = -96f;
    private const float CAM_Y_UPPER_BOUND = 999999f; // arbitrary large value.

    private int level;

    private float camYClamp;

    void Start()
    {
        // cameraOffset = player.transform.position;
        level = GameObject.Find("GlobalPlayer").GetComponent<GlobalPlayerScript>().level;
        if (level == 1)
        {
            camYClamp = CAM_Y_CANNOT_FALL_BEYOND; //- 87f;
        } else
        {
            camYClamp = CAM_Y_CANNOT_FALL_BEYOND;
        }
    }
    void LateUpdate()
    {
        Camera cam = Camera.main;
        Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
        cam.projectionMatrix = m;
        float yPos = Mathf.Clamp(player.transform.position.y, camYClamp, CAM_Y_UPPER_BOUND);
        cam.transform.position = new Vector3(cam.transform.position.x, yPos, cam.transform.position.z);
    }
    Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }
}
