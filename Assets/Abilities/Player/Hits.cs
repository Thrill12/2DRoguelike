using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hits : MonoBehaviour
{
    private PrefabManager prefabManager;

    public void DoOnHits(GameObject entityAffected, GameObject caster)
    {
        prefabManager = GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>();
        CheckForBleed(entityAffected, caster);
        CheckForIgnite(entityAffected, caster);
        CheckForMeteor(caster);
    }

    public void CheckForBleed(GameObject entityAffected, GameObject caster)
    {
        int rand = Random.Range(1, 101);

        if (rand <= caster.GetComponent<BaseEntity>().percentageToBleed)
        {
            if (!entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Bleed).Any())
            {
                GameObject bl = Instantiate(prefabManager.bleed, entityAffected.transform.position, Quaternion.identity);
                bl.transform.parent = entityAffected.transform;
                bl.GetComponent<BaseDebuff>().entityAffected = entityAffected.GetComponent<BaseEntity>();
                bl.GetComponent<BaseDebuff>().entityAffecting = caster.GetComponent<BaseEntity>();
                entityAffected.GetComponent<BaseEntity>().debuffs.Add(bl.GetComponent<BaseDebuff>());
                bl.GetComponent<BaseDebuff>().debStacks += 1;
                bl.GetComponent<BaseDebuff>().SetTime();
            }
            else
            {
                entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Bleed).Single().debStacks += 1;
                entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Bleed).Single().SetTime();
            }
        }
    }

    public void CheckForIgnite(GameObject entityAffected, GameObject caster)
    {
        int rand = Random.Range(1, 101);

        if (rand <= caster.GetComponent<BaseEntity>().percentageToIgnite)
        {
            if (!entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Ignite).Any())
            {
                GameObject ig = Instantiate(prefabManager.ignite, entityAffected.transform.position, Quaternion.identity);
                ig.transform.parent = entityAffected.transform;
                ig.GetComponent<BaseDebuff>().entityAffected = entityAffected.GetComponent<BaseEntity>();
                ig.GetComponent<BaseDebuff>().entityAffecting = caster.GetComponent<BaseEntity>();
                entityAffected.GetComponent<BaseEntity>().debuffs.Add(ig.GetComponent<BaseDebuff>());
                ig.GetComponent<BaseDebuff>().debStacks += 1;
                ig.GetComponent<BaseDebuff>().SetTime();
            }
            else
            {
                entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Ignite).Single().debStacks += 1;
                entityAffected.GetComponent<BaseEntity>().debuffs.Where(x => x is Ignite).Single().SetTime();
            }
        }
    }

    public void CheckForMeteor(GameObject caster)
    {
        int rand = Random.Range(1, 101);

        if(rand <= caster.GetComponent<BaseEntity>().percentageToMeteor)
        {
            Vector2 spawnPos = new Vector2(Random.Range(caster.transform.position.x - 5, caster.transform.position.x + 5), caster.transform.position.y + 15);
            GameObject obj = Instantiate(prefabManager.meteor, spawnPos, Quaternion.identity);
            obj.GetComponent<Meteor>().caster = caster;
            obj.GetComponent<Meteor>().angle = Random.Range(140, 221);

            if(caster.transform.tag == "Player")
            {
                obj.GetComponent<Meteor>().damage *= caster.GetComponent<BaseEntity>().damageMultiplier * caster.GetComponent<PlayerItemsSkills>().equippedItems.Count(x => x.GetComponent<BaseItemObject>() is Item_Ragnarok);
            }
            else
            {
                obj.GetComponent<Meteor>().damage *= caster.GetComponent<BaseEntity>().damageMultiplier;
            }
        }
    }

    public bool Crit(GameObject caster)
    {
        int rand = Random.Range(0, 101);

        if(rand < caster.GetComponent<BaseEntity>().percentageToCrit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Crit(BaseEntity caster)
    {
        int rand = Random.Range(0, 101);

        if (rand < caster.percentageToCrit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Leech(GameObject entityAffected, GameObject caster, float leechAmount)
    {
        caster.GetComponent<BaseEntity>().health += leechAmount;
    }
}
