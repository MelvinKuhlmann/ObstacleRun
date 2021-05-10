using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float SKIN_WIDTH = .02f;
    private const int TOTAL_HORIZONTAL_RAYS = 8;
    private const int TOTAL_VERTICAL_RAYS = 4;

    private int currentNumberOfJumps = 0;

    private Vector2 _velocity;
    private Transform _transform;
    private Vector3 localScale;
    private BoxCollider2D boxCollider;
    private PlayerParameters overrideParameters;
    private float jumpIn;
    private GameObject lastStandingOn;

    private Vector3
        _activeGlobalPlatformPoint,
        _activeLocalPlatformPoint;

    private Vector3
        raycastTopLeft,
        raycastBottomLeft,
        raycastBottomRight;

    private float verticalDistanceBetweenRays;
    private float horizontalDistanceBetweenRays;

    private bool isMoving;

    public LayerMask platformMask;
    public PlayerParameters defaultParameters;

    public PlayerState state { get; private set; }
    public Vector2 velocity { get { return _velocity; } }
    public bool handleCollisions { get; set; }
    public PlayerParameters parameters { get { return overrideParameters ?? defaultParameters; } }
    public GameObject standingOn { get; private set; }
    public Vector3 platformVelocity { get; private set; }
    public bool canJump
    {
        get
        {
            return jumpIn <  0 &&(state.isGrounded || currentNumberOfJumps <= parameters.numberOfDoubleJumps);
        }
    }

    public void Awake()
    {
        isMoving = false;
        handleCollisions = true;
        state = new PlayerState();
        _transform = transform;
        localScale = _transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();

        var colliderWidth = boxCollider.size.x * Mathf.Abs(localScale.x) - (2 * SKIN_WIDTH);
        horizontalDistanceBetweenRays = colliderWidth / (TOTAL_VERTICAL_RAYS - 1);

        var colliderHeight = boxCollider.size.y * Mathf.Abs(localScale.y) - (2 * SKIN_WIDTH);
        verticalDistanceBetweenRays = colliderHeight / (TOTAL_HORIZONTAL_RAYS - 1);
    }

    public void LateUpdate()
    {
        jumpIn -= Time.deltaTime;
        _velocity.y += parameters.gravity * Time.deltaTime;
        Move(velocity * Time.deltaTime);
    }

    public void AddForce(Vector2 force)
    {
        _velocity = force;
    }

    public void SetForce(Vector2 force)
    {
        _velocity += force;
    }

    public void SetHorizontalForce(float x)
    {
        _velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        _velocity.y = y;
    }
    public void Jump()
    {
        currentNumberOfJumps++;
        AddForce(new Vector2(_velocity.x, parameters.jumpMagnitude));
        jumpIn = parameters.jumpFrequency;
    }

    private void Move(Vector2 deltaMovement)
    {
        bool wasGrounded = state.isCollidingBelow;
        state.Reset();

        if (handleCollisions)
        {
            CalculateRayOrigins();

            if (Mathf.Abs(deltaMovement.x) > .001f)
            {
                MoveHorizontally(ref deltaMovement);
            }

            MoveVertically(ref deltaMovement);

            CorrectHorizontalPlacement(ref deltaMovement, true);
            CorrectHorizontalPlacement(ref deltaMovement, false);
        }

        _transform.Translate(deltaMovement, Space.World);

        if (Time.deltaTime > 0)
        {
            _velocity = deltaMovement / Time.deltaTime;
        }

        _velocity.x = Mathf.Min(_velocity.x, parameters.maxVelocity.x);
        _velocity.y = Mathf.Min(_velocity.y, parameters.maxVelocity.y);

        if (standingOn != null)
        {
            //calculate velocity of object where we are standing on
            _activeGlobalPlatformPoint = transform.position;
            _activeLocalPlatformPoint = standingOn.transform.InverseTransformPoint(transform.position);

            if (lastStandingOn != standingOn)
            {
                if (lastStandingOn != null)
                {
                    lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
                }
                standingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                lastStandingOn = standingOn;
            }
            else if (standingOn != null)
            {
                standingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (lastStandingOn != null)
        {
            lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
            lastStandingOn = null;

        }
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
    {
        var halfWidth = (boxCollider.size.x * localScale.x) / 2f;
        var rayOrigin = (raycastBottomRight + raycastBottomLeft) / 2;
        var rayDirection = isRight ? Vector2.right : -Vector2.right;
        var offset = 0f;

        for (var i = 1; i < TOTAL_HORIZONTAL_RAYS - 1; i++)
        {
            var rayVector = new Vector2(deltaMovement.x + rayOrigin.x, deltaMovement.y + rayOrigin.y + (i * verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);
            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, platformMask);
            if (!raycastHit)
            {
                continue;
            }
            offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));
        }
        deltaMovement.x += offset;
    }

    private void CalculateRayOrigins()
    {
        var size = new Vector2(boxCollider.size.x * Mathf.Abs(localScale.x), boxCollider.size.y * Mathf.Abs(localScale.y)) / 2;
        var center = new Vector2(boxCollider.offset.x * localScale.x, boxCollider.offset.y * localScale.y);

        raycastTopLeft = _transform.position + new Vector3(center.x - size.x + SKIN_WIDTH, center.y + size.y - SKIN_WIDTH);
        raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SKIN_WIDTH, center.y - size.y + SKIN_WIDTH);
        raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + SKIN_WIDTH, center.y - size.y + SKIN_WIDTH);
    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SKIN_WIDTH;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? raycastBottomRight : raycastBottomLeft;

        for (var i = 0; i < TOTAL_HORIZONTAL_RAYS; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, platformMask);
            if (!rayCastHit)
            {
                continue;
            }

            //the furthest we can move without hitting anything
            deltaMovement.x = rayCastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (isGoingRight)
            {
                isMoving = true;
                deltaMovement.x -= SKIN_WIDTH;
                state.isCollidingRight = true;
            }
            else
            {
                isMoving = true;
                deltaMovement.x += SKIN_WIDTH;
                state.isCollidingLeft = true;
            }

            if (rayDistance < SKIN_WIDTH + .0001f)
            {
                Debug.Log("Test");
                break;
            }
        }
    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SKIN_WIDTH;
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? raycastTopLeft : raycastBottomLeft;

        rayOrigin.x += deltaMovement.x;

        var standingOnDistance = float.MaxValue;

        for (var i = 0; i < TOTAL_VERTICAL_RAYS; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * horizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, platformMask);
            if (!rayCastHit)
            {
                continue;
            }

            if (!isGoingUp)
            {
                var verticalDistanceToHit = _transform.position.y - rayCastHit.point.y;
                if (verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    standingOn = rayCastHit.collider.gameObject;
                }
            }

            deltaMovement.y = rayCastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingUp)
            {
                deltaMovement.y -= SKIN_WIDTH;
                state.isCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += SKIN_WIDTH;
                state.isCollidingBelow = true;
                currentNumberOfJumps = 0;
            }

            if (rayDistance < SKIN_WIDTH + .0001f)
            {
                break;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<PlayerPhysics>();
        if (parameters == null)
        {
            return;
        }
        overrideParameters = parameters.parameters;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<PlayerPhysics>();
        if (parameters == null)
        {
            return;
        }
        overrideParameters = null;
    }
}
