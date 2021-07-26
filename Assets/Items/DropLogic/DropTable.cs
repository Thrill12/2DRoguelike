using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DropTable", menuName = "Loot Table")]
public class DropTable : ScriptableObject
{
    [SerializeField] private List<BaseRaritySerialize> rarities;

    [System.NonSerialized] private bool IsInitItems = false;
    [System.NonSerialized] private bool IsInitRarities = false;

    [System.NonSerialized] private float totalItemWeight;
    [System.NonSerialized] private float totalRarityWeight;
    public BaseRarity rarChosen;

    private void InitializeItems()
    {     
        if (!IsInitItems)
        {
            foreach(BaseItem item in rarChosen.rar.itemsInRarity)
            {
                totalItemWeight += item.itemWeight;
            }
            IsInitItems = true;
        }
    }

    private void InitializeRarities()
    {
        if (!IsInitRarities)
        {
            foreach(BaseRaritySerialize rar in rarities)
            {
                totalRarityWeight += rar.rarityWeight;
            }
            IsInitRarities = true;
        }
    }

    public GameObject GetRandomItem()
    {
        rarChosen = GetRandomRarity();

        InitializeItems();

        float diceRoll = Random.Range(0, totalItemWeight);

        foreach(BaseItem item in rarChosen.rar.itemsInRarity)
        {
            if(item.itemWeight >= diceRoll)
            {
                return item.itemObject;
            }

            diceRoll -= item.itemWeight;
        }

        throw new System.Exception("Item Generation Failed");
    }  

    public BaseRarity GetRandomRarity()
    {
        InitializeRarities();

        float diceRollR = Random.Range(0, totalRarityWeight);

        //Debug.Log(diceRollR + " is current dice roll");

        foreach (var rarity in rarities)
        {
            if(rarity.rarityWeight >= diceRollR)
            {
                //Debug.Log(rarity.rarity.rar.rarityName + " is rarity name");
                return rarity.rarity;
            }

            diceRollR -= rarity.rarityWeight;
        }

        throw new System.Exception("Rarity Generation Failed");
    }

//    InitializeItems();

//    float diceRoll = Random.Range(0, totalItemWeight);

//        foreach(var item in items)
//        {
//            if(item.rar. >= diceRoll)
//            {
//                return item.itemObject;
//            }

//diceRoll -= item.itemWeight;
//        }

//        throw new System.Exception("Item Generation Failed");
}
