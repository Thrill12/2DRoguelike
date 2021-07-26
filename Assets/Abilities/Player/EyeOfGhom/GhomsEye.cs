using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ghom's Eye")]
public class GhomsEye : ScrObjAbility
{
    public float damagePerTick;
    public GameObject laserObject;
    private GameObject instantiatedLaser;
    public LayerMask layerToAttack;

    public override void Activate(GameObject caster)
    {
        if(Time.timeScale > 0)
        {
            base.Activate(caster);
            damagePerTick = damagePerTick * caster.GetComponent<BaseEntity>().damageMultiplier;

            instantiatedLaser = Instantiate(laserObject, shootSource.transform.position, shootSource.transform.rotation);
            instantiatedLaser.GetComponent<GhomLaserDamage>().layerToAttack = layerToAttack;
            instantiatedLaser.GetComponent<GhomLaserDamage>().damage = damagePerTick;
            instantiatedLaser.GetComponent<GhomLaserDamage>().source = shootSource;
            instantiatedLaser.GetComponent<GhomLaserDamage>().caster = caster;
            instantiatedLaser.transform.eulerAngles = shootSource.transform.eulerAngles;
        }       
    }

    public override void BeginCooldown(GameObject caster)
    {
        base.BeginCooldown(caster);
        Destroy(instantiatedLaser);
    }
}
