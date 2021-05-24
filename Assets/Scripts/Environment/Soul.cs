using UnityEngine;

public class Soul : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Sprite sprite;
    private Animator itemAnimator;
    private int containsNumberOfSouls;

    private void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        // Add animator at runtime
        itemAnimator = gameObject.AddComponent<Animator>();
        // IMPORTANT: Assets in the Resources folder are the ones exposed through Resources.Load
        itemAnimator.runtimeAnimatorController = Resources.Load("Animations/Item/Soul") as RuntimeAnimatorController;
        itemAnimator.applyRootMotion = true;
        containsNumberOfSouls = Random.Range(1, 10);

        InitCollider2D();
    }

    private void InitCollider2D()
    {
        Vector2 spriteBoundsSize = sprite.bounds.size;

        boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        boxCollider2D.size = spriteBoundsSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player.ToString()))
        {
            InventoryManager.instance.AddCollectedSouls(containsNumberOfSouls);
            Destroy(gameObject);
        }
    }
}
