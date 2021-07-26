using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsistentObject : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(gameObject.transform.tag);

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
