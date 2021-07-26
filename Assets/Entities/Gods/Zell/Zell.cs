using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zell : BaseEnemyObject
{
    private bool isCoroRunning = false;
    private bool canMove;
    public float mirrorsToSpawn;
    public float maxMirrors;
    public GameObject minion;

    [Header("Bullet Rain")]

    public GameObject crossedBullet;
    public int maxRainBullets;
    public float rainBulletVel;
    public float damagePerBullet;
    public float timeToDestroy;

    private List<GameObject> mirrors;

    private bool isAttacking = false;

    public override void Start()
    {
        base.Start();
        mirrors = new List<GameObject>();
        StartCoroutine(Laugh());
    }

    public override void Update()
    {
        base.Update();

        mirrors = mirrors.Where(x => x != null).ToList();

        if (health > 0)
        {
            FlipToPlayer();
            MoveToPlayer();
            StartCoroutine(ShootingAbilityCoroutine());
        }
    }

    public void MoveToPlayer()
    {
        if (canMove)
        {
            anim.SetBool("IsRunning", true);
        }

        Vector2 dir = player.transform.position - transform.position;
        rb.velocity = new Vector2(moveSpeed * dir.normalized.x, rb.velocity.y);
    }

    public IEnumerator ShootingAbilityCoroutine()
    {
        if (isAttacking)
        {
            yield break;
        }

        isAttacking = true;

        Shoot();

        yield return new WaitForSeconds(4);

        if(mirrors.Count < maxMirrors)
        {
            SpawnMirrors();
        }
        else
        {
            Shoot();
        }

        yield return new WaitForSeconds(4);

        RainCrossedBullets();

        yield return new WaitForSeconds(4);

        isAttacking = false;
    }

    public void SpawnMirrors()
    {
        for (int i = 0; i < mirrorsToSpawn - mirrors.Count; i++)
        {
            Vector2 spawnPos = new Vector2(transform.position.x + Random.Range(-2, 3), transform.position.y + 2);
            GameObject mirror = Instantiate(minion, spawnPos, transform.rotation);
            mirror.transform.parent = null;
            mirror.transform.localScale = new Vector2(-transform.localScale.x / (mirrorsToSpawn + 3), transform.localScale.y / (mirrorsToSpawn + 3));

            mirrors.Add(mirror);
        }
    }

    public void Shoot()
    {
        StartCoroutine(ShootBulletsCoroutine());
    }

    public void RainCrossedBullets()
    {
        for(int i = -maxRainBullets / 2; i < maxRainBullets / 2; i++)
        {
            Vector2 spawnPos = new Vector2(player.transform.position.x + i * 4, player.transform.position.y + 20);
            GameObject proj = Instantiate(crossedBullet, spawnPos, Quaternion.identity);
            proj.transform.localEulerAngles = new Vector3(0, 0, -90);
            proj.GetComponent<SimpleBullet>().damageToHit = damageMultiplier * damagePerBullet;
            proj.GetComponent<SimpleBullet>().sender = gameObject;
            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -rainBulletVel);
            proj.GetComponent<SelfDestruction>().selfDestructTimer = timeToDestroy;
        }
    }

    public IEnumerator ShootBulletsCoroutine()
    {
        if (isCoroRunning)
        {
            yield break;
        }

        isCoroRunning = true;

        canMove = false;
        anim.SetBool("IsRunning", false);
        anim.SetTrigger("Shoot");

        yield return new WaitForSeconds(0.45f);

        GetComponents<SimpleShooting>()[0].DoAbilityWithCooldown();

        yield return new WaitForSeconds(0.1f);

        GetComponents<SimpleShooting>()[1].DoAbilityWithCooldown();        

        anim.SetBool("IsRunning", true);
        canMove = true;
        isCoroRunning = false;

        yield return new WaitForSeconds(5);
    }

    public IEnumerator Laugh()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(10);

            GetComponents<AudioSource>()[1].Play();

            yield return new WaitForSeconds(Random.Range(5, 30));
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();       
    }
}
