using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125F; 
    public Vector3 offset = new Vector3(0, 6, -1); // Standaard waarden, kunnen we later nog tweaken.

    [Header("Level Boundaries")] //TODO maybe move this to a LevelManager to prevents the values being overwritten when it is a in a prefab
    public bool enableCameraBoundaries = true;
    [SerializeField]
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float topLimit;
    [SerializeField]
    float bottomLimit;

    private float cameraHalfWidth;
    private float cameraHalfHeight;

    [Header("Shake Params")]
    public float shakeDuration = .4f;
    public float shakeDirection = 1f;
    public float shakeSpeed = 2f;
    private float startShakeDuration;
    private int shakeOrientation = 1; // 1 horizontal, -1 vertical
    private bool shakeEffect = false;

    #region Singleton
    public static CameraController instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }
    #endregion

    private void Start()
    {
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;
        startShakeDuration = shakeDuration;

    }

    private void FixedUpdate()
    {
        // camera current position
        Vector3 startPos = transform.position;

        // player current position
        Vector3 endPos = player.transform.position;

        endPos.x += offset.x;
        endPos.y += offset.y;
        endPos.z += offset.z;

        // smoothly move the camera towards the player position
        transform.position = Vector3.Lerp(startPos, endPos, smoothSpeed);

        if (enableCameraBoundaries)
        {
            // make sure the camera stays in the boundaries
            transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, leftLimit + cameraHalfWidth, rightLimit - cameraHalfWidth),
                Mathf.Clamp(transform.position.y, bottomLimit + cameraHalfHeight, topLimit - cameraHalfHeight),
                transform.position.z
            );
        }
        HandleEffects();
    }

    private void HandleEffects()
    {
        if (shakeEffect)
        {
            transform.position = new Vector3(
                shakeOrientation == 1 ? transform.position.x + (shakeDirection * shakeSpeed) : transform.position.x,
                shakeOrientation == -1 ? transform.position.y + (shakeDirection * shakeSpeed) : transform.position.y,
                transform.position.z
            );
            shakeDirection *= -1.0f;

            if (startShakeDuration <= 0)
            {
                shakeEffect = false;
            }
            else
            {
                startShakeDuration -= Time.deltaTime;
            }
        }
    }

    public void Shake(int shakeOrientation)
    {
        this.shakeOrientation = shakeOrientation;
        startShakeDuration = shakeDuration;
        shakeEffect = true;
    }

    private void OnDrawGizmos()
    {
        if (!enableCameraBoundaries) 
        { 
            return;
        }
        // draw a box around the camera boundaries
        Gizmos.color = Color.red;
        // top
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));
        // right
        Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(rightLimit, bottomLimit));
        // bottom
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
        // left
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(leftLimit, bottomLimit));
    }
}
