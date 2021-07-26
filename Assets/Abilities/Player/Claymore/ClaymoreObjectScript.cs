using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreObjectScript : MonoBehaviour
{
    public float damage;
    public bool hasLanded = false;
    public GameObject caster;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SelfDestruction>().projSender = caster;
        GetComponent<SelfDestruction>().damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLanded)
        {
            GetComponent<Rigidbody2D>().mass = 1;
            GetComponent<Rigidbody2D>().drag = 0.05f;
        }
        else
        {
            GetComponent<Rigidbody2D>().mass = 10000;
            GetComponent<Rigidbody2D>().drag = 10000;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BaseEnemyObject>())
        {
            GetComponent<SelfDestruction>().ExplodeDamage();
        }        
    }
}
