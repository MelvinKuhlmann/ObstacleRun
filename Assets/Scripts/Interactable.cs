using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public TMP_Text txtLabel;
    public string label;
    public SpriteRenderer spriteRenderer;
    public Sprite inactive;
    public Sprite active;

    private bool isActive;

    private void Start()
    {
        SetInactive();
        txtLabel.text = label;
    }

    private void LateUpdate()
    {
        if (isActive && Input.GetKey(KeyCode.X))
        {
            DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            if (!trigger.IsDialogueStarted()) 
            { 
                trigger.TriggerDialogue();
            }
        }
    }

    private void SetActive()
    {
        spriteRenderer.sprite = active;
        txtLabel.enabled = true;
        isActive = true;
    }

    private void SetInactive()
    {
        spriteRenderer.sprite = inactive;
        txtLabel.enabled = false;
        if (isActive)
        {
            DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            trigger.StopDialogue();
        }
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            SetActive();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            SetInactive();
        }
    }
}
