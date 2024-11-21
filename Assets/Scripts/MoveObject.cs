using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Transform player;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    private Vector3 initialPosition;
    private static bool isFlipped = false;

    private void Start()
    {
        initialPosition = transform.position;

        FlipManager.OnFlip += HandleFlip;
    }

    private void OnDestroy()
    {
        FlipManager.OnFlip -= HandleFlip;
    }

    private void Update()
    {
        Vector3 tempPosition = transform.position;
        tempPosition.y = initialPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = tempPosition;
    }

    private void HandleFlip()
    {
        isFlipped = !isFlipped;

        float distanceFromPlayer = transform.position.x - player.position.x;

        float flippedX = player.position.x - distanceFromPlayer;

        transform.position = new Vector3(
            flippedX,
            transform.position.y,
            transform.position.z
        );

        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1;
        transform.localScale = tempScale;
    }
}
