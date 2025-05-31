using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using UnityEngine.Rendering.Universal;  
using UnityEngine.UI;
using TMPro;
public class DownHitAbility : MonoBehaviour
{
    public GameObject ballPrefab;
    GameObject ball;
    public Transform spawnPoint;
    public float fadeDuration = 3f;
    public Collider2D Collider2D;
    public TestingMovement testingMovement;
    public AudioSource audioSource;
    public AudioClip clip;

    public Image Image1, Image2, Image3, Image4;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    private void Start()
    {

        SpawnBall();
       

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInUI());
            //testingMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //testingMovement.FreezeMovement();
            //testingMovement.enabled = false;
            //testingMovement.isAllowedToHit = true;
            other.GetComponent<TestingMovement>().isAllowedToSideHit = true;
            audioSource.PlayOneShot(clip);
        }
    }
    public void Continue()
    {
        //testingMovement.enabled = true;
        StartCoroutine(FadeOutUI());
        FadeOutBallPrefab();
    }
    private void SpawnBall()
    {
        StartCoroutine(SpawnBallWithFade());
    }
    IEnumerator SpawnBallWithFade()
    {
        ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        Renderer ballRenderer = ball.GetComponent<Renderer>();

        if (ballRenderer == null)
        {
            Debug.LogError("Ball does not have a Renderer component!");
            yield break;
        }

        Material ballMaterial = ballRenderer.material;
        Color color = ballMaterial.color;
        color.a = 0; // Start fully transparent
        ballMaterial.color = color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            ballMaterial.color = color;
            yield return null;
        }

        color.a = 1; // Ensure it's fully visible
        ballMaterial.color = color;

    }
    public void FadeOutBallPrefab()
    {
        Destroy(Collider2D);
      
        StartCoroutine(FadeOutBallPrefabCoroutine());
    }
    private IEnumerator FadeOutBallPrefabCoroutine()
    {
        if (ball == null)
        {
            Debug.LogError("No ballPrefab assigned!");
            yield break;
        }

        Renderer ballRenderer = ball.GetComponent<Renderer>();
        if (ballRenderer == null)
        {
            Debug.LogError("Ball does not have a Renderer component!");
            yield break;
        }

        Material ballMaterial = ballRenderer.material;
        Color color = ballMaterial.color;
        color.a = 1; 
        ballMaterial.color = color;

        // Fade out over time
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            ballMaterial.color = color;
            yield return null;
        }

       
        color.a = 0;
        ballMaterial.color = color;
        Destroy(ballPrefab); // Destroy the ball after fading out
        Destroy(this);
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

       
        CanvasGroup[] uiElements = {
        Image1.GetComponent<CanvasGroup>(),
        Image2.GetComponent<CanvasGroup>(),
        Image3.GetComponent<CanvasGroup>(),
        Image4.GetComponent<CanvasGroup>(),
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

       
        foreach (var ui in uiElements)
        {
            if (ui != null)
            {
                ui.alpha = 1;
            }
        }
    }
}
