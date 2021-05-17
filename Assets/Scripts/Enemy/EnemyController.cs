using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 5F;

    private GameObject player;
    private bool followPlayer = false;

    void Update()
    {
        if (followPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            player = other.gameObject;
            followPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            player = null;
            followPlayer = false;
        }
    }
}
