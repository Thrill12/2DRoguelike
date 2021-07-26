using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsSkills : MonoBehaviour
{
    #region Inspector

    public List<GameObject> equippedItems;    

    #endregion
    #region Private Variables
    private Player movement;
    private GameObject player;
    
    public Camera invDisplayCamera;
    #endregion

    [Space(10)]

    [Header("Skills")]

    public BaseAbility skill1;
    public BaseAbility skill2;
    public BaseAbility skill3;
    public BaseAbility skill4;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = player.GetComponentInChildren<Player>();
        invDisplayCamera = GameObject.FindGameObjectWithTag("InvDisplayCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        
    }

    public void AddItem(GameObject itemToAdd)
    {
        equippedItems.Add(itemToAdd);
        itemToAdd.GetComponent<BaseItemObject>().ActivateBonus();
    }

    public void ActivateItems()
    {
        foreach(GameObject item in equippedItems)
        {
            item.GetComponent<BaseItemObject>().ActivateBonus();
        }
    }
}
