using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerHorizontalState horizontalState;
    private PlayerVerticalState verticalState;
    private Animator animator;
    private Rigidbody2D rigidBody;

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

    private void HandleState()
    {
        switch (horizontalState)
        {
            case PlayerHorizontalState.IDLE:
                ChangeAnimation("idle");
                break;
            case PlayerHorizontalState.RUNNING_LEFT:
                transform.position -= transform.right * (Time.deltaTime * moveSpeed);
                ChangeAnimation("run_left");
                break;
            case PlayerHorizontalState.RUNNING_RIGHT:
                transform.position += transform.right * (Time.deltaTime * moveSpeed);
                ChangeAnimation("run_right");
                break;
            default:
                break;
        }        
    }

    private void HandleKeyboard()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalState = PlayerHorizontalState.RUNNING_RIGHT;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalState = PlayerHorizontalState.RUNNING_LEFT;
        } else
        {
            horizontalState = PlayerHorizontalState.IDLE;
        }

        if(Input.GetKey(KeyCode.Space) && verticalState.Equals(PlayerVerticalState.GROUNDED))
        {
            rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            verticalState = PlayerVerticalState.JUMPING;
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
            CreateDust();
            verticalState = PlayerVerticalState.GROUNDED;
        }
    }

    private void CreateDust()
    {
        dust.Play();
    }
}
