using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {
    public Transform block;
    
    // Use this for initialization
    void Start()
    {
        int platformLength = Random.Range(6, 10);
        int startUpperLimit = Random.Range(platformLength + 1, 17);
        for (int i=startUpperLimit; i>startUpperLimit-platformLength; i--)
        {
            float newX = transform.position.x + i * 1;
            Instantiate(block, new Vector3(newX, transform.position.y, 0), Quaternion.identity);
        }
        /*for (int x = 2; x < 18; x++)
        {
            float newX = transform.position.x + x * 1;
            Instantiate(block, new Vector3(newX, transform.position.y, 0), Quaternion.identity);
        }*/
    }
}
