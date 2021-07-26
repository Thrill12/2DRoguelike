using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [Header("Core Entity Stats")]

    public string name;

    [Space(5)]

    public float moveSpeed;
    public float maxSpeed;
    public float jumpForce;
    public float maxJumpSpeed;
    public float jumpsAvailable;
    public GameObject puff;

    [Space(2.5f)]

    [HideInInspector]
    public float baseMoveSpeed;
    [HideInInspector]
    public float baseJumpForce;

    [Space(5)]

    public float health;
    
    public float maxHealth;
    public float armour;

    public float healthRegen;
    public float healthRegenInterval;

    [Space(5)]

    public float damageMultiplier;
    public float cooldownMultiplier = 1;
    public float leechAmount;
    public float percentageToBleed;
    public float percentageToIgnite;
    public float percentageToMeteor;
    public float percentageToCrit;
    public float knockbackMultiplier = 1;

    [Space(10)]

    public DropTable dropTable;
    public Animator anim;
    public AudioClip hitSound;
    public AudioSource jumpSource;

    public float entityLevel = 0;
    public float xpCoefficientToGive = 1;

    [HideInInspector]
    public float baseMaxJumpSpeed;
    [HideInInspector]
    public float baseMaxHealth;
    [HideInInspector]
    public float baseArmour;
    [HideInInspector]
    public float baseHealthRegen;
    [HideInInspector]
    public float baseHealthRegenInterval;
    [HideInInspector]
    public float baseDamageMultiplier;

    private float nextRegenTickTime = 0;
    [HideInInspector]
    public GeneralManager genManager;
    [HideInInspector]
    public DifficultyManager diffManager;
    [HideInInspector]
    public PrefabManager pfManager;
    [HideInInspector]
    public UIManager uiManager;

    private float levelDifferenceTemp;

    public float jumpsMade = 0;

    public SoundClipLibrary sounds;

    [Space(5)]

    public float groundCheckRadius;

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public Rigidbody2D rb;

    [Space(5)]

    [Header("Debuffs")]

    public List<BaseDebuff> debuffs;

    [HideInInspector]
    public Vector2 startScale;

    public bool isDead;

    private Hits hits;
    private bool hasDroppedRewards;

    public virtual void Start()
    {
        hits = new Hits();
        startScale = transform.localScale;

        GetObjects();

        SetBaseValues();

        CheckLevelDifference();

        health = maxHealth;
    }

    public void GetObjects()
    {
        genManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
        diffManager = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>();
        pfManager = GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        sounds = GameObject.FindGameObjectWithTag("SoundClipLibrary").GetComponent<SoundClipLibrary>();
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void OnLevelWasLoaded(int level)
    {
        GetObjects();
    }

    public void SetBaseValues()
    {
        baseMaxJumpSpeed = maxJumpSpeed;
        baseMaxHealth = maxHealth;
        baseArmour = armour;
        baseDamageMultiplier = damageMultiplier;
        baseHealthRegen = healthRegen;
        baseHealthRegenInterval = healthRegenInterval;
    }

    public virtual void Update()
    {
        if(Time.timeScale != 0)
        {
            RegenHealth();
            ManageDebuffIconPositions();           

            if (health <= 0)
            {
                if (!isDead)
                {
                    OnDeath();
                }                
            }
        }       
    }

    public void ManageDebuffIconPositions()
    {
        if(debuffs.Count > 0)
        {            
            for(int i = 0; i < debuffs.Count; i++)
            {
                debuffs[i].gameObject.transform.position = new Vector2(transform.position.x + 
                    (debuffs[i].GetComponent<SpriteRenderer>().bounds.size.x * i), transform.position.y + 1.5f);               
            }
        }       
    }

    public virtual void CheckLevelDifference()
    {
        if(Mathf.FloorToInt(levelDifferenceTemp) != Mathf.FloorToInt(entityLevel) && Mathf.FloorToInt(entityLevel) > 1)
        {
            int intLvl = Mathf.FloorToInt(entityLevel);

            levelDifferenceTemp = entityLevel;

            maxHealth += (maxHealth * 0.07f);
            damageMultiplier += (damageMultiplier * 0.07f);
        }

        levelDifferenceTemp = entityLevel;
    }

    public virtual void TakeDamage(float pureDamageToTake, BaseEntity objectDamaging)
    {
        if(health > 0)
        {
            if (armour >= 0)
            {
                float damage = pureDamageToTake * (100 / (100 + armour));
                //Debug.Log("Damage is " + damage);
                if (hits.Crit(objectDamaging))
                {
                    health -= damage * 2;
                    OnTakeDamage(damage * 2, true);
                }
                else
                {
                    health -= damage;
                    OnTakeDamage(damage, false);
                }                               
            }
            else
            {
                float damage = pureDamageToTake * (2 - 100 / (100 - armour));
                //Debug.Log("Damage is " + damage);
                if (hits.Crit(objectDamaging))
                {
                    health -= damage * 2;
                    OnTakeDamage(damage * 2, true);
                }
                else
                {
                    health -= damage;
                    OnTakeDamage(damage, false);
                }                
            }
        }               
    }

    public virtual void TakeDamageNoArmour(float pureDamageToTake, BaseEntity objectDamaging)
    {
        if(health > 0)
        {
            if (hits.Crit(objectDamaging))
            {
                health -= pureDamageToTake * 2;
                OnTakeDamage(pureDamageToTake * 2, true);
            }
            else
            {
                health -= pureDamageToTake;
                OnTakeDamage(pureDamageToTake, false);
            }           
        }       
    }

    public virtual void OnTakeDamage(float damage, bool isCrit)
    {
        HitMarkerSound();

        Vector2 spawnPos = new Vector2(Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f), Random.Range(transform.position.y - 0.2f, transform.position.y + 0.2f));

        var popup = Instantiate(pfManager.damageTextPopup, spawnPos, Quaternion.identity);
        popup.transform.parent = null;
        popup.transform.localScale = new Vector3(1,1,1);
        if (isCrit)
        {
            popup.GetComponentInChildren<TextMesh>().color = Color.magenta;
            popup.transform.localScale = new Vector2(2, 2);
        }
        else
        {
            popup.GetComponentInChildren<TextMesh>().color = Color.red;
        }
        popup.GetComponentInChildren<TextMesh>().text = Mathf.CeilToInt(damage).ToString();
    }

    public virtual void OnRegen(float regenAmount)
    {
        Vector2 spawnPos = new Vector2(Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f), Random.Range(transform.position.y - 0.2f, transform.position.y + 0.2f));

        var popup = Instantiate(pfManager.regenHealthTextPopup, spawnPos, Quaternion.identity);
        popup.transform.parent = null;
        popup.transform.localScale = new Vector3(1, 1, 1);
        popup.GetComponentInChildren<TextMesh>().text = Mathf.CeilToInt(regenAmount).ToString();
    }

    public void HitMarkerSound()
    {
        if(hitSound == null)
        {
            GetComponent<AudioSource>().PlayOneShot(sounds.hitmarker);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(hitSound);
        }
    }

    public virtual void RegenHealth()
    {       
        if(healthRegenInterval != 0)
        {
            if (health < maxHealth && health > 0)
            {
                if (Time.time > nextRegenTickTime)
                {
                    nextRegenTickTime = Time.time + healthRegenInterval;
                    health += healthRegen;
                    OnRegen(healthRegen);
                }
            }
        }        

        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public virtual void RegenSpecific(float amountToRegen)
    {
        if (health < maxHealth && health > 0)
        {
            health += amountToRegen;
            OnRegen(amountToRegen);
        }
    }

    public bool IsGrounded()
    {
        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, groundCheckRadius);

        foreach (Collider2D col in objs)
        {
            if(col.gameObject.tag == "Platform" || col.gameObject.tag == "GroundPlatform")
            {
                return true;
            }
        }

        return false;
    }

    public virtual void OnHit(GameObject entityAffected)
    {
        hits.DoOnHits(entityAffected, gameObject);

        if(leechAmount != 0)
        {
            hits.Leech(entityAffected, gameObject, leechAmount);
        }
    }

    public virtual void OnDeath()
    {
        Die();      
    }

    public void ScorePoints()
    {
        if (!hasDroppedRewards)
        {
            genManager.gamePoints += maxHealth * genManager.levelsCompleted;
            player.GetComponent<Player>().enemiesKilled += 1;
            float totalXpToGive = maxHealth * xpCoefficientToGive;
            player.GetComponent<Player>().XP += totalXpToGive;

            if (gameObject.transform.tag == "Boss")
            {
                Debug.Log(genManager.GetComponent<GeneralManager>().soulMarks + " are the bosses soulamkrs");
                genManager.GetComponent<GeneralManager>().soulMarks += 1;
                GameObject gain = Instantiate(pfManager.soulmarkGain, player.transform);
                gain.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 3f);
            }

            hasDroppedRewards = true;
        }       
    }

    public virtual void Die()
    {
        //Death Animation for object
        Destroy(gameObject);
    }

    public virtual void Die(float timer)
    {
        //Death Animation for object
        Destroy(gameObject, timer);
    }

    public virtual void FlipToPlayer()
    {
        if(transform.position.x - player.transform.position.x >= 0)
        {
            transform.localScale = new Vector2(-startScale.x, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(startScale.x, transform.localScale.y);
        }        
    }

    public void AttackAnim()
    {
        if(anim != null)
        {
            anim.SetTrigger("Attack");
        }       
    }

    public virtual void Jump()
    {
        if (jumpsMade < jumpsAvailable)
        {
            Vector2 spawnPos = new Vector2((GetComponent<Collider2D>().bounds.min.x + GetComponent<Collider2D>().bounds.max.x) / 2,
                GetComponent<Collider2D>().bounds.min.y);

            Instantiate(puff, spawnPos, Quaternion.identity);
            if (jumpsMade < 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (jumpForce));
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, (jumpForce));
            }

            //rb.AddForce(Vector3.up * (jumpForce * valueMultiplier), ForceMode2D.Impulse);

            jumpSource.Play();
            jumpsMade += 1;
        }
    }

    public virtual void Jump(float forceToJump)
    {
        if (jumpsMade < jumpsAvailable)
        {
            Vector2 spawnPos = new Vector2((GetComponent<Collider2D>().bounds.min.x + GetComponent<Collider2D>().bounds.max.x) / 2,
                GetComponent<Collider2D>().bounds.min.y);

            Instantiate(puff, spawnPos, Quaternion.identity);
            rb.velocity = new Vector2(rb.velocity.x, (forceToJump));

            //rb.AddForce(Vector3.up * (jumpForce * valueMultiplier), ForceMode2D.Impulse);

            jumpSource.Play();
            jumpsMade += 1;
        }
    }

    public void EnemyJumping()
    {
        Jump();     
    }

    public void ResetJumpCounter()
    {
        jumpsMade = 0;
    }
}
