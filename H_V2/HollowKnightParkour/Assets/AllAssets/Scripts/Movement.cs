using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
public class Movement : MonoBehaviour
{

    public float speed = 5f;
    private bool isMoving;

    public float minJumpForce = 14f;
    public float maxJumpForce = 18f;
    public float jumpHoldTime = 0.2f;
    private bool canJump = true;
    private float jumpTimer = 0f;
    private bool isJumpingSoundPlayed = false;
    private float fallTime = 0f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float fallSpeedMultiplier = 0.75f;
    private bool isFallingSoundPlaying = false;
    private bool hasPlayedFallingSound = false;

    public float dashDistance = 5f;
    public float dashTime = 0.25f;
    public float dashCooldown = 0.5f;

    private bool canDash = true;
    private bool isDashing;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(6f, 14f);

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private float horizontal;
    private bool isFacingRight = true;

    public AudioClip jumpSound, dashSound, movement, falling, jumpingOnTheWall, SlidingOnTheWall;
    public AudioSource audioSource;
    private void Moving()
    {
        float move = 0;

        bool movingNow = false; // Track if movement is happening in this frame

        if (Input.GetKey(KeyCode.RightArrow))
        {
            move = 1;
            spriteRenderer.flipX = true;
            movingNow = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move = -1;
            spriteRenderer.flipX = false;
            movingNow = true;
        }

        // Play sound only when movement starts
        if (movingNow && isGrounded)
        {
            if (!audioSource.isPlaying)  // Prevents replaying the same sound
            {
                audioSource.clip = movement;
                audioSource.loop = true; // Loop the movement sound
                audioSource.Play();
            }
        }
        // Stop playing when movement stops
        else if (!movingNow && isMoving)
        {
            audioSource.Stop();
        }
        if (!isGrounded && audioSource.clip == movement) {

            audioSource.Stop();
        }

        isMoving = movingNow; // Update movement state
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Z) && wallJumpingCounter > 0f)
        {
            if (!isWallJumping)
            {
                Flip();
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                if (audioSource.clip == SlidingOnTheWall)
                {
                    audioSource.Stop();
                }
                audioSource.clip = jumpingOnTheWall;
                audioSource.PlayOneShot(jumpingOnTheWall);
            }
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                spriteRenderer.flipX = !spriteRenderer.flipX; // Updated line

            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.35f, wallLayer);
    }
    private void WallSlide()
    {

        if (isWalled() && horizontal != 0f && !isGrounded)
        {
            if (!isWallSliding)
            {
                isWallSliding = true;
                animator.SetBool("isWallSliding", isWallSliding);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                audioSource.clip = SlidingOnTheWall;
                audioSource.Play();
            }
        }
        else
        {
            if (isWallSliding)
            {
                isWallSliding = false;
                animator.SetBool("isWallSliding", isWallSliding);

            }
        }
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
    private void isGroundedd()
    {
        canJump = true;
        isGrounded =  Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer) == null)
        isGrounded = false;
        //Debug.LogError(isGrounded);
        if(isGrounded == true)
        {
            audioSource.clip = movement;
        }
    }
    private void Jump()
    {
        // Start Jump when Z is first pressed
        if (Input.GetKeyDown(KeyCode.Z) && !isDashing && isGrounded && canJump)
        {
            Debug.Log("Jump one");
            jumpTimer = 0f;
            canJump = false;
            animator.SetBool("isWallSliding", false);

            // Apply initial jump impulse
            rb.velocity = new Vector2(rb.velocity.x, minJumpForce);

            audioSource.clip = jumpSound;
            audioSource.PlayOneShot(jumpSound);
            Debug.Log("Jump Started");
        }

        // Continue increasing jump height while holding Z
        if (Input.GetKey(KeyCode.Z) && !isDashing && !canJump && jumpTimer < jumpHoldTime)
        {
            jumpTimer += Time.deltaTime;

            // Calculate jump strength based on hold duration
            float jumpProgress = Mathf.Clamp01(jumpTimer / jumpHoldTime);
            float targetJumpVelocity = Mathf.Lerp(minJumpForce, maxJumpForce, jumpProgress);

            // Directly set velocity instead of incrementing
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, targetJumpVelocity);
                Debug.Log("Jump Strength Adjusted: " + targetJumpVelocity);
            }
        }

        // Stop jump boost when Z is released
        if (Input.GetKeyUp(KeyCode.Z))
        {
            jumpTimer = jumpHoldTime; // Ensure immediate stop
            Debug.Log("Jump Released");
        }

        // Falling logic
        if (rb.velocity.y < 0 && !isGrounded)
        {
            fallTime += Time.deltaTime;

            if (fallTime > 1f && !hasPlayedFallingSound && !isWalled())
            {
                audioSource.clip = falling;
                audioSource.Play();
                hasPlayedFallingSound = true;
            }
        }
        else
        {
            fallTime = 0f;
        }

        // Stop falling sound when landing
        if (isGrounded && hasPlayedFallingSound)
        {
            audioSource.Stop();
            hasPlayedFallingSound = false;
        }
    }
    public void freezeAnimation()
    {
        animator.SetFloat("Speed", Mathf.Abs(0));
    }
    private IEnumerator Dash()
    {
        if (!canDash || isDashing) yield break;

        canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", isDashing);

        jumpTimer = jumpHoldTime;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 dashDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
        Vector2 dashStartPosition = rb.position;

        RaycastHit2D hit = Physics2D.Raycast(dashStartPosition, dashDirection, dashDistance, wallLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(dashStartPosition, dashDirection, dashDistance, groundLayer);

        float dashDistanceToApply = dashDistance;

        if (hit.collider != null || hit2.collider != null)
        {
            dashDistanceToApply = hit.distance;
        }

        // Play the dash sound once
        if (audioSource.clip != dashSound || !audioSource.isPlaying)
        {
            audioSource.clip = dashSound;
            audioSource.Play();
        }

        float currentTime = 0f;
        while (currentTime < dashTime)
        {
            rb.position = Vector2.Lerp(dashStartPosition, dashStartPosition + dashDirection * dashDistanceToApply, currentTime / dashTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // Stop the dash sound when dashing ends
        if (audioSource.clip == dashSound && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool("isDashing", isDashing);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void AnimationUpdating()
    {
        if (rb.velocity.y > 5 && !animator.GetBool("isJumping"))
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (isWallSliding)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        else if (rb.velocity.y < -3 && !isGrounded && !animator.GetBool("isFalling"))

        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else if (isGrounded && (animator.GetBool("isJumping") || animator.GetBool("isFalling")))
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isDashing) return;
       
        isGroundedd();
        Jump();
        Moving();
        WallSlide();
        WallJump();
        if (Input.GetKeyDown(KeyCode.C))
    {
       StartCoroutine(Dash());
    }
    AnimationUpdating();
}
    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
    {
        jumpTimer = 0f;
        isJumpingSoundPlayed = false;
        }
   if (audioSource.clip == falling && audioSource.isPlaying)
   {
        audioSource.Stop();
   }
 }
}

