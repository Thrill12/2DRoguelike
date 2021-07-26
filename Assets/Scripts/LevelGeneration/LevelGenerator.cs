using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> buildings;
    public List<GameObject> buildingsNoLamp;
    public List<GameObject> smallAttachments;
    public int genRuns;
    public int maxAttachments;
    public int maxStartNumber;
    public bool hasSpawnedTeleporter;

    public GameObject teleporter;
    public GameObject zellTeleporter;
    private List<GameObject> neighbours;

    private Player player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //SpawnStartBuildings();        
    }

    public void OnLevelWasLoaded(int level)
    {
        Debug.Log("On level was loaded triggered");
        SpawnStartBuildings();
    }

    public Vector2 PickRandomPlaceOnPlatform()
    {
        GameObject platform = GameObject.FindGameObjectWithTag("GroundPlatform");
        Vector2 spawnPos = new Vector2();
        neighbours = new List<GameObject>();

        int tries = 0;

        do
        {
            tries++;

            if (tries > 10)
            {
                break;
            }

            spawnPos = new Vector2(Random.Range(platform.GetComponent<Collider2D>().bounds.min.x + 5,
            platform.GetComponent<Collider2D>().bounds.max.x - 5), platform.GetComponent<Collider2D>().bounds.max.y);

            neighbours = GameObject.FindGameObjectsWithTag("Building").Where(b => Vector2.Distance(b.transform.position, spawnPos) < 3).ToList();            

        } while (neighbours.Count > 0);      

        return spawnPos;
    }

    public Vector2 PickRandomPlaceOnPlatformNoBuildReq()
    {
        GameObject platform = GameObject.FindGameObjectWithTag("GroundPlatform");
        Vector2 spawnPos = new Vector2();

        spawnPos = new Vector2(Random.Range(platform.GetComponent<Collider2D>().bounds.min.x + 5,
            platform.GetComponent<Collider2D>().bounds.max.x - 5), platform.GetComponent<Collider2D>().bounds.max.y);

        return spawnPos;
    }

    public void SpawnBuildings()
    {
        List<GameObject> surfaces = GameObject.FindGameObjectsWithTag("SpawnSurface").ToList();
        for (int i = 1; i <= genRuns; i++)
        {
            try
            {
                int surfChooser = Random.Range(0, surfaces.Where(x => x.GetComponent<SpawnSurfaceNew>().hasBeenUsed == false).ToList().Count);

                GameObject surff = surfaces.Where(x => x.GetComponent<SpawnSurfaceNew>().hasBeenUsed == false).ToList()[surfChooser];

                int buildIndex = Random.Range(0, buildings.Count);

                GameObject built = Instantiate(buildings[buildIndex], surff.transform.position, Quaternion.identity);
                built.transform.parent = GameObject.FindGameObjectWithTag("LevelGameObject").transform;

                int scaleChooser = Random.Range(0, 1);

                if (scaleChooser == 0)
                {
                    built.transform.localScale = new Vector3(-1, 1, 1);
                }

                Debug.Log("Spawned Attachment");

                surff.GetComponent<SpawnSurfaceNew>().hasBeenUsed = true;

                surfaces = GameObject.FindGameObjectsWithTag("SpawnSurface").ToList();
            }
            catch
            {
                Debug.Log("Something went wrong in generating building");
            }
           
        }

        Debug.Log("About to spawn attachments");

        SpawnAttachments();
    }

    public void SpawnAttachments()
    {
        List<GameObject> surfaces = GameObject.FindGameObjectsWithTag("SmallAttachmentSpawn").ToList();

        for(int i = 0; i < maxAttachments; i++)
        {

            int surfChooser = Random.Range(0, surfaces.Where(x => x.GetComponent<SpawnSurfaceNew>().hasBeenUsed == false).ToList().Count);

            GameObject surff = surfaces.Where(x => x.GetComponent<SpawnSurfaceNew>().hasBeenUsed == false).ToList()[surfChooser];

            int buildIndex = Random.Range(0, smallAttachments.Count);

            GameObject built = Instantiate(smallAttachments[buildIndex], surff.transform.position, Quaternion.identity);
            built.transform.parent = GameObject.FindGameObjectWithTag("LevelGameObject").transform;

            int scaleChooser = Random.Range(0, 1);

            if (scaleChooser == 0)
            {
                built.transform.localScale = new Vector3(-1, 1, 1);
            }

            surff.GetComponent<SpawnSurfaceNew>().hasBeenUsed = true;

            surfaces = GameObject.FindGameObjectsWithTag("SmallAttachmentSpawn").ToList();
        }
    }

    public void SpawnStartBuildings()
    {
        for(int i = 0; i <= maxStartNumber; i++)
        {
            int buildIndex = Random.Range(0, buildingsNoLamp.Count);

            GameObject built = Instantiate(buildingsNoLamp[buildIndex], PickRandomPlaceOnPlatform(), Quaternion.identity);
            built.transform.parent = GameObject.FindGameObjectWithTag("LevelGameObject").transform;

            int scaleChooser = Random.Range(0, 2);

            if(scaleChooser > 0)
            {
                built.transform.localScale = new Vector3(-1, 1, 1);
            }            
        }

        SpawnInteractables();
        SpawnBuildings();
        
    }

    public void SpawnTeleporter()
    {
        Debug.Log("Spawned TELEPORTER");

        if (SceneManager.GetActiveScene().name != "Fields of Desertion")
        {
            Vector2 spawnPos = new Vector2(Random.Range(player.transform.position.x - 5, player.transform.position.x + 5), -15);
            GameObject tp = Instantiate(teleporter, spawnPos, Quaternion.identity);
            hasSpawnedTeleporter = true;
            player.portal = tp;
            player.TogglePortalArrow();

            int rand = Random.Range(0, 101);

            if(rand <= 25)
            {
                GameObject zellTP = Instantiate(zellTeleporter, new Vector2(spawnPos.x + 7, spawnPos.y), Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("Must use SpawnTeleporter(Vector2 spawnPos)");
        }
    }

    public void SpawnTeleporter(Vector2 spawnPos)
    {
        GameObject tp = Instantiate(teleporter, spawnPos, Quaternion.identity);
        hasSpawnedTeleporter = true;
        player.portal = tp;
        player.TogglePortalArrow();
    }

    public void SpawnInteractables()
    {
        float chance = Random.Range(0, 101);

        if(chance < 100)
        {
            GameObject interactable = Instantiate(GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>().interactables.GetRandomObject(), 
                PickRandomPlaceOnPlatformNoBuildReq(), Quaternion.identity);

            chance = Random.Range(0, 101);

            if(chance < 100)
            {
                GameObject interactablee = Instantiate(GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>().interactables.GetRandomObject(),
                PickRandomPlaceOnPlatformNoBuildReq(), Quaternion.identity);
            }
        }
    }
}
