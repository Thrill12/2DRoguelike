using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naga : BaseEnemyObject
{
    public float maxRange;
    public float minRange;

    private bool isCoroRunning = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

       

        if (health > 0)
        {            
            if (Vector2.Distance(transform.position, player.transform.position) < maxRange && Vector2.Distance(transform.position, player.transform.position) > minRange)
            {
                MoveToPlayer();
            }
            else
            {
                anim.SetBool("IsWalking", false);
            }

            if (rb.velocity.x == 0)
            {
                anim.SetBool("IsWalking", false);
            }

            if (Vector2.Distance(player.transform.position, transform.position) <= GetComponent<MeleeHit>().maxRangeForEnemyTriggering)
            {
                StartCoroutine(AttackCoro());
            }
        }       
    }

    public void MoveToPlayer()
    {
        FlipToPlayer();
        anim.SetBool("IsWalking", true);
        Vector2 dir = player.transform.position - transform.position;

        rb.velocity = new Vector2(dir.normalized.x * moveSpeed, dir.normalized.y * moveSpeed);
    }

    public IEnumerator AttackCoro()
    {
        if (isCoroRunning)
        {
            yield break;
        }

        isCoroRunning = true;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.6f);

        GetComponent<MeleeHit>().DoAbilityWithCooldown();

        yield return new WaitForSeconds(0.6f);
        isCoroRunning = false;    
    }
}
