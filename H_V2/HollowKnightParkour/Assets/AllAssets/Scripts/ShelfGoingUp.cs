using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfGoingUp : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public float lifetime = 5f;   
    public float fadeDuration = 1f;

    private Renderer shelfRenderer;
    private Color originalColor;
    private float fadeTimer = 0f;
    private bool isFading = false;

    void Start()
    {
        shelfRenderer = GetComponent<Renderer>();
        if (shelfRenderer != null)
            originalColor = shelfRenderer.material.color;

        Invoke(nameof(StartFading), lifetime); // Start fading after 'lifetime' seconds
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime; // Move up

        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration); // Gradually reduce alpha
            if (shelfRenderer != null)
            {
                Color newColor = originalColor;
                newColor.a = alpha;
                shelfRenderer.material.color = newColor;
            }

            if (fadeTimer >= fadeDuration)
                Destroy(gameObject); // Destroy when fade is complete
        }
    }

    void StartFading()
    {
        isFading = true; // Trigger fading effect
    }
}
