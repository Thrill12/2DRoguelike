using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShurikenObjectScript : MonoBehaviour
{
    [HideInInspector]
    public float timeToReturn;
    [HideInInspector]
    public LayerMask layerToAttack;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public Vector2 dirToCaster;
    [HideInInspector]
    public float velocity;
    [HideInInspector]
    public GameObject caster;

    Hits hits;

    public void Start()
    {
        hits = new Hits();
        StartCoroutine(ComeBack());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Player")
        {
            collision.GetComponent<BaseEntity>().TakeDamage(damage, caster.GetComponent<BaseEntity>());
            caster.GetComponent<BaseEntity>().OnHit(collision.gameObject);
        }
    }

    public IEnumerator ComeBack()
    {
        yield return new WaitForSeconds(timeToReturn);

        GetComponent<Rigidbody2D>().velocity = dirToCaster.normalized * velocity;

        yield return new WaitForSeconds(timeToReturn);

        Destroy(gameObject);
    }
}
