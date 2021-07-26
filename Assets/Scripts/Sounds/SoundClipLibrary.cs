using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundClipLibrary : MonoBehaviour
{
    public List<AudioClip> music;
    public List<AudioClip> bossMusic;
    public List<AudioClip> shootSounds; 
    public AudioClip itemPickup;
    public AudioClip hitmarker;

    public AudioSource musicSource;
    public AudioSource bossMusicSource;

    public bool isPlayingBoss;

    [Header("Skill Sounds")]

    public AudioClip slashSound;
    public AudioClip energyWhipSound;
    public AudioClip shurikenSound;

    [Header("Player")]

    public AudioClip levelUp;
    public AudioClip throwSound;
    public AudioClip healSound;
    public AudioClip interactSound;

    private int previousSong;

    private GameObject player;
    private GeneralManager genManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        genManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
    }

    public void Update()
    {
        if(player != null)
        {
            transform.position = player.transform.position;
        }

        if (Application.isFocused)
        {
            if (!musicSource.isPlaying && !genManager.isLoading)
            {
                int index = Random.Range(0, music.Count);

                musicSource.clip = music[index];
                musicSource.Play();
            }

            if (!bossMusicSource.isPlaying && !genManager.isLoading)
            {
                int index = Random.Range(0, bossMusic.Count);

                bossMusicSource.clip = bossMusic[index];
                bossMusicSource.Play();
            }
        }        
    }

    public void SwitchToBossMusic()
    {
        if (!isPlayingBoss)
        {
            StartCoroutine(StartFade(musicSource, 5, 0));
            StartCoroutine(StartFade(bossMusicSource, 5, 0.2f));
            isPlayingBoss = true;
        }       
    }

    public void SwitchToNormalMusic()
    {
        if (isPlayingBoss)
        {
            StartCoroutine(StartFade(musicSource, 5, 0.2f));
            StartCoroutine(StartFade(bossMusicSource, 5, 0f));
            isPlayingBoss = false;
        }       
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
