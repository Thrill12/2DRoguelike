using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanOfKnivesSource : MonoBehaviour
{    
    public int numOfProjectiles = 3;
    [HideInInspector]
    public float projDamage = 5;
    [HideInInspector]
    public float speed = 35;
    [HideInInspector]
    public GameObject caster;
    [HideInInspector]
    public float timeToDestroy = 1;

    public GameObject projectilesToSpawn;

    public void Start()
    {
        SpawnKnives();
    }

    public void SpawnKnives()
    {
        for(int i = 0; i < numOfProjectiles; i++)
        {
            GameObject knife = Instantiate(projectilesToSpawn, transform.position, Quaternion.identity);

            float angle = i * (360 / numOfProjectiles);
            knife.transform.localEulerAngles = new Vector3(0, 0, angle);

            knife.GetComponent<Rigidbody2D>().velocity = knife.transform.right.normalized * speed;
            knife.GetComponent<KnifeObjectScript>().caster = caster;
            knife.GetComponent<KnifeObjectScript>().timeToDestroy = timeToDestroy;
            knife.GetComponent<KnifeObjectScript>().damage = projDamage;
        }
    }
}
