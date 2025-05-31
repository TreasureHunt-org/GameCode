using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MovingToAbyss : MonoBehaviour
{
    private bool playerInRange = false;
    public Image Image1, Image2, Image3;
    public TextMeshProUGUI textMeshProUGUI;
    public CanvasGroup fadePanel; 
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInUI());
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            StartCoroutine(FadeOutUI());
        }
    }

    IEnumerator FadeOutUI()
    {
        float duration = 1f;
        float elapsedTime = 0f;

        CanvasGroup[] uiElements = {
            Image1.GetComponent<CanvasGroup>(),
            Image2.GetComponent<CanvasGroup>(),
            Image3.GetComponent<CanvasGroup>(),
            textMeshProUGUI.GetComponent<CanvasGroup>()
        };

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / duration);

            foreach (var ui in uiElements)
            {
                if (ui != null) ui.alpha = alpha;
            }
            yield return null;
        }

        foreach (var ui in uiElements)
        {
            if (ui != null) ui.gameObject.SetActive(false);
        }
    }

    IEnumerator FadeInUI()
    {
        float duration = 1.5f;
        float elapsedTime = 0f;

        CanvasGroup[] uiElements = {
            Image1.GetComponent<CanvasGroup>(),
            Image2.GetComponent<CanvasGroup>(),
            Image3.GetComponent<CanvasGroup>(),
            textMeshProUGUI.GetComponent<CanvasGroup>()
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
                if (ui != null) ui.alpha = alpha;
            }
            yield return null;
        }

        foreach (var ui in uiElements)
        {
            if (ui != null) ui.alpha = 1;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKey(KeyCode.W))
        {
            StartCoroutine(FadeOutUI());
            StartCoroutine(FadeAndLoadScene("Abyss")); 
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
