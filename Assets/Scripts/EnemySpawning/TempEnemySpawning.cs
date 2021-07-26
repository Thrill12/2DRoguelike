using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempEnemySpawning : MonoBehaviour
{
    public float spawnInterval;
    public float baseSpawnInterval;
    public EnemyTable table;

    [Tooltip("Both on either side")]
    public float spawnSizeX;
    [Tooltip("This amount upwards only")]
    public float spawnSizeY;
    public float distanceFromPlayer;

    private float nextSpawn;

    private DifficultyManager diffM;
    private GameObject player;
    private LevelGenerator levelGen;
    private Coroutine cor;
    public bool canSpawn = true;

    private void Start()
    {
        diffM = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();

        spawnInterval = baseSpawnInterval;
        StopAllCoroutines();
        cor = StartCoroutine(SpawnEnemyCoroutine());

        if(SceneManager.GetActiveScene().name == "Coasts of Time")
        {
            canSpawn = false;
        }
        else if(SceneManager.GetActiveScene().name == "Fields of Desertion")
        {
            canSpawn = false;
        }
        else
        {
            canSpawn = true;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Start();
        spawnInterval = 2.5f * Mathf.Pow(0.9f, diffM.stagesCompleted - 1) + 0.5f;
    }

    public IEnumerator SpawnEnemyCoroutine()
    {
        while (!levelGen.hasSpawnedTeleporter)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);            
        }
    }

    public void SpawnEnemy()
    {       
        if(SceneManager.GetActiveScene().name != "Coasts of Time" && SceneManager.GetActiveScene().name != "Fields of Desertion")
        {
            if (canSpawn)
            {
                var obj = table.GetRandomEnemy();
                Instantiate(obj, PickSpawnLocation(), transform.rotation);
            }           
        }        
    }

    public Vector3 PickSpawnLocation()
    {
        Vector2 spawnLoc = new Vector3();

        do
        {
            spawnLoc = new Vector2(Random.Range(player.transform.position.x + distanceFromPlayer, player.transform.position.x - distanceFromPlayer),
            Random.Range(player.transform.position.y, player.transform.position.y + distanceFromPlayer));

        } while (spawnLoc.x < -spawnSizeX && spawnLoc.x > spawnSizeX && spawnLoc.y < 5 && spawnLoc.y > spawnSizeY && Vector2.Distance(spawnLoc, player.transform.position) > 1);

        return spawnLoc;
    }
}
