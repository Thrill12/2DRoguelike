using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : BaseDebuff
{
    public float healthToTakeAwayPerTick;

    public override void DoEffect()
    {
        base.DoEffect();
        entityAffected.TakeDamageNoArmour(healthToTakeAwayPerTick * debStacks * entityAffecting.damageMultiplier, entityAffecting.gameObject.GetComponent<BaseEntity>());
    }
}
