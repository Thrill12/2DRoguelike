using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public GameObject puffOfAir;
    private BaseEntity obj;

    private void Start()
    {
        obj = GetComponentInParent<BaseEntity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Platform") || collision.transform.CompareTag("GroundPlatform"))
        {
            obj.ResetJumpCounter();
            Vector2 spawnPos = new Vector2((GetComponent<Collider2D>().bounds.min.x + GetComponent<Collider2D>().bounds.max.x) / 2,
                GetComponent<Collider2D>().bounds.min.y);

            Instantiate(puffOfAir, spawnPos, Quaternion.identity);
        }      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Platform") || collision.transform.CompareTag("GroundPlatform"))
        {
            obj.ResetJumpCounter();
            Vector2 spawnPos = new Vector2((GetComponent<Collider2D>().bounds.min.x + GetComponent<Collider2D>().bounds.max.x) / 2,
                GetComponent<Collider2D>().bounds.min.y);

            Instantiate(puffOfAir, spawnPos, Quaternion.identity);
        }
    }
}
