using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// [RequireComponent(typeof(PlayerFlip))]
// public class MasterPlayerMovement : MonoBehaviour
// {
//     [Header("Movement Attributes")]
//     [SerializeField] private float moveSpeed = 5f;
//     [SerializeField] private float maxHorizontalSpeed;
//     [SerializeField] private float smoothTime;
//     [SerializeField] private float currentVelocity;
//     [SerializeField] private float jumpForce = 10f;

//     private bool canMove = true;
//     private bool isGrounded = false;

//     private PlayerFlip playerFlip;
//     private Rigidbody2D rb;
//     private Animator anim;

//     [Header("Ground Check")]
//     public Transform groundCheck;
//     public float checkRadius = 0.5f;
//     public LayerMask groundLayer;

//     [Header("UI and Game Elements")]
//     public GameObject levelFailPanel;
//     public List<Image> hearts;
//     public TimerScript timerScript;
//     public FireBaseAnalytics firebaseAnalytics;
//     public ClockRotation clockRotation;
//     public CameraScript cameraScript;

//     private int hitCount = 0;
//     private bool hasTriggeredFail = false;
//     private bool isZoomingCamera = true;

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         anim = GetComponent<Animator>();
//         playerFlip = GetComponent<PlayerFlip>();
//     }

//     void Start()
//     {
//         hasTriggeredFail = false;
//         PauseManager.OnPause += HandlePause;
//         InitializeHearts();
//         FlipManager.OnFlip += HandleFlip;

//         cameraScript = Camera.main.GetComponent<CameraScript>();
//         clockRotation = FindObjectOfType<ClockRotation>();
//         SetCanMove(!isZoomingCamera);
//     }

//     void OnDestroy()
//     {
//         PauseManager.OnPause -= HandlePause;
//         FlipManager.OnFlip -= HandleFlip;
//     }

//     void Update()
//     {
//         if (!canMove || isZoomingCamera)
//         {
//             rb.velocity = Vector2.zero;
//             return;
//         }

//         Move();
//         HandleJump();
//     }

//     void FixedUpdate()
//     {
//         isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
//     }

//    void Move()
// {
//     float horizontal = Input.GetAxis("Horizontal");
//     Debug.Log("Horizontal Input: " + horizontal);

//     float horizontalSpeed = (horizontal != 0f) ? SmoothSpeed(maxHorizontalSpeed, smoothTime) : SmoothSpeed(0, 0.2f);
//     rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

//     anim.SetInteger("movement", (int)Input.GetAxisRaw("Horizontal"));
//     playerFlip.OnFlip();
// }

// void HandleJump()
// {
//     if (Input.GetButtonDown("Jump") && isGrounded)
//     {
//         Debug.Log("Jump Pressed");
//         rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//         anim.SetBool("Jumping", true);
//     }
//     else
//     {
//         anim.SetBool("Jumping", !isGrounded);
//     }
// }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.gameObject.CompareTag("Trap"))
//         {
//             HandleTrap();
//         }

//         if (other.gameObject.CompareTag("Finish") && Collectible.collectiblesRemaining == 0)
//         {
//             canMove = false;
//             clockRotation?.StopRotation();
//         }
//     }

//     void HandleTrap()
//     {
//         if (hitCount < 3)
//         {
//             hitCount++;
//             StartCoroutine(AnimateHeartLoss());
//             cameraScript?.TriggerShakeAndFlash();
//         }

//         if (hitCount >= 3 && !hasTriggeredFail)
//         {
//             FailLevel();
//         }
//     }

//     IEnumerator AnimateHeartLoss()
//     {
//         if (hearts.Count > 0 && hitCount <= hearts.Count)
//         {
//             Image heart = hearts[hearts.Count - hitCount];
//             Vector3 originalScale = heart.transform.localScale;
//             Vector3 targetScale = originalScale * 1.5f;
//             float duration = 0.5f;

//             for (float elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime)
//             {
//                 heart.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
//                 yield return null;
//             }

//             for (float elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime)
//             {
//                 heart.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
//                 yield return null;
//             }

//             heart.enabled = false;
//         }
//     }

//     void FailLevel()
//     {
//         timerScript?.StopTimer();
//         hasTriggeredFail = true;
//         canMove = false;
//         levelFailPanel?.SetActive(true);
//         clockRotation?.StopRotation();
//         firebaseAnalytics.PostToFireBase(false);
//     }

//     void HandlePause(bool isPaused)
//     {
//         canMove = !isPaused;
//     }

//     void InitializeHearts()
//     {
//         foreach (var heart in hearts)
//         {
//             heart.enabled = true;
//         }
//     }

//     float SmoothSpeed(float targetSpeed, float smoothTime) => Mathf.SmoothDamp(moveSpeed, targetSpeed, ref currentVelocity, smoothTime);

//     void HandleFlip()
//     {
//         // Flip handler for player rotation
//     }

//     void SetCanMove(bool state)
//     {
//         canMove = state;
//         if (!state) rb.velocity = Vector2.zero;
//     }
// }
// Compare this snippet from Assets/Scripts/PlayerMovement.cs:

[RequireComponent(typeof(PlayerFlip))]
public class MasterPlayerMovement : MonoBehaviour
{
    [Header("Movement Attributes")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHorizontalSpeed = 4f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float jumpForce = 7f;

    private float currentVelocity = 0f;
    private bool canMove = true;
    private bool isGrounded = false;

    private PlayerFlip playerFlip;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.5f;
    public LayerMask groundLayer;

    void Awake()
    {
        playerFlip = GetComponent<PlayerFlip>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!canMove) return;

        Move();
        Jump();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float horizontalSpeed = (horizontal != 0f) ? SmoothSpeed(maxHorizontalSpeed, smoothTime) : SmoothSpeed(0f, 0.2f);

        rb.velocity = new Vector2(horizontal * horizontalSpeed, rb.velocity.y);
        anim.SetInteger("movement", (int)Input.GetAxisRaw("Horizontal")); // Update animation state

        playerFlip.OnFlip(); // Ensure sprite flips correctly
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("Jumping", true);
        }
        else
        {
            anim.SetBool("Jumping", !isGrounded);
        }
    }

    float SmoothSpeed(float targetSpeed, float smoothTime) => Mathf.SmoothDamp(rb.velocity.x, targetSpeed, ref currentVelocity, smoothTime);

    public void StopMove() => canMove = false;
    public void ActiveMove() => canMove = true;
}
