using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerHorizontalState horizontalState;
    private PlayerVerticalState verticalState;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private float jumpIn;

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

    void Update()
    {
        HandleKeyboard();
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
        switch (horizontalState)
        {
            case PlayerHorizontalState.IDLE:
                ChangeAnimation("idle");
                break;
            case PlayerHorizontalState.RUNNING_LEFT:
                transform.position -= transform.right * (Time.deltaTime * moveSpeed);
                ChangeAnimation("run");
                break;
            case PlayerHorizontalState.RUNNING_RIGHT:
                transform.position += transform.right * (Time.deltaTime * moveSpeed);
                ChangeAnimation("run");
                break;
            default:
                break;
        }

        switch (verticalState)
        {
            case PlayerVerticalState.JUMPING:
                ChangeAnimation("jump");
                break;
            case PlayerVerticalState.FALLING:
                ChangeAnimation("fall");
                break;
        }
    }

    private void HandleKeyboard()
    {
        // Een fail-safe om ervoor te zorgen dat er geen rare dingen gebeuren als men links en rechts tegelijk indrukt.
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Onderstaande if-statement is om ervoor te zorgen dat de beruchte flip van de sprite er niet meer is.
            if (horizontalState != PlayerHorizontalState.RUNNING_RIGHT)
            {
                animator.Play("idle_right");
            }

            horizontalState = PlayerHorizontalState.RUNNING_RIGHT;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Onderstaande if-statement is om ervoor te zorgen dat de beruchte flip van de sprite er niet meer is.
            if (horizontalState != PlayerHorizontalState.RUNNING_LEFT)
            {
                animator.Play("idle_left");
            }

            horizontalState = PlayerHorizontalState.RUNNING_LEFT;
        } else
        {
            horizontalState = PlayerHorizontalState.IDLE;
        }

        if(Input.GetKey(KeyCode.Space) && verticalState.Equals(PlayerVerticalState.GROUNDED))
        {
            if (jumpIn < 0)
            {
                jumpIn = jumpFrequency;
                rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                verticalState = PlayerVerticalState.JUMPING;
            }
        }
    }

    public void ChangeAnimation(string animationFlag, bool resetAll = true)
    {
        if (resetAll)
        {
            ResetAnimationParameters();
        }
        animator.SetBool(animationFlag, true);
    }

    public void Fall()
    {
        verticalState = PlayerVerticalState.FALLING;
    }

    private void ResetAnimationParameters()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ("FLOOR".Equals(other.gameObject.tag) && !verticalState.Equals(PlayerVerticalState.GROUNDED))
        {
            ChangeAnimation("idle");
            CreateDust();
            verticalState = PlayerVerticalState.GROUNDED;
        }
    }

    private void CreateDust()
    {
        dust.Play();
    }
}
