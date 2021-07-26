using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<ScrObjAbility> unlockedSkills;
    public List<ScrObjAbility> lockedSkills;

    public void Start()
    {
        GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>().Load();
    }

    public void UnlockSkill(ScrObjAbility skill)
    {
        if(lockedSkills == null)
        {
            lockedSkills = new List<ScrObjAbility>();
        }

        if(unlockedSkills == null)
        {
            unlockedSkills = new List<ScrObjAbility>();
        }

        if(lockedSkills != null)
        {
            if (lockedSkills.Where(x => x.name == skill.name).Any())
            {
                lockedSkills.Remove(skill);
            }
            else
            {
                Debug.Log("Unlocked " + skill.name);
            }
        }
        else
        {
            Debug.Log("Locked skills was null!");
        }
        
        if(unlockedSkills != null)
        {
            if (!unlockedSkills.Where(x => x.name == skill.name).Any())
            {
                unlockedSkills.Add(skill);
            }
            else
            {
                Debug.Log("Picked up " + skill.name);
            }
        }
        Debug.Log("Unlocked skills was null!");
        
    }

    public void ReplaceSkills(AbilityHolder holder, ScrObjAbility skill)
    {
        holder.ability = skill;
    }
}
