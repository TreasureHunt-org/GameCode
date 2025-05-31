using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeDialougeManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Button continueButton;
    private string[] currentDialogue;
    private int dialogueIndex;
    public float typingSpeed = 0.05f; 
    public Image Image1, Image2, Image3;
    public GameObject NPC;
    public AudioSource audioSource;
    public AudioClip clip1, clip2, clip3;
    private Coroutine typingCoroutine; 
    TestingMovement playerMovement;
    public Image image11, image12, image13;
    public TextMeshProUGUI t1, t2;
    public GameManager gameManager;
    private void Start()
    {
        playerMovement = FindObjectOfType<TestingMovement>();

    }
    public void StartDialogue(string[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogError("Dialogue lines are empty!");
            return;
        }
        playerMovement.FreezeMovement();
        playerMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;  // Stop movement
        playerMovement.enabled = false;
        currentDialogue = dialogueLines;
        dialogueIndex = 0;
        StartCoroutine(FadeInUI());
        ShowNextDialogue();
    }
    public void ShowNextDialogue()
    {
        if (dialogueIndex < currentDialogue.Length)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Stop previous typing if needed
            }
            typingCoroutine = StartCoroutine(TypeText(currentDialogue[dialogueIndex]));
            dialogueIndex++;
            if (dialogueIndex == 0)
            {
                audioSource.PlayOneShot(clip1);
            }
            else if (dialogueIndex == 1)
            {
                audioSource.PlayOneShot(clip2);
            }
            else if (dialogueIndex == 2)
            {
                audioSource.PlayOneShot(clip3);
            }
            else if (dialogueIndex == 3)
            {
                audioSource.PlayOneShot(clip1);
            }
        }
        else
        {
            EndDialogue();
            image11.gameObject.SetActive(true);
            image12.gameObject.SetActive(true);
            image13.gameObject.SetActive(true);
            t1.gameObject.SetActive(true);
            t2.gameObject.SetActive(true);
            gameManager.StartRound();

        }
    }
    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void EndDialogue()
    {
        StartCoroutine(FadeOutUI());

        playerMovement.enabled = true;

        if (NPC != null && dialogueIndex == 4)
        {
            StartCoroutine(FadeOutAndDestroy(NPC));
        }
    }
    IEnumerator FadeOutAndDestroy(GameObject npc)
    {
        SpriteRenderer spriteRenderer = npc.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Destroy(npc);
            yield break;
        }

        float fadeDuration = 1f;
        float elapsedTime = 0f;

        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        Destroy(npc);
        EventManager.TriggerNPCDestroyed();
    }
    IEnumerator FadeOutUI()
    {
        float duration = 1f;
        float elapsedTime = 0f;

        CanvasGroup[] uiElements = {
        Image1.GetComponent<CanvasGroup>(),
        Image2.GetComponent<CanvasGroup>(),
        Image3.GetComponent<CanvasGroup>(),
        dialogueText.GetComponent<CanvasGroup>(),
        continueButton.GetComponent<CanvasGroup>()
    };

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / duration);

            foreach (var ui in uiElements)
            {
                if (ui != null)
                {
                    ui.alpha = alpha;
                }
            }

            yield return null;
        }

        // Fully deactivate after fading out
        foreach (var ui in uiElements)
        {
            if (ui != null)
            {
                ui.gameObject.SetActive(false);
            }
        }

    ;
    }
    IEnumerator FadeInUI()
    {
        float duration = 1.5f; // Duration of fade effect
        float elapsedTime = 0f;

        // Enable all UI elements but set their transparency to 0
        CanvasGroup[] uiElements = {
        Image1.GetComponent<CanvasGroup>(),
        Image2.GetComponent<CanvasGroup>(),
        Image3.GetComponent<CanvasGroup>(),
        dialogueText.GetComponent<CanvasGroup>(),
        continueButton.GetComponent<CanvasGroup>()
    };

        foreach (var ui in uiElements)
        {
            if (ui != null)
            {
                ui.alpha = 0;
                ui.gameObject.SetActive(true);
            }
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / duration);

            foreach (var ui in uiElements)
            {
                if (ui != null)
                {
                    ui.alpha = alpha;
                }
            }

            yield return null;
        }

        // Ensure full opacity at the end
        foreach (var ui in uiElements)
        {
            if (ui != null)
            {
                ui.alpha = 1;
            }
        }
    }
}
