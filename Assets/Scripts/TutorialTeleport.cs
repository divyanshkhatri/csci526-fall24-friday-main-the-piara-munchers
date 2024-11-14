using System.Collections.Generic;
using UnityEngine;

public class TutorialTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public List<GameObject> showuiElements;
    public List<GameObject> hideuiElements;
    public int currentLevel = 0;

    private void Start()
    {
        Debug.Log("Initial Level: " + currentLevel);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportTarget.position;
            ShowUIElements();
            HideUIElements();
            IncrementLevel();
        }
    }

    private void IncrementLevel()
    {
        currentLevel++;
        Debug.Log("Teleported to Level: " + currentLevel);
    }

    private void ShowUIElements()
    {
        foreach (GameObject uiElement in showuiElements)
        {
            uiElement.SetActive(true);
        }
    }


    public void HideUIElements()
    {
        foreach (GameObject uiElement in hideuiElements)
        {
            uiElement.SetActive(false);
        }
    }
}
