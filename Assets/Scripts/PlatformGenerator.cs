using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {
    // All assets possible for blocks
    public GameObject block;

    public Sprite block1;
    public Sprite block2;
    public Sprite block3;
    public Sprite block4;
    public Sprite block5;
    public Sprite block6;
    public Sprite block7;
    private Sprite[] blockCollections;

    private const int PLATFORM_MIN_LENGTH = 10;
    private const int PLATFORM_MAX_LENGTH = 12;

    // Use this for initialization
    void Start()
    {
        blockCollections = new Sprite[] {block1, block2, block3, block4, block5, block6, block7};
        int platformLength = Random.Range(PLATFORM_MIN_LENGTH, PLATFORM_MAX_LENGTH);
        int startUpperLimit = Random.Range(platformLength + 3, PLATFORM_MAX_LENGTH + 5);
        for (int i=startUpperLimit; i>startUpperLimit-platformLength; i--)
        {
            createBlock(i, transform.position.y);
        }
    }

    void createBlock(int xCounter, float yPos)
    {
        float newX = transform.position.x + xCounter * 1;
        GameObject newBlock =  (GameObject) Instantiate(block, new Vector3(newX, yPos, 0), Quaternion.identity);
        newBlock.GetComponent<SpriteRenderer>().sprite = blockCollections[Random.Range(0, blockCollections.Length)];
        generateVerticalBlockOnChance(newX, yPos, 0.2);
    }

    private void generateVerticalBlockOnChance(float newX, float yPos, double chance)
    {
        float newY = yPos + 1;
        if (Random.Range(0f, 1f) <= chance)
        {
            GameObject newBlock = (GameObject) Instantiate(block, new Vector3(newX, yPos, 0), Quaternion.identity);
            newBlock.GetComponent<SpriteRenderer>().sprite = blockCollections[Random.Range(0, blockCollections.Length - 1)];
            generateVerticalBlockOnChance(newX, newY, chance - 0.05);
        }
    }
}
