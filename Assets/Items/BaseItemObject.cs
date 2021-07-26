using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.U2D;

public class BaseItemObject : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;

    public BaseRarity rarity;

    public GameObject trail;
    public GameObject itemLightObject;
    public string itemName;
    public string description;

    private PrefabManager pfManager;
    [HideInInspector]
    public GeneralManager genManager;
    private UIManager uiManager;
    private GameObject invBucket;
    private bool isInInv;
    private Light2D itemLight;

    public virtual void Start()
    {
        invBucket = GameObject.FindGameObjectWithTag("InvBucket");
        player = GameObject.FindGameObjectWithTag("Player");
        genManager = GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>();
        pfManager = GameObject.FindGameObjectWithTag("PrefabManager").GetComponent<PrefabManager>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        itemLight = GetComponentInChildren<Light2D>();

        itemLight.color = rarity.rar.rarityColor;
    }

    public virtual void ActivateBonus()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !player.GetComponent<PlayerItemsSkills>().equippedItems.Contains(gameObject))
        {            
            var highlighter = Instantiate(pfManager.itemTextPopup, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            var nameTxt = highlighter.GetComponentsInChildren<TextMesh>();

            player.GetComponent<PlayerItemsSkills>().AddItem(gameObject);

            genManager.SaveValueInPlayerPrefs("ItemsPickedUp", genManager.GetValueInPlayerPrefs("ItemsPickedUp") + 1);

            MoveToBucket();            

            nameTxt[0].text = itemName + " (" + player.GetComponent<PlayerItemsSkills>().equippedItems.Count(x => x.GetComponent<BaseItemObject>().GetType().Name == this.GetType().Name) + ")";
            nameTxt[0].color = rarity.rar.rarityColor;
            nameTxt[1].text = description;
            nameTxt[2].text = "(" + rarity.rar.rarityName + ")";
            nameTxt[2].color = rarity.rar.rarityColor;
            highlighter.transform.parent = null;                                 
        }        
    }

    private void MoveToBucket()
    {
        isInInv = true;
        trail.SetActive(false);
        
        gameObject.transform.position = invBucket.transform.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((Random.Range(-5, 5)), 0);
        gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        gameObject.transform.parent = invBucket.gameObject.transform;
        gameObject.transform.localScale = rarity.rar.scaleInInv;       

        Invoke("GoSleep", 5);
    }

    private void GoSleep()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.isStatic = true;
        gameObject.GetComponentInChildren<Light2D>().enabled = false;
    }
}
