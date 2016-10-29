using UnityEngine;
using System.Collections;

public class ItemsGenerator : MonoBehaviour
{
    public GameObject background;
    public GameObject diamond;
    public GameObject diamond2;
    public GameObject trap;
    public GameObject trap2;
    public GameObject trap3;
    public GameObject treasureChest;
    public GameObject specialChest;
    public GameObject bat;
    public GameObject bat2;
    public GameObject bat3;
    public GameObject mole;
    public GameObject mole2;
    public GameObject mole3;
    public GameObject worm2;
    public GameObject worm3;

    private int level;

    private const float CHANCE_SPAWN_BETTER_DIAMOND = 0.05f;

    private Collider2D[] allPlatforms;
    // Handle the bounds of the map / background
    private Vector3 topLeft;
    private Vector3 btmRight;

    // constants
    private const float DIST_NO_SPAWN_FROM_BTM = 30f;
    // Regular Vertical
    private const int NUM_REGULAR_VERTICAL_SET = 3; // number of sets
    private const int NUM_REGULAR_VERTICAL_SET_LEVEL_2 = 4;
    private const int NUM_REGULAR_VERTICAL_SET_LEVEL_3 = 5;
    private const int NUM_IN_REGULAR_VERTICAL = 3; // 1 set consist of how many diamonds
    private const float SPACING_REGULAR_VERTICAL = 1.75f;
    // Regular Horizontal
    private const int NUM_REGULAR_HORIZONTAL_SET = 3; // number of sets
    private const int NUM_REGULAR_HORIZONTAL_SET_LEVEL_2 = 4;
    private const int NUM_REGULAR_HORIZONTAL_SET_LEVEL_3 = 5;
    private const int NUM_IN_REGULAR_HORIZONTAL = 3; // 1 set consist of how many diamonds
    private const float SPACING_REGULAR_HORIZONTAL = 1.75f;
    private const float SPACING_OFF_GROUND_REGULAR_HORIZONTAL = 1.25f;
    // Regular diagonal right
    private const int NUM_REGULAR_DIAGONAL_R_SET = 3; // number of sets
    private const int NUM_REGULAR_DIAGONAL_R_SET_LEVEL_2 = 4;
    private const int NUM_REGULAR_DIAGONAL_R_SET_LEVEL_3 = 5;
    private const int NUM_IN_REGULAR_DIAGONAL_R = 3; // 1 set consist of how many diamonds
    private const float SPACING_REGULAR_DIAGONAL_R = 1.25f;
    // Regular diagonal left
    private const int NUM_REGULAR_DIAGONAL_L_SET = 3; // number of sets
    private const int NUM_REGULAR_DIAGONAL_L_SET_LEVEL_2 = 4;
    private const int NUM_REGULAR_DIAGONAL_L_SET_LEVEL_3 = 5;
    private const int NUM_IN_REGULAR_DIAGONAL_L = 3; // 1 set consist of how many diamonds
    private const float SPACING_REGULAR_DIAGONAL_L = 1.25f;
    // Lone diamond
    private const int NUM_LONE_DIAMOND_SET = 2;
    private const int NUM_LONE_DIAMOND_SET_LEVEL_2 = 6;
    private const int NUM_LONE_DIAMOND_SET_LEVEL_3 = 7;
    private const float SPACING_OFF_GROUND_LONE_DIAMOND = 1.25f;

    // Vertical diamond with spike
    private const int NUM_TOTAL_SPIKES = 8; // inclusive of those with diamonds
    private const int NUM_TOTAL_SPIKES_LEVEL_TWO = 12;
    private const int NUM_TOTAL_SPIKES_LEVEL_THREE = 16;
    private const int NUM_VERTICAL_SPIKE = 3; // number of sets
    private const int NUM_VERTICAL_SPIKE_LEVEL_TWO = 4;
    private const int NUM_VERTICAL_SPIKE_LEVEL_THREE = 7;
    private const int NUM_IN_VERTICAL_SPIKE = 3; // 1 set consist of how many diamonds
    private const float SPACING_VERTICAL_SPIKE = 1.5f;
    private const float SPACING_OFF_GROUND_VERTICAL_SPIKE = 1.1f;

    // Horizontal diamond with chest
    private const int NUM_TOTAL_CHESTS = 3; // inclusive of those with diamonds
    private const int NUM_TOTAL_CHESTS_LEVEL_TWO = 4;
    private const int NUM_TOTAL_CHESTS_LEVEL_THREE = 5;
    private const int NUM_HORIZONTAL_CHEST = 1; // number of sets
    private const int NUM_HORIZONTAL_CHEST_LEVEL_TWO = 3;
    private const int NUM_HORIZONTAL_CHEST_LEVEL_THREE = 4;
    private const int NUM_IN_HORIZONTAL_CHEST = 3; // 1 set consist of how many diamonds
    private const float SPACING_HORIZONTAL_CHEST = 1.75f;
    private const float SPACING_OFF_GROUND_HORIZONTAL_CHEST = 1.1f;

    // special chest
    private const float SPACING_OFF_GROUND_CHEST = 1.1f;
    // Monsters
    private const int NUM_BATS = 8;
    private const int NUM_BATS_LEVEL_2 = 8;
    private const int NUM_BATS_LEVEL_3 = 13;
    private const int NUM_MOLES = 10;
    private const int NUM_MOLES_LEVEL_2 = 12;
    private const int NUM_MOLES_LEVEL_3 = 13;
    private const int NUM_WORMS = 0;
    private const int NUM_WORMS_LEVEL_2 = 2;
    private const int NUM_WORMS_LEVEL_3 = 5;
    private const float SPACING_OFF_GROUND_MOLES = 1f;
    private const float SPACING_OFF_GROUND_WORMS = 1.3f;

    void Start()
    {
        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
        topLeft = transform.position;
        Vector3 sizeBg = sr.bounds.size;
        btmRight = new Vector3(topLeft.x + sizeBg.x, topLeft.y - sizeBg.y + DIST_NO_SPAWN_FROM_BTM, topLeft.z);

        level = GlobalPlayerScript.Instance.level;

        storeAllPlatforms();
        generateDiamonds();
        generateSpecialChest();
        generateMonsters();
    }

    private void storeAllPlatforms()
    {
        Vector2 ptA = new Vector2(topLeft.x, topLeft.y);
        Vector2 ptB = new Vector2(btmRight.x, btmRight.y);
        allPlatforms = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 8); // platform detection
    }

    private void generateDiamonds()
    {
        generateVerticalRegularSet();
        generateHorizontalRegularSet();
        generateDiagonalRightRegularSet();
        generateDiagonalLeftRegularSet();
        generateLoneDiamondSet();
        generateSpikes();
        generateChests();
    }

    private void generateMonsters()
    {
        // Bats
        int numBats = 0;
        if (level == 1)
        {
            numBats = NUM_BATS;
        } else if (level == 2)
        {
            numBats = NUM_BATS_LEVEL_2;
        } else if (level == 3)
        {
            numBats = NUM_BATS_LEVEL_3;
        }

        // Moles
        int numMoles = 0;
        if (level == 1)
        {
            numMoles = NUM_MOLES;
        }
        else if (level == 2)
        {
            numMoles = NUM_MOLES_LEVEL_2;
        }
        else if (level == 3)
        {
            numMoles = NUM_MOLES_LEVEL_3;
        }

        // Worms
        int numWorms = 0;
        if (level == 1)
        {
            numWorms = NUM_WORMS;
        }
        else if (level == 2)
        {
            numWorms = NUM_WORMS_LEVEL_2;
        }
        else if (level == 2)
        {
            numWorms = NUM_WORMS_LEVEL_3;
        }

        for (int i = 0; i < numBats; i++)
        {
            spawnBat();
        }
        for (int i = 0; i < numMoles; i++)
        {
            spawnMole();
        }
        for (int i = 0; i < numWorms; i++)
        {
            spawnWorm();
        }
    }

    private void generateSpikes()
    {
        int numSpikes = 0;
        if (level == 1)
        {
            numSpikes = NUM_TOTAL_SPIKES;
        } else if (level == 2)
        {
            numSpikes = NUM_TOTAL_SPIKES_LEVEL_TWO;
        } else if (level == 3)
        {
            numSpikes = NUM_TOTAL_SPIKES_LEVEL_THREE;
        }

        int numVerticalSpike = 0;
        if (level == 1)
        {
            numVerticalSpike = NUM_VERTICAL_SPIKE;
        } else if (level == 2)
        {
            numVerticalSpike = NUM_VERTICAL_SPIKE_LEVEL_TWO;
        } else if (level == 3)
        {
            numVerticalSpike = NUM_VERTICAL_SPIKE_LEVEL_THREE;
        }

        for (int i = 0; i < numSpikes; i++)
        {
            if (i < numVerticalSpike)
            {
                generateVerticalSpike();
            } else
            {
                generateSpike();
            }
         }
    }

    private void generateChests()
    {
        int numChests = 0;
        if (level == 1)
        {
            numChests = NUM_TOTAL_CHESTS;
        }
        else if (level == 2)
        {
            numChests = NUM_TOTAL_CHESTS_LEVEL_TWO;
        }
        else if (level == 3)
        {
            numChests = NUM_TOTAL_CHESTS_LEVEL_THREE;
        }

        int numHorizontalChests = 0;
        if (level == 1)
        {
            numHorizontalChests = NUM_HORIZONTAL_CHEST;
        }
        else if (level == 2)
        {
            numHorizontalChests = NUM_HORIZONTAL_CHEST_LEVEL_TWO;
        }
        else if (level == 3)
        {
            numHorizontalChests = NUM_HORIZONTAL_CHEST_LEVEL_THREE;
        }

        for (int i = 0; i < numChests; i++)
        {
            if (i < numHorizontalChests)
            {
                generateHorizontalChest();
            }
            else
            {
                generateChest();
            }
        }
    }

    private void generateLoneDiamondSet()
    {
        int numLone = 0;
        if (level == 1)
        {
            numLone = NUM_LONE_DIAMOND_SET;
        } else if (level == 2)
        {
            numLone = NUM_LONE_DIAMOND_SET_LEVEL_2;
        } else if (level == 3)
        {
            numLone = NUM_LONE_DIAMOND_SET_LEVEL_3;
        }

        for (int i = 0; i < numLone; i++)
        {
            generateLoneDiamond();
        }
    }

    private void generateDiagonalRightRegularSet()
    {
        int numRegDiagR = 0;
        if (level == 1)
        {
            numRegDiagR = NUM_REGULAR_DIAGONAL_R_SET;
        } else if (level == 2)
        {
            numRegDiagR = NUM_REGULAR_DIAGONAL_R_SET_LEVEL_2;
        } else if (level == 3)
        {
            numRegDiagR = NUM_REGULAR_DIAGONAL_R_SET_LEVEL_3;
        }

        for (int i = 0; i < numRegDiagR; i++)
        {
            generateDiagonalRightRegular();
        }
    }

    private void generateDiagonalLeftRegularSet()
    {
        int numRegDiagL = 0;
        if (level == 1)
        {
            numRegDiagL = NUM_REGULAR_DIAGONAL_L_SET;
        } else if (level == 2)
        {
            numRegDiagL = NUM_REGULAR_DIAGONAL_L_SET_LEVEL_2;
        } else if (level == 3)
        {
            numRegDiagL = NUM_REGULAR_DIAGONAL_L_SET_LEVEL_3;
        }

        for (int i = 0; i < numRegDiagL; i++)
        {
            generateDiagonalLeftRegular();
        }
    }

    private void generateVerticalRegularSet()
    {
        int numRegVert = 0;
        if (level == 1)
        {
            numRegVert = NUM_REGULAR_VERTICAL_SET;
        } else if (level == 2)
        {
            numRegVert = NUM_REGULAR_VERTICAL_SET_LEVEL_2;
        } else if (level == 3)
        {
            numRegVert = NUM_REGULAR_VERTICAL_SET_LEVEL_3;
        }

        for (int i = 0; i < numRegVert; i++)
        {
            generateVerticalRegular();
        }
    }

    private void generateHorizontalRegularSet()
    {
        int numRegHori = 0;
        if (level == 1)
        {
            numRegHori = NUM_REGULAR_HORIZONTAL_SET;
        } else if (level == 2)
        {
            numRegHori = NUM_REGULAR_HORIZONTAL_SET_LEVEL_2;
        } else if (level == 3)
        {
            numRegHori = NUM_REGULAR_HORIZONTAL_SET_LEVEL_3;
        }

        for (int i = 0; i < numRegHori; i++)
        {
            generateHorizontalRegular();
        }
    }

    private void spawnBat()
    {
        float randX = Random.Range(topLeft.x, btmRight.x);
        float randY = Random.Range(btmRight.y, topLeft.y);
        
        // Check if spawning bats will collide
        Vector2 ptA = new Vector2((float)(randX - 0.5), (float)(randY + 0.5));
        Vector2 ptB = new Vector2((float)(randX + 0.5), (float)(randY - 0.5));
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 262143);
        if (col.Length > 0 ||  isCollideWithMonster(randX, randY)) // If collide with something else, spawn again.
        {
            spawnBat();
            return;
        }
        // Spawn bat
        if (level == 1)
        {
            Instantiate(bat, new Vector3(randX, randY, 0), Quaternion.identity);
        } else if (level == 2)
        {
            Instantiate(bat2, new Vector3(randX, randY, 0), Quaternion.identity);
        } else if (level == 3)
        {
            Instantiate(bat3, new Vector3(randX, randY, 0), Quaternion.identity);
        }

    }

    private bool isCollideWithMonster(float x, float y)
    {
        Vector2 ptA = new Vector2((float)(x - 3), (float)(y + 3));
        Vector2 ptB = new Vector2((float)(x + 3), (float)(y - 3));
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 10);

        return (col.Length > 0);
    }

    private bool isCollideWithSpike(float x, float y)
    {
        Vector2 ptA = new Vector2((float)(x - 20), (float)(y + 3));
        Vector2 ptB = new Vector2((float)(x + 20), (float)(y - 3));
        Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 1 << 11);

        return (col.Length > 0);
    }

    private void spawnMole()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_MOLES;

        // Check if spawning mole will collide
        Vector2 ptC = new Vector2((float)(randX - 0.5), (float)(randY + 0.5));
        Vector2 ptD = new Vector2((float)(randX + 0.5), (float)(randY - 0.5));
        Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

        if (coll.Length > 0 || isCollideWithMonster(randX, randY)) // If collide with something else, spawn again.
        {
            spawnMole();
            return;
        }

        // Spawn mole
        if (level == 1)
        {
            Instantiate(mole, new Vector3(randX, randY - 0.1f, 0), Quaternion.identity);
        } else if (level == 2)
        {
            Instantiate(mole2, new Vector3(randX, randY - 0.1f, 0), Quaternion.identity);
        } else if (level == 3)
        {
            Instantiate(mole3, new Vector3(randX, randY - 0.1f, 0), Quaternion.identity);
        }

    }

    private void spawnWorm()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_WORMS;

        // Check if spawning worm will collide
        Vector2 ptC = new Vector2((float)(randX - 0.5), (float)(randY + 0.5));
        Vector2 ptD = new Vector2((float)(randX + 0.5), (float)(randY - 0.5));
        Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

        if (coll.Length > 0 || isCollideWithMonster(randX, randY)) // If collide with something else, spawn again.
        {
            spawnWorm();
            return;
        }

        // Spawn worm
        if (level == 1)
        {
            // level 1 has no worm.
            print("Error in spawn worm for level 1");
        }
        else if (level == 2)
        {
            Instantiate(worm2, new Vector3(randX, randY - 0.1f, 0), Quaternion.identity);
        }
        else if (level == 3)
        {
            Instantiate(worm3, new Vector3(randX, randY - 0.1f, 0), Quaternion.identity);
        }

    }

    private void generateSpike()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_VERTICAL_SPIKE;

        // Check if spawning spikes will collide
        Vector2 ptC = new Vector2((float)(randX - 0.5), (float)(randY + 0.5));
        Vector2 ptD = new Vector2((float)(randX + 0.5), (float)(randY - 0.5));
        Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

        if (coll.Length > 0 || isCollideWithSpike(randX, randY)) // If collide with something else, spawn again.
        {
            generateSpike();
            return;
        }
        
        // Spawn spike
        if (level == 1)
        {
            Instantiate(trap, new Vector3(randX, randY + 0.1f, 0), Quaternion.identity);
        } else if (level == 2)
        {
            Instantiate(trap2, new Vector3(randX, randY + 0.1f, 0), Quaternion.identity);
        } else if (level == 3)
        {
            Instantiate(trap3, new Vector3(randX, randY + 0.1f, 0), Quaternion.identity);
        }
        
    }

    private void generateChest()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_CHEST;

        // Check if spawning chests will collide
        Vector2 ptC = new Vector2((float)(randX - 0.5), (float)(randY + 0.5));
        Vector2 ptD = new Vector2((float)(randX + 0.5), (float)(randY - 0.5));
        Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

        if (coll.Length > 0) // If collide with something else, spawn again.
        {
            generateChest();
            return;
        }

        // Spawn chest
        Instantiate(treasureChest, new Vector3(randX, randY, 0), Quaternion.identity);
    }

    private void generateSpecialChest()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_CHEST;

        // Check if spawning chests will collide
        Vector2 ptC = new Vector2((float)(randX - 0.5), (float)(randY + 0.5));
        Vector2 ptD = new Vector2((float)(randX + 0.5), (float)(randY - 0.5));
        Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

        if (coll.Length > 0) // If collide with something else, spawn again.
        {
            generateSpecialChest();
            return;
        }

        // Spawn special chest
        Instantiate(specialChest, new Vector3(randX, randY, 0), Quaternion.identity);
    }

    private void generateDiagonalLeftRegular()
    {
        float randX = Random.Range(topLeft.x, btmRight.x);
        float randY = Random.Range(btmRight.y, topLeft.y);
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_REGULAR_DIAGONAL_L; i++)
        { // Check if spawning diamonds will collide
            Vector2 ptA = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptB = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 262143);
            if (col.Length > 0) // If collide with something else, spawn again.
            {
                generateDiagonalLeftRegular();
                return;
            }
            tempX -= SPACING_REGULAR_DIAGONAL_L;
            tempY -= SPACING_REGULAR_DIAGONAL_L;
        }
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_REGULAR_DIAGONAL_L; i++)
        {
            spawnDiamond(randX, randY);
            randX -= SPACING_REGULAR_DIAGONAL_L;
            randY -= SPACING_REGULAR_DIAGONAL_L;
        }
    }

    private void generateDiagonalRightRegular()
    {
        float randX = Random.Range(topLeft.x, btmRight.x);
        float randY = Random.Range(btmRight.y, topLeft.y);
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_REGULAR_DIAGONAL_R; i++)
        { // Check if spawning diamonds will collide
            Vector2 ptA = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptB = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 262143);
            if (col.Length > 0) // If collide with something else, spawn again.
            {
                generateDiagonalRightRegular();
                return;
            }
            tempX += SPACING_REGULAR_DIAGONAL_R;
            tempY -= SPACING_REGULAR_DIAGONAL_R;
        }
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_REGULAR_DIAGONAL_R; i++)
        {
            spawnDiamond(randX, randY);
            randX += SPACING_REGULAR_DIAGONAL_R;
            randY -= SPACING_REGULAR_DIAGONAL_R;
        }
    }

    private void generateHorizontalChest()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_HORIZONTAL_CHEST;
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_HORIZONTAL_CHEST + 1; i++)
        { // Check if spawning diamonds will collide
            Vector2 ptC = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptD = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

            if (coll.Length > 0) // If collide with something else, spawn again.
            {
                generateHorizontalChest();
                return;
            }
            tempX += SPACING_HORIZONTAL_CHEST;
        }
        
        // Spawn chest
        Instantiate(treasureChest, new Vector3(randX, randY, 0), Quaternion.identity);
        randX += SPACING_HORIZONTAL_CHEST;
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_HORIZONTAL_CHEST; i++)
        {
            spawnDiamond(randX, randY);
            randX += SPACING_HORIZONTAL_CHEST;
        }
    }

    private void generateHorizontalRegular()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_REGULAR_HORIZONTAL;
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_REGULAR_HORIZONTAL; i++)
        { // Check if spawning diamonds will collide
            Vector2 ptC = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptD = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

            if (coll.Length > 0) // If collide with something else, spawn again.
            {
                generateHorizontalRegular();
                return;
            }
            tempX += SPACING_REGULAR_HORIZONTAL;
        }
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_REGULAR_HORIZONTAL; i++)
        {
            spawnDiamond(randX, randY);
            randX += SPACING_REGULAR_HORIZONTAL;
        }
    }

    private void generateVerticalSpike()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_VERTICAL_SPIKE;
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_VERTICAL_SPIKE + 1; i++)
        { // Check if spawning diamonds will collide
            Vector2 ptC = new Vector2((float)(tempX - 0.3), (float)(tempY + 0.3));
            Vector2 ptD = new Vector2((float)(tempX + 0.3), (float)(tempY - 0.3));
            Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

            if (coll.Length > 0 || isCollideWithSpike(tempX, tempY)) // If collide with something else, spawn again.
            {
                generateVerticalSpike();
                return;
            }
            tempY += SPACING_VERTICAL_SPIKE;
        }
        // Spawn spike
        if (level == 1)
        {
            Instantiate(trap, new Vector3(randX, randY + 0.1f, 0), Quaternion.identity);
        } else if (level == 2)
        {
            Instantiate(trap2, new Vector3(randX, randY + 0.1f, 0), Quaternion.identity);
        } else if (level == 3)
        {
            Instantiate(trap3, new Vector3(randX, randY + 0.1f, 0), Quaternion.identity);
        }
        
        randY += SPACING_VERTICAL_SPIKE;
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_VERTICAL_SPIKE; i++)
        {
            spawnDiamond(randX, randY);
            randY += SPACING_VERTICAL_SPIKE;
        }
    }

    private void generateVerticalRegular()
    {
        float randX = Random.Range(topLeft.x, btmRight.x);
        float randY = Random.Range(btmRight.y, topLeft.y);
        float tempX = randX;
        float tempY = randY;

        for (int i = 0; i < NUM_IN_REGULAR_VERTICAL; i++ )
        { // Check if spawning diamonds will collide
            Vector2 ptA = new Vector2((float)(tempX - 0.5), (float)(tempY + 0.5));
            Vector2 ptB = new Vector2((float)(tempX + 0.5), (float)(tempY - 0.5));
            Collider2D[] col = Physics2D.OverlapAreaAll(ptA, ptB, 262143);
            if (col.Length > 0) // If collide with something else, spawn again.
            {
                generateVerticalRegular();
                return;
            }
            tempY += SPACING_REGULAR_VERTICAL;
        }
        // Spawn all diamonds
        for (int i = 0; i < NUM_IN_REGULAR_VERTICAL; i++)
        {
            spawnDiamond(randX, randY);
            randY += SPACING_REGULAR_VERTICAL;
        }
    }

    private void generateLoneDiamond()
    {
        int choiceOfPlatform = Random.Range(0, allPlatforms.Length - 1);
        Collider2D chosenPlatform = allPlatforms[choiceOfPlatform];

        float randX = chosenPlatform.transform.position.x;
        float randY = chosenPlatform.transform.position.y + SPACING_OFF_GROUND_LONE_DIAMOND;

        // Check if spawning diamond will collide
        Vector2 ptC = new Vector2((float)(randX - 0.5), (float)(randY + 0.5));
        Vector2 ptD = new Vector2((float)(randX + 0.5), (float)(randY - 0.5));
        Collider2D[] coll = Physics2D.OverlapAreaAll(ptC, ptD, 262143);

        if (coll.Length > 0) // If collide with something else, spawn again.
        {
            generateLoneDiamond();
            return;
        }

        // Spawn lone diamond
        spawnDiamond(randX, randY);
    }

    private void spawnDiamond(float x, float y)
    {
        if (Random.Range(0f, 1f) <= CHANCE_SPAWN_BETTER_DIAMOND)
        {
            Instantiate(diamond2, new Vector3(x, y, 0), Quaternion.identity);
        } else
        {
            Instantiate(diamond, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}

