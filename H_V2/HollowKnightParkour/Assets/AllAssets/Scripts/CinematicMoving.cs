using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicMoving : MonoBehaviour
{
    public float speed = 2f;
    private bool isMoving = false;
    public CanvasGroup fadePanel;
    public AudioSource audioo;
    public AudioClip clip;
    void Start()
    {
        StartCoroutine(StartMovement());
        audioo.clip = clip; 
    }

    IEnumerator StartMovement()
    {
        yield return new WaitForSeconds(2f); // Wait 2 seconds before starting movement
        isMoving = true;
        yield return new WaitForSeconds(15f); // Move for 15 seconds
        isMoving = false;
        if(isMoving == false)
        {
            StartCoroutine(FadeAndLoadScene("SampleScene"));
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
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
