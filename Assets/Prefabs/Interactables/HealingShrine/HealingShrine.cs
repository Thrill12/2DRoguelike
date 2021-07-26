using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingShrine : BaseInteractable
{
    public float healAmount;
    public float healInterval;

    public GameObject area;

    private bool activated;

    public override void Interact()
    {
        base.Interact();

        if (!activated)
        {
            if (player.health > player.maxHealth / 2)
            {
                player.health -= player.maxHealth / 2;
                activated = true;
                area.SetActive(true);
                GetComponent<AudioSource>().Play();
                GetComponentsInChildren<Animator>()[1].SetTrigger("AreaOn");
                StartCoroutine(HealEntity(player.gameObject));
            }
        }       
    }

    public IEnumerator HealEntity(GameObject obj)
    {
        if (activated)
        {
            while (true)
            {
                obj.GetComponent<BaseEntity>().RegenSpecific(healAmount);

                yield return new WaitForSeconds(healInterval);
            }           
        }
        else
        {
            yield break;
        }       
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (activated)
        {
            if (collision.transform.tag == "Player")
            {
                StartCoroutine(HealEntity(collision.gameObject));
            }
        }
        else
        {
            return;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (activated)
        {
            if (collision.transform.tag == "Player")
            {
                StopAllCoroutines();
            }
        }
        else
        {
            return;
        }
        
    }
}
