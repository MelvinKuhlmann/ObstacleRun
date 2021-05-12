using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerState state;
    private Animator animator;

    public float moveSpeed;
    
    void Start()
    {
        moveSpeed = 5F;
        state = PlayerState.IDLE;
        animator = GetComponent<Animator>();
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
        switch (state)
        {
            case PlayerState.IDLE:
                ChangeAnimation("idle");
                break;
            case PlayerState.RUNNING_LEFT:
                transform.position -= transform.right * (Time.deltaTime * moveSpeed);
                ChangeAnimation("run_left");
                break;
            case PlayerState.RUNNING_RIGHT:
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
            state = PlayerState.RUNNING_RIGHT;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            state = PlayerState.RUNNING_LEFT;
        } else
        {
            state = PlayerState.IDLE;
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
}
