using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseRaritySerializeNoWeight
{
    public string rarityName;
    public Color rarityColor;
    public Vector2 scaleInInv;

    public List<BaseItem> itemsInRarity;
}
