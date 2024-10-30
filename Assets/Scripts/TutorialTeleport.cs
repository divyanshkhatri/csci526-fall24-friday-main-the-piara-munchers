using UnityEngine;

public class TutorialTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject uiElement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportTarget.position;
            CheckAndShowUI();
        }
    }
    private void CheckAndShowUI()
    {
        uiElement.SetActive(true);
    }
}