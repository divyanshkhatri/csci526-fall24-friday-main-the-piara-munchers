using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTutorialKeyPress : MonoBehaviour
{
    public GameObject objectToDisable;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            objectToDisable.SetActive(false);
        }
    }
}
