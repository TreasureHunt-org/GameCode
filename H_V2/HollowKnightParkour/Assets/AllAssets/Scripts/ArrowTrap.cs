using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public GameObject arrow;
    public Vector2 arrowDirection = Vector2.up; // Default direction
    public float arrowSpeed = 5f;
    public float destroyTime = 2f;
    public float shakeDuration = 0.5f; // Less shake time for faster activation
    public float shakeIntensity = 0.15f; // Stronger shake
    public float spikeMoveDuration = 0.2f; // SUPER fast spike movement
    public float spikeMoveDistance = 1.2f; // Spike goes up enough to hit the player
    public float spikeStartDelay = 0.3f; // Spikes move up VERY quickly after shake starts

    private bool triggered = false;
    private Vector3 originalPosition;
    private Vector3 arrowStartPos;

    void Start()
    {
        originalPosition = transform.position;
        if (arrow != null)
        {
            arrowStartPos = arrow.transform.localPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(StartShaking());
        }
    }

    IEnumerator StartShaking()
    {
        float elapsed = 0f;
        bool spikeActivated = false;

        while (elapsed < shakeDuration)
        {
            float xOffset = Random.Range(-shakeIntensity, shakeIntensity);
            float yOffset = Random.Range(-shakeIntensity, shakeIntensity);

            transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

            elapsed += Time.deltaTime;

            // Spikes start moving up VERY SOON (0.3s delay)
            if (!spikeActivated && elapsed >= spikeStartDelay)
            {
                spikeActivated = true;
                StartCoroutine(MoveSpike());
            }

            yield return null;
        }

        transform.position = originalPosition; // Reset position
    }

    IEnumerator MoveSpike()
    {
        if (arrow != null)
        {
            arrow.SetActive(true);

            Vector3 startPos = arrow.transform.localPosition;
            Vector3 targetPos = startPos + (Vector3)arrowDirection * spikeMoveDistance;
            float elapsed = 0f;

            while (elapsed < spikeMoveDuration)
            {
                arrow.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / spikeMoveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            arrow.transform.localPosition = targetPos; // Ensure exact position
        }
    }
}
