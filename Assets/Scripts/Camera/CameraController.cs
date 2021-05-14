using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125F; 
    public Vector3 offset = new Vector3(0, 6, -1); // Standaard waarden, kunnen we later nog tweaken.

    [Header("Level Boundaries")] //TODO maybe move this to a LevelManager to prevents the values being overwritten when it is a in a prefab
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

    private void Start()
    {
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;
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

        // make sure the camera stays in the boundaries
        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, leftLimit + cameraHalfWidth, rightLimit - cameraHalfWidth),
            Mathf.Clamp(transform.position.y, bottomLimit + cameraHalfHeight, topLimit - cameraHalfHeight),
            transform.position.z
        );
    }

    private void OnDrawGizmos()
    {
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
