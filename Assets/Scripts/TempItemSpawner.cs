using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempItemSpawner : MonoBehaviour
{
    public float spawnInterval;
    public DropTable table;

    private float nextSpawn;

    public void Update()
    {      
        if(Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnInterval;

            SpawnItem();
        }
    }

    public void SpawnItem()
    {
        var obj = table.GetRandomItem();
        Instantiate(obj, transform);
    }
}
