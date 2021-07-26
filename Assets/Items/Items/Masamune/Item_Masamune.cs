using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Masamune : BaseItemObject
{
    public float percentageChanceToBleed;

    public override void ActivateBonus()
    {      
        base.ActivateBonus();
        player.GetComponent<Player>().percentageToBleed += percentageChanceToBleed;
    }
}
