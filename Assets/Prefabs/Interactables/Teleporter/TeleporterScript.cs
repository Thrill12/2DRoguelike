using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : BaseInteractable
{   
    public GameObject pairedTeleporter;     

    public override void Interact()
    {
        player.transform.position = new Vector2(pairedTeleporter.transform.position.x, pairedTeleporter.transform.position.y + 1);
    }
}
