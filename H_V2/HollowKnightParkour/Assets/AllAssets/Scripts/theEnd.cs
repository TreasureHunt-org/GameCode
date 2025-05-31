using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class theEnd : MonoBehaviour
{
    public CanvasGroup fadePanel;
    TestingMovement movement;
    public AudioSource source,stop;
public AudioClip clip;
    void Start()
    {
        movement = FindObjectOfType<TestingMovement>();
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            movement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            movement.FreezeMovement();
            movement.final = true;
            Rigidbody2D rb = movement.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static; 


            stop.Stop();
            Invoke(nameof(startSound), 2f);
            StartCoroutine(DelayedSceneLoad());
        }
    }
    void startSound()
    {
        source.PlayOneShot(clip);
    }
    private IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(5f);  // Wait for 5 seconds
        StartCoroutine(FadeAndLoadScene("BossFight"));  // Then start the fade and load
    }
    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            float duration = 1.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                fadePanel.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
                yield return null;
            }
        }

        SceneManager.LoadScene(sceneName);
    }
}
