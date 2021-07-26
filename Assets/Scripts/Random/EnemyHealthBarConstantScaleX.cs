using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarConstantScaleX : MonoBehaviour
{
    void Update()
    {
        transform.localScale = new Vector2(-gameObject.transform.parent.transform.localScale.x / -gameObject.transform.parent.transform.localScale.x, transform.localScale.y);
    }
}
