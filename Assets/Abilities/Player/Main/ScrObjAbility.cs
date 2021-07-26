using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Ability", menuName = "Ability"), System.Serializable]
public class ScrObjAbility : ScriptableObject
{
    public new string name;
    public Sprite skillIcon;
    public float cooldownTime;
    public float activeTime;

    [HideInInspector]
    public InputManager inputManager;
    [HideInInspector]
    public PrefabManager prefabManager;
    [HideInInspector]
    public SoundClipLibrary sounds;

    [HideInInspector]
    public GameObject caster;
    [HideInInspector]
    public GameObject shootSource;

    private float baseCooldownTime;
    [HideInInspector]
    public Vector3 mousePos;

    public void Awake()
    {
        baseCooldownTime = cooldownTime;
    }

    public virtual void Activate(GameObject caster)
    {
        if(Time.timeScale > 0)
        {
            this.caster = caster;
            cooldownTime = baseCooldownTime * caster.GetComponent<BaseEntity>().cooldownMultiplier;
            shootSource = caster.transform.Find("ShootSource").gameObject;
            inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
            mousePos = inputManager.mousePos;
            prefabManager = GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>();
            sounds = GameObject.FindGameObjectWithTag("SoundClipLibrary").GetComponent<SoundClipLibrary>();
        }       
    }

    public virtual void BeginCooldown(GameObject caster)
    {
        cooldownTime = baseCooldownTime * caster.GetComponent<BaseEntity>().cooldownMultiplier;
    }
}
