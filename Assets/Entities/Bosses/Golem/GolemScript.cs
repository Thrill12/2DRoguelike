using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GolemScript : BaseEnemyObject
{
    private bool hasShotHand;
    private bool coroRunning;
    public AudioClip deathSound;
    public float detectionRadius;
    public GameObject laserObject;
    public GameObject laserPos;
    public LayerMask layerToAttack;

    [Space(5)]

    public float laserDamagePerTick;

    private bool isDying = false;
    private GameObject laser;

    public override void Start()
    {
        base.Start();

        laserDamagePerTick *= damageMultiplier;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Update()
    {
        base.Update();        

        if (health > 0)
        {
            FlipToPlayer();
            StartCoroutine(ShootCoroutine());
            MoveToPlayer();
        }                      
    }    

    public IEnumerator ShootCoroutine()
    {
        if (coroRunning)
        {
            yield break;
        }
        coroRunning = true;

        if (health > 0)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < detectionRadius)
            {
                yield return new WaitForSeconds(1.5f);

                StartCoroutine(ShootHand());

                yield return new WaitForSeconds(2);

                ShootLaser();

                yield return new WaitForSeconds(4f);

                if(diffManager.stagesCompleted > 4)
                {
                    StartCoroutine(ShootRandomLasers());
                }
                else
                {
                    ShootLaser();
                }

                yield return new WaitForSeconds(1);
            }

            coroRunning = false;
        }        
    }

    public void ShootLaser()
    {
        if(health > 0)
        {
            laser = Instantiate(laserObject, laserPos.transform);
            laser.GetComponent<GhomLaserDamage>().layerToAttack = layerToAttack;
            laser.GetComponent<GhomLaserDamage>().caster = gameObject;

            Destroy(laser, 2);           
        }        
    }

    public IEnumerator ShootHand()
    {
        if(health > 0)
        {
            hasShotHand = false;
            if (hasShotHand != true)
            {
                if (Vector2.Distance(player.transform.position, transform.position) <= GetComponent<SimpleShooting>().maxDistanceForEnemyShooting)
                {
                    hasShotHand = true;
                    anim.SetTrigger("ShootHand");

                    yield return new WaitForSeconds(0.4f);

                    GetComponent<SimpleShooting>().DoAbilityWithCooldown();
                    GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();
                }
            }
        }        
    }

    public IEnumerator ShootRandomLasers()
    {
        if(health > 0)
        {
            List<GameObject> lasers = new List<GameObject>();

            for(int i = 0; i < 2; i++)
            {
                Vector2 spawnPos = new Vector2(Random.Range(player.transform.position.x - 10, player.transform.position.x + 10), transform.position.y);
                GameObject obj = Instantiate(laserObject, spawnPos, Quaternion.identity);
                obj.transform.localEulerAngles = new Vector3(0, 0, 90);
                obj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

                obj.GetComponent<GhomLaserDamage>().layerToAttack = layerToAttack;
                obj.GetComponent<GhomLaserDamage>().caster = gameObject;
                obj.GetComponent<GhomLaserDamage>().damage = (laserDamagePerTick * damageMultiplier) / 2;
                lasers.Add(obj);
            }

            yield return new WaitForSeconds(2);

            foreach(GameObject obj in lasers)
            {
                Destroy(obj);
            }
        }
    }

    public void MoveToPlayer()
    {
        Vector2 dir = (player.transform.position - transform.position);

        rb.velocity = new Vector2(dir.normalized.x * moveSpeed, rb.velocity.y);
    }

    public override void OnDeath()
    {
        StartCoroutine(GolemOnDeath());
    }

    public IEnumerator GolemOnDeath()
    {
        if (!isDying)
        {
            isDying = true;
            
            if(laser != null)
            {
                Destroy(laser);
            }
            anim.SetTrigger("GolemDie");
            AudioSource src = GetComponents<AudioSource>()[1];

            if (!src.isPlaying)
            {
                src.PlayOneShot(deathSound);
            }

            Debug.Log("Before yield");

            yield return new WaitForSeconds(3);

            Debug.Log("Base Death");
            StopAllCoroutines();
            base.OnDeath();
        }
        else
        {
            yield break;
        }        
    }

    public override void FlipToPlayer()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector2(-startScale.x, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(startScale.x, transform.localScale.y);
        }
    }
}
