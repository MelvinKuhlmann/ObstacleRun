using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected bool inPause = false;
    private PlayerHorizontalState horizontalState;
    private PlayerVerticalState verticalState;
    private Animator animator;
    private Rigidbody2D rigidBody;
    public Damager meleeDamager;
    public Damageable damageable;
    private float horizontal;

    public ParticleSystem dust;

    [Header("Movement")]
    public float moveSpeed = 10F;
    public float jumpForce = 25F;
    public float dashSpeed;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    private float dashTimeCounter;
    public float dashTime;

    [Header("Ground check")]
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Wall Jump")]
    public Transform frontCheck;
    public float wallSlidingSpeed;
    public float horizontalWallForce;
    public float verticalWallForce;
    public float wallJumpTime;
    private bool wallSliding;
    private bool isTouchingFront;
    private bool wallJumping;

    #region Singleton
    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void SkillUnlocked(Skill skill)
    {
        switch (skill.skillType)
        {
            case PlayerSkills.SkillType.MoveSpeed_1:
                // SetMovementSpeed(50f);
                break;
            case PlayerSkills.SkillType.MoveSpeed_2:
                // SetMovementSpeed(70f);
                break;
            case PlayerSkills.SkillType.HealthMax_1:
            case PlayerSkills.SkillType.HealthMax_2:
            case PlayerSkills.SkillType.HealthMax_3:
                damageable.IncreaseMaxHealth();
                break;
        }
    }

    void Start()
    {
        horizontalState = PlayerHorizontalState.IDLE;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        meleeDamager.DisableDamage();

        dashTimeCounter = dashTime;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        HandleMenu();
    }

    void FixedUpdate()
    {
        if (!isGrounded && rigidBody.velocity.y < 0)
        {
            verticalState = PlayerVerticalState.FALLING;
        }

        HandleState();

        //TODO move to handle state
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);
        wallSliding = isTouchingFront && !isGrounded && horizontalState != PlayerHorizontalState.IDLE;
        if (wallSliding)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (wallJumping)
        {
            rigidBody.velocity = new Vector2(horizontalWallForce * -horizontal, verticalWallForce);
        }
    }

    private void HandleState()
    {
        string animation = "";

        if (!PlayerIsInTheAir())
        {
            switch (horizontalState)
            {
                case PlayerHorizontalState.IDLE:
                    animation = horizontal == -1 ? "idle_left" : "idle_right";
                    break;
                case PlayerHorizontalState.RUNNING:
                    animation = horizontal == -1 ? "run_left" : "run_right";
                    break;
                default:
                    break;
            }
        }

        if (PlayerIsInTheAir())
        {
            switch (verticalState)
            {
                case PlayerVerticalState.JUMPING:
                    animation = horizontal == -1 ? "jump_left" : "jump_right";
                    break;
                case PlayerVerticalState.FALLING:
                    animation = horizontal == -1 ? "fall_left" : "fall_right";
                    break;
            }
        }

        if (PlayerHorizontalState.ATTACK_SLASH == horizontalState)
        {
            animation = horizontal == -1 ? "attack_down_slash_right" : "attack_down_slash_right";
        }

        if (animation.Length != 0)
        {
            PlayAnimation(animation);
        }
    }

    private void PlayAnimation(string animation)
    {
        animator.Play(animation);
    }

    public void HandleJump()
    {
        if (!PlayerIsInTheAir())
        {
            rigidBody.velocity = Vector2.up * jumpForce;
            verticalState = PlayerVerticalState.JUMPING;
            isJumping = true;
            jumpTimeCounter = jumpTime;
        }

        if (wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }

    public void HandleKeepJumping()
    {
        if (isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rigidBody.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            verticalState = PlayerVerticalState.JUMPING;
        } else
        {
            isJumping = false; //TODO this can probably be removed
            HandleJump();
        }
    }

    public void HandleStopJump()
    {
        isJumping = false;
    }

    public void HandleIdle()
    {
        horizontalState = PlayerHorizontalState.IDLE;
    }

    public void HandleLeftDash()
    {
        if (!CanUseDash())
        {
            return;
        }
        if (dashTimeCounter > 0) 
        {
            CreateDust();
            dashTimeCounter -= Time.deltaTime;
            rigidBody.velocity = Vector2.left * dashSpeed;
        } else
        {
            dashTimeCounter = dashTime;
            rigidBody.velocity = Vector2.zero;
        }
    }

    public void HandleRightDash()
    {
        if (!CanUseDash())
        {
            return;
        }
        if (dashTimeCounter > 0)
        {
            CreateDust();
            dashTimeCounter -= Time.deltaTime;
            rigidBody.velocity = Vector2.right * dashSpeed;
        } else
        {
            dashTimeCounter = dashTime;
            rigidBody.velocity = Vector2.zero;
        }
    }

    public void HandleRightMovement()
    {
        transform.position += transform.right * (Time.deltaTime * moveSpeed);
        horizontalState = PlayerHorizontalState.RUNNING;
    }

    public void HandleLeftMovement()
    {
        transform.position -= transform.right * (Time.deltaTime * moveSpeed);
        horizontalState = PlayerHorizontalState.RUNNING;
    }

    public void HandleAttack()
    {
        meleeDamager.EnableDamage();
        horizontalState = PlayerHorizontalState.ATTACK_SLASH;
        meleeDamager.disableDamageAfterHit = true;
        Invoke("DisableMeleeAttack", 0.2F);
    }

    public void DisableMeleeAttack()
    {
        meleeDamager.DisableDamage();
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private bool PlayerIsInTheAir()
    {
        return !isGrounded;
    }

    public void Fall()
    {
        if (!isGrounded)
        {
            verticalState = PlayerVerticalState.FALLING;
        }
    }

    private bool PlayerIsJumping()
    {
        return verticalState == PlayerVerticalState.JUMPING;
    }

    private bool PlayerIsFalling()
    {
        return verticalState == PlayerVerticalState.FALLING;
    }

    public void OnHurt(Damager damager, Damageable damageable)
    {
        //if the player don't have control, we shouldn't be able to be hurt as this wouldn't be fair
      //  if (!PlayerInput.Instance.HaveControl)
      //      return;

      //  UpdateFacing(damageable.GetDamageDirection().x > 0f);
        damageable.EnableInvulnerability();

        //   m_Animator.SetTrigger(m_HashHurtPara);

        //we only force respawn if helath > 0, otherwise both forceRespawn & Death trigger are set in the animator, messing with each other.
        if (damageable.CurrentHealth > 0 && damager.forceRespawn)
        {
        //    m_Animator.SetTrigger(m_HashForcedRespawnPara);
        }

      //  m_Animator.SetBool(m_HashGroundedPara, false);
      //  hurtAudioPlayer.PlayRandomSound();

        //if the health is < 0, mean die callback will take care of respawn
        if (damager.forceRespawn && damageable.CurrentHealth > 0)
        {
          //  StartCoroutine(DieRespawnCoroutine(false, true));
        }
    }

    public bool CanUseEarthShatter() 
    {
        return GetComponent<PlayerSkills>().IsSkillUnlocked(PlayerSkills.SkillType.EarthShatter);
    }

    public bool CanUseDash()
    {
        return GetComponent<PlayerSkills>().IsSkillUnlocked(PlayerSkills.SkillType.Dash);
    }

    public void HandleMenu()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!inPause)
            {
                if (ScreenFader.IsFading)
                    return;

               // PlayerInput.Instance.ReleaseControl(false);
               // PlayerInput.Instance.Pause.GainControl();
                inPause = true;
                Time.timeScale = 0;
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("UIMenus", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            else
            {
                Unpause();
            }
        }
    }

    public void Unpause()
    {
        //if the timescale is already > 0, we 
        if (Time.timeScale > 0)
            return;

        StartCoroutine(UnpauseCoroutine());
    }

    protected IEnumerator UnpauseCoroutine()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("UIMenus");
      //  PlayerInput.Instance.GainControl();
        //we have to wait for a fixed update so the pause button state change, otherwise we can get in case were the update
        //of this script happen BEFORE the input is updated, leading to setting the game in pause once again
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();
        inPause = false;
    }
}
