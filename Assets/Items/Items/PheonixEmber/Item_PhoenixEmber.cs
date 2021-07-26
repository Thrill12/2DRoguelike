using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PhoenixEmber : BaseItemObject
{
    public float percentageToIgnite;

    public override void ActivateBonus()
    {
        base.ActivateBonus();
        player.GetComponent<BaseEntity>().percentageToIgnite += percentageToIgnite;
    }
}
