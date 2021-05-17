using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerHorizontalState horizontalState;
    private PlayerVerticalState verticalState;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private float jumpIn;
    private float horizontal;
    private float vertical;

    public float jumpFrequency = 0.7F;
    public float moveSpeed = 10F;
    public float jumpHeight = 25F;
    public ParticleSystem dust;
    
    void Start()
    {
        horizontalState = PlayerHorizontalState.IDLE;
        verticalState = PlayerVerticalState.GROUNDED;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate()
    {
        HandleMovement();
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

        if (animation.Length != 0)
        {
            PlayAnimation(animation);
        }
    }

    private void PlayAnimation(string animation)
    {
        animator.Play(animation);
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.Space) && !PlayerIsInTheAir())
        {
            if (jumpIn < 0)
            {
                jumpIn = jumpFrequency;
                rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                verticalState = PlayerVerticalState.JUMPING;
            }
        }

        // Een fail-safe om ervoor te zorgen dat er geen rare dingen gebeuren als men links en rechts tegelijk indrukt.
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            HandleRightMovement();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            HandleLeftMovement();
        }
        else
        {
            horizontalState = PlayerHorizontalState.IDLE;
        }
    }

    private void HandleRightMovement()
    {
        transform.position += transform.right * (Time.deltaTime * moveSpeed);
        horizontalState = PlayerHorizontalState.RUNNING;
    }

    private void HandleLeftMovement()
    {
        transform.position -= transform.right * (Time.deltaTime * moveSpeed);
        horizontalState = PlayerHorizontalState.RUNNING;
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
}
