using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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
    public ClockRotation clockRotation;
    private bool isZoomingCamera = true;
    public SpotlightEffect spotlightEffect;


    [SerializeField] private Image redOverlay; // Assign a UI Image in the canvas
    [SerializeField] private float flashDuration = 0.2f; // Duration of the red flash effect in seconds


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
<<<<<<< Updated upstream
                if (hitCount == 2) {
                    spotlightEffect.TriggerFocusOnHeart();
=======
                if (hitCount == 2)
                {
>>>>>>> Stashed changes
                    cameraScript?.TriggerShakeAndFlash();
                }
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
                clockRotation?.StopRotation();
            }
        }

        if (other.gameObject.CompareTag("Finish") && Collectible.collectiblesRemaining == 0)
        {
            canMove = false;
            clockRotation?.StopRotation();
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

    // void UpdateHearts()
    // {
    //     for (int i = 0; i < hearts.Count; i++)
    //     {
    //         hearts[i].enabled = i < 3 - hitCount;
    //     }
    // }
    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < 3 - hitCount)
            {
                hearts[i].enabled = true; // Keep hearts visible
            }
            else
            {
                if (hearts[i].enabled) // Only animate hearts that are currently visible
                {
                    // Replace the original heart with the broken heart
                    GameObject brokenHeart = GameObject.Find($"brokenHeart_{i + 1}");
                    if (brokenHeart != null)
                    {
                        AnimateHeartLoss(hearts[i], brokenHeart);
                    }
                    else
                    {
                        Debug.LogError($"Broken heart not found: brokenHeart_{i + 1}");
                    }
                }
                hearts[i].enabled = false; // Hide heart after animation
            }
        }

        // Trigger other damage effects like screen flash or sound
        TriggerDamageEffects();
    }





    void TriggerDamageEffects()
    {
        Debug.Log("Triggering damage effects");

        // Flash the red overlay
        FlashRedOverlay();

        // Play a damage sound (ensure an AudioSource is added)
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }


    void FlashRedOverlay()
    {
        if (redOverlay == null) return;

        redOverlay.gameObject.SetActive(true); // Ensure the overlay is visible

        // Fade in the red overlay to 30% opacity
        LeanTween.value(gameObject, 0f, 0.3f, flashDuration / 2f)
            .setOnUpdate((float alpha) =>
            {
                Color color = redOverlay.color;
                color.a = alpha;
                redOverlay.color = color;
            })
            .setEaseInOutQuad()
            .setOnComplete(() =>
            {
                // Fade out the red overlay to 0% opacity
                LeanTween.value(gameObject, 0.3f, 0f, flashDuration / 2f)
                    .setOnUpdate((float alpha) =>
                    {
                        Color color = redOverlay.color;
                        color.a = alpha;
                        redOverlay.color = color;
                    })
                    .setEaseInOutQuad()
                    .setOnComplete(() =>
                    {
                        redOverlay.gameObject.SetActive(false); // Hide the overlay after fading out
                    });
            });
    }



    void AnimateHeartLoss(Image heart, GameObject brokenHeart)
    {
        if (heart == null || brokenHeart == null) return;

        // Scale down the original heart and then hide it
        LeanTween.scale(heart.rectTransform, Vector3.zero, 0.3f)
            .setEaseInBack()
            .setOnComplete(() =>
            {
                heart.enabled = false; // Hide the original heart
                heart.rectTransform.localScale = Vector3.one; // Reset scale for future use

                // Activate and animate the broken heart
                brokenHeart.SetActive(true);
                LeanTween.scale(brokenHeart.GetComponent<RectTransform>(), Vector3.one, 0.3f).setEaseOutBounce();
                LeanTween.alpha(brokenHeart.GetComponent<RectTransform>(), 1f, 0.3f).setEaseInOutQuad();
            });
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
}
