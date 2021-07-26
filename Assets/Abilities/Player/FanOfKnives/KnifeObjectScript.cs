using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnifeObjectScript : MonoBehaviour
{
    [HideInInspector]
    public float timeToDestroy;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public GameObject caster;

    private Hits hits;

    public void Start()
    {
        hits = new Hits();
        Destroy(gameObject, timeToDestroy);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Player")
        {
            if (collision.GetComponent<BaseEntity>())
            {
                collision.GetComponent<BaseEntity>().TakeDamage(damage, caster.GetComponent<BaseEntity>());
                caster.GetComponent<BaseEntity>().OnHit(collision.gameObject);
            }           
        }
    }
}
