using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTurning : MonoBehaviour
{
    public float turnSpeed;
    private GameObject player;
    private SpriteRenderer rend;
    public GameObject sherman;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= sherman.GetComponent<MrSherman>().shermanMoveRange)
        {
            Vector3 vectorToTarget = player.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turnSpeed);

            FlipToPlayer();
        }       
    }

    public void FlipToPlayer()
    {
        this.rend.flipY = player.transform.position.x < this.transform.position.x;
    }
}
