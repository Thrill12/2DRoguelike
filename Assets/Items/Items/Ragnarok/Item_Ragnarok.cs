using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Ragnarok : BaseItemObject
{
    public float damage;
    public float procChance;
    public GameObject meteor;
    public GameObject caster;

    public override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        caster = player;
    }

    public override void ActivateBonus()
    {
        base.ActivateBonus();
        player.GetComponent<BaseEntity>().percentageToMeteor = 5;
    }

    public void SpawnMeteor()
    {
        Vector2 spawnPos = new Vector2(Random.Range(caster.transform.position.x - 5, caster.transform.position.x + 5), caster.transform.position.y + 10);
        GameObject obj = Instantiate(meteor, spawnPos, Quaternion.identity);
        obj.GetComponent<Meteor>().damage = damage;
        obj.GetComponent<Meteor>().caster = caster;
    }
}
