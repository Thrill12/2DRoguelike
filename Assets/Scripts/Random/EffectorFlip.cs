using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectorFlip : MonoBehaviour
{
    private float waitTime = 0.1f;
    private PlatformEffector2D effector;
    private InputManager input;
    private List<Collider2D> cols;


    private void Start()
    {
        cols = new List<Collider2D>();
        effector = GetComponent<PlatformEffector2D>();
        input = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }

    private void Update()
    {
        if (Input.GetKey(input.fallEffector))
        {
            if(waitTime <= 0)
            {
                effector.rotationalOffset = 180;
                waitTime = 0.3f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(input.fallEffector))
        {
            waitTime = 0.3f;
        }

        if (Input.GetKey(input.jump))
        {
            effector.rotationalOffset = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        effector.rotationalOffset = 0;
    }
}
