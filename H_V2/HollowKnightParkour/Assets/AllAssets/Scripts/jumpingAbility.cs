using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class JumpingAbility : MonoBehaviour
{
    public Image Image1, Image2, Image3, Image4;
    public TextMeshProUGUI textMeshProUGUI;
    public Button continueButton;
    public TestingMovement TestingMovement;
    public BallSpawner BallSpawner;
    public float fadeDuration = 1f; 
    private Renderer ballRenderer;
    private Material ballMaterial;
    public AudioSource audioSource;
    public AudioClip clip;

    public void startDialouge()
    {
        TestingMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        TestingMovement.FreezeMovement();
        TestingMovement.enabled = false;
        StartCoroutine(FadeInUI());
        audioSource.PlayOneShot(clip);

    }
    public void EndDialouge()
    {
        StartCoroutine(FadeOutUI());
        TestingMovement.jumpForce = 7;
        TestingMovement.jumpTime = 0.35f;
        BallSpawner.FadeOutBallPrefab();
        TestingMovement.enabled = true;
    }
    IEnumerator FadeOutUI()
    {
        float duration = 1f;
        float elapsedTime = 0f;

        CanvasGroup[] uiElements = {
        Image1.GetComponent<CanvasGroup>(),
        Image2.GetComponent<CanvasGroup>(),
        Image3.GetComponent<CanvasGroup>(),
        Image4.GetComponent<CanvasGroup>(),
        textMeshProUGUI.GetComponent<CanvasGroup>(),
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

      
        foreach (var ui in uiElements)
        {
            if (ui != null)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator FadeInUI()
    {
        float duration = 1.5f; // Duration of fade effect
        float elapsedTime = 0f;

        
        CanvasGroup[] uiElements = {
        Image1.GetComponent<CanvasGroup>(),
        Image2.GetComponent<CanvasGroup>(),
        Image3.GetComponent<CanvasGroup>(),
        Image4.GetComponent<CanvasGroup>(),
        textMeshProUGUI.GetComponent<CanvasGroup>(),
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

        
        foreach (var ui in uiElements)
        {
            if (ui != null)
            {
                ui.alpha = 1;
            }
        }
    }
    private IEnumerator FadeOut(GameObject obj, float duration)
    {
        if (ballMaterial == null) yield break; 

        float elapsedTime = 0f;
        Color originalColor = ballMaterial.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            ballMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        obj.SetActive(false);
    }
}
