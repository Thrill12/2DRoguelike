using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shadow : BaseEnemyObject
{
    public float minRange;
    public GameObject deathSound;

    private bool coroRunning = false;
    public bool canSpawnMirrors = true;
    public List<GameObject> mirrors;

    public GameObject shadowMirror;

    private int mirrorsToSpawn;

    public override void Start()
    {
        base.Start();
        mirrors = new List<GameObject>();
        mirrorsToSpawn = Mathf.RoundToInt(Mathf.Pow(1.3f, genManager.levelsCompleted));       
    }

    public override void Update()
    {
        base.Update();

        if (!isDead)
        {
            FlipToPlayer();

            if (Vector2.Distance(player.transform.position, transform.position) < minRange)
            {
                anim.SetBool("IsWalking", false);
                GetComponent<MeleeHit>().DoAbilityWithCooldown();
            }
            else
            {
                MoveToPlayer();               
            }

            StartCoroutine(ShootCoroutine());
        }       
    }

    public void MoveToPlayer()
    {        
        anim.SetBool("IsWalking", true);
        Vector2 dir = player.transform.position - transform.position;

        rb.velocity = new Vector2(dir.normalized.x * moveSpeed, rb.velocity.y);
    }

    public override void OnDeath()
    {
        StartCoroutine(DeathCoroutine());
    }

    public IEnumerator DeathCoroutine()
    {
        if (!isDead)
        {
            isDead = true;
            anim.SetBool("IsWalking", false);
            anim.SetTrigger("Death");
            Instantiate(deathSound, transform);

            yield return new WaitForSeconds(1.3f);

            base.OnDeath();
        }        
    }

    public IEnumerator ShootCoroutine()
    {
        if (coroRunning)
        {
            yield break;
        }
        coroRunning = true;

        Debug.Log("Started coroutnie");

        if (health > 0)
        {
            yield return new WaitForSeconds(1.5f);

            Shoot();

            yield return new WaitForSeconds(2);

            if(diffManager.stagesCompleted > 4)
            {
                mirrors = mirrors.Where(x => x != null).ToList();

                if (canSpawnMirrors)
                {
                    if (mirrors.Count < mirrorsToSpawn)
                    {
                        SpawnMirrors();
                    }
                    else
                    {
                        Shoot();
                    }

                }
                else
                {
                    Shoot();
                }

                Jump();
            }
            else
            {
                Shoot();
            }           

            yield return new WaitForSeconds(1.5f);

            coroRunning = false;
        }      
    }

    public void Shoot()
    {    
        if (Vector2.Distance(player.transform.position, transform.position) < GetComponent<SimpleShooting>().maxDistanceForEnemyShooting)
        {
            GetComponent<SimpleShooting>().DoAbilityWithCooldown();
        }
    }

    public void SpawnMirrors()
    {       
        for (int i = 0; i < mirrorsToSpawn - mirrors.Count; i++)
        {
            Vector2 spawnPos = new Vector2(transform.position.x + mirrorsToSpawn, transform.position.y + 2);
            GameObject mirror = Instantiate(shadowMirror, spawnPos, transform.rotation);
            mirror.transform.parent = null;

            mirror.GetComponent<BaseEntity>().maxHealth = maxHealth / (mirrorsToSpawn + 3);
            mirror.GetComponent<BaseEntity>().damageMultiplier = damageMultiplier / (mirrorsToSpawn + 3);

            mirror.GetComponent<BaseEntity>().moveSpeed += mirrorsToSpawn;
            mirror.GetComponent<Shadow>().canSpawnMirrors = false;

            mirrors.Add(mirror);
        }
    }
}
