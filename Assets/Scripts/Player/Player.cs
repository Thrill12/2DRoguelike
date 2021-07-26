using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using GameAnalyticsSDK;

public class Player : BaseEntity
{
    #region Inspector
    [Header("Movement Stats")]

    public float dragToStop;

    [Space(10)]
    public float valueMultiplier;
    public float XP = 0;
    public float XPBacklog;
    private float baseXPToNextLevel = 1000;
    public float XPToNextLevel = 1000;
    public float xpCoefficientForLevelUp;

    [Space(10)]

    [Header("Objects")]

    public GameObject groundCheck;
    public GameObject portalArrow;

    [Space(20)]

    public GameObject frontShootSource;
    public float frontShootSourceRadius;
    public ParticleSystem playerParticles;
    public AudioSource particleSoundSource;    

    #endregion

    #region Private Variables
    private float inputX;
    private float inputY;

    private float privHealth;

    private InputManager inputManager;
    private PlayerItemsSkills items;
    private MenuManager menuManager;
    private CinemachineImpulseSource camShakeSource;

    [HideInInspector]
    public GameObject pixlCamera;

    private GameObject vcam;
    [HideInInspector]
    public GameObject portal;
    private Collider2D[] col;
    private bool isTouchingEffector;
    
    public int enemiesKilled;

    private float tempJump;
    private bool isJumping;
    private bool isLevelingUp;

    #endregion

    #region Unity Functions

    public override void Start()
    {
        baseXPToNextLevel = XPToNextLevel;
        GameAnalytics.Initialize();
        camShakeSource = GetComponentInChildren<CinemachineImpulseSource>();        

        privHealth = 0;
        rb = GetComponent<Rigidbody2D>();        

        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        //items = GetComponent<PlayerItemsSkills>();

        col = gameObject.GetComponents<Collider2D>();

        GetComponentInChildren<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        GetObjects();
        base.Start();

        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();        
    }

    public override void OnLevelWasLoaded(int level)
    {
        GetObjects();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        col = gameObject.GetComponents<Collider2D>();
        pixlCamera = GameObject.FindGameObjectWithTag("MainCamera");

        GetComponentInChildren<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        privHealth = 0;
        menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;

        if (portalArrow.activeInHierarchy)
        {
            TogglePortalArrow();
        }        
    }

    public override void Update()
    {
        if(Time.timeScale != 0)
        {
            TakeInputs();
            TakeJumpInput();
            RotateShootSource();
            SetPlayerParticles();
            CheckForLevelUp();
            base.Update();
        }     
        
        if(cooldownMultiplier < 0.2f)
        {
            cooldownMultiplier = 0.2f;
        }
    }

    public void CheckForLevelUp()
    {
        if (!isLevelingUp)
        {
            isLevelingUp = true;
            if (XP == XPToNextLevel)
            {
                XPToNextLevel = baseXPToNextLevel * Mathf.Pow(xpCoefficientForLevelUp, entityLevel);
                XP = 0;
                entityLevel += 1;
                CheckLevelDifference();
                GetComponent<AudioSource>().PlayOneShot(sounds.levelUp);
                Debug.Log("Level Up");
            }
            else if(XP > XPToNextLevel)
            {
                float diff = XP - XPToNextLevel;
                XPToNextLevel = baseXPToNextLevel * Mathf.Pow(xpCoefficientForLevelUp, entityLevel);
                XP = 0;
                XP += diff;
                entityLevel += 1;
                CheckLevelDifference();
                GetComponent<AudioSource>().PlayOneShot(sounds.levelUp);
                Debug.Log("Level up with difference of " + diff);
            }
            isLevelingUp = false;
        }       
    }

    public void SetPlayerParticles()
    {
        var playerParticlesMain = playerParticles.main;

        playerParticlesMain.maxParticles = Mathf.FloorToInt(health);        
    }

    private void FixedUpdate()
    {
        MoveBasicInputs();
        if(portal != null)
        {
            MovePortalArrow();
        }        
    }

    #endregion

    #region Moving

    private void TakeInputs()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if(inputX != 0)
        {
            particleSoundSource.volume = 0.15f;
        }
        else
        {
            particleSoundSource.volume = 0;
        }
    }

    private void MoveBasicInputs()
    {
        //rb.AddForce(new Vector2(inputX * moveSpeed * valueMultiplier, 0));
        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
    }

    #endregion

    #region Jumping

    private void TakeJumpInput()
    {
        if (Input.GetKeyDown(inputManager.jump))
        {
            Jump();
        }
    }

    #endregion

    public void RotateShootSource()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dirToMouse = worldMousePos - gameObject.transform.position;
        dirToMouse = dirToMouse.normalized;

        frontShootSource.transform.position = gameObject.transform.position + (dirToMouse * frontShootSourceRadius);

        Vector3 vectorToMouse = worldMousePos - transform.position;
        float angle = Mathf.Atan2(vectorToMouse.y, vectorToMouse.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        frontShootSource.transform.rotation = q;
    }

    public override void TakeDamage(float pureDamageToTake, BaseEntity objectDamaging)
    {
        base.TakeDamage(pureDamageToTake, objectDamaging);
        uiManager.UpdateHealthBar();
    }

    public override void OnTakeDamage(float damage, bool isCrit)
    {
        base.OnTakeDamage(damage, isCrit);
        ShakeCamera();
        StartCoroutine(FlashRedVignette());
        StartCoroutine(StopParticles());
    }

    public override void OnRegen(float regenAmount)
    {
        base.OnRegen(regenAmount);
    }

    public override void RegenHealth()
    {
        base.RegenHealth();
        uiManager.UpdateHealthBar();
    }

    public IEnumerator StopParticles()
    {        
        var particles = playerParticles.main;
        var origSpeed = particles.simulationSpeed;
        particles.simulationSpeed = 0;

        yield return new WaitForSeconds(0.05f);

        particles.simulationSpeed = 1;
    }

    public IEnumerator FlashRedVignette()
    {
        Vignette vg = new Vignette();
        GetComponentInChildren<Volume>().profile.TryGet<Vignette>(out vg);
        vg.intensity.value = 0.6f;

        yield return new WaitForSeconds(0.05f);

        vg.intensity.value = 0;
    }

    public void ShakeCamera()
    {
        camShakeSource.GenerateImpulse();
    }

    public override void OnDeath()
    {
        genManager.SaveValueInPlayerPrefs("TotalDeaths", genManager.GetValueInPlayerPrefs("TotalDeaths") + 1);
        Camera.main.transform.parent = null;
        //vcam.transform.parent = null;

        genManager.SaveValueInPlayerPrefs("SoulMarks", genManager.gamePoints);
        genManager.soulMarks = genManager.GetValueInPlayerPrefs("SoulMarks");

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Loop " + diffManager.stagesCompleted, "Score " + genManager.gamePoints, "Death");
        menuManager.ShowDeathMenu();

        base.OnDeath();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Platform")
        {
            col[0].isTrigger = false;
        }       
    }    

    public void TogglePortalArrow()
    {
        if (!portalArrow.activeInHierarchy)
        {
            portalArrow.SetActive(true);
            portalArrow.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            portalArrow.SetActive(false);
        }
    }

    public void MovePortalArrow()
    {
        Vector3 dir = portal.transform.position - gameObject.transform.position;
        dir = dir.normalized;

        portalArrow.transform.position = gameObject.transform.position + (dir * frontShootSourceRadius * 1.2f);

        Vector3 vector = portal.transform.position - transform.position;
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        portalArrow.transform.rotation = q;
    }
}
