using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ps : MonoBehaviour
{
    private ParticleSystem pss;

    private void Start()
    {
        pss = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (pss)
        {
            if (!pss.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
