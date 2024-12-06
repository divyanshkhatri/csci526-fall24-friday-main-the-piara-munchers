using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.4f;
    public float heightOffsetPercentage = 0.5f;
    public float shakeDuration = 0.4f;
    public float shakeMagnitude = 0.1f;
    public float levelMargin = 0.5f; // Reduced from 2f to 0.5f
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
    public GameObject[] walls = new GameObject[3]; // Replace zoomTargetObjects with walls

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
        if (walls == null || walls.Length != 3)
        {
            Debug.LogWarning("Exactly 3 walls must be assigned for proper camera zoom.");
            return;
        }

        // Calculate the center point of the three walls
        Vector3 centerPoint = Vector3.zero;
        Vector2 minPoint = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxPoint = new Vector2(float.MinValue, float.MinValue);

        // Find the top wall and calculate its bounds
        GameObject topWall = null;
        float highestY = float.MinValue;
        foreach (GameObject wall in walls)
        {
            if (wall != null && wall.transform.position.y > highestY)
            {
                highestY = wall.transform.position.y;
                topWall = wall;
            }
        }

        // Calculate bounds for all walls
        foreach (GameObject wall in walls)
        {
            if (wall != null)
            {
                centerPoint += wall.transform.position;
                SpriteRenderer renderer = wall.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    Bounds bounds = renderer.bounds;

                    // Update minPoint and maxPoint for horizontal bounds
                    minPoint.x = Mathf.Min(minPoint.x, bounds.min.x);
                    maxPoint.x = Mathf.Max(maxPoint.x, bounds.max.x);

                    // For vertical bounds, explicitly consider the top wall's bounds
                    if (wall == topWall)
                    {
                        maxPoint.y = Mathf.Max(maxPoint.y, bounds.max.y);
                    }
                    minPoint.y = Mathf.Min(minPoint.y, bounds.min.y);
                }
            }
        }
        centerPoint /= 3f;

        Camera cam = GetComponent<Camera>();

        // Calculate the minimum required orthographic size to fit the walls
        float width = maxPoint.x - minPoint.x;
        float height = maxPoint.y - minPoint.y;

        float verticalSize = height / 2f;
        float horizontalSize = (width / 2f) / cam.aspect;

        // Use the larger of the two sizes to ensure all walls are visible
        initialOrthographicSize = Mathf.Max(verticalSize, horizontalSize);
        cam.orthographicSize = initialOrthographicSize;

        // Position the camera at the center of the walls
        zoomStartPosition = new Vector3(centerPoint.x, minPoint.y + verticalSize, transform.position.z);
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
