using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Movement")]
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode fallEffector;

    [Space(5)]
    [Header("Skills")]
    public KeyCode mainShoot;
    public KeyCode secondaryShoot;    

    [Space(5)]
    [Header("Interaction")]
    public KeyCode interact;

    [HideInInspector]
    public Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
