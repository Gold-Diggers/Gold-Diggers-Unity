using UnityEngine;
using System.Collections;

public class MonsterBehaviour : MonoBehaviour {

    private const float EDGE_DETECTION_THRESHOLD = 0.05f;

    public Vector3 movements;
    private Bounds monsterBound;

    private SpriteRenderer mobSpriteRend;

    // Attributes that will be wrapped around with by the 'property' wrapper for OOP
    private float moveX = 0;
    private float moveY = 0;
    private float moveZ = 0;
    private bool isFlying = false;
    private bool isFacingLeft = true;    // boolean variable to test for monster facing-direction in horizontal axis
    private bool isFacingUp = false;     // boolean variable to test for monster facing-direction in vertical axis

    /*
     * In C#, fields (or attributes) are immutable.
     * Therefore, we wrap these attributes into a property so we can get and set their values.
     * To wrap an attribute into a property, we do this:
     * 
     * <visibility> <data_type> <property_name>
     * {
     *     get
     *     {
     *         return <attribute_name>;
     *     }
     *     set
     *     {
     *         <attribute_name> = value; (NOTE: 'value' here is a keyword)
     *     }
     * }
     */
    public float MoveX
    {
        get
        {
            return moveX;
        }
        set
        {
            moveX = value;
        }
    }

    public float MoveY
    {
        get
        {
            return moveY;
        }
        set
        {
            moveY = value;
        }
    }

    public float MoveZ
    {
        get
        {
            return moveZ;
        }
        set
        {
            moveZ = value;
        }
    }

    public bool IsFlying
    {
        get
        {
            return isFlying;
        }
        set
        {
            isFlying = value;
        }
    }

    public bool IsFacingLeft
    {
        get
        {
            return isFacingLeft;
        }
        set
        {
            isFacingLeft = value;
        }
    }

    public bool IsFacingUp
    {
        get
        {
            return isFacingUp;
        }
        set
        {
            isFacingUp = value;
        }
    }

    /*
     * Implementing inheritance and polymorphism:
     * Use 'virtual' keyword to allow the method to be overriden by subclasses.
     * 
     * Note that:
     * a) Non-virtual methods cannot be overriden.
     * b) You cannot use 'virtual' keyword on 'static',
     *    'abstract' (default blank visibility), 'private' (visibility)
     *    and 'override' modifiers
     */

    // Use this for initialization
    public virtual void Start()
    {
        mobSpriteRend = GetComponent<SpriteRenderer>();
        monsterBound = GetComponent<Collider2D>().bounds;
        InitializeMovements();
    }

    public virtual void InitializeMovements()
    {
        movements = new Vector3(moveX, moveY, moveZ);
    }

    public virtual void FixedUpdate()
    {
        if (!isFlying)
        {
            checkMovementLimits();
        }
        
        Move(IsFacingLeft, IsFacingUp);
    }

    public virtual void Move(bool toMoveLeft, bool toMoveUp)
    {
        adjustHorizontalDirection(toMoveLeft);
        transform.position += movements * Time.deltaTime;
    }

    public virtual void adjustHorizontalDirection(bool toMoveLeft)
    {
        if (toMoveLeft)
        {
            movements.x = -1 * Mathf.Abs(movements.x);
        }
        else if (!toMoveLeft)
        {
            movements.x = Mathf.Abs(movements.x);
        }
    }

    public virtual void checkMovementLimits()
    {
        Vector3 diff = monsterBound.max - monsterBound.min;
        /*
         *            ______
         *           |      |
         *           | Mons |
         *           |      |
         *   |``````|````````|``````|
         *   |   A  |        |  B   |
         *   |      |        |      |
         *   ````````        ````````
         * A and B are bounded regions used to test movement limit
         * (i.e. detecting platform edges on left and right).
         * 
         * There is a defined EDGE_DETECTION_THRESHOLD between each bounding
         * region and the monster to avoid touching-borders counting towards
         * the intersection count in OverlapAreaAll() method.
         * 
         * The diagonal points are taken in general convention,
         * i.e. bottom-left and top-right corners.
         */
        float leftBoundAX = transform.position.x - 1f * diff.x - EDGE_DETECTION_THRESHOLD;
        float leftBoundAY = transform.position.y - 1f * diff.y;
        float leftBoundBX = transform.position.x - 0.5f * diff.x - EDGE_DETECTION_THRESHOLD;
        float leftBoundBY = transform.position.y - 0.5f * diff.y;
        float rightBoundAX = transform.position.x + 0.5f * diff.x + EDGE_DETECTION_THRESHOLD;
        float rightBoundAY = leftBoundAY;
        float rightBoundBX = transform.position.x + 1f * diff.x + EDGE_DETECTION_THRESHOLD;
        float rightBoundBY = leftBoundBY;
        int leftIntersections = Physics2D.OverlapAreaAll(new Vector2(leftBoundAX, leftBoundAY), new Vector2(leftBoundBX, leftBoundBY), 1<<8).Length;
        int rightIntersections = Physics2D.OverlapAreaAll(new Vector2(rightBoundAX, rightBoundAY), new Vector2(rightBoundBX, rightBoundBY), 1<<8).Length;
        if (leftIntersections == 0)
        {
            IsFacingLeft = false;
            flipMob(IsFacingLeft);
        }
        if (rightIntersections == 0)
        {
            IsFacingLeft = true;
            flipMob(IsFacingLeft);
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("BackgroundBoundary"))
        {
            handleHorizontalMovements();
        } else if (coll.gameObject.CompareTag("Platform"))
        {
             handleHorizontalMovements();
        }
    }

    private void handleHorizontalMovements()
    {
        if (IsFacingLeft)
        {
            IsFacingLeft = false;
            flipMob(IsFacingLeft);
        }
        else if (!IsFacingLeft)
        {
            IsFacingLeft = true;
            flipMob(IsFacingLeft);
        }
    }

    private void flipMob(bool IsFacingLeft)
    {
        if (IsFacingLeft)
        {
            mobSpriteRend.flipX = false;
        } else
        {
            mobSpriteRend.flipX = true;
        }
    }

    // Deliberately kept empty; This is to prepare for the introduction of the worm monsters
    public virtual void Attack()
    {

    }
}
