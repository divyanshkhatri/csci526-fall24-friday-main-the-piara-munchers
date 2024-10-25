using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 2f;
    private Vector2 startPos;
    private bool movingUp = true;

    void Start()
    {

        startPos = transform.position;
    }

    void Update()
    {

        if (movingUp)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);


            if (transform.position.y >= startPos.y + moveDistance)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);


            if (transform.position.y <= startPos.y - moveDistance)
            {
                movingUp = true;
            }
        }
    }
}