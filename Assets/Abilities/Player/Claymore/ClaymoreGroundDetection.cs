using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreGroundDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Platform" || collision.transform.tag == "GroundPlatform")
        {
            GetComponentInParent<ClaymoreObjectScript>().hasLanded = true;
        }
    }
}
