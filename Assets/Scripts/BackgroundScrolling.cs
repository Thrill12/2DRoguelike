using UnityEngine;
using System.Collections;

public class BackgroundScrolling : MonoBehaviour
{
    private float lengthX;
    private float lengthY;
    private float startPosX;
    private float startPosY;

    public GameObject cam;
    public float parallax;
    public bool shouldRepete;
    public bool shouldRepeteY;
    public bool shouldFollowOnY;

    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void Update()
    {
        float tempX = cam.transform.position.x * (1 - parallax);
        float tempY = cam.transform.position.y * (1 - parallax);
        var distanceX = cam.transform.position.x * parallax;
        var distanceY = cam.transform.position.y * parallax;

        if (shouldFollowOnY)
        {
            transform.position = new Vector2(startPosX + distanceX, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPosX + distanceX, startPosY + distanceY);
        }

        if (shouldRepete)
        {
            if (tempX > startPosX + lengthX)
            {
                startPosX += lengthX;
            }
            else if (tempX < startPosX - lengthX)
            {
                startPosX -= lengthX;
            }
        }

        if (shouldRepeteY)
        {
            if(tempY > startPosY + lengthY)
            {
                startPosY += lengthY;
            }
            else if(tempY < startPosY - lengthY)
            {
                startPosY -= lengthY;
            }
        }
    }

}