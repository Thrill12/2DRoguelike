using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseDebuff : MonoBehaviour
{
    public string debuffName;
    public Sprite debuffIcon;
    public float cooldown;
    public float debuffExpiry;

    public TextMesh effectCounter;

    private float nextTrigger;
    private float expireTime;

    public float debStacks;

    public GameObject particles;

    [HideInInspector]
    public BaseEntity entityAffected;

    [HideInInspector]
    public BaseEntity entityAffecting;

    [HideInInspector]
    public PrefabManager pfManager;

    public virtual void Start()
    {
        pfManager = GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>();

        for(int i = 0; i < entityAffected.debuffs.Count(x => x.GetType().Name == this.GetType().Name); i++)
        {
            entityAffected.debuffs[i].SetTime();
        }

        SetTime();

        
    }

    public virtual void DoEffect()
    {
        
    }

    public virtual void Update()
    {
        if(effectCounter != null)
        {
            effectCounter.text = "x" + debStacks.ToString();
        }        

        if(entityAffected != null)
        {
            DoEffectWithCooldown();
        }

        particles.transform.position = entityAffected.transform.position;
    }

    public void DoEffectWithCooldown()
    {
        if(Time.time < expireTime)
        {
            if (Time.timeScale != 0)
            {
                if (Time.time > nextTrigger)
                {
                    nextTrigger = Time.time + cooldown;

                    DoEffect();
                }
            }
        }
        else
        {
            entityAffected.debuffs.Remove(gameObject.GetComponent<BaseDebuff>());
            Destroy(gameObject);
        }        
    }

    public void SetTime()
    {
        expireTime = Time.time + debuffExpiry;
    }
}
