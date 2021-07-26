using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Claymore", menuName ="Abilities/Claymore")]
public class Claymore : ScrObjAbility
{
    public float throwVelocity;
    public float damage;
    public GameObject claymore;
    private GameObject instantiatedClaymore;

    public override void Activate(GameObject caster)
    {
        if(Time.timeScale > 0)
        {
            base.Activate(caster);
            ThrowClaymore();
        }       
    }

    public void ThrowClaymore()
    {
        float distanceModifier = Vector2.Distance(mousePos, caster.transform.position);
        if (throwVelocity * distanceModifier / 2 < throwVelocity)
        {
            throwVelocity = throwVelocity * distanceModifier / 4;
        }

        Vector2 dir = (Vector2)((mousePos - caster.transform.position));
        dir.Normalize();

        caster.GetComponent<AudioSource>().PlayOneShot(sounds.throwSound);
        instantiatedClaymore = Instantiate(claymore, shootSource.transform.position, Quaternion.identity);
        instantiatedClaymore.transform.SetParent(null);
        instantiatedClaymore.GetComponent<Rigidbody2D>().AddForce(dir * throwVelocity + caster.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        instantiatedClaymore.GetComponent<ClaymoreObjectScript>().caster = caster;
        instantiatedClaymore.GetComponent<ClaymoreObjectScript>().damage = damage * caster.GetComponent<BaseEntity>().damageMultiplier;
    }
}
