using UnityEngine;

public class PlayerPlatformAttachment : MonoBehaviour
{
    private Transform movingPlatform;
    private Vector3 previousPlatformPosition;
    private bool isOnPlatform = false;
    private Vector2 platformVelocity;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            movingPlatform = collision.transform;
            previousPlatformPosition = movingPlatform.position;
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            movingPlatform = null;
            isOnPlatform = false;
            platformVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isOnPlatform && movingPlatform != null)
        {
            Vector3 platformMovement = movingPlatform.position - previousPlatformPosition;
            platformVelocity = platformMovement / Time.fixedDeltaTime;
            previousPlatformPosition = movingPlatform.position;
        }
        else
        {
            platformVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (isOnPlatform && movingPlatform != null)
        {
            rb.velocity += platformVelocity;
        }
    }
}
