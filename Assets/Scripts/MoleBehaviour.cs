using UnityEngine;
using System.Collections;

public class MoleBehaviour : MonsterBehaviour {
    private const float MOLE_MOVE_X = -2f;
    private const float MOLE2_MOVE_X = -2.5f;
    private const float MOLE_MOVE_Y = 0;
    private const float MOLE_MOVE_Z = 0;
    private const bool MOLE_IS_FLYING = false;
    private const bool MOLE_IS_FACING_LEFT = true;
    private const bool MOLE_IS_FACING_UP = false;

    /*
     * To override methods, simply use the 'override' keyword.
     * Note that the method MUST have the 'virtual' keyword.
     * 
     * To use a method from the superclass, use 'base' keyword.
     * --> base.<method_name>(args)
     */

	// Use this for initialization
	public override void Start () {
        if (gameObject.name == "Mole(Clone)")
        {
            this.MoveX = MOLE_MOVE_X;
        }
        else if (gameObject.name == "Mole2(Clone)")
        {
            this.MoveX = MOLE2_MOVE_X;
        }

        this.MoveY = MOLE_MOVE_Y;
        this.MoveZ = MOLE_MOVE_Z;
        this.IsFlying = MOLE_IS_FLYING;
        this.IsFacingLeft = MOLE_IS_FACING_LEFT;
        this.IsFacingUp = MOLE_IS_FACING_UP;
        base.Start();
	}
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();
	}
}
