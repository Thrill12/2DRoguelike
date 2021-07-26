using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(menuName = "Abilities/Slash")]
public class Slash : ScrObjAbility
{
    public float baseDamage;
    public float attackPointRadius;
    public LayerMask layer;

    private GameObject attackPoint;
    private AudioSource source;
    private float damage;

    private Hits hits;

    public override void Activate(GameObject caster)
    {        
        if(Time.timeScale > 0)
        {
            base.Activate(caster);
            SetPrivates();
            SlashVoid();
        }      
    }

    public void SetPrivates()
    {
        attackPoint = caster.transform.Find("ShootSource").gameObject;
        source = caster.GetComponent<AudioSource>();

        hits = new Hits();
    }

    private void SlashVoid()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackPointRadius, layer);
        Instantiate(prefabManager.slashEffect, attackPoint.transform.position, attackPoint.transform.rotation);

        source.PlayOneShot(sounds.energyWhipSound);
        caster.GetComponent<Player>().ShakeCamera();

        damage = baseDamage * caster.GetComponent<BaseEntity>().damageMultiplier;

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<BaseEntity>())
            {
                cols[i].GetComponent<BaseEntity>().TakeDamage(damage, caster.GetComponent<BaseEntity>());
                caster.GetComponent<BaseEntity>().OnHit(cols[i].gameObject);
            }
        }
    }
}
