using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using UnityEngine.Rendering.Universal;  


public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    private GameObject ball;
    public Transform spawnPoint;
    public float fadeDuration = 3f;
    public Collider2D Collider2D;
    public Light2D light2D; 
    public float targetIntensity = 1f; 
    public float fadeInDuration = 2f;

    private void OnEnable()
    {
        EventManager.OnNPCDestroyed += SpawnBall;
    }
    private void OnDisable()
    {
        EventManager.OnNPCDestroyed -= SpawnBall;
    }
    private void SpawnBall()
    {
        StartCoroutine(FadeInLight());
        StartCoroutine(SpawnBallWithFade());
    }
    public IEnumerator FadeInLight()
    {
        float startIntensity = light2D.intensity;
        float timeElapsed = 0f;

        // Gradually increase intensity
        while (timeElapsed < fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            light2D.intensity = Mathf.Lerp(startIntensity, targetIntensity, timeElapsed / fadeInDuration);
            yield return null;
        }

        light2D.intensity = targetIntensity; 
    }
    public IEnumerator FadeOutLight()
    {
        float startIntensity = light2D.intensity;
        float timeElapsed = 0f;

       
        while (timeElapsed < fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            light2D.intensity = Mathf.Lerp(startIntensity, 0f, timeElapsed / fadeInDuration);
            yield return null;
        }

        light2D.intensity = 0f; 
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
        color.a = 0; 
        ballMaterial.color = color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            ballMaterial.color = color;
            yield return null;
        }

        color.a = 1; 
        ballMaterial.color = color;

    }
   public void FadeOutBallPrefab()
   {
    Destroy(Collider2D);
    StartCoroutine(FadeOutBallPrefabCoroutine());
    StartCoroutine(FadeOutLight());
    }
private IEnumerator FadeOutBallPrefabCoroutine()
{
    if (ball == null)
    {
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
    Destroy(ballPrefab); 
    Destroy(this);
    }

}

