using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public ScrObjAbility ability;
    public float cooldownTime;
    float activeTime;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    AbilityState state = AbilityState.ready;

    public KeyCode key;
    private ScrObjAbility temp;

    public void Start()
    {
        ScrObjAbility abilityInstance = Instantiate(ability);
        ability = abilityInstance;
    }

    private void Update()
    {        
        if(Time.timeScale > 0)
        {
            switch (state)
            {
                case AbilityState.ready:
                    if (Input.GetKey(key))
                    {
                        ability.Activate(gameObject);
                        state = AbilityState.active;
                        activeTime = ability.activeTime;
                    }
                    break;
                case AbilityState.active:
                    if (activeTime > 0)
                    {
                        activeTime -= Time.deltaTime;
                    }
                    else
                    {
                        ability.BeginCooldown(gameObject);
                        state = AbilityState.cooldown;
                        cooldownTime = ability.cooldownTime;
                    }
                    break;
                case AbilityState.cooldown:
                    if (cooldownTime > 0)
                    {
                        cooldownTime -= Time.deltaTime;
                    }
                    else
                    {
                        state = AbilityState.ready;
                    }
                    break;
            }
        }             
    }
}
