using System;
using System.Collections;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingMovement : MonoBehaviour
{
    float InputX;
    public static TestingMovement instance;
    public LayerMask whatIsGround;
    public LayerMask DefaultLayer;
    public LayerMask wallLayer;
    public Transform feetPos;
    public Transform wallCheck;
    public float checkRadius;
    public Vector3 spawnPosition;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;

    public float speed;
    float moveInput;
    bool isGrounded;

    public float jumpForce;
    public float jumpTime;
    float JumpTimerCounter;
    bool isJumping = false;
    bool isAllowedToJump;

    public float doubleJumpForce;
    public int howManyJumping = 0;
    bool isDoubleJumping = false;
    bool queuedDoubleJump = false;

    public float dashDistance;
    private float dashCooldown = 1;
    float nextDashTime = 0f;
    public bool canDashInAir = true;
    bool isDashing;
    public float dashTime;
    Vector2 dashDirection;

    private bool isWallSliding;
    public float wallSlidingSpeed;

    public float wallJumpingTime;
    public float wallJumpingDuration;
    public float WalljumpForce;
    float wallJumpingDirection;
    public float WalljumpTime;
    float wallJumpingCounter;
    bool isWallJumping;
    bool HasWallJumped = false;
    float lastWallTime = -1f;
    float wallGraceTime = 0.05f;
    float WallJumpTimerCounter;
    bool isJumpingOnWall = false;

    public AudioSource audioSource,audioSource2;
    public AudioClip MovingSound,JumpingSound,DashingSound,WallJumpingSound,doubleJump,Hit,SpikeHit,takenDamage;


    bool isLookingDown;
    bool isDownHitting = true;
    bool canDownHit = true;
    float downHitCooldown = 0.7f; 
    float nextDownHitTime = 0f;
    public bool isSwordColliding = false;

    public bool isAllowedToDownHit = false;
    public bool isAllowedToSideHit = false;

    public GameObject HitBox;
    bool isSideHitting = false;
    float timeToAttack = 0.25f;
    float timer = 0f;

    public int maxHealth;
    HealthManager healthManager;
    public  int currentHealth;
    Vector3 lastCheckpoint;
    HealthUI healthUI;
    bool isInvincible = false;
    public float invincibilityDuration;
    public float flickerSpeed;

    public bool final = true;

    public string currentScene;
    public string PreviousScene;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource2.clip = MovingSound;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthManager = FindAnyObjectByType<HealthManager>();
        currentHealth = maxHealth;
        lastCheckpoint = transform.position;
        healthUI = FindObjectOfType<HealthUI>(); 
        healthUI.UpdateHealth(currentHealth); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = spawnPosition;
    }
    void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = 2;
        }
        InputX = Input.GetAxisRaw("Horizontal");
        currentHealth = healthManager.currentHealth;
        CheckGrounded();
            HandleMovement();
            HandleJump();
            WallSlide();
            isntWalled();
            LookingDown();
            SideHit();
            ApplyFallingAndJumpingAnimation();
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(Dash());
        }

        CheckFinal();
        if(currentScene == "BossFight")final = false;
    }
    void FixedUpdate()
    {
        ApplyMovement();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == HitBox)
        {
            if (collision.CompareTag("Spikes"))
            {
                isSwordColliding = false; 
            }
            else
            {
                isSwordColliding = true; 
            }
        }
       
    }
    void CheckFinal()
    {
        if(final == true)
        {
            animator.SetBool("Final", true);
        }
    }
    public void MoveToNextScene(Vector3 nextSpawnPoint)
    {
        spawnPosition = nextSpawnPoint; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }
    public void TakeDamage()
    {
        if (isInvincible) return; 

        currentHealth--;

        FindObjectOfType<HealthUI>().UpdateHealth(currentHealth);
        FindAnyObjectByType<HealthManager>().TakeDamage();

        if (currentHealth > 0)
        {
            audioSource.PlayOneShot(takenDamage);
            StartCoroutine(InvincibilityCoroutine());
            RespawnAtCheckpoint();
        }
        else
        {
            Die();
        }
    }
    public void TakeDamageWithoutRespwning()
    {
        if (isInvincible) return; 

        currentHealth--;

       
        FindObjectOfType<HealthUI>().UpdateHealth(currentHealth);
        FindAnyObjectByType<HealthManager>().TakeDamageWithoutCheckPoint();

        if (currentHealth > 0)
        {
            audioSource.PlayOneShot(takenDamage);
            StartCoroutine(InvincibilityCoroutine());
        }
        else
        {
            Die();
        }
    }
    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += flickerSpeed)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; 
            yield return new WaitForSeconds(flickerSpeed);
        }

        spriteRenderer.enabled = true; 
        isInvincible = false; 
    }
    void RespawnAtCheckpoint()
    {
        transform.position = lastCheckpoint;
    }
    void Die()
    {
        PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("TryAgain");
    }
    void DownHit()
    {
        if (isAllowedToDownHit)
        {
            if (isLookingDown && Input.GetKeyDown(KeyCode.X) && canDownHit)
            {
                HitBox.gameObject.SetActive(true);
                isDownHitting = true;
                animator.SetBool("isDownHitting", isDownHitting);
                if (!isSwordColliding)
                {
                    audioSource.PlayOneShot(Hit);
                }
                canDownHit = false;
                nextDownHitTime = Time.time + downHitCooldown;
            }
            else
            {
                Invoke(nameof(DisableHitBox), 0.5f);
                isDownHitting = false;
                animator.SetBool("isDownHitting", isDownHitting);
            }
            if (!canDownHit && Time.time >= nextDownHitTime)
            {
                canDownHit = true;
            }
        }
    }
    void DisableHitBox()
    {
        if (isAllowedToSideHit)
        {
            HitBox.gameObject.SetActive(false);
            isSideHitting = false;
            animator.SetBool("isSideHitting", isSideHitting);
        }
    }
    void SideHit()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isLookingDown && isAllowedToSideHit)
        {
                isSideHitting = true;
                HitBox.gameObject.SetActive(isSideHitting);
                audioSource.PlayOneShot(Hit);
                animator.SetBool("isSideHitting", isSideHitting);
                Invoke(nameof(DisableHitBox), 0.15f);
        }
    }
    void LookingDown()
    {
        if(!isGrounded && Input.GetKey(KeyCode.DownArrow))
        {
            isLookingDown = true;
            animator.SetBool("isLookingDown", isLookingDown);
        }
        else
        {
            isLookingDown = false;
            animator.SetBool("isLookingDown", isLookingDown);
        }
    }
    public void FreezeMovement()
    {
        animator.SetFloat("Speed", Mathf.Abs(0));
        audioSource2.Stop();
    }
    void CheckGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded && !wasGrounded) 
        {
            rb.gravityScale = 3;
            isDoubleJumping = false;
            howManyJumping = 0;
            canDashInAir = true;
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
            animator.SetBool("isWallSliding", false);
        }
    }
    void HandleMovement()
    {
        if (!isDashing)
        {
            bool isMoving = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

            if (isMoving)
            {
                moveInput = Input.GetKey(KeyCode.LeftArrow) ? -1 : 1;
                animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
                FlipCharacter();

                if (isGrounded && rb.velocity.x != 0 && !audioSource2.isPlaying)
                {
                    audioSource2.clip = MovingSound;
                    audioSource2.loop = true;
                    audioSource2.Play();
                }
            }
            else
            {
                moveInput = 0;
                animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

                if (audioSource2.isPlaying)
                {
                    audioSource2.Stop();
                }
            }

            if (!isGrounded && audioSource2.isPlaying)
            {
                audioSource2.Stop();
            }
        }
    }
    void ApplyMovement()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
    void ApplyFallingAndJumpingAnimation()
    {
        if (rb.velocity.y < 0 && !isJumping)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
        }
        else if (rb.velocity.y < 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
        }
        else if (rb.velocity.y < 0 && !isWalled())
        {
            animator.SetBool("isWallSliding", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
        }
        else if (rb.velocity.y > 0 && isJumping && howManyJumping == 1)
        {
            animator.SetBool("isJumping", true);
            animator.SetFloat("Speed", 0);
            animator.SetBool("isFalling", false);
            animator.SetBool("isDoubleJumping", false);
        }
        else if (isGrounded == true)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isDoubleJumping", false);
        }
        else if (!isGrounded && Input.GetKeyDown(KeyCode.Z) && !isWalled() && howManyJumping == 1 && doubleJumpForce > 0)
        {
            animator.SetBool("isDoubleJumping", true);
            animator.SetFloat("Speed", 0);
        }
    }
    void FlipCharacter()
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isWalled()&&HasWallJumped == false)
            {
                WallJump();
            }
            else if (isGrounded && !isWalled())
            {
                Jump();
            }
            else if ( howManyJumping == 1 && doubleJumpForce>0)
            {
                if (isDashing){queuedDoubleJump = true;}
                else
                {
                    DoubleJump();
                }
            }
            else if(!isGrounded&&howManyJumping == 0&& !isWalled() && doubleJumpForce>0)
            {
                if (isDashing) queuedDoubleJump = true;
                else
                {
                    DoubleJump();
                }
            }
        }
        if (Input.GetKey(KeyCode.Z) && (isJumping || isWallJumping))
        {
            ContinueJump();
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isJumping = false;
            isJumpingOnWall = false;
        }
    }
    void Jump()
    {
        if (jumpForce > 0)
        {
            howManyJumping = 1;
            JumpTimerCounter = jumpTime;
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
            audioSource.PlayOneShot(JumpingSound);
            FlipCharacter();
        }
    }
    void DoubleJump()
    {
        rb.velocity = Vector2.up * doubleJumpForce;
        animator.SetBool("isDoubleJumping", true);
        animator.SetBool("isFalling", false);
        animator.SetBool("isJumping", false);
        animator.SetFloat("Speed", 0);
        isDoubleJumping = true;
        isJumping = false;
        howManyJumping = 2;
        audioSource.PlayOneShot(doubleJump);
    }
    void WallJump()
    {
        if (!isWalled()) return; 

        animator.SetBool("isJumping", true);
        animator.SetBool("isFalling", false);
        isJumpingOnWall = true;
        isJumping = true;
        HasWallJumped = true;
        howManyJumping = 1;
        WallJumpTimerCounter = WalljumpTime;
        rb.velocity = Vector2.up * WalljumpForce;
        audioSource.PlayOneShot(WallJumpingSound);
    }
    void ContinueJump()
    {
        if (isJumping && JumpTimerCounter > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            JumpTimerCounter -= Time.deltaTime;
            animator.SetBool("isFalling", false);
            FlipCharacter();
        }
        else if (isJumpingOnWall && WallJumpTimerCounter > 0)
        {
            if (isWalled())
            {
                rb.velocity = Vector2.up * WalljumpForce;
                WallJumpTimerCounter -= Time.deltaTime;
            }
            else
            {
                isJumpingOnWall = false;
            }
        }
        else
        {
            isJumping = false;
            isJumpingOnWall = false;
        }
    }
    IEnumerator Dash()
    {
        if (Time.time < nextDashTime || isDashing || (!canDashInAir && !isGrounded)) yield break;
        isDashing = true;
        animator.SetBool("isDashing", isDashing);

        if (dashDistance != 0)
        audioSource.PlayOneShot(DashingSound);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 dashStartPosition = rb.position;
        Vector2 dashDirection = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

        RaycastHit2D GroundHit = Physics2D.Raycast(dashStartPosition, dashDirection, dashDistance, whatIsGround);
        RaycastHit2D WallHit = Physics2D.Raycast(dashStartPosition, dashDirection, dashDistance, wallLayer);
        RaycastHit2D DefaultHit = Physics2D.Raycast(dashStartPosition, dashDirection, dashDistance, DefaultLayer);
        float dashDistanceToApply = dashDistance;

        if (GroundHit.collider != null)
        {
            dashDistanceToApply = GroundHit.distance;
        }
        if (WallHit.collider != null)
        {
            dashDistanceToApply = WallHit.distance;
        }
        if (DefaultHit.collider != null)
        {
            dashDistanceToApply = DefaultHit.distance;
        }

        float currentTime = 0f;
        while (currentTime < dashTime)
        {
            rb.position = Vector2.Lerp(dashStartPosition, dashStartPosition + dashDirection * dashDistanceToApply, currentTime / dashTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.gravityScale = originalGravity;
        isDashing = false;

        animator.SetBool("isDashing", isDashing);

        nextDashTime = Time.time + dashCooldown;

        if (!isGrounded)
        {
            canDashInAir = false;
        }
        if (queuedDoubleJump)
        {
            queuedDoubleJump = false; 
            DoubleJump();
        }

    }
    void HandleWallJump()
    {
        if (isWalled() && Input.GetKeyDown(KeyCode.Z))
        {
            WallJumpTimerCounter = WalljumpTime;
            isJumpingOnWall = true;
            rb.velocity = Vector2.up * WalljumpForce;
            howManyJumping++;

            audioSource.PlayOneShot(WallJumpingSound);
        }
        if (Input.GetKey(KeyCode.Z) && isJumpingOnWall)
        {
            if (WallJumpTimerCounter > 0)
            {
                rb.velocity = Vector2.up * WalljumpForce;
                WallJumpTimerCounter -= Time.deltaTime;
            }
            else
            {
                isJumpingOnWall = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            isJumpingOnWall = false;
        }
    }
    void StopWallJumping()
    {
        isWallJumping = false;
    }
    bool isWalled()
    {
        bool touchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.35f, wallLayer);

        if (touchingWall)
        {
            canDashInAir = true;
            lastWallTime = Time.time;
            howManyJumping = 0;
        }
        return touchingWall || (Time.time - lastWallTime <= wallGraceTime);
    }
    bool isntWalled()
    {
        if (Physics2D.OverlapCircle(wallCheck.position, 0.25f, wallLayer) != true)
            HasWallJumped = false;

        return HasWallJumped;
    }
    void WallSlide()
    {
        if (isWalled() && InputX != 0f && !isGrounded)
        {
            if (!isWallSliding)
            {
                isWallSliding = true;
                animator.SetBool("isWallSliding", isWallSliding);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
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
}
