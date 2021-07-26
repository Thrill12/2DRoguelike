using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_CoagulatedHeart : BaseItemObject
{
    public float healthToIncrease;

    public override void ActivateBonus()
    {
        base.ActivateBonus();

        player.GetComponent<Player>().maxHealth += healthToIncrease;
    }
}
