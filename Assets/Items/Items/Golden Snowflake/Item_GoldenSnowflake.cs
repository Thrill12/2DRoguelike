using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_GoldenSnowflake : BaseItemObject
{
    public override void ActivateBonus()
    {
        player.GetComponent<Player>().maxHealth = player.GetComponent<Player>().maxHealth / 2;
        player.GetComponent<Player>().damageMultiplier += 1;
    }
}
