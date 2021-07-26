using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Item_Squirtpack : BaseItemObject
{
    [Tooltip("Write as a percentage.")]
    public float jumpForceIncreasePercentage;

    private bool hasActivated;

    public override void ActivateBonus()
    {
        int numOfItems = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItemsSkills>().equippedItems.Count(x => x.GetComponent<BaseItemObject>() is Item_Squirtpack);

        if (!hasActivated)
        {            
            player.GetComponent<Player>().jumpForce += (player.GetComponent<Player>().baseJumpForce / 100) * jumpForceIncreasePercentage;

            if(15.3f - Mathf.Pow(1.2f, 17.29f - numOfItems) + player.GetComponent<Player>().baseMaxJumpSpeed > player.GetComponent<Player>().baseMaxJumpSpeed)
            {
                player.GetComponent<Player>().maxJumpSpeed = 15.3f - Mathf.Pow(1.2f, 17.29f - numOfItems) + player.GetComponent<Player>().baseMaxJumpSpeed;
            }            

            hasActivated = true;
        }
    }
}
