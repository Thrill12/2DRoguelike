using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FanOfKnives", menuName ="Abilities/Fan of Knives")]
public class FanOfKnives : ScrObjAbility
{
    public float damage;
    public float velocity;
    public int numOfKnives;
    public float timeToDestroy;
    public GameObject knifeSource;

    private GameObject instantiatedKnifeSource;

    public override void Activate(GameObject caster)
    {
        if(Time.timeScale > 0)
        {
            base.Activate(caster);
            instantiatedKnifeSource = Instantiate(knifeSource, caster.transform.position, Quaternion.identity);
            instantiatedKnifeSource.GetComponent<FanOfKnivesSource>().numOfProjectiles = numOfKnives;
            instantiatedKnifeSource.GetComponent<FanOfKnivesSource>().projDamage = damage;
            instantiatedKnifeSource.GetComponent<FanOfKnivesSource>().speed = velocity;
            instantiatedKnifeSource.GetComponent<FanOfKnivesSource>().caster = caster;
            instantiatedKnifeSource.GetComponent<FanOfKnivesSource>().timeToDestroy = timeToDestroy;
            caster.GetComponent<AudioSource>().PlayOneShot(sounds.slashSound);
        }      
    }

    public override void BeginCooldown(GameObject caster)
    {
        base.BeginCooldown(caster);
        Destroy(instantiatedKnifeSource);
    }
}
