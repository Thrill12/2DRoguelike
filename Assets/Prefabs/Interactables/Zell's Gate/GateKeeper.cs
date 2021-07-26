using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GateKeeper : BaseInteractable
{
    public GameObject zellGate;
    public GameObject zell;

    private GameObject zellSpawned;

    private bool hasSpawnedZell;
    private bool hasKilledBoss;
    private bool hasUpdated;
    private bool isTransitioning = false;
    private bool hasClicked;

    private Color origColorZellGate;
    private SoundClipLibrary sounds;
    private GeneralManager genManager;

    public override void Start()
    {
        base.Start();
        hasUpdated = false;
        origColorZellGate = zellGate.GetComponentInChildren<Light2D>().color;
        sounds = GameObject.FindGameObjectWithTag("SoundClipLibrary").GetComponent<SoundClipLibrary>();
        genManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
    }

    public override void Interact()
    {
        if (!hasSpawnedZell)
        {
            base.Interact();
            hasClicked = true;           
            Invoke("SpawnZell", 4.5f);
            zellGate.GetComponentInChildren<Light2D>().color = Color.red;            
            zellGate.GetComponent<AudioSource>().Play();
            sounds.SwitchToBossMusic();
            GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().hasSpawnedTeleporter = true;
            genManager.hasSpawnedPortal = true;
        }
        if(hasKilledBoss)
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                genManager.SaveValueInPlayerPrefs("TotalLoops", genManager.GetValueInPlayerPrefs("TotalLoops") + 1);
                genManager.NextLevel();
            }
        }
    }

    public override void Update()
    {
        base.Update();

        List<GameObject> bossesRemaining = GameObject.FindGameObjectsWithTag("Boss").Select(x => x.gameObject).ToList();

        if (bossesRemaining.Count == 0 && hasSpawnedZell && !hasUpdated)
        {
            hasKilledBoss = true;
            sounds.SwitchToNormalMusic();
            zellGate.GetComponentInChildren<Light2D>().color = origColorZellGate;
            player.portalArrow.GetComponent<SpriteRenderer>().color = origColorZellGate;

            hasUpdated = true;
        }
    }

    public void SpawnZell()
    {
        GameObject zellSpawned = Instantiate(zell, zellGate.transform.position, Quaternion.identity);
        hasSpawnedZell = true;
    }
}
