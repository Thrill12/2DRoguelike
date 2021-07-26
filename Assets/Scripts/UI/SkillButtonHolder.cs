using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkillButtonHolder : MonoBehaviour
{
    public ScrObjAbility skill;
    public int holderNum;
    private Image image;
    private TMP_Text text;
    private SkillManager skills;
    private UIManager uiManager;
    private AbilityHolder holder;

    public void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
        skills = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkillManager>();
        uiManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<UIManager>();
        holder = GameObject.FindGameObjectWithTag("Player").GetComponents<AbilityHolder>()[holderNum];        
    }

    public void OnEnable()
    {
        GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>().Load();
    }

    public void Update()
    {
        if (skills.lockedSkills.Select(x => x.name).Where(x => x == skill.name).Any())
        {
            image.sprite = uiManager.lockedSkill;
            text.text = "Locked";
        }
        else
        {
            image.sprite = skill.skillIcon;
            text.text = skill.name;
        }
    }

    public void EquipSkill()
    {
        if (skills.unlockedSkills.Contains(skill))
        {
            holder.ability = Instantiate(skill);
        }        
    }
}
