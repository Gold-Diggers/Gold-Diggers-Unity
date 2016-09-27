using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {
    public Transform block;
    private const int PLATFORM_MIN_LENGTH = 8;
    private const int PLATFORM_MAX_LENGTH = 16;

    // Use this for initialization
    void Start()
    {
        int platformLength = Random.Range(PLATFORM_MIN_LENGTH, PLATFORM_MAX_LENGTH);
        int startUpperLimit = Random.Range(platformLength + 1, 18);
        for (int i=startUpperLimit; i>startUpperLimit-platformLength; i--)
        {
            createBlock(i, transform.position.y);
        }
    }

    void createBlock(int xCounter, float yPos)
    {
        float newX = transform.position.x + xCounter * 1;
        Instantiate(block, new Vector3(newX, yPos, 0), Quaternion.identity);
        generateVerticalBlockOnChance(newX, yPos, 0.2);
    }

    private void generateVerticalBlockOnChance(float newX, float yPos, double chance)
    {
        float newY = yPos + 1;
        if (Random.Range(0f, 1f) <= chance)
        {
            Instantiate(block, new Vector3(newX, newY, 0), Quaternion.identity);
            generateVerticalBlockOnChance(newX, newY, chance - 0.05);
        }
    }
}
