using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_QuicksilverMiasma : BaseItemObject
{
    public float amountToReduceCooldowns;

    public override void ActivateBonus()
    {
        player.GetComponent<BaseEntity>().cooldownMultiplier -= amountToReduceCooldowns;

        foreach(AbilityHolder hold in player.GetComponents<AbilityHolder>())
        {
            hold.ability.cooldownTime *= player.GetComponent<BaseEntity>().cooldownMultiplier;
        }
        base.ActivateBonus();
    }
}
