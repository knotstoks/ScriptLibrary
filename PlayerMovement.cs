using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private LayerMask platformMask;
    private float jumpHeight = 5f;
    private float jumpTime = 0.5f;
    private float gravity;
    private float jumpSpeed;
    private float fallMultiplier = 2.2f;
    private float lowJumpMultiplier = 2f;
    private float walkSpeed = 9f;
    private bool isGrounded;
    // Components
    private Rigidbody2D rb;
    private BoxCollider2D _collider;
    private PlayerStates playerStates;
    private SpriteRenderer spriteRenderer;
    // Dash
    private bool dashActive;
    // Horizontal and Vertical Movement
    private float xAxis;
    private float yAxis;
    private void Start() 
    {
        playerStates = GetComponent<PlayerStates>();
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gravity = -2 * jumpHeight / Mathf.Pow(jumpTime, 2);
        Physics2D.gravity = new Vector2(0, gravity);
        jumpSpeed = -gravity * jumpTime;
    }

    private void Update() 
    {
        if (!playerStates.gamePaused || !playerStates.inDialogue || !playerStates.inCutscene) 
        {
            GetInputs();
            Walk(xAxis);
            Jump();
            if (Input.GetButtonDown("Dash"))
            {
                StartCoroutine(Dash());
            }
            Grounded();
        }

        if (dashActive)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.cyan;
        }
    }

    private void GetInputs() 
    {
        // Horizontal and Vertical Movement
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
    }

    // Ground checks
    private void Grounded() 
    {
        Bounds bounds = _collider.bounds;
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.down, 0.1f, platformMask);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.down, 0.1f, platformMask);

        if (!dashActive && !playerStates.isDashing) 
        {
            dashActive = hitLeft || hitRight;
        }
        isGrounded = hitLeft || hitRight;
    }

    // Walking
    private void Walk(float move) 
    {
        if (!playerStates.isDashing) 
        {
            rb.velocity = new Vector2(move * walkSpeed, rb.velocity.y);
            playerStates.walking = Mathf.Abs(move) > 0;
        }
    }

    // Jumping
    private void Jump() 
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = Vector2.up * jumpSpeed;
        }

        if (rb.velocity.y < 0) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    // Dashing
    private IEnumerator Dash()
    {
        if (dashActive && !playerStates.isDashing) 
        {
            if (xAxis != 0 || yAxis != 0)
            {
                dashActive = false;
                playerStates.isDashing = true;
                rb.velocity = Vector2.zero;
                float tempGravity = rb.gravityScale;
                rb.gravityScale = 0;
                rb.AddForce(new Vector2(xAxis, yAxis).normalized * 30, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.2f);
                rb.gravityScale = tempGravity;
                playerStates.isDashing = false;
            }
        }
    }
}