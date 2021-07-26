using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_BootsOfSwiftness : BaseItemObject
{
    public float speedIncrease;

    public override void ActivateBonus()
    {
        player.GetComponent<BaseEntity>().moveSpeed += speedIncrease;
        player.GetComponent<BaseEntity>().maxSpeed += speedIncrease;
    }
}
