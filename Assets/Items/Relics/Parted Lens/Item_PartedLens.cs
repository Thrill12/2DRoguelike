using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PartedLens : BaseItemObject
{
    public override void ActivateBonus()
    {
        base.ActivateBonus();

        player.GetComponent<BaseEntity>().maxHealth /= 2;
        player.GetComponent<BaseEntity>().damageMultiplier *= 2;
    }
}
