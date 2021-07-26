using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemyObject : BaseEntity
{
    [Header("Spawning")]

    public float weight;

    [Space(5)]

    public int intPercentageToDropItem = 30;

    [Space(5)]

    public float healthBarHeight;
    public float healthBarScale;

    private Image healthBar;
    [HideInInspector]
    public GameObject healthBarObject;

    private bool isJumping = false;

    private bool strobing;
    public Shader originalShader;
    public Shader flashShader;
    private bool isDying;

    public override void Start()
    {
        originalShader = GetComponent<SpriteRenderer>().material.shader;
        flashShader = Shader.Find("GUI/Text Shader");

        diffManager = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>();

        base.Start();

        healthBarObject = Instantiate(pfManager.normalEnemyHealthBar, transform.position, Quaternion.identity);
        healthBarObject.transform.localScale = new Vector2(healthBarScale, healthBarScale);
        healthBar = healthBarObject.transform.Find("Canvas").transform.Find("HealthBar").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player");        

        UpdateHealthBarPosition();

        UpdateHealthBar();
    }

    public override void CheckLevelDifference()
    {
        float numberOfSillySyringes = player.GetComponent<PlayerItemsSkills>().equippedItems.Where(x => x.GetComponent<BaseItemObject>().itemName == "Silly Syringe").Count();

        if(numberOfSillySyringes == 0)
        {
            numberOfSillySyringes = 1;
        }

        if(transform.tag != "Boss")
        {
            maxHealth = baseMaxHealth * Mathf.Pow(1.45f, diffManager.stagesCompleted - 1);
            damageMultiplier = baseDamageMultiplier * Mathf.Pow(1.45f, diffManager.stagesCompleted - 1) * (2 * numberOfSillySyringes);
        }
        else if(transform.tag == "Boss")
        {
            maxHealth = baseMaxHealth * Mathf.Pow(1.7f, genManager.levelsCompleted - 1);
            damageMultiplier = baseDamageMultiplier * Mathf.Pow(1.7f, diffManager.stagesCompleted - 1) * (2 * numberOfSillySyringes);
        }
    }

    public override void Update()
    {
        if(healthBarObject != null)
        {
            UpdateHealthBarPosition();
        }

        if (health <= 0)
        {
            health = 0;
        }        

        base.Update();
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;     
    }

    public void UpdateHealthBarPosition()
    {
        healthBarObject.transform.position = new Vector2(transform.position.x, transform.position.y + healthBarHeight);
        healthBarObject.transform.rotation = Quaternion.identity;
    }

    public override void OnTakeDamage(float damage, bool isCrit)
    {
        base.OnTakeDamage(damage, isCrit);
        UpdateHealthBar();
        FlashWhite();
    }

    public override void TakeDamage(float pureDamageToTake, BaseEntity objectDamaging)
    {
        float dmg = pureDamageToTake;
        if (player.GetComponent<PlayerItemsSkills>().equippedItems.Where(x => x.GetComponent<BaseItemObject>().itemName == "Voodoo Compass").Count() > 0)
        {
            dmg = pureDamageToTake + ((5 / (Vector2.Distance(gameObject.transform.position, objectDamaging.transform.position))) *
            player.GetComponent<PlayerItemsSkills>().equippedItems.Where(x => x.GetComponent<BaseItemObject>().itemName == "Voodoo Compass").Count());
            Debug.Log(dmg);
        }

        base.TakeDamage(dmg, objectDamaging);
        UpdateHealthBar();
    }

    public override void TakeDamageNoArmour(float pureDamageToTake, BaseEntity objectDamaging)
    {
        float dmg = pureDamageToTake;
        if (player.GetComponent<PlayerItemsSkills>().equippedItems.Where(x => x.GetComponent<BaseItemObject>().itemName == "Voodoo Compass").Count() > 0)
        {
            dmg = pureDamageToTake + ((5 / (Vector2.Distance(gameObject.transform.position, objectDamaging.transform.position))) *
            player.GetComponent<PlayerItemsSkills>().equippedItems.Where(x => x.GetComponent<BaseItemObject>().itemName == "Voodoo Compass").Count());
            Debug.Log(dmg);
        }

        base.TakeDamageNoArmour(dmg, objectDamaging); 
        UpdateHealthBar();
    }

    public virtual void FlashWhite()
    {
        StartCoroutine(FlashWhiteCor());
    }

    public virtual IEnumerator FlashWhiteCor()
    {
        GetComponent<SpriteRenderer>().material.shader = flashShader;

        yield return new WaitForSeconds(0.05f);

        GetComponent<SpriteRenderer>().material.shader = originalShader;
    }

    public override void OnDeath()
    {
        if (!isDying)
        {
            isDying = true;
            genManager.SaveValueInPlayerPrefs("TotalKills", genManager.GetValueInPlayerPrefs("TotalKills") + 1);
            if (healthBarObject != null)
            {
                Destroy(healthBarObject);
            }

            ScorePoints();

            try
            {
                DropItem();
            }
            catch
            {
                Debug.Log("Couldn't drop item");
            }

            Die();
        }             
    }

    public void DropItem()
    {
        int rnd = Random.Range(0, 100);

        if (rnd < intPercentageToDropItem)
        {
            GameObject obj = Instantiate(dropTable.GetRandomItem());
            obj.transform.parent = null;
            obj.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
        }
    }

    public override void Die()
    {
        diffManager.diffCoeff += 0.001f;        

        base.Die();
    }
}
