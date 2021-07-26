using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCameraOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ShakeCamera();
    }
}
