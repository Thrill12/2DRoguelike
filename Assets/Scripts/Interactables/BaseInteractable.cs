using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
    private InputManager inputManager;
    private bool canActivate;
    public GameObject textPopup;
    [HideInInspector]
    public Player player;
    private SoundClipLibrary sounds;

    private bool hasActivated;

    public virtual void Start()
    {
        sounds = GameObject.FindGameObjectWithTag("SoundClipLibrary").GetComponent<SoundClipLibrary>();
        textPopup.SetActive(false);
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void Update()
    {
        TakeActivationInput();

        if (hasActivated)
        {
            textPopup.SetActive(false);
        }
    }

    public void TakeActivationInput()
    {
        if (Input.GetKeyDown(inputManager.interact) && canActivate)
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        if (!hasActivated)
        {
            sounds.gameObject.GetComponent<AudioSource>().PlayOneShot(sounds.interactSound);
            hasActivated = true;
        }       
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasActivated)
        {
            if (collision.transform.CompareTag("Player"))
            {
                canActivate = true;
                textPopup.SetActive(true);
            }
        }    
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasActivated)
        {
            if (collision.transform.CompareTag("Player"))
            {
                canActivate = false;
                textPopup.SetActive(false);
            }
        }      
    }
}
