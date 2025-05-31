using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BallSpawner2 : MonoBehaviour
{
    public GameObject ballPrefab;
    private GameObject ball;
    public Transform spawnPoint;
    public float fadeDuration = 3f;
    public Collider2D Collider2D;
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

        color.a = 1; 
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
