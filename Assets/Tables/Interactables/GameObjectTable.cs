using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectTable", menuName = "GameObject Table")]
public class GameObjectTable : ScriptableObject
{
    [System.NonSerialized] private bool IsInitEnemies = false;

    [System.NonSerialized] private float totalEnemyWeight;
    [SerializeField] private List<BaseWeightedInteractable> objs;

    private void InitializeItems()
    {
        if (!IsInitEnemies)
        {
            foreach (BaseWeightedInteractable obj in objs)
            {
                totalEnemyWeight += obj.weight;
            }
            IsInitEnemies = true;
        }
    }

    public GameObject GetRandomObject()
    {
        InitializeItems();

        float diceRoll = Random.Range(0, totalEnemyWeight);

        foreach (BaseWeightedInteractable obj in objs)
        {
            if (obj.weight >= diceRoll)
            {
                return obj.interactable;
            }

            diceRoll -= obj.weight;
        }

        throw new System.Exception("Interactable Generation Failed");
    }
}
