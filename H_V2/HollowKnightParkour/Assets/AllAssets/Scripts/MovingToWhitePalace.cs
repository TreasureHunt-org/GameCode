using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingToWhitePalace : MonoBehaviour
{
    public CanvasGroup fadePanel;
  
    void Start()
    {

    }

  
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            StartCoroutine(FadeAndLoadScene("WhitePalace"));
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
