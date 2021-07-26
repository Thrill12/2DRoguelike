using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ignite : BaseDebuff
{
    public float fireDamage;
    public float fireSpreadRange;

    private List<GameObject> alreadyAdded;

    public override void Start()
    {
        base.Start();
        debStacks = 1;
        alreadyAdded = new List<GameObject>();
    }

    public override void Update()
    {
        base.Update();        
    }

    public override void DoEffect()
    {       
        base.DoEffect();
        entityAffected.TakeDamage(fireDamage * debStacks * entityAffecting.damageMultiplier, entityAffecting.gameObject.GetComponent<BaseEntity>());
        List<GameObject> nearby = new List<GameObject>();
        nearby = GameObject.FindGameObjectsWithTag("Enemy").
            Where(b => Vector2.Distance(b.transform.position, transform.position) < fireSpreadRange).ToList();
        alreadyAdded.Add(gameObject.transform.parent.gameObject);

        int rand = Random.Range(0, nearby.Count);

        if (!alreadyAdded.Contains(nearby[rand]))
        {
            if (!nearby[rand].GetComponent<BaseEntity>().debuffs.Where(x => x is Ignite).Any())
            {
                GameObject ig = Instantiate(pfManager.ignite, nearby[rand].transform.position, Quaternion.identity);
                ig.transform.parent = nearby[rand].transform;
                ig.GetComponent<BaseDebuff>().entityAffected = nearby[rand].GetComponent<BaseEntity>();
                nearby[rand].GetComponent<BaseEntity>().debuffs.Add(ig.GetComponent<BaseDebuff>());
            }
            else
            {
                nearby[rand].GetComponent<BaseEntity>().debuffs.Where(x => x is Ignite).Single().debStacks += 1;                
            }

            alreadyAdded.Add(nearby[rand]);
        }          
    }
}
