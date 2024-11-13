using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.4f;
    public float heightOffsetPercentage = 0.5f;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 shakeOffset = Vector3.zero;  // Offset applied during shake

    void LateUpdate()
    {
        Camera cam = GetComponent<Camera>();
        float cameraHeight;

        if (cam.orthographic)
        {
            cameraHeight = cam.orthographicSize * 2f;
        }
        else
        {
            cameraHeight = 2f * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * Mathf.Abs(transform.position.z - player.position.z);
        }

        float heightOffset = (cameraHeight * heightOffsetPercentage) - 2;

        // Calculate the target position based on player position
        Vector3 desiredPosition = new Vector3(player.position.x + cam.orthographicSize / 2, player.position.y + heightOffset, transform.position.z);
        Vector3 targetPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply shake offset
        transform.position = targetPosition + shakeOffset;
    }

    public void TriggerShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Generate a random offset for the shake
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0;  // Keep shake in 2D

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Reset shake offset after shaking ends
        shakeOffset = Vector3.zero;
    }
}