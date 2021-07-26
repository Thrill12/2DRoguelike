using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertionTeleporter : BaseInteractable
{
    private bool hasClicked;

    public override void Interact()
    {
        base.Interact();
        if (!hasClicked)
        {
            hasClicked = true;
            GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>().LoadIntoScene("Fields of Desertion", player.gameObject);
        }
    }
}
