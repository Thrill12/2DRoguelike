using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspiciosCloudScript : BaseEnemyObject
{   
    public float explosionRadius;
    public float damage;

    private float tempCounter;
    private bool isPlayerInRadius;
    private bool isExploded;
    public GameObject explosionRing;
    private GameObject activeExplosionRing;

    // Start is called before the first frame update
    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {        
        if(Time.timeScale != 0)
        {
            TriggerExplosionTimer();
            MoveToPlayer();

            base.Update();
        }

        if (isPlayerInRadius)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void MoveToPlayer()
    {
        Vector2 dirToPlayer = player.transform.position - transform.position;

        rb.velocity = new Vector2(dirToPlayer.normalized.x * moveSpeed, dirToPlayer.normalized.y * moveSpeed);     
    }

    public override void OnDeath()
    {
        GameObject ring = Instantiate(explosionRing, transform.position, Quaternion.identity);
        ring.transform.parent = null;

        base.OnDeath();
    }

    void Explode()
    {
        isExploded = true;

        Collider2D[] objsInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in objsInRadius)
        {
            if (col.GetComponent<BaseEntity>() && col.transform.tag == "Player")
            {
                float damageToGive = Vector2.Distance(transform.position, player.transform.position) +(damageMultiplier * damage);

                col.GetComponent<BaseEntity>().TakeDamage(damage, gameObject.GetComponent<BaseEntity>());
            }
        }
        OnDeath();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Player")
        {
            isPlayerInRadius = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Player")
        {
            tempCounter = 0;
            transform.localScale = new Vector3(3, 3, 3);
            isPlayerInRadius = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    public void TriggerExplosionTimer()
    {
        if (!isExploded && isPlayerInRadius)
        {
            tempCounter += 1;
            transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);

            if(tempCounter >= 200)
            {
                Explode();
            }
        }
    }
}
