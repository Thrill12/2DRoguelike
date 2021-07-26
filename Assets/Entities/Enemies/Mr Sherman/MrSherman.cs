using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrSherman : BaseEnemyObject
{
    private SpriteRenderer rend;
    private SimpleShooting[] shooting;

    public float shermanMoveRange;
    public float minShermanRange;
    public GameObject shermanExplosionParticles;

    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        shooting = GetComponents<SimpleShooting>();

        base.Start();
    }

    public override void Update()
    {    
        if(Time.timeScale != 0)
        {
            MoveToPlayer();
            //rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, 0, maxSpeed), rb.velocity.y);
            ShootAtPlayer();

            base.Update();
        }        
    }

    public void MoveToPlayer()
    {
        if (IsGrounded())
        {
            FlipToPlayer();
            Vector2 dir = (player.transform.position - transform.position);

            if (Vector2.Distance(player.transform.position, transform.position) <= shermanMoveRange && Vector2.Distance(player.transform.position, transform.position) >= minShermanRange)
            {
                dir = new Vector2(dir.x, 0);
                rb.velocity = new Vector2(dir.normalized.x * moveSpeed, rb.velocity.y);                  
            }               
        }       
    }

    public override void FlashWhite()
    {
        StartCoroutine(FlashWhiteCor());
    }

    public override IEnumerator FlashWhiteCor()
    {
        GetComponent<SpriteRenderer>().material.shader = flashShader;
        GetComponentInChildren<SpriteRenderer>().material.shader = flashShader;

        yield return new WaitForSeconds(0.05f);

        GetComponentInChildren<SpriteRenderer>().material.shader = originalShader;
        GetComponent<SpriteRenderer>().material.shader = originalShader;       
    }

    public override void OnDeath()
    {
        GameObject obj = Instantiate(shermanExplosionParticles, transform.position, Quaternion.identity);
        obj.transform.parent = null;

        base.OnDeath();
    }

    public void ShootAtPlayer()
    {
        if(Vector2.Distance(player.transform.position, transform.position) < shooting[0].maxDistanceForEnemyShooting)
        {
            foreach(SimpleShooting s in shooting)
            {
                s.DoAbilityWithCooldown();
            }
        }
    }

    public override void FlipToPlayer()
    {
        rend.flipX = player.transform.position.x < this.transform.position.x;
    }
}
