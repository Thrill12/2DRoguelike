using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Abilities/Rifling")]
public class Rifling : ScrObjAbility
{
    public float baseDamage;
    public float velocity;
    public float destroyTimer;
    public GameObject projectile;

    [HideInInspector]
    public float realDamage;

    private GameObject puff;
    private AudioClip shootSound;

    public override void Activate(GameObject caster)
    {
        if(Time.timeScale > 0)
        {
            base.Activate(caster);
            SetPrivates();
            Shoot();
        }       
    }

    public void Shoot()
    {
        realDamage = baseDamage * caster.GetComponent<Player>().damageMultiplier;

        Vector2 dir = GetDir();

        AudioSource source = caster.GetComponent<AudioSource>();

        if (shootSound == null)
        {
            source.PlayOneShot(sounds.shootSounds[Random.Range(0, sounds.shootSounds.Count - 1)]);
        }
        else
        {
            source.PlayOneShot(shootSound);
        }

        GameObject light = Instantiate(prefabManager.simpleShootingShootLight, shootSource.transform);

        GameObject proj = Instantiate(projectile, shootSource.transform.position, Quaternion.identity);
        proj.GetComponent<SimpleBullet>().sender = caster;
        proj.GetComponent<SimpleBullet>().damageToHit = realDamage;
        proj.GetComponent<SelfDestruction>().selfDestructTimer = destroyTimer;
        GameObject pufff = Instantiate(puff, shootSource.transform.position, shootSource.transform.rotation);

        pufff.transform.localScale = new Vector2(3, 3);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        proj.transform.rotation = q;

        proj.GetComponent<Rigidbody2D>().velocity = dir * velocity;

        caster.GetComponent<Player>().ShakeCamera();
    }

    public void SetPrivates()
    {     
        mousePos = inputManager.mousePos;
        puff = prefabManager.puff;
        shootSound = sounds.shootSounds[0];
    }

    public Vector2 GetDir()
    {
        Vector2 dir = new Vector2();

        dir = mousePos - shootSource.transform.position;
        dir.Normalize();

        return dir;
    }
}
