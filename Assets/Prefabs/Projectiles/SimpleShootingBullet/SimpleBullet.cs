using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public GameObject sender;
    public float damageToHit;
    private PrefabManager pfManager;

    private Hits hits;
    private string senderTag;

    private BaseEntity senderEntityCopy;
    private float timeInAir;

    private void Start()
    {
        hits = new Hits();
        pfManager = GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>();
        if(sender.transform.tag != "Player")
        {
            damageToHit = sender.GetComponent<SimpleShooting>().realDamage;
        }
        senderTag = sender.tag;
        senderEntityCopy = sender.GetComponent<BaseEntity>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject.GetComponent<Collider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    public void CheckCollision(Collider2D collision)
    {
        if (senderTag == "Player")
        {
            if (collision.transform.tag != "Player")
            {
                if (collision.gameObject.GetComponent<BaseEntity>())
                {
                    collision.gameObject.GetComponent<BaseEntity>().TakeDamage(damageToHit, senderEntityCopy);
                    if(sender != null)
                    {
                        sender.GetComponent<BaseEntity>().OnHit(collision.gameObject);
                    }                   
                    Destroy(gameObject);
                }
            }
        }
        else if (senderTag == "Enemy" || sender.transform.tag == "Boss")
        {
            if (collision.transform.tag != "Enemy" && collision.transform.tag != "Boss")
            {
                if (collision.gameObject.GetComponent<BaseEntity>())
                {
                    collision.gameObject.GetComponent<BaseEntity>().TakeDamage(damageToHit, senderEntityCopy);
                    if (sender != null)
                    {
                        sender.GetComponent<BaseEntity>().OnHit(collision.gameObject);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
