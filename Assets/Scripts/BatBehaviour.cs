using UnityEngine;
using System.Collections;

public class BatBehaviour : MonsterBehaviour {

    private const float BAT_MOVE_X = -2.5f;
    private const float BAT_MOVE_Y = 0;
    private const float BAT_MOVE_Z = 0;
    private const bool BAT_IS_FLYING = true;
    private const bool BAT_IS_FACING_LEFT = true;
    private const bool BAT_IS_FACING_UP = false;

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
        this.MoveX = BAT_MOVE_X;
        this.MoveY = BAT_MOVE_Y;
        this.MoveZ = BAT_MOVE_Z;
        this.IsFlying = BAT_IS_FLYING;
        this.IsFacingLeft = BAT_IS_FACING_LEFT;
        this.IsFacingUp = BAT_IS_FACING_UP;
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
