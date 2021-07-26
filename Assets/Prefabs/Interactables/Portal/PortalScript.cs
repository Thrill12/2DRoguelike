using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PortalScript : BaseInteractable
{
    private GeneralManager gen;
    private bool hasClicked = false;
    private bool hasKilledBoss;
    private DifficultyManager diffManager;
    public Light2D portalLight;
    public Color colorInBoss;
    public Color colorAfterBoss;
    private SoundClipLibrary sounds;
    private GeneralManager genManager;

    private bool hasUpdated;
    public AudioSource hornSource;

    private bool isTransitioning = false;

    public override void Start()
    {
        base.Start();
        hasUpdated = false;
        gen = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
        diffManager = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>();
        sounds = GameObject.FindGameObjectWithTag("SoundClipLibrary").GetComponent<SoundClipLibrary>();
        genManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
    }

    public override void Update()
    {
        base.Update();

        List<GameObject> bossesRemaining = GameObject.FindGameObjectsWithTag("Boss").Select(x => x.gameObject).ToList();

        if(bossesRemaining.Count == 0 && hasClicked && !hasUpdated)
        {
            hasKilledBoss = true;
            sounds.SwitchToNormalMusic();            
            portalLight.color = colorAfterBoss;
            player.portalArrow.GetComponent<SpriteRenderer>().color = colorAfterBoss;
            
            hasUpdated = true;
        }
    }

    public override void Interact()
    {
        if (!hasClicked)
        {            
            if(SceneManager.GetActiveScene().name != "Coasts of Time")
            {
                base.Interact();
                hasClicked = true;               
                SpawnBoss();
                sounds.SwitchToBossMusic();
            }
            else
            {
                hasClicked = true;
                base.Interact();
            }     
        }    
        else if (hasKilledBoss)
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                genManager.SaveValueInPlayerPrefs("TotalLoops", genManager.GetValueInPlayerPrefs("TotalLoops") + 1);
                NextLevel();               
            }            
        }
    }

    public void NextLevel()
    {
        gen.NextLevel();
    }

    public void SoundHorn()
    {
        hornSource.Play();
    }

    public void SpawnBoss()
    {
        SoundHorn();
        portalLight.color = colorInBoss;
        player.portalArrow.GetComponent<SpriteRenderer>().color = colorInBoss;
        diffManager.SpawnBoss();
    }
}
