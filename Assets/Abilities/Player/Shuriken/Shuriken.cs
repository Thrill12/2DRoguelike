using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Boomerang")]
public class Shuriken : ScrObjAbility
{
    public float damage;
    public float velocity;
    public float timeToReturn;
    public GameObject projectile;
    public LayerMask layerToAttack;

    private GameObject instantiatedProjectile;

    public override void Activate(GameObject caster)
    {
        if(Time.timeScale > 0)
        {
            base.Activate(caster);
            mousePos = inputManager.mousePos;
            ThrowBoomerang();
        }        
    }

    public void ThrowBoomerang()
    {
        GameObject boom = Instantiate(projectile, shootSource.transform.position, shootSource.transform.rotation);
        Vector2 dir = boom.transform.position - mousePos;
        boom.GetComponent<Rigidbody2D>().velocity = -dir.normalized * velocity;
        boom.GetComponent<ShurikenObjectScript>().damage = damage * caster.GetComponent<BaseEntity>().damageMultiplier;
        boom.GetComponent<ShurikenObjectScript>().layerToAttack = layerToAttack;
        boom.GetComponent<ShurikenObjectScript>().timeToReturn = timeToReturn;
        boom.GetComponent<ShurikenObjectScript>().dirToCaster = dir;
        boom.GetComponent<ShurikenObjectScript>().velocity = velocity;
        boom.GetComponent<ShurikenObjectScript>().caster = caster;
        boom.transform.parent = null;
        caster.GetComponent<AudioSource>().PlayOneShot(sounds.shurikenSound);
    }

    public override void BeginCooldown(GameObject caster)
    {
        base.BeginCooldown(caster);
    }
}
