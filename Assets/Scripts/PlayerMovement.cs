using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Attributes")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float currentVelocity = 0f;
    public bool canMove = true;
    public bool isGrounded = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.5f;
    public LayerMask groundLayer;

    [Header("UI and Game Elements")]
    public GameObject levelFailPanel;
    public List<Image> hearts;
    public TimerScript timerScript;
    public FireBaseAnalytics firebaseAnalytics;
    public ClockRotation clockRotation;
    public CameraScript cameraScript;
    public Button restartButton;

    public int hitCount = 0;
    public bool hasTriggeredFail = false;

    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    public Transform originalParent;
    public bool isOnPlatform = false;
    public Transform currentPlatform;
    public bool isZoomingCamera = true;
    public int flipCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        hasTriggeredFail = false;
        PauseManager.OnPause += HandlePause;
        FlipManager.OnFlip += HandleFlip;

        InitializeHearts();

        cameraScript = Camera.main.GetComponent<CameraScript>();
        originalParent = transform.parent;
        clockRotation = FindObjectOfType<ClockRotation>();
        SetCanMove(!isZoomingCamera);
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
        Jump();
        FlexibleJump();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        // float horizontalSpeed = (moveInput != 0f) ? SmoothSpeed(moveSpeed, 0.1f) : SmoothSpeed(0, 0.2f);
        float horizontalSpeed = moveSpeed;

        rb.velocity = new Vector2(moveInput * horizontalSpeed, rb.velocity.y);

        // Update animation state
        anim.SetInteger("movement", (int)Input.GetAxisRaw("Horizontal"));

        // Flip the player sprite
        if (moveInput < 0) spriteRenderer.flipX = true;
        else if (moveInput > 0) spriteRenderer.flipX = false;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("Jumping", true);
        }
        else if (isGrounded)
        {
            anim.SetBool("Jumping", false);
        }
    }

    void FlexibleJump()
    {
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Trap"))
        {
            HandleTrap();
        }

        if (other.gameObject.CompareTag("Finish") && Collectible.collectiblesRemaining == 0)
        {
            canMove = false;
            clockRotation?.StopRotation();
        }
    }

    void HandleTrap()
    {
        if (hitCount < 3)
        {
            hitCount++;
            StartCoroutine(AnimateHeartLoss());
            cameraScript?.TriggerShakeAndFlash();
        }

        if (hitCount >= 3 && !hasTriggeredFail)
        {
            FailLevel();
        }
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = i < 3 - hitCount;
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

            for (float elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime)
            {
                heart.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
                yield return null;
            }

            for (float elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime)
            {
                heart.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
                yield return null;
            }

            heart.enabled = false;
        }
    }

    void FailLevel()
    {
        timerScript?.StopTimer();
        hasTriggeredFail = true;
        canMove = false;
        levelFailPanel?.SetActive(true);
        clockRotation?.StopRotation();
        firebaseAnalytics.PostToFireBase(false);
    }

    void HandlePause(bool isPaused)
    {
        canMove = !isPaused;
    }

    void InitializeHearts()
    {
        foreach (var heart in hearts)
        {
            heart.enabled = true;
        }
    }

    void HandleFlip()
    {
        flipCount++;
    }

    float SmoothSpeed(float targetSpeed, float smoothTime) => Mathf.SmoothDamp(rb.velocity.x, targetSpeed, ref currentVelocity, smoothTime);

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

    // Added methods
    public bool IsOnPlatform()
    {
        return isOnPlatform;
    }

    public Transform GetCurrentPlatform()
    {
        return currentPlatform;
    }
}
