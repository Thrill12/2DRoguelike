using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhomLaserDamage : MonoBehaviour
{
    [HideInInspector]
    public LayerMask layerToAttack;

    [HideInInspector]
    public float damage = 0;
    private List<GameObject> colliding;

    [HideInInspector]
    public GameObject source;

    [HideInInspector]
    public GameObject caster;

    private Hits hits;

    // Start is called before the first frame update
    void Start()
    {
        colliding = new List<GameObject>();

        hits = new Hits();

        if(transform.parent != null)
        {
            damage = GetComponentInParent<GolemScript>().laserDamagePerTick * GetComponentInParent<GolemScript>().damageMultiplier;            
        }

        StartCoroutine(StartDamage());
    }

    public void Update()
    {
        if(source != null)
        {
            transform.position = source.transform.position;
            transform.rotation = source.transform.rotation;
        }        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        colliding.Add(collision.gameObject);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        colliding.Remove(collision.gameObject);   
    }

    public IEnumerator StartDamage()
    {
        ContactFilter2D filt = new ContactFilter2D();
        filt.layerMask = layerToAttack;

        while (true)
        {
            colliding = colliding.Where(x => x != null).ToList();

            foreach(GameObject obj in colliding.Select(x => x.gameObject).Where(x => x.GetComponent<BaseEntity>()))
            {
                if(source != null)
                {
                    if(obj.transform.tag != "Player")
                    {
                        if (obj != null)
                        {
                            obj.GetComponent<BaseEntity>().TakeDamage(damage, caster.GetComponent<BaseEntity>());
                            caster.GetComponent<BaseEntity>().OnHit(obj);
                        }
                        else
                        {
                            colliding.Remove(obj);
                        }                        
                    }                   
                }
                else if(source == null)
                {
                    if (obj.transform.tag != "Enemy" || obj.transform.tag != "Boss")
                    {
                        if(obj != null)
                        {
                            obj.GetComponent<BaseEntity>().TakeDamage(damage, caster.GetComponent<BaseEntity>());
                            caster.GetComponent<BaseEntity>().OnHit(obj);
                        }
                        else
                        {
                            colliding.Remove(obj);
                        }
                    }                  
                }
            }

            if (source != null)
            {
                source.transform.parent.GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();
            }

            yield return new WaitForSeconds(0.1f);
        }       
    }
}
