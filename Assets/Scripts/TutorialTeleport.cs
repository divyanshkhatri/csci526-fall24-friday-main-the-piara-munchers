using System.Collections.Generic; 
using UnityEngine;

public class TutorialTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public List<GameObject> showuiElements;
    public List<GameObject> hideuiElements;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportTarget.position;
            ShowUIElements();
            HideUIElements();
        }
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
