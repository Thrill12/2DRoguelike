using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotWheel : BaseEnemyObject
{
    public float detectionRange;
    public GameObject deathScream;

    public override void Update()
    {
        base.Update();

        if (!isDead)
        {
            if (Vector2.Distance(player.transform.position, transform.position) <= detectionRange)
            {
                MoveToPlayer();
                if (player.transform.position.y > transform.position.y)
                {
                    EnemyJumping();
                }
            }
            else
            {
                anim.SetBool("IsWalking", false);
            }

            if (Vector2.Distance(player.transform.position, transform.position) < GetComponent<SimpleShooting>().maxDistanceForEnemyShooting)
            {
                ShootAtPlayer();
            }

            if (IsGrounded())
            {
                ResetJumpCounter();
            }
        }       
    }

    public void MoveToPlayer()
    {
        anim.SetBool("IsWalking", true);

        FlipToPlayer();
        Vector2 dirToPlayer = player.transform.position - transform.position;

        rb.velocity = new Vector2(dirToPlayer.normalized.x * moveSpeed, rb.velocity.y);
    }

    public void ShootAtPlayer()
    {
        if(health > 0)
        {
            GetComponent<SimpleShooting>().DoAbilityWithCooldown();
        }       
    }

    public override void OnDeath()
    {
        StopAllCoroutines();
        StartCoroutine(DeathCoro());
    }

    public IEnumerator DeathCoro()
    {
        if (!isDead)
        {
            isDead = true;
            anim.SetBool("IsWalking", false);
            anim.SetTrigger("Death");
            GameObject obj = Instantiate(deathScream, gameObject.transform.position, Quaternion.identity);
            obj.transform.parent = null;            
            yield return new WaitForSeconds(0.2f);
            base.OnDeath();
        }        
    }
}
