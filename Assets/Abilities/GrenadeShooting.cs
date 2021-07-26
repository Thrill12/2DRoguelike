using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeShooting : BaseAbility
{
    [Header("Specific Stats")]

    public float damage;
    public float projVelocity;

    [Space(10)]

    public GameObject shootSource;
    public GameObject projectile;

    private Vector2 mousePos;

    private void Update()
    {
        if (Input.GetKey(triggerKey))
        {
            DoAbilityWithCooldown();
        }       

        mousePos = Input.mousePosition;
    }

    public override void DoAbility()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        float distanceModifier = Vector2.Distance(worldMousePos, gameObject.transform.position);
        if(projVelocity * distanceModifier / 2 < projVelocity)
        {
            projVelocity = projVelocity * distanceModifier / 2;
        }      

        Vector2 dir = (Vector2)((worldMousePos - transform.position));
        dir.Normalize();

        GameObject proj = Instantiate(projectile, shootSource.transform);
        proj.transform.SetParent(null);
        proj.GetComponent<Rigidbody2D>().AddForce(dir * projVelocity + gameObject.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        proj.GetComponent<SelfDestruction>().projSender = gameObject;

        //Debug.Log("Shot Grenade");
    }
}
