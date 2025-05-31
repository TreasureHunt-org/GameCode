using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomFoutScriptTwo : MonoBehaviour
{
    [System.Serializable]
    public class ArrowWave
    {
        public GameObject[] arrowsInWave;  
    }

    public List<ArrowWave> waves = new List<ArrowWave>(); // List of waves and the arrows for each wave
    public float timeBetweenArrows = 0f;  // Time between each arrow activation in a wave
    public float timeBetweenWaves = 3f;   // Time between each wave
    private bool isTriggered = false;
    public AudioSource Action, Original;
    public AudioClip action, original;
    public AudioSource wallDropSound;
    public AudioClip clip;
    public GameObject Portal;

    public Transform leftWall;
    public Transform rightWall;
    public Vector3 leftWallTarget;
    public Vector3 rightWallTarget;
    public float moveSpeed = 0.5f;
 
    private bool isActivated = false;
    private void Start()
    {
        Action.clip = action;
        Original.clip = original;
        wallDropSound.clip = clip;
    }
    private void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;

            Action.Play();

            Original.Stop();
            Original.clip = null;


            StartCoroutine(TriggerSequence());
        }
    }
    private IEnumerator TriggerSequence()
    {
        yield return StartCoroutine(MoveWalls()); // Wait for walls to finish moving
        yield return new WaitForSeconds(1f);      // Small delay to prevent stutter
        StartCoroutine(ArrowWaves());
    }
    private System.Collections.IEnumerator MoveWalls()
    {
        Vector3 leftStart = leftWall.position;
        Vector3 rightStart = rightWall.position;

        float time = 0f;

        wallDropSound.PlayOneShot(clip);
        while (time < 1f)
        {
            time += Time.deltaTime * moveSpeed;
            leftWall.localPosition = Vector3.Lerp(leftStart, leftWallTarget, time);
            rightWall.localPosition = Vector3.Lerp(rightStart, rightWallTarget, time);

            yield return null;
        }

        leftWall.position = leftWallTarget;
        rightWall.position = rightWallTarget;

    }
    private void startTheScript()
    {
        StartCoroutine(ArrowWaves());
    }
    private void StartTheEarthShaking()
    {
        FindObjectOfType<FloorCollapse>().StartCollapseSequence();
    }
    private IEnumerator ArrowWaves()
    {
        for (int waveIndex = 0; waveIndex < waves.Count; waveIndex++)
        {
            StartCoroutine(ActivateArrows(waveIndex));

            if (waveIndex == 5)
            {
                timeBetweenWaves = 15;
                Invoke(nameof(StartTheEarthShaking), 15f);
            }
            if (waveIndex == 6)
            {
                timeBetweenWaves = 10;
            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        yield return new WaitForSeconds(2f);
        DeactivateArrows();
    }
    private void DeactivateArrows()
    {
        Original.clip = original;  // Assign the original background music again
        Original.Play();

        Action.Stop();             // Stop the action music properly
        Action.clip = null;        // Free memory

        foreach (ArrowWave wave in waves)
        {
            foreach (GameObject arrow in wave.arrowsInWave)
            {
                StartCoroutine(FadeOutAndDeactivate(arrow));
            }
        }
    }
    private IEnumerator FadeOutAndDeactivate(GameObject arrowGroup)
    {
        SpriteRenderer[] spriteRenderers = arrowGroup.GetComponentsInChildren<SpriteRenderer>();

        if (spriteRenderers.Length == 0)
        {
            yield break;
        }

        float fadeDuration = 1.5f;
        float elapsedTime = 0f;

        // Store original colors
        Color[] originalColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }

            yield return null;
        }

        // Fully transparent, now deactivate the whole arrow group
        arrowGroup.SetActive(false);
        Portal.SetActive(true);
    }
    private IEnumerator ActivateArrows(int waveIndex)
    {
        ArrowWave currentWave = waves[waveIndex];

        foreach (GameObject arrow in currentWave.arrowsInWave)
        {
            arrow.SetActive(true);

            var moveRight = arrow.GetComponent<ArrowMovingRight>();
            var moveLeft = arrow.GetComponent<ArrowMovingLeft>();
            var moveDown = arrow.GetComponent<ArrowMovingDown>();
            var moveSlightlyUp = arrow.GetComponent<ArrowsSlightlyAbove>();
            var moveSlightlyLeft = arrow.GetComponent<ArrowsMovingSlightlyLeft>();
            var moveSlightlyRight = arrow.GetComponent<ArrowsMovingSlightlyRight>();

            if (moveRight != null)
            {
                moveRight.StartMoving();  // Start moving right
            }
            else if (moveLeft != null)
            {
                moveLeft.StartMoving();   // Start moving left
            }
            else if (moveDown != null)
            {
                moveDown.StartMoving();     // Start moving up
            }
            else if (moveSlightlyUp != null)
            {
                moveSlightlyUp.StartMoving();
            }
            else if (moveSlightlyLeft != null)
            {
                moveSlightlyLeft.StartMoving();
            }
            else if (moveSlightlyRight != null)
            {
                moveSlightlyRight.StartMoving();
            }


            // Wait before activating the next arrow
            yield return new WaitForSeconds(timeBetweenArrows);
        }
    }
}


