using UnityEngine;
using System.Collections;

public class WormBehaviour : MonsterBehaviour {

    private const float WORM2_MOVE_X = 0;
    private const float WORM3_MOVE_X = 0;
    private const float WORM_MOVE_Y = 0;
    private const float WORM_MOVE_Z = 0;
    private const bool WORM_IS_FLYING = false;
    private const bool WORM_IS_FACING_LEFT = true;
    private const bool WORM_IS_FACING_UP = false;

    private const float WORM_ATTACK_RANGE_X = 1.5f;

    private Animator anim;
    private int level;

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
        anim = GetComponent<Animator>();
        if (gameObject.name == "Worm2(Clone)")
        {
            this.MoveX = WORM2_MOVE_X;
            level = 2;
        }
        else if (gameObject.name == "Worm3(Clone)")
        {
            this.MoveX = WORM3_MOVE_X;
            level = 3;
        }

        this.MoveY = WORM_MOVE_Y;
        this.MoveZ = WORM_MOVE_Z;
        this.IsFlying = WORM_IS_FLYING;
        this.IsFacingLeft = WORM_IS_FACING_LEFT;
        this.IsFacingUp = WORM_IS_FACING_UP;
        base.Start();
    }

    private void checkAttack()
    {
        if (!isAttackingAnim())
        {
            float topLeftX = transform.position.x - WORM_ATTACK_RANGE_X;
            float topLeftY = transform.position.y + 0.25f;
            float btmRightX = transform.position.x + WORM_ATTACK_RANGE_X;
            float btmRightY = transform.position.y - 0.25f;
            int intersection = Physics2D.OverlapAreaAll(new Vector2(topLeftX, topLeftY), new Vector2(btmRightX, btmRightY),1<<16).Length;

            if (intersection != 0)
            {
                anim.SetBool("isAttacking", true);
            }
        }
    }

    public override void Attack()
    {
        if (!isDamagingAnim())
        {
            return;
        }
        float topLeftX = transform.position.x - WORM_ATTACK_RANGE_X;
        float topLeftY = transform.position.y + 0.25f;
        float btmRightX = transform.position.x + WORM_ATTACK_RANGE_X;
        float btmRightY = transform.position.y - 0.25f;
        Collider2D[] col = Physics2D.OverlapAreaAll(new Vector2(topLeftX, topLeftY), new Vector2(btmRightX, btmRightY), 1 << 16);

        foreach (Collider2D collider in col)
        {
            PlayerCollisionController controller = collider.gameObject.GetComponent<PlayerCollisionController>();
            if (!controller.getHurt())
            {
                controller.enforceInjury();
            }
            
        }
    }

    private void updateAttack()
    {
        if (!isAttackingAnim())
        {
            anim.SetBool("isAttacking", false);
        }
    }

    private bool isAttackingAnim()
    {
        AnimatorStateInfo currState = anim.GetCurrentAnimatorStateInfo(0);

        return (currState.IsName("worm attack") || currState.IsName("begin attack"));
    }

    private bool isDamagingAnim()
    {
        AnimatorStateInfo currState = anim.GetCurrentAnimatorStateInfo(0);

        return (currState.IsName("worm attack"));
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (level == 3)
        {
            updateAttack();
            checkAttack();
            Attack();
        }
    }
}
