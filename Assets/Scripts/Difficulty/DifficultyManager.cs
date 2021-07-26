using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public float timeFactor = 0.1f;
    public float diffCoeff = 1;
    public int enemyCount;

    public int stagesCompleted;

    private GeneralManager genManager;
    private UIManager uiManager;
    private float timeInMinutes;
    public float stageFactor;
    public float playerFactor = 1f;

    public EnemyTable bossTable;

    private void Start()
    {
        genManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
    }

    private void OnLevelWasLoaded(int level)
    {
        Start();
    }

    public void Update()
    {
        timeInMinutes = genManager.timer / 60;
        stageFactor = Mathf.Pow(1.06f, stagesCompleted);

        diffCoeff = (playerFactor + timeInMinutes * timeFactor) * Mathf.Pow(1.7f, stageFactor);
     
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void SpawnBoss()
    {
        for(int i = 0; i < genManager.bossesToSpawn; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(GameObject.FindGameObjectWithTag("Portal").transform.position.x - 10,
                GameObject.FindGameObjectWithTag("Portal").transform.position.x + 10), GameObject.FindGameObjectWithTag("Portal").transform.position.y + 40);

            GameObject boss = Instantiate(bossTable.GetRandomEnemy(), spawnPos, Quaternion.identity);
        }            
    }
}
