using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [HideInInspector]
    public GameObject obj;
    public GameObject spawnSurface;

    private void Start()
    {
        obj = gameObject;
    }

    public Building(GameObject obj, GameObject spawnSurface)
    {
        this.obj = obj;
        this.spawnSurface = spawnSurface;
    }
}
