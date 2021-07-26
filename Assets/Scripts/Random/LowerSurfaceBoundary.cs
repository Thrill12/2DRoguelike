using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerSurfaceBoundary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.Translate(new Vector2(0, 15));
    }
}
