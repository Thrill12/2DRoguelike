using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using System.Linq;

public class GeneralManager : MonoBehaviour
{   
    public float gamePoints;
    public float timer;
    public bool canCountTime = true;
    public float bossesToSpawn;

    private GameObject player;
    private DifficultyManager diffManager;
    [HideInInspector]
    public bool hasSpawnedPortal;
    private LevelGenerator levelGen;
    private UIManager uiManager;
    private MenuManager menuManager;
    private int temp;

    [Space(10)]

    public GameObject playerPrefab;
    public GameObject difficultyManager;
    public GameObject globalVolume;
    public GameObject inputManager;
    public GameObject prefabManager;

    [Space(10)]

    public int levelsCompleted;
    public int enemiesToKill;

    [Space(5)]

    public Material lit;
    public Material unlit;

    [Space(5)]

    public List<string> sceneNames;

    private string index;

    public float soulMarks;
    private float tempMarks;
    public bool isLoading = false;

    private string savingPath;

    // Start is called before the first frame update
    void Start()
    {
        savingPath = Application.persistentDataPath + "/FractSave.frctsv";

        InitializeObjects();
        levelsCompleted = 1;

        if(player != null)
        {
            enemiesToKill = Mathf.FloorToInt(20 * Mathf.Pow(levelsCompleted, 0.7f)) - player.gameObject.GetComponent<PlayerItemsSkills>().equippedItems.
            Where(x => x.GetComponent<BaseItemObject>().itemName == "Silly Syringe").Count();
        }      

        soulMarks = GetValueInPlayerPrefs("SoulMarks");
    }

    public void InitializeObjects()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player");
            diffManager = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>();
            globalVolume = GameObject.FindGameObjectWithTag("GlobalVolume");
            inputManager = GameObject.FindGameObjectWithTag("InputManager");
            prefabManager = GameObject.FindGameObjectWithTag("PrefabManager");
            uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            levelGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
            menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        }
        catch
        {
            Debug.Log("An object couldn't be found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            bossesToSpawn = player.GetComponent<PlayerItemsSkills>().equippedItems.Where(x => x.GetComponent<BaseItemObject>().itemName == "Blunt Butter Blade").Count() + 1;

            if (temp != player.GetComponent<Player>().enemiesKilled)
            {
                if (player.GetComponent<Player>().enemiesKilled >= enemiesToKill)
                {
                    if (!hasSpawnedPortal)
                    {
                        hasSpawnedPortal = true;
                        levelGen.SpawnTeleporter();
                    }
                }
            }

            temp = player.GetComponent<Player>().enemiesKilled;
        }     

        if (canCountTime && SceneManager.GetActiveScene().name != "Coasts of Time")
        {
            timer += Time.deltaTime;
        }        

        var tempScore = gamePoints;

        if (gamePoints > GetValueInPlayerPrefs("GamePoints"))
        {
            SaveValueInPlayerPrefs("GamePoints", gamePoints);
        }

        SaveValueInPlayerPrefs("SoulMarks", soulMarks);
    }

    public void SaveValueInPlayerPrefs(string valueNameInPref, float value)
    {
        PlayerPrefs.SetFloat(valueNameInPref, value);
    }

    public float GetValueInPlayerPrefs(string valueNameInPref)
    {
        return PlayerPrefs.GetFloat(valueNameInPref);
    }

    public void Save()
    {
        Save save = CreateSaveObject();

        string json = JsonUtility.ToJson(save);
        StreamWriter file = new StreamWriter(savingPath, false);
        file.Write(json);
        file.Close();

        Debug.Log("Game Saved at " + savingPath);
    }

    public void Load()
    {
        if(File.Exists(savingPath))
        {
            StreamReader file = new StreamReader(savingPath);
            string json = file.ReadToEnd();
            Save save = new Save();
            JsonUtility.FromJsonOverwrite(json, save);

            player.GetComponentInChildren<SkillManager>().unlockedSkills = save.unlockedSkills;
            player.GetComponentInChildren<SkillManager>().lockedSkills = save.lockedSkills;
        }
        else
        {
            Debug.Log("No game loaded");
        }
    }

    public Save CreateSaveObject()
    {
        Save save = new Save();

        List<ScrObjAbility> unlocked = player.GetComponentInChildren<SkillManager>().unlockedSkills;
        save.unlockedSkills = unlocked;
        List<ScrObjAbility> locked = player.GetComponentInChildren<SkillManager>().lockedSkills;
        save.lockedSkills = locked;

        return save;
    }

    public void NextLevel()
    {
        index = sceneNames[Random.Range(0, sceneNames.Count)];

        LoadIntoScene(index, player);   
    }

    public void LoadIntoScene(string sceneName, GameObject playerr)
    {
        isLoading = true;
        Debug.Log("Loading scene " + sceneName);
        InitializeObjects();
        diffManager.stagesCompleted = levelsCompleted;
        StartCoroutine(LoadNextLevel(sceneName, playerr));
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Loop " + levelsCompleted, "Score " + gamePoints);
        InitializeObjects();
        isLoading = false;
    }

    private IEnumerator LoadNextLevel(string index, GameObject player)
    {
        menuManager.PanelFadeIn();

        Debug.Log("Panel Faded In");

        yield return new WaitForSecondsRealtime(1.5f);

        player.GetComponent<Player>().enemiesKilled = 0;

        Debug.Log("About to load scene " + index);

        SceneManager.LoadScene(index);

        player.transform.position = new Vector2(0, 4);          

        if (SceneManager.GetActiveScene().name != "Coasts of Time")
        {
            levelsCompleted += 1;
        }      

        Debug.Log("Successfully loaded scene " + index);      

        Time.timeScale = 0;

        Debug.Log("About to show loop displayeer");

        menuManager.LoopDisplayerFadeIn();

        yield return new WaitForSecondsRealtime(2f);

        menuManager.LoopDisplayerFadeOut();        

        yield return new WaitForSecondsRealtime(0.5f);

        ResumeTime();
    }

    public void ResumeTime()
    {
        Debug.Log("Resuming time");
        Time.timeScale = 1;
    }

    public void OnLevelWasLoaded(int level)
    {
        enemiesToKill = Mathf.FloorToInt(20 * Mathf.Pow(levelsCompleted, 0.7f)) - player.gameObject.GetComponent<PlayerItemsSkills>().equippedItems.
            Where(x => x.GetComponent<BaseItemObject>().itemName == "Silly Syringe").Count();
        InitializeObjects();
        //RestartEssentials();    
        
        levelGen.hasSpawnedTeleporter = false;
        hasSpawnedPortal = false;
    }
}
