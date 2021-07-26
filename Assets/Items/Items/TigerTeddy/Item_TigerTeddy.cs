using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_TigerTeddy : BaseItemObject
{
    public override void ActivateBonus()
    {
        player.GetComponent<Player>().jumpsAvailable += 1;
        base.ActivateBonus();
    }
}
