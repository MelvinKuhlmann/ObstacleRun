using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isFacingRight;
    private PlayerController controller;
    private float normalizedHorizontalSpeed;

    public float maxSpeed = 12f;
    public float speedAccelerationOnGround = 16f;
    public float speedAccelerationInAir = 8f;
    public bool isDead { get; private set; }

    public void Awake()
    {
        controller = GetComponent<PlayerController>();
        isFacingRight = transform.localScale.x > 0;
    }

    public void Update()
    {
        if (!isDead)
        {
            HandleInput();
        }

        var movementFactor = controller.state.isGrounded ? speedAccelerationOnGround : speedAccelerationInAir;

        if (isDead)
        {
            controller.SetHorizontalForce(0);
        }
        else
        {
            controller.SetHorizontalForce(Mathf.Lerp(controller.velocity.x, normalizedHorizontalSpeed * maxSpeed, Time.deltaTime * movementFactor));
        }
    }

    public void Kill()
    {
        controller.handleCollisions = false;
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
        controller.SetForce(new Vector2(0, 10f));
    }

    public void RespawnAt(Transform spawnPoint)
    {
        if (!isFacingRight)
        {
            Flip();
        }
        isDead = false;
        GetComponent<Collider2D>().enabled = true;
        controller.handleCollisions = true;

        transform.position = spawnPoint.position;
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            normalizedHorizontalSpeed = 1;
            if (!isFacingRight)
            {
                Flip();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            normalizedHorizontalSpeed = -1;
            if (isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            normalizedHorizontalSpeed = 0;
        }

        if (controller.canJump && Input.GetKey(KeyCode.Space))
        {
            controller.Jump();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = transform.localScale.x > 0;
    }
}
