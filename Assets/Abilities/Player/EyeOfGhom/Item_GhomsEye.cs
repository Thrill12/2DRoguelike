using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_GhomsEye : BaseItemObject
{
    public ScrObjAbility skill;

    public override void ActivateBonus()
    {
        base.ActivateBonus();
        player.GetComponentInChildren<SkillManager>().UnlockSkill(skill);
        genManager.Save();
        genManager.Load();
    }
}
