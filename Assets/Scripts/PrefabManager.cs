using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [Header("Popups")]

    public GameObject damageTextPopup;
    public GameObject regenHealthTextPopup;
    public GameObject itemTextPopup;

    [Space(5)]

    [Header("Healthbars")]

    public GameObject normalEnemyHealthBar;

    [Space(5)]

    [Header("Lights")]

    public GameObject simpleShootingShootLight;

    [Space(5)]

    [Header("Debuffs")]

    public GameObject bleed;
    public GameObject ignite;

    [Space(5)]

    [Header("Effects")]

    public GameObject puff;
    public GameObject soulmarkGain;

    [Space(5)]

    [Header("Skill Effects")]

    public GameObject slashEffect;

    [Space(5)]

    [Header("Item Effects")]

    public GameObject meteor;

    [Space(5)]

    [Header("Interactables")]

    public GameObjectTable interactables;

}
