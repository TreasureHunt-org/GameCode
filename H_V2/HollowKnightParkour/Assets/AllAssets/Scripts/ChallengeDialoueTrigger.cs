using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeDialoueTrigger : MonoBehaviour
{
    public ChallengeDialougeManager dialogueManager;
    public string[] dialogueLines;

    private bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("here");
            playerInRange = true;
            dialogueManager.StartDialogue(dialogueLines);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueManager.EndDialogue();
        }
    }
}
