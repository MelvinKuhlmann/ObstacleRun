using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            other.gameObject.GetComponent<PlayerHealth>().GetDamage(0.5F);
        }
    }
}
