using UnityEngine;

public class Interactable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            trigger.TriggerDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            trigger.StopDialogue();
        }
    }
}
