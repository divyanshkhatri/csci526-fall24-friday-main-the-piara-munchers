using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return; // Early return to block further input processing
        }

        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput < 0)
        {
            moveInput = 0;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
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
            }

            Debug.Log("Hit count: " + hitCount);


            if (hitCount == 1)
            {

                spriteRenderer.color = Color.magenta;
            }
            else if (hitCount == 2)
            {

                spriteRenderer.color = new Color(0.8f, 0.0f, 0.4f);
            }
            else if (hitCount >= 3)
                {
                    hitCount = 3;
                    canMove = false;
                    levelFailPanel.SetActive(true);
                    other.isTrigger = false;
                    SessionManager.Instance.PostSessionDataToFireBase();
                }
        }
        if (other.gameObject.CompareTag("Finish") && Collectible.collectiblesRemaining == 0)
        {
            canMove = false;
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}


