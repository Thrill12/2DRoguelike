using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightCulling : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > 35)
            {
                GetComponent<Light2D>().enabled = false;
            }
            else
            {
                GetComponent<Light2D>().enabled = true;
            }
        }        
    }
}
