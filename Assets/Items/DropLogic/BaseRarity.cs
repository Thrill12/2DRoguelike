using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rarity", menuName = "Rarity")]
public class BaseRarity : ScriptableObject
{
    [SerializeField] public BaseRaritySerializeNoWeight rar;
    [SerializeField] public float rarityWeight;
}
