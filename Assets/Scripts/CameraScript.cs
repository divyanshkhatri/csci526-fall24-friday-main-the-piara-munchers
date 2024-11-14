using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.4f;
    public float heightOffsetPercentage = 0.5f;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 shakeOffset = Vector3.zero;
    private Vector3 currentVelocity;
    private float smoothDampTime = 0.2f;

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

        float heightOffset = (cameraHeight * heightOffsetPercentage) - 1;

        Vector3 desiredPosition = new Vector3(player.position.x + cam.orthographicSize / 2, player.position.y + heightOffset, transform.position.z);
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothDampTime);

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
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0;

            elapsed += Time.deltaTime;

            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}
