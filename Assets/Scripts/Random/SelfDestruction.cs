using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    public bool exploding;
    public bool canBeShotToExplode;
    public float explosionRadius;
    public float selfDestructTimer;
    public GameObject projSender;
    public GameObject explosionParticle;
    [HideInInspector]
    public float damage;

    // Start is called before the first frame update
    void Start()
    {     
        if (!exploding)
        {
            Destroy(gameObject, selfDestructTimer);
        }
        else
        {
            Invoke("ExplodeDamage", selfDestructTimer);
        }      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Projectile" && canBeShotToExplode)
        {
            Destroy(collision.gameObject);
            ExplodeDamage();
        }
    }

    public void ExplodeDamage()
    {
        Collider2D[] objsInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        if (projSender.transform.tag == "Player")
        {            
            foreach (Collider2D col in objsInRadius)
            {
                if (col.GetComponent<BaseEntity>())
                {
                    Vector2 dir = transform.position - col.transform.position;

                    col.GetComponent<Rigidbody2D>().AddForce((-dir * damage / 2));
                    col.GetComponent<BaseEntity>().TakeDamage(damage, projSender.GetComponent<BaseEntity>());
                }
            }
        }
        else
        {
            foreach (Collider2D col in objsInRadius)
            {
                if (col.GetComponent<BaseEntity>())
                {
                    float damage = projSender.GetComponent<BaseEntity>().damageMultiplier * projSender.GetComponent<GrenadeShooting>().damage;

                    Vector2 dir = transform.position - col.transform.position;

                    col.GetComponent<Rigidbody2D>().AddForce(-dir * damage / 2);
                    col.GetComponent<BaseEntity>().TakeDamage(damage, projSender.GetComponent<BaseEntity>());
                }
            }
        }        

        GameObject obj = Instantiate(explosionParticle);
        obj.transform.position = gameObject.transform.position;
        obj.transform.parent = null;
        Destroy(gameObject);
    }
}
