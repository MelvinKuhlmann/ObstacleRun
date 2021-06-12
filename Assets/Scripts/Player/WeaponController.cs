using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag(Tags.Enemy.ToString()))
        {
            // TODO spawn hitmarks on collision.contacts
          //  collision.collider.gameObject.GetComponent<EnemyController>().TakeDamage(1);
        }
    }
}
