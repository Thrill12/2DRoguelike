using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    [Header("Core Stats")]
    public float cooldown;
    public float maxRangeForEnemyTriggering;

    [Space(20)]

    public KeyCode triggerKey;

    private float nextFire;
    [HideInInspector]
    public MenuManager menuManager;
    [HideInInspector]
    public PrefabManager pfManager;

    [HideInInspector]
    public Vector3 target;

    private void Start()
    {       
        SetTarget();
        try
        {
            menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
            pfManager = GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>();
        }
        catch
        {
            Debug.LogError("Couldnt find an object in baseabillity of " + gameObject.name);
        }       
    }

    public virtual void Update()
    {
        
    }

    public void DoAbilityWithCooldown()
    {
        if(Time.timeScale != 0)
        {
            if (gameObject.transform.tag == "Player")
            {
                if (Input.GetKey(triggerKey) && Time.time > nextFire)
                {
                    nextFire = Time.time + cooldown;

                    DoAbility();
                }
            }
            else
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + cooldown;

                    DoAbility();
                }
            }
        }             
    }

    public virtual void DoAbility()
    {

    }

    public void SetTarget()
    {
        if (gameObject.transform.tag == "Player")
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }
}
