using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Grenading")]
public class Grenading : ScrObjAbility
{
    public float damage;
    public float velocity;
    public float timer;
    public float radius;

    public GameObject projectile;

    public override void Activate(GameObject caster)
    {
        if(Time.timeScale > 0)
        {
            base.Activate(caster);
            SetPrivates();
            ThrowGrenade();
        }      
    }

    public void SetPrivates()
    {
        shootSource = caster.transform.Find("ShootSource").gameObject;
        mousePos = inputManager.mousePos;
    }

    public void ThrowGrenade()
    {
        float distanceModifier = Vector2.Distance(mousePos, caster.transform.position);
        if (velocity * distanceModifier / 2 < velocity)
        {
            velocity = velocity * distanceModifier / 4;
        }

        Vector2 dir = (Vector2)((mousePos - caster.transform.position));
        dir.Normalize();

        caster.GetComponent<AudioSource>().PlayOneShot(sounds.throwSound);
        GameObject proj = Instantiate(projectile, shootSource.transform);
        proj.transform.SetParent(null);
        proj.GetComponent<Rigidbody2D>().AddForce(dir * velocity + caster.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        proj.GetComponent<SelfDestruction>().projSender = caster.gameObject;
        proj.GetComponent<SelfDestruction>().damage = caster.GetComponent<BaseEntity>().damageMultiplier * damage;
        proj.GetComponent<SelfDestruction>().selfDestructTimer = timer;
        proj.GetComponent<SelfDestruction>().explosionRadius = radius;
    }
}
