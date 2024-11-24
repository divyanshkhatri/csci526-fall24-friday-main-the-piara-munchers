using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.4f;
    public float heightOffsetPercentage = 0.5f;
    public float shakeDuration = 0.4f;
    public float shakeMagnitude = 0.1f;

    private Vector3 shakeOffset = Vector3.zero;
    private Vector3 currentVelocity;
    private float smoothDampTime = 0.2f;
    private Material redFlashMaterial;
    private float flashDuration = 0.4f;

    void Start()
    {
        redFlashMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        redFlashMaterial.color = new Color(1f, 0f, 0f, 0f);
    }

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

    public void TriggerShakeAndFlash()
    {
        StartCoroutine(Shake());
        StartCoroutine(RedFlash());
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

    private IEnumerator RedFlash()
    {
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            float alpha = Mathf.Lerp(0.3f, 0f, elapsed / flashDuration);
            redFlashMaterial.color = new Color(1f, 0f, 0f, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        redFlashMaterial.color = new Color(1f, 0f, 0f, 0f);
    }

    void OnGUI()
    {
        if (redFlashMaterial.color.a > 0)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, redFlashMaterial.color, 0, 0);
        }
    }
}
