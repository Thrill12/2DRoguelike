using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPopupToggle : MonoBehaviour
{
    public void Toggle()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            gameObject.SetActive(true);
            Time.timeScale = 0;
            
        }
    }

    public void Update()
    {
        GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>().Load();
    }
}
