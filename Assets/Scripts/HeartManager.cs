using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public GameObject heartsHUD; // Reference to the Hearts UI element

    private void OnEnable()
    {
        if (heartsHUD != null)
        {
            heartsHUD.SetActive(true); // Show the hearts HUD when SubScene4 becomes active
        }
    }

    private void OnDisable()
    {
        if (heartsHUD != null)
        {
            heartsHUD.SetActive(false); // Hide the hearts HUD when SubScene4 becomes inactive
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (heartsHUD != null)
            {
                heartsHUD.SetActive(true); // Show hearts HUD when player enters SubScene4
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (heartsHUD != null)
            {
                heartsHUD.SetActive(false); // Hide hearts HUD when player exits SubScene4
            }
        }
    }
}
