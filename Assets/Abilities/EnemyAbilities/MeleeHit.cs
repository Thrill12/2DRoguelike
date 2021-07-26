using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeHit : BaseAbility
{
    public float damage;
    public float attackPointRange;
    public GameObject attackPoint;
    public LayerMask layerToAttack;
    public Animator anim;
    public AudioSource hitSource;
    public bool shouldAnimate = true;

    public override void Update()
    {
        base.Update();        
    }

    public override void DoAbility()
    {
        base.DoAbility();

        BaseEntity obj = GetComponent<BaseEntity>();
        if (shouldAnimate)
        {
            obj.AttackAnim();
        }       

        Collider2D[] cols = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackPointRange, layerToAttack);

        if(anim != null)
        {
            if (shouldAnimate)
            {
                anim.SetTrigger("Attack");
            }
        }

        if(hitSource != null)
        {
            hitSource.Play();
        }

        for(int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<BaseEntity>())
            {
                cols[i].GetComponent<BaseEntity>().TakeDamage(damage, gameObject.GetComponent<BaseEntity>());
                CheckForOnHits(cols[i].gameObject);
            }
        }
    }

    public void CheckForOnHits(GameObject entityAffected)
    {
        CheckForBleed(entityAffected);
        CheckForIgnite(entityAffected);
    }

    public void CheckForBleed(GameObject entityAffected)
    {
        int rand = Random.Range(1, 101);

        if (rand <= GetComponent<BaseEntity>().percentageToBleed)
        {
            if (!entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Bleed).Any())
            {
                GameObject bl = Instantiate(pfManager.bleed, entityAffected.transform.position, Quaternion.identity);
                bl.transform.parent = entityAffected.transform;
                bl.GetComponent<BaseDebuff>().entityAffected = entityAffected.GetComponent<BaseEntity>();
                bl.GetComponent<BaseDebuff>().entityAffecting = GetComponent<BaseEntity>();
                entityAffected.GetComponent<BaseEntity>().debuffs.Add(bl.GetComponent<BaseDebuff>());
                bl.GetComponent<BaseDebuff>().debStacks += 1;
                bl.GetComponent<BaseDebuff>().SetTime();
            }
            else
            {
                entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Bleed).Single().debStacks += 1;
                entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Bleed).Single().SetTime();
            }
        }
    }

    public void CheckForIgnite(GameObject entityAffected)
    {
        int rand = Random.Range(1, 101);

        if (rand <= GetComponent<BaseEntity>().percentageToIgnite)
        {
            if (!entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Ignite).Any())
            {
                GameObject ig = Instantiate(pfManager.ignite, entityAffected.transform.position, Quaternion.identity);
                ig.transform.parent = entityAffected.transform;
                ig.GetComponent<BaseDebuff>().entityAffected = entityAffected.GetComponent<BaseEntity>();
                ig.GetComponent<BaseDebuff>().entityAffecting = GetComponent<BaseEntity>();
                entityAffected.GetComponent<BaseEntity>().debuffs.Add(ig.GetComponent<BaseDebuff>());
                ig.GetComponent<BaseDebuff>().debStacks += 1;
                ig.GetComponent<BaseDebuff>().SetTime();
            }
            else
            {
                entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Ignite).Single().debStacks += 1;
                entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Ignite).Single().SetTime();
            }
        }
    }
}
