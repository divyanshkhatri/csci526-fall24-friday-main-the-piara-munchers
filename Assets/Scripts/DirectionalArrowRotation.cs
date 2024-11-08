using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrowRotation : MonoBehaviour
{
    public KeyCode rotateKey = KeyCode.LeftArrow;
    private bool isFlipped = false;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (Input.GetKeyDown(rotateKey))
        {
            RotateArrow();
        }
    }

    void RotateArrow()
    {
        if (isFlipped)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        isFlipped = !isFlipped;
    }
}

