using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_CriticalClover : BaseItemObject
{
    public float critPercentage;

    public override void ActivateBonus()
    {
        base.ActivateBonus();
        player.GetComponent<BaseEntity>().percentageToCrit += critPercentage;
    }
}
