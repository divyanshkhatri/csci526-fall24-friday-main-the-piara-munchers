using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public class PlayerJump : MonoBehaviour
{
    public delegate bool OnGround();
    public static event OnGround onGround;

    [SerializeField] private float jumpForce;

    Rigidbody2D rb;
    Animator anim;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Jump();
        FlexibeJump();
        onGround.Invoke();
    }

    void Update()
    {
        anim.SetFloat("JumpAxis", rb.velocity.y);
        anim.SetBool("Jumping", !onGround.Invoke());
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
            if(onGround.Invoke())
                rb.velocity = new Vector3(rb.velocity.x, Time.fixedDeltaTime * jumpForce);
        anim.SetBool("Jumping", !onGround.Invoke());
    }

    void FlexibeJump()
    {
        if(Input.GetButtonUp("Jump") &&  rb.velocity.y > 0)
            rb.velocity = new Vector3(rb.velocity.x, Time.deltaTime * 0.50f);
    }
}
