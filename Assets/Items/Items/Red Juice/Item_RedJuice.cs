using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RedJuice : BaseItemObject
{
    public float percentageToIncrease;

    public override void ActivateBonus()
    {
        base.ActivateBonus();
        player.GetComponent<BaseEntity>().cooldownMultiplier -= (player.GetComponent<BaseEntity>().cooldownMultiplier / 100) * percentageToIncrease;
        player.GetComponent<BaseEntity>().damageMultiplier *= 1 + (percentageToIncrease / 100);
        player.GetComponent<BaseEntity>().moveSpeed *= 1 + (percentageToIncrease / 100);
        player.GetComponent<BaseEntity>().jumpForce *= 1 + (percentageToIncrease / 100);
        player.GetComponent<BaseEntity>().maxHealth *= 1 + (percentageToIncrease / 100);
        player.GetComponent<BaseEntity>().armour *= 1 + (percentageToIncrease / 100);
        player.GetComponent<BaseEntity>().healthRegen *= 1 + (percentageToIncrease / 100);
    }
}
