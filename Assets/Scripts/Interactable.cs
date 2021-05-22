using UnityEngine;

public class Interactable : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite inactive;
    public Sprite active;

    private void Start()
    {
        spriteRenderer.sprite = inactive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            //  DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            //  trigger.TriggerDialogue();
            // TODO show message to press to be able to show the dialogue
            spriteRenderer.sprite = active;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            trigger.StopDialogue();
            spriteRenderer.sprite = inactive;
        }
    }
}
