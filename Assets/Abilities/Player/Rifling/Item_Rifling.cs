using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Rifling : BaseItemObject
{
    public ScrObjAbility skill;

    public override void ActivateBonus()
    {
        base.ActivateBonus();
        player.GetComponentInChildren<SkillManager>().UnlockSkill(skill);
        genManager.Save();
    }
}
