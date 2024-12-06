using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 7f;
    public Rigidbody2D rb;
    private bool isGrounded = false;
    public int hitCount = 0;
    public SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;
    public GameObject levelFailPanel;
    public bool canMove = true;
    public Button restartButton;
    private bool hasTriggeredFail = false;
    public TimerScript timerScript;
    public List<Image> hearts;
    private CameraScript cameraScript;
    public int flipCount = 0;
    public FireBaseAnalytics firebaseAnalytics;
    private Transform originalParent;
    private bool isOnPlatform = false;
    private Transform currentPlatform;
    public ClockRotation clockRotation;
    private bool isZoomingCamera = true;
    public Animator anim;

    public KeyCode flipKey = KeyCode.LeftArrow;
    public FlipManager flipManager;
    private ContactPoint2D[] contactPoints = new ContactPoint2D[4];
    private int contactCount = 0;
    private bool isTouchingWall = false;
    private readonly float wallCheckThreshold = 80f;
    private Vector3 originalScale;  // Add this field at the top with other fields

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hasTriggeredFail = false;
        PauseManager.OnPause += HandlePause;
        InitializeHearts();

        FlipManager.OnFlip += HandleFlip;


        cameraScript = Camera.main.GetComponent<CameraScript>();
        originalParent = transform.parent;
        clockRotation = FindObjectOfType<ClockRotation>();
        SetCanMove(!isZoomingCamera);
        originalScale = transform.localScale;  // Add this line
    }

    void OnDestroy()
    {
        PauseManager.OnPause -= HandlePause;
        FlipManager.OnFlip -= HandleFlip;
    }

    void Update()
    {
        if (!canMove || isZoomingCamera)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Move();

        if (Input.GetKeyDown(flipKey))
        {
            flipManager?.ToggleFlip();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            transform.SetParent(originalParent);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            anim.SetBool("Jumping", true);
        }

        if (isGrounded)
        {
            anim.SetBool("Jumping", false);
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(flipKey))
        {
            anim.SetInteger("movement", 0);
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        if (moveInput > 0 && !isTouchingWall)
        {
            float horizontalSpeed = moveSpeed;
            rb.velocity = new Vector2(moveInput * horizontalSpeed, rb.velocity.y);
            anim.SetInteger("movement", (int)Input.GetAxisRaw("Horizontal"));
            
            // Set scale to face right when moving right
            if (!isOnPlatform)
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetInteger("movement", 0);
        }
    }

    void FixedUpdate()
    {
        contactCount = rb.GetContacts(contactPoints);
        isGrounded = false;
        isTouchingWall = false;

        for (int i = 0; i < contactCount; i++)
        {
            float angle = Vector2.Angle(contactPoints[i].normal, Vector2.up);

            if (angle <= 45f)
            {
                isGrounded = true;
            }
            else if (angle >= wallCheckThreshold)
            {
                isTouchingWall = true;
            }
        }

        if (isTouchingWall)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        Debug.DrawRay(transform.position, Vector2.down * checkRadius, isGrounded ? Color.green : Color.red);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Trap"))
        {
            Debug.Log("Player hit the trap!");

            if (hitCount < 3)
            {
                hitCount++;
                StartCoroutine(AnimateHeartLoss());
                cameraScript?.TriggerShakeAndFlash();
                cameraScript?.TriggerShake();
            }

            Debug.Log("Hit count: " + hitCount);

            if (hitCount >= 3 && !hasTriggeredFail)
            {
                Debug.Log("Hit count reached 3, stopping timer.");
                timerScript?.StopTimer();
                hasTriggeredFail = true;
                hitCount = 3;
                canMove = false;
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.isKinematic = true;
                }
                if (anim != null)
                {
                    anim.enabled = false;
                }
                levelFailPanel?.SetActive(true);
                if (SessionManager.Instance != null)
                {
                    Checkpoint closestCheckpoint = SessionManager.Instance.GetClosestUpcomingCheckpoint(transform.position);
                    if (closestCheckpoint != null)
                    {
                        SessionManager.Instance.AddCheckpoint(closestCheckpoint.checkpointID);
                        Debug.Log("Closest upcoming checkpoint: " + closestCheckpoint.checkpointID);
                    }
                    SessionManager.Instance.PostSessionDataToFireBase();
                }
                firebaseAnalytics.PostToFireBase(false);
                clockRotation?.StopRotation();
            }
        }

        if (other.gameObject.CompareTag("Finish") && Collectible.collectiblesRemaining == 0)
        {
            canMove = false;
            clockRotation?.StopRotation();
        }
    }

    IEnumerator AnimateHeartLoss()
    {
        if (hearts.Count > 0 && hitCount <= hearts.Count)
        {
            Image heart = hearts[hearts.Count - hitCount];
            Vector3 originalScale = heart.transform.localScale;
            Vector3 targetScale = originalScale * 1.5f;
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                heart.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < duration)
            {
                heart.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            heart.enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform);
            
            // Preserve the current facing direction
            float currentScaleX = transform.localScale.x;
            transform.localScale = new Vector3(
                currentScaleX >= 0 ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
            
            isOnPlatform = true;
            currentPlatform = collision.transform;
            rb.interpolation = RigidbodyInterpolation2D.None;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(originalParent);
            isOnPlatform = false;
            currentPlatform = null;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    public void RestartScene()
    {
        hasTriggeredFail = false;
        SessionManager.Instance.isDataPosted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void HandlePause(bool isPaused)
    {
        canMove = !isPaused;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = isPaused;
        }

        if (anim != null)
        {
            anim.enabled = !isPaused;
        }
    }

    void InitializeHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = true;
        }
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = i < 3 - hitCount;
        }
    }

    void HandleFlip()
    {
        flipCount++;
    }

    public bool IsOnPlatform()
    {
        return isOnPlatform;
    }

    public Transform GetCurrentPlatform()
    {
        return currentPlatform;
    }

    public void SetZoomState(bool isZooming)
    {
        isZoomingCamera = isZooming;
        SetCanMove(!isZooming);
    }

    private void SetCanMove(bool state)
    {
        canMove = state;
        if (!state)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
