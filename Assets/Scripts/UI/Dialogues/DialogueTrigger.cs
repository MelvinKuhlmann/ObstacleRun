using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private DialogueManager manager;

    private void Start()
    {
        manager = DialogueManager.instance;
    }

    public void TriggerDialogue()
    {
        manager.StartDialogue(dialogue);
    }

    public bool IsDialogueStarted()
    {
        return manager.IsDialogueStarted();
    }

    public void StopDialogue()
    {
        manager.EndDialogue();
    }
}
