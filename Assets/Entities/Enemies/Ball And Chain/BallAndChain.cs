using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAndChain : BaseEnemyObject
{
    public float detectionRange;

    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        MoveToPlayer();

        EnemyJumping();

        if (Vector2.Distance(player.transform.position, transform.position) <= GetComponent<MeleeHit>().maxRangeForEnemyTriggering)
        {
            GetComponent<MeleeHit>().DoAbilityWithCooldown();
        }
    }

    public void MoveToPlayer()
    {
        if (IsGrounded())
        {
            if (Vector2.Distance(player.transform.position, transform.position) <= detectionRange)
            {
                FlipToPlayer();
                anim.SetBool("IsRunning", true);

                Vector2 dir = (Vector2)(player.transform.position - transform.position);
                dir = dir.normalized;

                rb.velocity = new Vector2(dir.normalized.x * moveSpeed, rb.velocity.y);
            }
            else
            {
                anim.SetBool("IsRunning", false);
            }
        }       
    }    
}
