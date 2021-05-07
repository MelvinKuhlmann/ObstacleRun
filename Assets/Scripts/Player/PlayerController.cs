using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float skinWidth = .02f;
    private const int totalHorizontalRays = 8;
    private const int totalVerticalRays = 4;
    public int currentNumberOfJumps = 0;

    public LayerMask platformMask;
    public PlayerParameters defaultParameters;

    public PlayerState state { get; private set; }
    public Vector2 velocity { get { return _velocity; } }
    public bool handleCollisions { get; set; }
    public PlayerParameters parameters { get { return _overrideParameters ?? defaultParameters; } }
    public GameObject standingOn { get; private set; }
    public Vector3 platformVelocity { get; private set; }
    public bool canJump
    {
        get
        {
            return _jumpIn <  0 &&(state.isGrounded || currentNumberOfJumps <= parameters.numberOfDoubleJumps);
        }
    }

    private Vector2 _velocity;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider;
    private PlayerParameters _overrideParameters;
    private float _jumpIn;
    private GameObject _lastStandingOn;

    private Vector3
        _activeGlobalPlatformPoint,
        _activeLocalPlatformPoint;

    private Vector3
        _raycastTopLeft,
        _raycastBottomLeft,
        _raycastBottomRight;

    private float verticalDistanceBetweenRays;
    private float horizontalDistanceBetweenRays;

    public void Awake()
    {
        handleCollisions = true;
        state = new PlayerState();
        _transform = transform;
        _localScale = _transform.localScale;
        _boxCollider = GetComponent<BoxCollider2D>();

        var colliderWidth = _boxCollider.size.x * Mathf.Abs(_localScale.x) - (2 * skinWidth);
        horizontalDistanceBetweenRays = colliderWidth / (totalVerticalRays - 1);

        var colliderHeight = _boxCollider.size.y * Mathf.Abs(_localScale.y) - (2 * skinWidth);
        verticalDistanceBetweenRays = colliderHeight / (totalHorizontalRays - 1);
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
        _jumpIn = parameters.jumpFrequency;
    }

    public void LateUpdate()
    {
        _jumpIn -= Time.deltaTime;
        _velocity.y += parameters.gravity * Time.deltaTime;
        Move(velocity * Time.deltaTime);
    }

    private void Move(Vector2 deltaMovement)
    {
        var wasGrounded = state.isCollidingBelow;
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

            if (_lastStandingOn != standingOn)
            {
                if (_lastStandingOn != null)
                {
                    _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
                }
                standingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                _lastStandingOn = standingOn;
            }
            else if (standingOn != null)
            {
                standingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (_lastStandingOn != null)
        {
            _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
            _lastStandingOn = null;

        }
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
    {
        var halfWidth = (_boxCollider.size.x * _localScale.x) / 2f;
        var rayOrigin = (_raycastBottomRight + _raycastBottomLeft) / 2;
        var rayDirection = isRight ? Vector2.right : -Vector2.right;
        var offset = 0f;

        for (var i = 1; i < totalHorizontalRays - 1; i++)
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
        var size = new Vector2(_boxCollider.size.x * Mathf.Abs(_localScale.x), _boxCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        var center = new Vector2(_boxCollider.offset.x * _localScale.x, _boxCollider.offset.y * _localScale.y);

        _raycastTopLeft = _transform.position + new Vector3(center.x - size.x + skinWidth, center.y + size.y - skinWidth);
        _raycastBottomRight = _transform.position + new Vector3(center.x + size.x - skinWidth, center.y - size.y + skinWidth);
        _raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + skinWidth, center.y - size.y + skinWidth);
    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

        for (var i = 0; i < totalHorizontalRays; i++)
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
                deltaMovement.x -= skinWidth;
                state.isCollidingRight = true;
            }
            else
            {
                deltaMovement.x += skinWidth;
                state.isCollidingLeft = true;
            }

            if (rayDistance < skinWidth + .0001f)
            {
                break;
            }
        }
    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;

        rayOrigin.x += deltaMovement.x;

        var standingOnDistance = float.MaxValue;

        for (var i = 0; i < totalVerticalRays; i++)
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
                deltaMovement.y -= skinWidth;
                state.isCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += skinWidth;
                state.isCollidingBelow = true;
                currentNumberOfJumps = 0;
            }

            if (rayDistance < skinWidth + .0001f)
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
        _overrideParameters = parameters.parameters;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<PlayerPhysics>();
        if (parameters == null)
        {
            return;
        }
        _overrideParameters = null;
    }
}
