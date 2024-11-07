using UnityEngine;

public class PlayerPlatformAttachment : MonoBehaviour
{
    private Transform platformTransform;
    private Vector3 lastPlatformPosition;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is on a moving platform
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformTransform = collision.transform;
            lastPlatformPosition = platformTransform.position;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Update the player’s position based on the platform's movement
        if (platformTransform != null)
        {
            Vector3 platformMovement = platformTransform.position - lastPlatformPosition;
            transform.position += platformMovement;
            lastPlatformPosition = platformTransform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reset platform tracking when leaving the platform
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformTransform = null;
        }
    }
}
