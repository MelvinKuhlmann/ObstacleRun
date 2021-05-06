using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;
    public Vector2
        margin,
        smoothing;
    public BoxCollider2D bounds;
    public bool isFollowing { get; set; }

    private Vector3
        _min,
        _max;
    private Camera _camera;

    public void Start()
    {
        _camera = GetComponent<Camera>();
        _min = bounds.bounds.min;
        _max = bounds.bounds.max;
        isFollowing = true;
    }

    public void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;

        if (isFollowing)
        {
            if (Mathf.Abs(x - player.position.x) > margin.x)
            {
                x = Mathf.Lerp(x, player.position.x, smoothing.x * Time.deltaTime);
            }
            if (Mathf.Abs(y - player.position.y) > margin.y)
            {
                y = Mathf.Lerp(y, player.position.y, smoothing.y * Time.deltaTime);
            }
        }

        var cameraHalfWidth = _camera.orthographicSize * ((float)Screen.width / Screen.height);

        x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, _min.y + _camera.orthographicSize, _max.y - _camera.orthographicSize);

        transform.position = new Vector3(x, y, transform.position.z);
    }
}