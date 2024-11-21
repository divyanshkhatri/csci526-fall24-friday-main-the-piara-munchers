using UnityEngine;

public class PlayerPlatformAttachment : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isOnPlatform;
    private Transform platformTransform;
    private Vector3 lastPlatformPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isOnPlatform = true;
            platformTransform = collision.transform;
            lastPlatformPosition = platformTransform.position;
        }
    }

    private void FixedUpdate()
    {
        if (isOnPlatform)
        {
            Vector3 platformDelta = platformTransform.position - lastPlatformPosition;
            rb.velocity += new Vector2(platformDelta.x, platformDelta.y) / Time.fixedDeltaTime;
            lastPlatformPosition = platformTransform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isOnPlatform = false;
            platformTransform = null;
        }
    }
}
