using UnityEngine;

public class DestroyableDoor : MonoBehaviour
{
    public int endurance = 2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag(Tags.Weapon.ToString()))
        {
            // TODO spawn hitmarks on collision.contacts
            endurance -= 1;
            if (endurance <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
