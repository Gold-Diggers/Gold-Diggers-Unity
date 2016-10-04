using UnityEngine;
using System.Collections;

public class WormBehaviour : MonsterBehaviour {

    private const float WORM_MOVE_X = -5f;
    private const float WORM_MOVE_Y = 0;
    private const float WORM_MOVE_Z = 0;
    private const bool WORM_IS_FLYING = false;
    private const bool WORM_IS_FACING_LEFT = true;
    private const bool WORM_IS_FACING_UP = false;

    /*
     * To override methods, simply use the 'override' keyword.
     * Note that the method MUST have the 'virtual' keyword.
     * 
     * To use a method from the superclass, use 'base' keyword.
     * --> base.<method_name>(args)
     */

    // Use this for initialization
    public override void Start()
    {
        this.MoveX = WORM_MOVE_X;
        this.MoveY = WORM_MOVE_Y;
        this.MoveZ = WORM_MOVE_Z;
        this.IsFlying = WORM_IS_FLYING;
        this.IsFacingLeft = WORM_IS_FACING_LEFT;
        this.IsFacingUp = WORM_IS_FACING_UP;
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
