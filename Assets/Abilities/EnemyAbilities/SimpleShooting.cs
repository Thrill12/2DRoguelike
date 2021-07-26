using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SimpleShooting : BaseAbility
{
    [Header("Specific Stats")]

    public float baseDamage;
    public float projVelocity;
    public float destroyTimer;

    [Space(10)]

    public GameObject shootSource;
    public GameObject projectile;
    public GameObject puff;
    public AudioClip shootSound;
    public float maxDistanceForEnemyShooting;

    [Space(5)]

    public bool doRecoil;
    public bool shootIndependentOfShootSource;

    private Vector2 mousePos;
    [HideInInspector]
    public float realDamage;

    public Animator anim;

    private Rigidbody2D rb;
    private InputManager inputManager;
    private SoundClipLibrary sounds;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        pfManager = GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>();
        sounds = GameObject.FindGameObjectWithTag("SoundClipLibrary").GetComponent<SoundClipLibrary>();
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

        realDamage = baseDamage * GetComponent<BaseEntity>().damageMultiplier;

        if (gameObject.transform.tag == "Player")
        {
            if (Input.GetKey(triggerKey))
            {
                DoAbilityWithCooldown();
            }
        }
        else if (transform.tag == "Enemy" && Vector2.Distance(target, transform.position) < maxDistanceForEnemyShooting)
        {
            DoAbilityWithCooldown();
        }           

        mousePos = Input.mousePosition;        
    }

    public Vector2 GetDir()
    {
        Vector2 dir = new Vector2();

        if (shootIndependentOfShootSource)
        {
            dir = target - shootSource.transform.position;
            dir.Normalize();
        }
        else
        {
            dir = shootSource.transform.right;
            dir.Normalize();
        }

        return dir;
    }

    public override void DoAbility()
    {
        SetTarget();

        Vector2 dir = GetDir();      

        AudioSource source = gameObject.GetComponent<AudioSource>();

        if(shootSound == null)
        {
            source.PlayOneShot(sounds.shootSounds[Random.Range(0, sounds.shootSounds.Count - 1)]);
        }
        else
        {
            source.PlayOneShot(shootSound);
        }       

        GameObject light = Instantiate(pfManager.simpleShootingShootLight, shootSource.transform);

        GameObject proj = Instantiate(projectile, shootSource.transform.position, Quaternion.identity);
        proj.GetComponent<SimpleBullet>().sender = gameObject;
        proj.GetComponent<SelfDestruction>().selfDestructTimer = destroyTimer;
        proj.layer = 14;
        GameObject pufff = Instantiate(puff, shootSource.transform.position, shootSource.transform.rotation);

        if(transform.tag == "Player")
        {
            pufff.transform.localScale = new Vector2(3,3);
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        proj.transform.rotation = q;

        proj.GetComponent<Rigidbody2D>().velocity = dir * projVelocity;        

        if (doRecoil)
        {
            rb.AddForceAtPosition(shootSource.transform.position, -dir * projVelocity * baseDamage);
        }

        if (gameObject.transform.tag == "Player")
        {
            gameObject.GetComponent<Player>().ShakeCamera();
        }
    }
}
