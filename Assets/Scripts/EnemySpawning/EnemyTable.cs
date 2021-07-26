using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTable", menuName = "Enemy Table")]
public class EnemyTable : ScriptableObject
{
    [System.NonSerialized] private bool IsInitEnemies = false;

    [System.NonSerialized] private float totalEnemyWeight;
    [SerializeField] private List<BaseEnemy> enemies;

    private void InitializeItems()
    {
        if (!IsInitEnemies)
        {
            foreach (BaseEnemy entity in enemies)
            {
                totalEnemyWeight += entity.weight;
            }
            IsInitEnemies = true;
        }
    }

    public GameObject GetRandomEnemy()
    {
        InitializeItems();

        float diceRoll = Random.Range(0, totalEnemyWeight);

        foreach (BaseEnemy entity in enemies)
        {
            if (entity.weight >= diceRoll)
            {
                return entity.enemy;
            }

            diceRoll -= entity.weight;
        }

        throw new System.Exception("Item Generation Failed");
    }
}
