using UnityEngine;

[RequireComponent(typeof(PlayerFlip))]
public class AnimationPlayerMovement : MonoBehaviour
{ 
    [Header("Movement Attributes")]

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float maxHorizontalSpeed;

    [SerializeField] private float smoothTime;
    [SerializeField] private float currentVelocity;

    private bool canMove = true;

    private PlayerFlip playerFlip;
    private Rigidbody2D rb;
    private Animator anim;

    void Awake()
    {
        playerFlip = GetComponent<PlayerFlip>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }

    void FixedUpdate()
    {
        Move();

        if (!canMove) { rb.velocity = new Vector2(rb.velocity.x * 0, rb.velocity.y); }
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        horizontalSpeed = (horizontal != 0f) ? SmoothSpeed(maxHorizontalSpeed ,smoothTime) : SmoothSpeed(0 , 0.2f);

        rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * horizontalSpeed, rb.velocity.y);
        
        anim.SetInteger("movement", (int) Input.GetAxisRaw("Horizontal"));
        // playerFlip.OnFlip(); // Removed to avoid conflict
    }


    void OnEnable()
    {
        PlayerAttack.disableMovement += StopMove;
        PlayerAttack.enableMovements += ActiveMove;
    }

    void OnDisable()
    {
        PlayerAttack.disableMovement -= StopMove;
        PlayerAttack.enableMovements -= ActiveMove;
    }

    float SmoothSpeed(float targetSpeed, float smoothTime) => Mathf.SmoothDamp(horizontalSpeed, targetSpeed, ref currentVelocity, smoothTime);
    
    void StopMove() => canMove = false;
    void ActiveMove() => canMove = true;
}
