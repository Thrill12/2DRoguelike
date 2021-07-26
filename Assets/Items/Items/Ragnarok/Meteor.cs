using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Meteor : MonoBehaviour
{
    public float speed;
    public float damage;
    public GameObject caster;
    public float angle;

    private Hits hits;

    public void Start()
    {
        hits = new Hits();
        transform.localEulerAngles = new Vector3(0, 0, angle);
        GetComponent<Rigidbody2D>().velocity = speed * transform.up;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BaseEnemyObject>())
        {
            collision.gameObject.GetComponent<BaseEnemyObject>().TakeDamage(damage, caster.GetComponent<BaseEntity>());
            caster.GetComponent<BaseEntity>().OnHit(collision.gameObject);
        }
    }
}
