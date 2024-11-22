using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Rigidbody2D rb;
    private bool isGrounded = false;
    public int hitCount = 0;
    public SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public float checkRadius = 0.5f;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hasTriggeredFail = false;
        PauseManager.OnPause += HandlePause;
        InitializeHearts();
        FlipManager.OnFlip += HandleFlip;

        cameraScript = Camera.main.GetComponent<CameraScript>();
        originalParent = transform.parent;
    }

    void OnDestroy()
    {
        PauseManager.OnPause -= HandlePause;
        FlipManager.OnFlip -= HandleFlip;
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput < 0)
        {
            moveInput = 0;
        }
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            transform.SetParent(originalParent);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
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
                UpdateHearts();
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
            }
        }

        if (other.gameObject.CompareTag("Finish") && Collectible.collectiblesRemaining == 0)
        {
            canMove = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform);
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
    }

    void InitializeHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = true;
        }
    }

    void UpdateHearts()
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
}
