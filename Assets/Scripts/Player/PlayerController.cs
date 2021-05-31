using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerHorizontalState horizontalState;
    private PlayerVerticalState verticalState;
    private PlayerSkills playerSkills;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private float jumpIn;
    private float horizontal;
    private float vertical;

    public ParticleSystem dust;

    [Header("Health")]
   // public float health;
    public int maxHealth = 3;

    [Header("Movement")]
    public float jumpFrequency = 0.7F;
    public float moveSpeed = 10F;
    public float jumpHeight = 25F;
    public float dashSpeed;

    private float dashTime;
    public float startDashTime;

    #region Singleton
    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            playerSkills = new PlayerSkills();
            playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        switch (e.skill.skillType)
        {
            case PlayerSkills.SkillType.MoveSpeed_1:
                // SetMovementSpeed(50f);
                break;
            case PlayerSkills.SkillType.MoveSpeed_2:
                // SetMovementSpeed(70f);
                break;
            case PlayerSkills.SkillType.HealthMax_1:
                maxHealth= 4;
                HealthVisual.Instance.SetHealthSystem(new HealthSystem(maxHealth));
                break;
            case PlayerSkills.SkillType.HealthMax_2:
                maxHealth = 5;
                HealthVisual.Instance.SetHealthSystem(new HealthSystem(maxHealth));
                break;
            case PlayerSkills.SkillType.HealthMax_3:
                maxHealth = 6;
                HealthVisual.Instance.SetHealthSystem(new HealthSystem(maxHealth));
                break;
        }
    }

    void Start()
    {
        HealthVisual.Instance.SetHealthSystem(new HealthSystem(maxHealth));
       // UISkillTree.Instance.SetPlayerSkills(playerSkills);
        horizontalState = PlayerHorizontalState.IDLE;
        verticalState = PlayerVerticalState.GROUNDED;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        dashTime = startDashTime;
    }

    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate()
    {
        HandleState();
    }

    private void LateUpdate()
    {
        jumpIn -= Time.deltaTime;
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
            if (jumpIn < 0)
            {
                jumpIn = jumpFrequency;
                rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                verticalState = PlayerVerticalState.JUMPING;
            }
        }
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
        if (dashTime <= 0)
        {
            dashTime = startDashTime;
            rigidBody.velocity = Vector2.zero;
        }
        else
        {
            CameraController.instance.Shake(1);
            CreateDust();
            dashTime -= Time.deltaTime;
            rigidBody.velocity = Vector2.left * dashSpeed;
        }
    }

    public void HandleRightDash()
    {
        if (!CanUseDash())
        {
            return;
        }
        if (dashTime <= 0)
        {
            dashTime = startDashTime;
            rigidBody.velocity = Vector2.zero;
        }
        else
        {
            CameraController.instance.Shake(1);
            CreateDust();
            dashTime -= Time.deltaTime;
            rigidBody.velocity = Vector2.right * dashSpeed;
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
        horizontalState = PlayerHorizontalState.ATTACK_SLASH;
    }

    public void Fall()
    {
        if (verticalState != PlayerVerticalState.GROUNDED)
        {
            verticalState = PlayerVerticalState.FALLING;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Tags.Floor.ToString()) && !verticalState.Equals(PlayerVerticalState.GROUNDED))
        {
            CreateDust();
//            CameraController.instance.Shake(-1);
            verticalState = PlayerVerticalState.GROUNDED;
        }
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private bool PlayerIsInTheAir()
    {
        return PlayerIsJumping() || PlayerIsFalling();
    }

    private bool PlayerIsJumping()
    {
        return verticalState == PlayerVerticalState.JUMPING;
    }

    private bool PlayerIsFalling()
    {
        return verticalState == PlayerVerticalState.FALLING;
    }

    public void GetDamage(float damage)
    {
        HealthVisual.Instance.healthSystem.Damage(2);
    }

    public void ReceiveHealth(float healthReceived)
    {
        HealthVisual.Instance.healthSystem.Heal(4);
    }

    public bool CanUseEarthShatter() {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.EarthShatter);
    }

    public bool CanUseDash()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.Dash);
    }
}
