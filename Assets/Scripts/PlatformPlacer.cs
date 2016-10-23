using UnityEngine;
using System.Collections;

public class PlatformPlacer : MonoBehaviour {
    private Vector3 currPos;
    private const int NUM_LEVEL_OF_PLATFORMS = 21;
    private const float GAP_BTW_PLATFORM_MIN = 5;
    private const float GAP_BTW_PLATFORM_MAX = 7;

    // All assets possible for blocks
    public GameObject block;

    public Sprite block1;
    public Sprite block2;
    public Sprite block3;
    public Sprite block4;
    public Sprite block5;
    public Sprite block6;
    public Sprite block7;
    public Sprite block_level2;
    private Sprite[] blockCollections;

    private const int PLATFORM_MIN_LENGTH = 3;
    private const int PLATFORM_MAX_LENGTH = 12;
    private const float CHANCE_FOR_HOLE_IN_PLATFORM = 0.1f;

    // Use this for initialization
    void Start()
    {
        PlayerCollisionController player = FindObjectOfType<PlayerCollisionController>();
        int level = player.level;
        if (level == 1)
        {
            blockCollections = new Sprite[] { block1, block2, block3, block4, block5, block6, block7 };
        } else if (level == 2)
        {
            blockCollections = new Sprite[] { block_level2 };
        } else
        {
            print("Error in PlatformPlacer.cs for level checking.");
        }
        

        currPos = transform.position;
        float randY = currPos.y;
        for (int i = 0; i < NUM_LEVEL_OF_PLATFORMS; i++)
        {
            randY -= Random.Range(GAP_BTW_PLATFORM_MIN, GAP_BTW_PLATFORM_MAX);
            generateBlockAtLevel(currPos.x, randY);
        }
    }

    void generateBlockAtLevel(float xPos, float yPos)
    {
        int platformLength = Random.Range(PLATFORM_MIN_LENGTH, PLATFORM_MAX_LENGTH);
        int startUpperLimit = Random.Range(platformLength + 5, PLATFORM_MAX_LENGTH + 6);
        for (int i = startUpperLimit; i > startUpperLimit - platformLength; i--)
        {
            createBlock(xPos, yPos, i);
            //createBlock(i, transform.position.y);
        }
    }

    void createBlock(float xPos, float yPos, int xCounter)
    {
        if (Random.Range(0f, 1f) <= CHANCE_FOR_HOLE_IN_PLATFORM)
        {
            return;
        }
        float newX = xPos + xCounter * 1;
        GameObject newBlock = (GameObject)Instantiate(block, new Vector3(newX, yPos, 0), Quaternion.identity);
        newBlock.GetComponent<SpriteRenderer>().sprite = blockCollections[Random.Range(0, blockCollections.Length)];
        generateVerticalBlockOnChance(newX, yPos, 0.2);
    }

    private void generateVerticalBlockOnChance(float newX, float yPos, double chance)
    {
        float newY = yPos + 1;
        if (Random.Range(0f, 1f) <= chance)
        {
            GameObject newBlock = (GameObject)Instantiate(block, new Vector3(newX, yPos, 0), Quaternion.identity);
            newBlock.GetComponent<SpriteRenderer>().sprite = blockCollections[Random.Range(0, blockCollections.Length - 1)];
            generateVerticalBlockOnChance(newX, newY, chance - 0.05);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
