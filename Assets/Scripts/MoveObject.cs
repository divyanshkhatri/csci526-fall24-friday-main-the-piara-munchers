using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Transform player; // The player serves as the reference point for flipping
    public float amplitude = 0.5f; // Vertical movement amplitude
    public float frequency = 1f; // Vertical movement frequency
    private Vector3 initialPosition; // The object's starting position
    private static bool isFlipped = false; // Global flip state to track the world state

    private void Start()
    {
        // Store the initial position of the object
        initialPosition = transform.position;

        // Subscribe to the global flip event
        FlipManager.OnFlip += HandleFlip;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the global flip event
        FlipManager.OnFlip -= HandleFlip;
    }

    private void Update()
    {
        // Apply vertical movement (up-down oscillation)
        Vector3 tempPosition = transform.position;
        tempPosition.y = initialPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = tempPosition;
    }

    private void HandleFlip()
    {
        // Toggle the global flip state
        isFlipped = !isFlipped;

        // Calculate the object's x-distance from the player
        float distanceFromPlayer = transform.position.x - player.position.x;

        // Update the object's x-position to flip its distance to the other side of the player
        float flippedX = player.position.x - distanceFromPlayer;

        // Update the object's position
        transform.position = new Vector3(
            flippedX,
            transform.position.y, // Maintain the current vertical position
            transform.position.z
        );

        // Flip the object's scale horizontally
        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1;
        transform.localScale = tempScale;
    }
}
