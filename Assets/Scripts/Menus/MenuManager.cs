using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject whitePanel;
    public GameObject greyPanel;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject loopDisplayer;

    [Space(5)]

    [Header("Death screen texts")]

    public TMP_Text currentRunScore;
    public TMP_Text highScoreText;
    public TMP_Text totalKills;
    public TMP_Text totalDeaths;
    public TMP_Text itemsPickedUp;
    public TMP_Text totalLoops;

    [Space(5)]

    public GameObject discordPresence;

    private GeneralManager genManager;

    private bool canPause = false;
    public bool isPaused = false;

    public void Start()
    {
        Instantiate(discordPresence);
        isPaused = false;
        PanelFadeOut();
    }

    public void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseMenu.activeInHierarchy)
                {
                    DisablePauseMenu();
                }
                else
                {
                    EnablePauseMenu();
                }
            }
        }
    }

    public void SetDeathScreenStats()
    {
        currentRunScore.text = "Score: " + Mathf.RoundToInt(genManager.gamePoints).ToString();
        highScoreText.text = "Highscore: " + Mathf.RoundToInt(genManager.GetValueInPlayerPrefs("GamePoints")).ToString();
        totalKills.text = "Total kills: " + genManager.GetValueInPlayerPrefs("TotalKills").ToString();
        totalDeaths.text = "Total deaths: " + genManager.GetValueInPlayerPrefs("TotalDeaths").ToString();
        itemsPickedUp.text = "Items picked up: " + genManager.GetValueInPlayerPrefs("ItemsPickedUp").ToString();
        totalLoops.text = "Total loops: " + genManager.GetValueInPlayerPrefs("TotalLoops").ToString();
    }

    public void AnimateToPlayScene()
    {
        StartCoroutine(AnimateToPlayScenee());
    }

    private IEnumerator AnimateToPlayScenee()
    {
        PanelFadeIn();

        yield return new WaitForSeconds(2.1f);

        ToPlayScene();
    }

    private void ToPlayScene()
    {        
        canPause = true;

        HideDeathMenu();

        SceneManager.LoadScene("Coasts of Time");
        mainMenu.SetActive(false);

        DeleteConsistentObjects();
    }

    public void OnLevelWasLoaded(int level)
    {
        try
        {
            genManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
        }
        catch
        {
            Debug.LogWarning("Couldn't find general manager");
        }

        PanelFadeOut();
        Time.timeScale = 1;
    }

    public void EnablePauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void DisablePauseMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void AnimateQuitToMenu()
    {
        StartCoroutine(AnimateQuitToMenuu());
    }

    private IEnumerator AnimateQuitToMenuu()
    {
        PanelFadeIn();

        yield return new WaitForSecondsRealtime(2.1f);

        QuitToMenu();
    }

    public void DeleteConsistentObjects()
    {
        ConsistentObject[] objs = Object.FindObjectsOfType<ConsistentObject>();

        foreach(ConsistentObject obj in objs)
        {
            if(obj.gameObject != gameObject)
            {
                Destroy(obj.gameObject);
            }
        }
    }

    private void QuitToMenu()
    {       
        canPause = false;

        HideDeathMenu();
        DisablePauseMenu();

        SceneManager.LoadScene(0);

        mainMenu.SetActive(true);

        DeleteConsistentObjects();      
    }

    public void ShowDeathMenu()
    {
        SetDeathScreenStats();

        deathMenu.SetActive(true);
        greyPanel.SetActive(true);        
    }

    public void HideDeathMenu()
    {
        deathMenu.SetActive(false);
        greyPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AnimateNextStage()
    {
        StartCoroutine(AnimateNextStagee());
    }

    private IEnumerator AnimateNextStagee()
    {
        PanelFadeIn();

        yield return new WaitForSecondsRealtime(0.6f);

        Time.timeScale = 0;

        loopDisplayer.GetComponent<TMP_Text>().text = genManager.levelsCompleted.ToString();
        LoopDisplayerFadeIn();
    }

    #region UIAnimations

    public void PanelFadeOut()
    {
        whitePanel.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public void PanelFadeIn()
    {
        whitePanel.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void PanelFadeInFromRight()
    {
        whitePanel.GetComponent<Animator>().SetTrigger("FadeInFromRight");
    }

    public void LoopDisplayerFadeIn()
    {
        loopDisplayer.GetComponent<TMP_Text>().text = genManager.levelsCompleted.ToString();
        loopDisplayer.GetComponent<Animator>().SetTrigger("LoopNumberShow");
    }

    public void LoopDisplayerFadeOut()
    {
        loopDisplayer.GetComponent<Animator>().SetTrigger("LoopNumberHide");
    }

    #endregion
}
