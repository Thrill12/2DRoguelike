using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PollenOfMending : BaseItemObject
{
    public float healthRegenAddon;

    public override void ActivateBonus()
    {
        player.GetComponent<Player>().healthRegen += healthRegenAddon;
        base.ActivateBonus();
    }
}
