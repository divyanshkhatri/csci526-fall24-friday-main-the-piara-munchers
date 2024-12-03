using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.4f;
    public float heightOffsetPercentage = 0.5f;
    public float shakeDuration = 0.4f;
    public float shakeMagnitude = 0.1f;
    public float levelMargin = 2f;
    private Vector3 shakeOffset = Vector3.zero;
    private Vector3 currentVelocity;
    private float smoothDampTime = 0.2f;
    private Material redFlashMaterial;
    private float flashDuration = 0.4f;
    private float initialOrthographicSize;
    private float targetOrthographicSize;
    private float zoomDuration = 1.5f;
    public bool IsZooming { get { return isZooming; } }
    private bool isZooming = true;
    private Vector3 zoomStartPosition;
    private Vector3 zoomEndPosition;
    private float initialDelay = 1f;
    public Canvas hudCanvas;
    public GameObject letters;
    private PlayerMovement playerMovement;
    private FlipManager flipManager;

    void Start()
    {
        redFlashMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        redFlashMaterial.color = new Color(1f, 0f, 0f, 0f);

        Camera cam = GetComponent<Camera>();
        targetOrthographicSize = cam.orthographicSize;
        SetInitialCameraView();
        if (hudCanvas != null)
        {
            hudCanvas.enabled = false;
        }
        if (letters != null)
        {
            letters.SetActive(false);
        }
        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetZoomState(true);
        }
        flipManager = FindObjectOfType<FlipManager>();
        if (flipManager != null)
        {
            flipManager.SetZoomState(true);
        }
        StartCoroutine(ZoomToTarget());
    }

    private void SetInitialCameraView()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        float leftmost = float.MaxValue;
        float rightmost = float.MinValue;
        float topmost = float.MinValue;
        float bottommost = float.MaxValue;

        foreach (GameObject obj in allObjects)
        {
            if (obj.activeInHierarchy && obj.GetComponent<Renderer>())
            {
                Vector3 position = obj.transform.position;
                leftmost = Mathf.Min(leftmost, position.x);
                rightmost = Mathf.Max(rightmost, position.x);
                topmost = Mathf.Max(topmost, position.y);
                bottommost = Mathf.Min(bottommost, position.y);
            }
        }

        leftmost -= levelMargin;
        rightmost += levelMargin;

        float width = rightmost - leftmost;
        float height = topmost - bottommost;
        Camera cam = GetComponent<Camera>();

        initialOrthographicSize = Mathf.Max(width / cam.aspect, height) / 1.8f;
        cam.orthographicSize = initialOrthographicSize;

        zoomStartPosition = new Vector3((leftmost + rightmost) / 2f, (topmost + bottommost) / 2f, transform.position.z);
        transform.position = zoomStartPosition;

        float cameraHeight = targetOrthographicSize * 2f;
        float heightOffset = (cameraHeight * heightOffsetPercentage) - 1;
        zoomEndPosition = new Vector3(
            player.position.x + targetOrthographicSize / 2,
            player.position.y + heightOffset,
            transform.position.z
        );
    }

    private IEnumerator ZoomToTarget()
    {
        Camera cam = GetComponent<Camera>();

        yield return new WaitForSeconds(initialDelay);

        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomDuration;

            cam.orthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, t);
            transform.position = Vector3.Lerp(zoomStartPosition, zoomEndPosition, t);

            yield return null;
        }

        isZooming = false;
        if (playerMovement != null)
        {
            playerMovement.SetZoomState(false);
        }
        if (flipManager != null)
        {
            flipManager.SetZoomState(false);
        }
        if (hudCanvas != null)
        {
            hudCanvas.enabled = true;
        }
        if (letters != null)
        {
            letters.SetActive(true);
        }
    }

    void LateUpdate()
    {
        if (isZooming) return;
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
