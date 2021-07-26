using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using DigitalRuby.Tween;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Image healthBar;
    public Image bossHealthBar;
    public Image progressBar;
    public Image itemTooltipBackground;
    public Image soulMarksImage;
    public Image xpBar;

    [Space(5)]

    public TMP_Text progressText;
    public TMP_Text healthText;
    public TMP_Text bossHealthText;
    public TMP_Text bossNameText;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text levelsCompleted;
    public TMP_Text itemTooltipName;
    public TMP_Text itemTooltipDescription;
    public TMP_Text soulMarksText;
    public TMP_Text xpText;

    [Space(5)]

    public Color progressBarNormalColor;

    [Space(10)]

    public GameObject itemTooltip;
    public GameObject bossBar;

    [Space(10)]

    public Image skill1;
    public Image skill1Cool;
    public TMP_Text skill1TextCool;
    public TMP_Text skill1Key;
    public Image skill2;
    public Image skill2Cool;
    public TMP_Text skill2TextCool;
    public TMP_Text skill2Key;
    public Image skill3;
    public Image skill3Cool;
    public TMP_Text skill3TextCool;
    public TMP_Text skill3Key;
    public Image skill4;
    public Image skill4Cool;
    public TMP_Text skill4TextCool;
    public TMP_Text skill4Key;

    public Sprite lockedSkill;

    private GameObject player;
    private Player playerS;
    private PlayerItemsSkills playerSkills;
    private GeneralManager genManager;
    private GameObject healthObject;
    private GameObject progressBarObject;
    private List<AbilityHolder> holders;

    // Start is called before the first frame update
    void Start()
    {
        healthObject = healthText.gameObject;
        progressBarObject = progressBar.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        playerS = player.GetComponent<Player>();
        playerSkills = player.GetComponent<PlayerItemsSkills>();
        genManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
        holders = player.GetComponents<AbilityHolder>().ToList();
    }

    private void OnLevelWasLoaded(int level)
    {
        Start();
    }

    public void UpdateSkillButtons()
    {
        if(player.GetComponents<AbilityHolder>()[0].ability != null)
        {
            skill1.sprite = player.GetComponents<AbilityHolder>()[0].ability.skillIcon;
            skill1Key.text = "[" + player.GetComponents<AbilityHolder>()[0].key + "]";
        }
        else
        {
            skill1.sprite = lockedSkill;
            skill1Key.text = "N/A";
        }
        if (player.GetComponents<AbilityHolder>()[1].ability != null)
        {
            skill2.sprite = player.GetComponents<AbilityHolder>()[1].ability.skillIcon;
            skill2Key.text = "[" + player.GetComponents<AbilityHolder>()[1].key + "]";
        }
        else
        {
            skill2.sprite = lockedSkill;
            skill2Key.text = "N/A";
        }
        if (player.GetComponents<AbilityHolder>()[2].ability != null)
        {
            skill3.sprite = player.GetComponents<AbilityHolder>()[2].ability.skillIcon;
            skill3Key.text = "[" + player.GetComponents<AbilityHolder>()[2].key + "]";
        }
        else
        {
            skill3.sprite = lockedSkill;
            skill3Key.text = "N/A";
        }
        if (player.GetComponents<AbilityHolder>()[3].ability != null)
        {
            skill4.sprite = player.GetComponents<AbilityHolder>()[3].ability.skillIcon;
            skill4Key.text = "[" + player.GetComponents<AbilityHolder>()[3].key + "]";
        }
        else
        {
            skill4.sprite = lockedSkill;
            skill4Key.text = "N/A";
        }

        UpdateSkillCooldowns(0, skill1Cool, skill1TextCool);
        UpdateSkillCooldowns(1, skill2Cool, skill2TextCool);
        UpdateSkillCooldowns(2, skill3Cool, skill3TextCool);
        UpdateSkillCooldowns(3, skill4Cool, skill4TextCool);
    }

    public void UpdateSkillCooldowns(int holder, Image cool, TMP_Text text)
    {
        if(holders[holder].ability != null)
        {
            var readyDecimal = holders[holder].cooldownTime / holders[holder].ability.cooldownTime;

            cool.fillAmount = readyDecimal;
            text.text = Math.Round(holders[holder].cooldownTime, 1).ToString();

            if (cool.fillAmount == 0)
            {
                cool.enabled = false;
                text.enabled = false;
            }
            else
            {
                cool.enabled = true;
                text.enabled = true;
            }
        }
        else
        {
            cool.enabled = false;
            text.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        UpdateScoreText();
        UpdateProgressBar();
        UpdateBossBar();
        UpdateSkillButtons();
        UpdateXPBar();
    }

    public void UpdateHealthBar()
    {
        healthObject.Tween("ChangeHealth", healthBar.fillAmount, playerS.health / playerS.maxHealth, 0.2f, TweenScaleFunctions.CubicEaseOut, v => { healthBar.fillAmount = v.CurrentValue;});
        healthText.text = Mathf.CeilToInt(playerS.health).ToString() + "/" + Mathf.CeilToInt(playerS.maxHealth).ToString();
    }

    public void UpdateXPBar()
    {
        xpText.text = "Level: " + playerS.entityLevel;
        xpBar.fillAmount = playerS.XP / playerS.XPToNextLevel;
    }

    public void UpdateBossBar()
    {
        if(GameObject.FindGameObjectsWithTag("Boss").Length > 0)
        {
            bossBar.SetActive(true);

            List<GameObject> bosses = GameObject.FindGameObjectsWithTag("Boss").Where(x => x.GetComponent<BaseEntity>()).ToList();

            float totalHealth = bosses.Sum(x => x.GetComponent<BaseEntity>().maxHealth);
            float currentHealth = bosses.Sum(x => x.GetComponent<BaseEntity>().health);

            bossHealthText.text = Mathf.CeilToInt(currentHealth).ToString() + "/" + Mathf.CeilToInt(totalHealth).ToString();
            bossHealthBar.fillAmount = currentHealth / totalHealth;
            bossNameText.text = string.Join(" and ", bosses.Select(x => x.GetComponent<BaseEntity>().name).Distinct());
        }
        else
        {
            bossBar.SetActive(false);
        }
    }

    public void UpdateProgressBar()
    {
        //progressBarObject.Tween("ChangeProgress", progressBar.fillAmount, (playerS.enemiesKilled / genManager.enemiesToKill) * 100, 0.2f, TweenScaleFunctions.CubicEaseOut, v => { progressBar.fillAmount = v.CurrentValue; });
        
        progressBar.fillAmount = ((float)playerS.enemiesKilled / (float)genManager.enemiesToKill);
        progressText.text = Mathf.FloorToInt(((float)playerS.enemiesKilled / (float)genManager.enemiesToKill * 100)).ToString() + "%";

        if(((float)playerS.enemiesKilled / (float)genManager.enemiesToKill) >= 1)
        {
            progressBar.color = Color.yellow;
        }
        else
        {
            progressBar.color = progressBarNormalColor;
        }

        levelsCompleted.text = genManager.levelsCompleted.ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text = Mathf.RoundToInt(genManager.gamePoints).ToString();
        soulMarksText.text = "Soulmarks: " + genManager.soulMarks.ToString();
    }

    public void UpdateTimer()
    {
        float minutes = Mathf.Floor(genManager.timer / 60);
        float seconds = Mathf.RoundToInt(genManager.timer % 60);

        string mins;
        string secs;

        if (minutes < 10)
        {
            mins = "0" + minutes.ToString();
        }
        else
        {
            mins = minutes.ToString();
        }
        if (seconds < 10)
        {
            secs = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        else
        {
            secs = seconds.ToString();
        }

        timerText.text = mins + ":" + secs;
    }
}
