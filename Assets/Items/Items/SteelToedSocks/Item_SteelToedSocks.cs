using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SteelToedSocks : BaseItemObject
{
    public float armourToGive;

    public override void ActivateBonus()
    {
        player.GetComponent<Player>().armour += armourToGive;

        base.ActivateBonus();
    }

}
