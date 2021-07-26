using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_LapisOfLeeching : BaseItemObject
{
    public float leechAmount;

    public override void ActivateBonus()
    {
        base.ActivateBonus();
        player.GetComponent<BaseEntity>().leechAmount += leechAmount;
    }
}
